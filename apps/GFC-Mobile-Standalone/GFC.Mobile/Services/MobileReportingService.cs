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

        // [POS PARITY] Trigger immediate sync when network returns
        _connectivity.ConnectivityChanged += async isOnline => {
            if (isOnline) {
                Console.WriteLine("[SYNC TRACE] Network restored - forcing immediate vault sweep...");
                await SafeFlushAsync();
            }
        };
        
        // Initial count
        _ = RefreshPendingCountAsync();
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
        var key = isRental ? $"gfc_outbox_{date:yyyy-MM-dd}_Hall" : $"gfc_outbox_{date:yyyy-MM-dd}_{shiftType}";
        try {
            var json = await _js.InvokeAsync<string>("window.gfcGetAsync", key);
            if (!string.IsNullOrEmpty(json)) {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var localData = JsonSerializer.Deserialize<MobileShiftData>(json, options);
                if (localData != null) {
                    localData.ExistingEntryFound = true; // Mark as existing if found in outbox
                    return localData;
                }
            }
        } catch { }

        // 2. Server Fetch (Only if online)
        try
        {
            if (!await _connectivity.GateAsync("GetShiftReportData")) {
                // [OFFLINE HARDENING] Try to get previous shift's ending totals from the local vault 
                // so carryover math works without the server.
                var data = new MobileShiftData { Date = date, ShiftType = shiftType, IsRentalHall = isRental };
                await PopulatePreviousShiftDataOffline(data);
                return data;
            }

            var url = $"/api/mobile-reporting/data?date={date:yyyy-MM-dd}&shiftType={shiftType}&isRental={isRental}";
            var serverData = await _http.GetFromJsonAsync<MobileShiftData>(url);
            
            if (serverData == null) serverData = new MobileShiftData { Date = date, ShiftType = shiftType, IsRentalHall = isRental };

            // [CARRYOVER BRIDGE] Even if online, if the server doesn't have the previous shift data (Previous shift not synced yet)
            // we check the local vault to bridge the gap and ensure math works.
            if (serverData.PrevDaySales == null || serverData.PrevDaySales == 0)
            {
                await PopulatePreviousShiftDataOffline(serverData);
            }
            
            return serverData;
        }
        catch { 
            var data = new MobileShiftData { Date = date, ShiftType = shiftType, IsRentalHall = isRental };
            await PopulatePreviousShiftDataOffline(data);
            return data; 
        }
    }

    public async Task<bool> IsShiftSubmittedAsync(DateTime date, string shiftType, bool isRental)
    {
        // 1. Check local vault first (Offline-First)
        var key = isRental ? $"gfc_outbox_{date:yyyy-MM-dd}_Hall" : $"gfc_outbox_{date:yyyy-MM-dd}_{shiftType}";
        try {
            var json = await _js.InvokeAsync<string>("window.gfcGetAsync", key);
            if (!string.IsNullOrEmpty(json)) {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<MobileShiftData>(json, options);
                if (data?.Status == "Submitted") return true;
            }
        } catch { }

        // 2. Fallback to Server check
        try {
            if (!await _connectivity.GateAsync("IsShiftSubmitted")) return false;
            var url = $"/api/mobile-reporting/is-submitted?date={date:yyyy-MM-dd}&shiftType={shiftType}&isRental={isRental}";
            return await _http.GetFromJsonAsync<bool>(url);
        } catch { return false; }
    }

    private async Task PopulatePreviousShiftDataOffline(MobileShiftData data)
    {
        string prevKey = "";
        if (data.ShiftType == "Night")
        {
            prevKey = $"gfc_outbox_{data.Date:yyyy-MM-dd}_Day";
            var draftKey = $"gfc_shift_draft_{data.Date:yyyyMMdd}_Day";
            await TryApplyPreviousData(data, prevKey, draftKey);
        }
        else if (data.ShiftType == "Day")
        {
            // Day shift looks at yesterday's Night shift
            var yesterday = data.Date.AddDays(-1);
            prevKey = $"gfc_outbox_{yesterday:yyyy-MM-dd}_Night";
            var draftKey = $"gfc_shift_draft_{yesterday:yyyyMMdd}_Night";
            await TryApplyPreviousData(data, prevKey, draftKey);
        }
    }

    private async Task TryApplyPreviousData(MobileShiftData data, string outboxKey, string draftKey)
    {
        try {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string? json = await _js.InvokeAsync<string>("window.gfcGetAsync", outboxKey);
            if (string.IsNullOrEmpty(json)) json = await _js.InvokeAsync<string>("window.gfcGetAsync", draftKey);

            if (!string.IsNullOrEmpty(json)) {
                var prevData = JsonSerializer.Deserialize<MobileShiftData>(json, options);
                if (prevData != null) {
                    data.PrevDaySales = prevData.LottoSales;
                    data.PrevDayCashes = prevData.LottoCashes;
                    data.PrevDayTickets = prevData.LottoInstantTickets;
                    data.PrevDayNetDue = prevData.LottoNetDue;
                    
                    // [POS PARITY] Carry over Ending Cash minus settlements
                    decimal envelope = prevData.EnvelopeAmount ?? 0;
                    decimal refill = prevData.BagRefillAmount ?? 0;
                    decimal counted = prevData.LottoCashCounted ?? 0;
                    
                    data.LottoOpeningCash = counted - envelope - refill;
                    
                    // Fallback to 1200 if math results in 0 or less
                    if (data.LottoOpeningCash <= 0) data.LottoOpeningCash = 1200;
                }
            }
        } catch { }
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
        await RefreshPendingCountAsync();
    }

    private async Task RefreshPendingCountAsync()
    {
        var legacyCount = (await LoadOutboxAsync()).Count;
        int vaultCount = 0;
        try {
            var vaultItemsRaw = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            if (vaultItemsRaw.ValueKind == JsonValueKind.Array) {
                foreach (var item in vaultItemsRaw.EnumerateArray()) {
                    var k = item.GetProperty("key").GetString();
                    if (k != null && k.StartsWith("gfc_outbox_")) vaultCount++;
                }
            }
        } catch { }
        
        PendingCount = legacyCount + vaultCount;
        OutboxChanged?.Invoke();
    }

    public async Task FlushOutboxAsync()
    {
        if (!_connectivity.IsOnline) {
            Console.WriteLine("[SYNC TRACE] Skipping flush: Service is Offline.");
            return;
        }

        // 0. MIGRATION: Check for legacy localStorage outbox and move to IndexedDB
        try {
            var legacyJson = await _js.InvokeAsync<string>("localStorage.getItem", OutboxKey);
            if (!string.IsNullOrEmpty(legacyJson)) {
                Console.WriteLine("[SYNC TRACE] Found legacy localStorage outbox. Migrating to Vault...");
                var legacyItems = JsonSerializer.Deserialize<List<OutboxEntry>>(legacyJson);
                if (legacyItems != null && legacyItems.Any()) {
                    var currentOutbox = await LoadOutboxAsync();
                    currentOutbox.AddRange(legacyItems);
                    await SaveOutboxAsync(currentOutbox);
                    await _js.InvokeVoidAsync("localStorage.removeItem", OutboxKey);
                    Console.WriteLine($"[SYNC TRACE] Successfully migrated {legacyItems.Count} items.");
                    await RefreshPendingCountAsync();
                }
            }
        } catch (Exception migEx) {
            Console.WriteLine($"[SYNC TRACE] Migration error: {migEx.Message}");
        }

        Console.WriteLine($"[SYNC TRACE] Starting Flush Sweep. Server: {_http.BaseAddress}");

        // 1. Process Legacy List-based Outbox (Now in IndexedDB)
        var legacyJsonRaw = await _js.InvokeAsync<string>("window.gfcGetAsync", OutboxKey);
        // [ZOMBIE PURGE] If the outbox contains empty shells, clear them
        if (!string.IsNullOrEmpty(legacyJsonRaw) && legacyJsonRaw.Contains("\"id\":\"\",\"type\":\"\",\"payload\":\"\"")) {
            Console.WriteLine("[SYNC TRACE] !!! CORRUPT ZOMBIE ENTRIES DETECTED. Purging legacy outbox...");
            await _js.InvokeVoidAsync("window.gfcRemoveAsync", OutboxKey);
            await RefreshPendingCountAsync();
            return;
        }

        var all = await LoadOutboxAsync();
        if (all.Any()) {
            Console.WriteLine($"[SYNC TRACE] Processing {all.Count} legacy items...");
            var first = all.First();
            Console.WriteLine($"[SYNC TRACE] DEBUG: First item Type='{first.Type}', PayloadLength={first.Payload?.Length ?? -1}");
            var remaining = new List<OutboxEntry>();
            foreach (var entry in all) {
                try {
                    var data = JsonSerializer.Deserialize<MobileShiftData>(entry.Payload);
                    if (data == null) {
                        Console.WriteLine($"[SYNC TRACE] ! ERR: Legacy item {entry.Id} payload is null or invalid.");
                        continue;
                    }
                    var user = string.IsNullOrEmpty(data.ModifiedBy) ? "System.Outbox" : data.ModifiedBy;
                    var endpoint = entry.Type == "SubmitShiftReport" ? "/api/mobile-reporting/submit" : "/api/mobile-reporting/save";
                    
                    Console.WriteLine($"[SYNC TRACE] Delivering legacy {entry.Type} for {data.Date:yyyy-MM-dd} as {user}...");
                    var resp = await _http.PostAsJsonAsync($"{endpoint}?username={user}", data);
                    
                    if (resp.IsSuccessStatusCode) {
                        Console.WriteLine($"[SYNC TRACE] ✓ Legacy {entry.Id} delivered.");
                        await RefreshPendingCountAsync();
                    } else {
                        var err = await resp.Content.ReadAsStringAsync();
                        Console.WriteLine($"[SYNC TRACE] ✗ Legacy delivery failed: {resp.StatusCode} - {err}");
                        remaining.Add(entry);
                    }
                } catch (Exception ex) { 
                    Console.WriteLine($"[SYNC TRACE] !!! Legacy processing error for {entry.Id}: {ex.Message}");
                    Console.WriteLine($"[SYNC TRACE] RAW PAYLOAD (first 200 chars): {(entry.Payload?.Length > 200 ? entry.Payload.Substring(0, 200) : entry.Payload)}");
                    remaining.Add(entry); 
                }
            }
            await SaveOutboxAsync(remaining);
        } else {
            Console.WriteLine("[SYNC TRACE] No legacy items in queue.");
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
                        Console.WriteLine($"[SYNC TRACE] Inspecting vault key: {key}");
                        
                        if (key != null && key.StartsWith("gfc_outbox_")) {
                            Console.WriteLine($"[SYNC TRACE] FOUND MATCH: {key}");
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
                                    await RefreshPendingCountAsync();
                                } else {
                                    var errorBody = await resp.Content.ReadAsStringAsync();
                                    Console.WriteLine($"[SYNC TRACE] ✗ FAILED: {key} (Status: {resp.StatusCode}, Error: {errorBody})");
                                    if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
                                        Console.WriteLine("[SYNC TRACE] !!! Unauthorized. Stopping sync loop.");
                                        return; // Stop processing the rest of the vault if auth is dead
                                    }
                                }
                            } else {
                                Console.WriteLine($"[SYNC TRACE] ! SKIP: Could not deserialize data for {key}");
                                var raw = dataElement.GetRawText();
                                Console.WriteLine($"[SYNC TRACE] RAW DATA (first 200 chars): {(raw.Length > 200 ? raw.Substring(0, 200) : raw)}");
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

        // 3. Final count update
        await RefreshPendingCountAsync();
    }

    public async Task<int> GetPendingCountAsync()
    {
        var legacyCount = (await LoadOutboxAsync()).Count;
        int vaultCount = 0;
        try {
            var vaultItemsRaw = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            if (vaultItemsRaw.ValueKind == JsonValueKind.Array) {
                foreach (var item in vaultItemsRaw.EnumerateArray()) {
                    var k = item.GetProperty("key").GetString();
                    if (k != null && k.StartsWith("gfc_outbox_")) vaultCount++;
                }
            }
        } catch { }
        
        PendingCount = legacyCount + vaultCount;
        return PendingCount;
    }

    private async Task<List<OutboxEntry>> LoadOutboxAsync()
    {
        try
        {
            // [VAULT FIX] Read outbox from the persistent IndexedDB vault, not localStorage
            var json = await _js.InvokeAsync<string>("window.gfcGetAsync", OutboxKey);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return string.IsNullOrEmpty(json)
                ? new List<OutboxEntry>()
                : JsonSerializer.Deserialize<List<OutboxEntry>>(json, options) ?? new List<OutboxEntry>();
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
