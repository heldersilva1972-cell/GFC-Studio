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
    public DateTime? LastSyncTime { get; private set; }
    public string? LastSyncStatus { get; private set; }

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
            var localData = await _js.InvokeAsync<MobileShiftData>("window.gfcGetAsync", key);
            if (localData != null) {
                localData.ExistingEntryFound = true;
                return localData;
            }
        } catch (Exception ex) {
            Console.WriteLine($"[GFC VAULT] READ ERROR for {key}: {ex.Message}");
        }

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
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var serverData = await _http.GetFromJsonAsync<MobileShiftData>(url, cts.Token);
            
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
            var data = await _js.InvokeAsync<MobileShiftData>("window.gfcGetAsync", key);
            if (data?.Status == "Submitted") return true;
        } catch { }

        // 2. Fallback to Server check
        try {
            if (!await _connectivity.GateAsync("IsShiftSubmitted")) return false;
            var url = $"/api/mobile-reporting/is-submitted?date={date:yyyy-MM-dd}&shiftType={shiftType}&isRental={isRental}";
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            return await _http.GetFromJsonAsync<bool>(url, cts.Token);
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
            var prevData = await _js.InvokeAsync<MobileShiftData>("window.gfcGetAsync", outboxKey);
            if (prevData == null) prevData = await _js.InvokeAsync<MobileShiftData>("window.gfcGetAsync", draftKey);

            if (prevData != null) {
                // If it's a Day shift looking for previous day's Night shift,
                // we only care about the carryover machine totals (Cumulative reading at EOD)
                if (data.ShiftType == "Day")
                {
                    data.PrevDaySales = prevData.LottoSales;
                    data.PrevDayCashes = prevData.LottoCashes;
                    data.PrevDayTickets = prevData.LottoInstantTickets;
                    data.PrevDayNetDue = prevData.LottoNetDue;
                    
                    // The Day shift's opening cash is the Night shift's ending state
                    decimal envelope = prevData.EnvelopeAmount ?? 0;
                    decimal refill = prevData.BagRefillAmount ?? 0;
                    decimal counted = prevData.LottoCashCounted ?? 0;
                    data.LottoOpeningCash = counted - envelope - refill;
                    if (data.LottoOpeningCash <= 0) data.LottoOpeningCash = 1200;
                }
                else if (data.ShiftType == "Night")
                {
                    data.PrevDaySales = prevData.LottoSales;
                    data.PrevDayCashes = prevData.LottoCashes;
                    data.PrevDayTickets = prevData.LottoInstantTickets;
                    data.PrevDayNetDue = prevData.LottoNetDue;

                    // [POS PARITY] Carry over Ending Cash minus settlements
                    decimal envelope = prevData.EnvelopeAmount ?? 0;
                    decimal refill = prevData.BagRefillAmount ?? 0;
                    decimal counted = prevData.LottoCashCounted ?? 0;
                    
                    data.LottoOpeningCash = counted - envelope - refill;
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
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            return await _http.GetFromJsonAsync<decimal>(url, cts.Token);
        }
        catch { return 1200; }
    }

    public async Task<decimal> GetCumulativeBagDebtAsync(DateTime date)
    {
        try
        {
            if (!await _connectivity.GateAsync("GetBagDebt")) return 0;
            var url = $"/api/mobile-reporting/bag-debt?date={date:yyyy-MM-dd}";
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            return await _http.GetFromJsonAsync<decimal>(url, cts.Token);
        }
        catch { return 0; }
    }

    public async Task<DailyShiftSummary> GetDailySummaryAsync(DateTime date)
    {
        try
        {
            if (!await _connectivity.GateAsync("GetDailySummary")) return new DailyShiftSummary();
            var url = $"/api/mobile-reporting/summary?date={date:yyyy-MM-dd}";
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            return await _http.GetFromJsonAsync<DailyShiftSummary>(url, cts.Token) ?? new DailyShiftSummary();
        }
        catch { return new DailyShiftSummary(); }
    }

    public async Task<string> GetServerVersionAsync()
    {
        try { 
            if (!await _connectivity.GateAsync("GetVersion")) return "Offline";
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var timestamp = DateTime.UtcNow.Ticks;
            return await _http.GetStringAsync($"/api/mobile-reporting/version?t={timestamp}", cts.Token); 
        }
        catch { return "Offline"; }
    }

    // ─── BINGO OPERATIONS ───

    public async Task<List<BingoSheetDefinition>> GetBingoProgramAsync()
    {
        const string cacheKey = "gfc_bingo_program_cache";
        
        // 1. Try server first (Only if online)
        try
        {
            if (await _connectivity.GateAsync("GetBingoProgram"))
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var program = await _http.GetFromJsonAsync<List<BingoSheetDefinition>>("/api/bingo/program", cts.Token);
                if (program != null && program.Any())
                {
                    // Update cache
                    await _js.InvokeVoidAsync("window.gfcSetAsync", cacheKey, program);
                    return program;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[BINGO] Server fetch failed: {ex.Message}");
        }

        // 2. Fallback to Local Cache
        try
        {
            var cached = await _js.InvokeAsync<List<BingoSheetDefinition>>("window.gfcGetAsync", cacheKey);
            if (cached != null) return cached;
        }
        catch { }

        return new List<BingoSheetDefinition>(); // Empty if all fails
    }
    public async Task<List<BingoAdmissionDefinition>> GetBingoAdmissionsAsync()
    {
        const string cacheKey = "gfc_bingo_admissions_cache";
        
        try
        {
            if (await _connectivity.GateAsync("GetBingoAdmissions"))
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var admissions = await _http.GetFromJsonAsync<List<BingoAdmissionDefinition>>("/api/bingo/admissions", cts.Token);
                if (admissions != null && admissions.Any())
                {
                    await _js.InvokeVoidAsync("window.gfcSetAsync", cacheKey, admissions);
                    return admissions;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[BINGO] Server fetch admissions failed: {ex.Message}");
        }

        try
        {
            var cached = await _js.InvokeAsync<List<BingoAdmissionDefinition>>("window.gfcGetAsync", cacheKey);
            if (cached != null) return cached;
        }
        catch { }

        return new List<BingoAdmissionDefinition>();
    }

    public async Task<BingoSettingsDto> GetBingoSettingsAsync()
    {
        const string cacheKey = "gfc_bingo_settings_cache";
        
        try
        {
            if (await _connectivity.GateAsync("GetBingoSettings"))
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var settings = await _http.GetFromJsonAsync<BingoSettingsDto>("/api/bingo/settings", cts.Token);
                if (settings != null)
                {
                    // [FORCE REFRESH] Always overwrite cache with fresh server data
                    await _js.InvokeVoidAsync("window.gfcSetAsync", cacheKey, settings);
                    return settings;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[BINGO] Server fetch settings failed: {ex.Message}");
        }

        try
        {
            var cached = await _js.InvokeAsync<BingoSettingsDto>("window.gfcGetAsync", cacheKey);
            if (cached != null) return cached;
        }
        catch { }

        return new BingoSettingsDto(); // Default $15/$1
    }

    public async Task<bool> SubmitBingoSessionAsync(BingoSession session, string username)
    {
        session.CreatedBy = username;
        session.Status = "Submitted";

        // Save to Vault (Predictable Key for deduplication)
        var vaultKey = $"gfc_bingo_outbox_{session.SessionDate:yyyy-MM-dd}";
        await _js.InvokeVoidAsync("window.gfcSetAsync", vaultKey, session);

        _ = RefreshPendingCountAsync();

        if (_connectivity.IsOnline)
            _ = FlushOutboxAsync();

        return true;
    }

    public async Task<BingoSession?> GetBingoSessionByDateAsync(DateTime date)
    {
        // 1. Try local vault/outbox first
        var vaultKey = $"gfc_bingo_outbox_{date:yyyy-MM-dd}";
        try
        {
            var cached = await _js.InvokeAsync<BingoSession>("window.gfcGetAsync", vaultKey);
            if (cached != null) return cached;
        }
        catch { }

        // 2. Query Server
        try
        {
            if (await _connectivity.GateAsync("GetBingoSessionByDate"))
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var session = await _http.GetFromJsonAsync<BingoSession>($"/api/bingo/session-by-date?date={date:yyyy-MM-dd}", cts.Token);
                return session;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[BINGO] GetBingoSessionByDate server fetch failed: {ex.Message}");
        }

        return null;
    }

    public async Task<LotteryCommissionRate> GetLotteryRateAsync(int year)
    {
        try
        {
            if (!await _connectivity.GateAsync("GetRate")) 
                return new LotteryCommissionRate { Year = year };

            var url = $"/api/mobile-reporting/lottery-rate?year={year}";
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            return await _http.GetFromJsonAsync<LotteryCommissionRate>(url, cts.Token) ?? new LotteryCommissionRate { Year = year };
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
                    if (k != null && (k.StartsWith("gfc_outbox_") || k.StartsWith("gfc_bingo_outbox_"))) vaultCount++;
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

        // 1. Process Legacy List-based Outbox (Now in IndexedDB)
        var all = await LoadOutboxAsync();
        
        if (all.Any()) {
            Console.WriteLine($"[SYNC TRACE] Processing {all.Count} legacy items...");
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
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                    var resp = await _http.PostAsJsonAsync($"{endpoint}?username={user}", data, cts.Token);
                    
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
                    remaining.Add(entry); 
                }
            }
            await SaveOutboxAsync(remaining);
        }

        // 2. [GLOBAL VAULT SWEEP] Process Individual-key Reports
        try {
            var vaultItemsRaw = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            
            if (vaultItemsRaw.ValueKind == JsonValueKind.Array) {
                foreach (var item in vaultItemsRaw.EnumerateArray()) {
                    try {
                        var key = item.GetProperty("key").GetString();
                        if (key != null && (key.StartsWith("gfc_outbox_") || key.StartsWith("gfc_bingo_outbox_"))) {
                            var dataElement = item.GetProperty("data");
                            var isBingo = key.StartsWith("gfc_bingo_outbox_");
                            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                            
                            if (isBingo) {
                                // [COMPATIBILITY HACK] Handle rename from LotteryPercent/ClubPercent to Percentage
                                var json = dataElement.GetRawText();
                                if (json.Contains("\"LotteryPercent\":") || json.Contains("\"ClubPercent\":")) {
                                    json = json.Replace("\"LotteryPercent\":", "\"LotteryPercentage\":")
                                               .Replace("\"ClubPercent\":", "\"ClubPercentage\":");
                                }

                                var bingoData = JsonSerializer.Deserialize<BingoSession>(json, options);
                                if (bingoData != null) {
                                    Console.WriteLine($"[SYNC TRACE] Delivering Bingo session for {bingoData.SessionDate:yyyy-MM-dd}...");
                                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                                    var resp = await _http.PostAsJsonAsync($"/api/bingo/session", bingoData, cts.Token);
                                    if (resp.IsSuccessStatusCode) {
                                        await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                        await RefreshPendingCountAsync();
                                        LastSyncStatus = "Success";
                                        Console.WriteLine($"[SYNC TRACE] ✓ Bingo session for {bingoData.SessionDate:yyyy-MM-dd} delivered.");
                                    } else {
                                        var err = await resp.Content.ReadAsStringAsync();
                                        LastSyncStatus = $"Error: {resp.StatusCode} - {err}";
                                        Console.WriteLine($"[SYNC TRACE] ✗ Bingo delivery failed: {resp.StatusCode} - {err}");
                                    }
                                }
                            } else {
                                var data = JsonSerializer.Deserialize<MobileShiftData>(dataElement.GetRawText(), options);
                                if (data != null) {
                                    var user = string.IsNullOrEmpty(data.ModifiedBy) ? "System.Outbox" : data.ModifiedBy;
                                    var isSubmit = string.Equals(data.Status, "Submitted", StringComparison.OrdinalIgnoreCase);
                                    var endpoint = isSubmit ? "/api/mobile-reporting/submit" : "/api/mobile-reporting/save";
                                    
                                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                                    var resp = await _http.PostAsJsonAsync($"{endpoint}?username={user}", data, cts.Token);
                                    
                                    if (resp.IsSuccessStatusCode) {
                                        await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                        await RefreshPendingCountAsync();
                                    }
                                }
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
        
        LastSyncTime = DateTime.Now;
        if (string.IsNullOrEmpty(LastSyncStatus)) LastSyncStatus = "Success";
        OutboxChanged?.Invoke();
    }

    public async Task PurgeOutboxAsync()
    {
        try {
            await _js.InvokeVoidAsync("localStorage.removeItem", OutboxKey);
            
            var vaultItemsRaw = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            if (vaultItemsRaw.ValueKind == JsonValueKind.Array) {
                foreach (var item in vaultItemsRaw.EnumerateArray()) {
                    var k = item.GetProperty("key").GetString();
                    if (k != null && (k.StartsWith("gfc_outbox_") || k.StartsWith("gfc_bingo_outbox_") || k == OutboxKey)) {
                        await _js.InvokeVoidAsync("window.gfcRemoveAsync", k);
                    }
                }
            }
            
            await RefreshPendingCountAsync();
            Console.WriteLine("[SYNC] Outbox purged manually by user.");
        } catch (Exception ex) {
            Console.WriteLine($"[SYNC] Purge failed: {ex.Message}");
        }
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
                    if (k != null && (k.StartsWith("gfc_outbox_") || k.StartsWith("gfc_bingo_outbox_"))) vaultCount++;
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
            // [VAULT FIX] Read outbox from the persistent IndexedDB vault.
            // Since gfcGetAsync returns the object directly, we let JS Interop handle the mapping.
            var entries = await _js.InvokeAsync<List<OutboxEntry>>("window.gfcGetAsync", OutboxKey);
            return entries ?? new List<OutboxEntry>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Outbox] Load failed: {ex.Message}");
            return new List<OutboxEntry>(); 
        }
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
