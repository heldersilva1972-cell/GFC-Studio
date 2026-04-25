using System.Net.Http.Json;
using System.Text.Json;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.JSInterop;

namespace GFC.Mobile.Services;

/// <summary>
/// Offline-first shift reporting service.
/// All saves go to localStorage FIRST (instant, always succeeds),
/// then sync to server in background when online.
/// </summary>
public class MobileReportingService : IMobileReportingService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private readonly IConnectivityService _connectivity;

    private const string OutboxKey = "gfc_mobile_outbox";
    private const int MaxAttempts = 5;
    private const int MaxRetentionDays = 7;

    // Raised whenever the outbox count changes so UI badges can update
    public event Action? OutboxChanged;

    public int PendingCount { get; private set; }

    private readonly SemaphoreSlim _syncLock = new(1, 1);
    private readonly Timer _syncTimer;

    public MobileReportingService(HttpClient http, IJSRuntime js, IConnectivityService connectivity)
    {
        _http = http;
        _js = js;
        _connectivity = connectivity;

        // [SYNC HEARTBEAT] Check for trapped data every 30 seconds
        _syncTimer = new Timer(async _ => await SafeFlushAsync(), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30));
    }

    private async Task SafeFlushAsync()
    {
        if (!await _syncLock.WaitAsync(0)) return;
        try { await FlushOutboxAsync(); }
        finally { _syncLock.Release(); }
    }

    // ─── READ OPERATIONS (try server, fall back to nothing — reads don't go in outbox) ───

    public async Task<MobileShiftData> GetShiftReportDataAsync(DateTime date, string shiftType, bool isRental)
    {
        // 1. Instant Local Vault Read (Offline-First)
        var key = $"gfc_outbox_{date:yyyy-MM-dd}_{shiftType}";
        try {
            var json = await _js.InvokeAsync<string>("window.gfcGetAsync", key);
            if (!string.IsNullOrEmpty(json)) {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<MobileShiftData>(json, options) ?? new();
            }
        } catch { }

        // 2. Server Fetch (Only if online)
        try
        {
            if (!await _connectivity.GateAsync("GetShiftReportData"))
                return new MobileShiftData { Date = date, ShiftType = shiftType, IsRentalHall = isRental };

            var url = $"/api/mobile-reporting/data?date={date:yyyy-MM-dd}&shiftType={shiftType}&isRental={isRental}";
            var data = await _http.GetFromJsonAsync<MobileShiftData>(url);
            return data ?? new MobileShiftData { Date = date, ShiftType = shiftType, IsRentalHall = isRental };
        }
        catch { return new MobileShiftData { Date = date, ShiftType = shiftType, IsRentalHall = isRental }; }
    }

    public async Task<decimal> GetCarryoverCashAsync(DateTime date, string shiftType)
    {
        try
        {
            if (!await _connectivity.GateAsync("GetCarryoverCash")) return 1200;

            var url = $"/api/mobile-reporting/carryover?date={date:yyyy-MM-dd}&shiftType={shiftType}";
            return await _http.GetFromJsonAsync<decimal>(url);
        }
        catch { return 1200; }
    }

    public async Task<decimal> GetCumulativeBagDebtAsync(DateTime date)
    {
        try
        {
            if (!await _connectivity.GateAsync("GetBagDebt")) return 0;
            var url = $"/api/mobile-reporting/bag-debt?date={date:yyyy-MM-dd}";
            return await _http.GetFromJsonAsync<decimal>(url);
        }
        catch { return 0; }
    }

    public async Task<DailyShiftSummary> GetDailySummaryAsync(DateTime date)
    {
        try
        {
            if (!await _connectivity.GateAsync("GetDailySummary")) return new DailyShiftSummary();
            var url = $"/api/mobile-reporting/summary?date={date:yyyy-MM-dd}";
            return await _http.GetFromJsonAsync<DailyShiftSummary>(url) ?? new DailyShiftSummary();
        }
        catch { return new DailyShiftSummary(); }
    }

    public async Task<string> GetServerVersionAsync()
    {
        try { 
            if (!await _connectivity.GateAsync("GetVersion")) return "Offline";
            return await _http.GetStringAsync("/api/mobile-reporting/version"); 
        }
        catch { return "GFC Mobile Revision 2.1.51 (Settlement Hardening)"; }
    }

    public async Task<LotteryCommissionRate> GetLotteryRateAsync(int year)
    {
        try
        {
            if (!await _connectivity.GateAsync("GetRate")) 
                return new LotteryCommissionRate { Year = year };

            var url = $"/api/mobile-reporting/lottery-rate?year={year}";
            return await _http.GetFromJsonAsync<LotteryCommissionRate>(url) ?? new LotteryCommissionRate { Year = year };
        }
        catch { return new LotteryCommissionRate { Year = year }; }
    }

    // ─── WRITE OPERATIONS (outbox-first) ─────────────────────────────────────────────────

    public async Task<bool> SaveShiftReportAsync(MobileShiftData data, string username)
    {
        data.ModifiedBy = username;
        await EnqueueAsync("SaveShiftReport", data);

        // Non-blocking background flush — user doesn't wait
        _ = FlushOutboxAsync();

        return true; // Always succeeds locally
    }

    public async Task<bool> SubmitFinalReportAsync(MobileShiftData data, string username)
    {
        data.Status = "Submitted";
        data.ModifiedBy = username;
        await EnqueueAsync("SubmitShiftReport", data);

        if (_connectivity.IsOnline)
            _ = FlushOutboxAsync();

        return true;
    }

    // ─── OUTBOX ──────────────────────────────────────────────────────────────────────────

    private async Task EnqueueAsync(string type, MobileShiftData data)
    {
        var entry = new OutboxEntry
        {
            Id = Guid.NewGuid().ToString(),
            Type = type,
            Payload = JsonSerializer.Serialize(data),
            SavedAt = DateTime.UtcNow,
            Attempts = 0
        };

        var all = await LoadOutboxAsync();

        // Deduplicate: if a pending entry for the same date+shift already exists, replace it
        all.RemoveAll(e =>
        {
            try
            {
                var existing = JsonSerializer.Deserialize<MobileShiftData>(e.Payload);
                return existing?.Date.Date == data.Date.Date &&
                       existing?.ShiftType == data.ShiftType &&
                       e.Type == type;
            }
            catch { return false; }
        });

        all.Add(entry);
        await SaveOutboxAsync(all);
        PendingCount = all.Count;
        // Sync Hardening Phase - Revision 2.1.32
        OutboxChanged?.Invoke();
    }

    public async Task FlushOutboxAsync()
    {
        if (!_connectivity.IsOnline) return;

        // 1. Process Legacy List-based Outbox (Old Delivery Method)
        var all = await LoadOutboxAsync();
        if (all.Any()) {
            var remaining = new List<OutboxEntry>();
            foreach (var entry in all) {
                try {
                    var data = JsonSerializer.Deserialize<MobileShiftData>(entry.Payload);
                    if (data == null) continue;
                    var endpoint = entry.Type == "SubmitShiftReport" ? "/api/mobile-reporting/submit" : "/api/mobile-reporting/save";
                    var resp = await _http.PostAsJsonAsync($"{endpoint}?username={data.ModifiedBy}", data);
                    if (!resp.IsSuccessStatusCode) remaining.Add(entry);
                } catch { remaining.Add(entry); }
            }
            await SaveOutboxAsync(remaining);
        }

        // 2. [GLOBAL VAULT SWEEP] Process Individual-key Reports (Revision 2.1.30)
        try {
            var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            
            if (vaultItems.ValueKind == JsonValueKind.Array) {
                int count = vaultItems.GetArrayLength();
                if (count > 0) Console.WriteLine($"[SYNC TRACE] Found {count} potential items in vault.");
                
                foreach (var item in vaultItems.EnumerateArray()) {
                    try {
                        var key = item.GetProperty("key").GetString();
                        
                        if (key != null && key.StartsWith("gfc_outbox_")) {
                            Console.WriteLine($"[SYNC TRACE] Found pending report: {key}");
                            var dataElement = item.GetProperty("data");
                            
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            var data = JsonSerializer.Deserialize<MobileShiftData>(dataElement.GetRawText(), options);
                            
                            if (data != null) {
                                var user = string.IsNullOrEmpty(data.ModifiedBy) ? "System.Outbox" : data.ModifiedBy;
                                
                                // Determine endpoint based on data status
                                var isSubmit = string.Equals(data.Status, "Submitted", StringComparison.OrdinalIgnoreCase);
                                var endpoint = isSubmit ? "/api/mobile-reporting/submit" : "/api/mobile-reporting/save";
                                
                                Console.WriteLine($"[SYNC TRACE] Attempting delivery for {data.Date:yyyy-MM-dd} {data.ShiftType} as {user} to {endpoint}");
                                
                                var resp = await _http.PostAsJsonAsync($"{endpoint}?username={user}", data);
                                
                                if (resp.IsSuccessStatusCode) {
                                    Console.WriteLine($"[SYNC TRACE] ✓ SUCCESS: {key} delivered.");
                                    await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                    OutboxChanged?.Invoke();
                                } else {
                                    Console.WriteLine($"[SYNC TRACE] ✗ FAILED: {key} (Status: {resp.StatusCode})");
                                    if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                                        Console.WriteLine("[SYNC TRACE] !!! Unauthorized. Stopping sync loop.");
                                        return; // Stop processing the rest of the vault if auth is dead
                                    }
                                }
                            } else {
                                Console.WriteLine($"[SYNC TRACE] ! SKIP: Could not deserialize data for {key}");
                            }
                        }
                    } catch (Exception loopEx) {
                        Console.WriteLine($"[SYNC TRACE] Error processing individual vault item: {loopEx.Message}");
                    }
                }
            }
        } catch (Exception ex) {
            Console.WriteLine($"[SYNC TRACE] !!! CRITICAL ERROR during vault sweep: {ex.Message}");
        }

        // 3. Update Pending Count
        var legacyCount = (await LoadOutboxAsync()).Count;
        var vaultItemsRaw = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
        int vaultCount = 0;
        if (vaultItemsRaw.ValueKind == JsonValueKind.Array) {
            foreach (var item in vaultItemsRaw.EnumerateArray()) {
                var k = item.GetProperty("key").GetString();
                if (k != null && k.StartsWith("gfc_outbox_")) vaultCount++;
            }
        }
        
        PendingCount = legacyCount + vaultCount;
        OutboxChanged?.Invoke();
    }

    public async Task<int> GetPendingCountAsync()
    {
        var all = await LoadOutboxAsync();
        PendingCount = all.Count;
        return PendingCount;
    }

    private async Task<List<OutboxEntry>> LoadOutboxAsync()
    {
        try
        {
            // [VAULT FIX] Read outbox from the persistent IndexedDB vault, not localStorage
            var json = await _js.InvokeAsync<string>("window.gfcGetAsync", OutboxKey);
            return string.IsNullOrEmpty(json)
                ? new List<OutboxEntry>()
                : JsonSerializer.Deserialize<List<OutboxEntry>>(json) ?? new List<OutboxEntry>();
        }
        catch { return new List<OutboxEntry>(); }
    }

    private async Task SaveOutboxAsync(List<OutboxEntry> entries)
    {
        try
        {
            // [VAULT FIX] Save to IndexedDB using the flattened handshake bridge
            await _js.InvokeVoidAsync("window.gfcSetAsync", OutboxKey, entries);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Outbox] Failed to save: {ex.Message}");
        }
    }

    private class OutboxEntry
    {
        public string Id { get; set; } = "";
        public string Type { get; set; } = "";
        public string Payload { get; set; } = "";
        public DateTime SavedAt { get; set; }
        public DateTime? SyncedAt { get; set; }
        public int Attempts { get; set; }
    }
}
