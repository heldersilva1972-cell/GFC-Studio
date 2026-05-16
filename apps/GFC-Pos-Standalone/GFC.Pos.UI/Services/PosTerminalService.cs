using System.Net.Http.Json;
using GFC.Core.DTOs;
using Microsoft.JSInterop;
using System.Text.Json;

namespace GFC.Pos.UI.Services;

/// <summary>
/// Offline-first POS terminal service.
/// Saves go to localStorage FIRST (instant). 
/// Auto-flushes both sales and Z-reports when connectivity is restored.
/// </summary>
public class PosTerminalService : IPosTerminalService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private readonly ConnectivityService _connectivity;

    private const string VaultPrefixSales = "gfc_pos_vault_sale_";
    private const string VaultPrefixZ     = "gfc_pos_vault_z_";
    private const string ShiftLogPrefix   = "gfc_shift_log_";
    private const string CachedMenuKey      = "gfc_pos_cached_menu";
    private const string AuthorizedUsersKey = "gfc_pos_authorized_users";
    private const int MaxAttempts           = 5;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public event Action? OutboxChanged;

    public int PendingSalesCount  { get; private set; }
    public int PendingZCount      { get; private set; }
    public int TotalPendingCount  => PendingSalesCount + PendingZCount;
    public DateTime? LastSynced   { get; private set; }

    // ─── LOCAL SHIFT DATABASE (PROPER ARCHITECTURE) ───────────────────────────────────

    public async Task AddSaleToShiftAsync(PosSaleDto sale)
    {
        var key = $"{ShiftLogPrefix}{sale.Id}";
        await _js.InvokeVoidAsync("window.gfcSetAsync", key, sale);
    }

    public async Task VoidSaleAsync(Guid saleId, string reason)
    {
        var key = $"{ShiftLogPrefix}{saleId}";
        var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
        if (vaultItems.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in vaultItems.EnumerateArray())
            {
                var k = item.GetProperty("key").GetString();
                if (k == key)
                {
                    var sale = JsonSerializer.Deserialize<PosSaleDto>(item.GetProperty("data").GetRawText(), _jsonOptions);
                    if (sale != null)
                    {
                        sale.IsVoided = true;
                        sale.AdjustmentReason = reason;
                        await _js.InvokeVoidAsync("window.gfcSetAsync", key, sale);
                        
                        // Sync to outbox too
                        var vKey = $"{VaultPrefixSales}{saleId}";
                        await _js.InvokeVoidAsync("window.gfcSetAsync", vKey, sale);
                    }
                }
            }
        }
    }

    public async Task<ShiftAuditDto> GetShiftAuditAsync()
    {
        var audit = new ShiftAuditDto();
        try
        {
            var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            if (vaultItems.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in vaultItems.EnumerateArray())
                {
                    var key = item.GetProperty("key").GetString();
                    if (key != null && key.StartsWith(ShiftLogPrefix))
                    {
                        var data = JsonSerializer.Deserialize<PosSaleDto>(item.GetProperty("data").GetRawText(), _jsonOptions);
                        if (data != null)
                        {
                            if (data.IsVoided) 
                            {
                                audit.VoidedSales.Add(data);
                                continue; 
                            }

                            if (audit.LatestSale == null || data.Timestamp > audit.LatestSale.Timestamp)
                            {
                                audit.LatestSale = data;
                            }

                            List<GFC.Pos.UI.Pages.PosTerminal.ProductItem>? salesItems = null;
                            if (!string.IsNullOrEmpty(data.ItemsJson))
                            {
                                salesItems = JsonSerializer.Deserialize<List<GFC.Pos.UI.Pages.PosTerminal.ProductItem>>(data.ItemsJson, _jsonOptions);
                                if (salesItems != null)
                                {
                                    bool isDeposit = data.ItemsJson.Contains("TAB DEPOSIT:");
                                    
                                    // TRACK TOTALS
                                    if (!isDeposit)
                                    {
                                        // Gross Total is the sum of all FULL PRICE items (Price > 0)
                                        // Token Credits is the sum of all NEGATIVE items (Price < 0)
                                        audit.GrossTotal += salesItems.Where(i => i.Price > 0).Sum(i => i.Price * i.Quantity);
                                        audit.TokenCredits += salesItems.Where(i => i.Price < 0).Sum(i => Math.Abs(i.Price * i.Quantity));
                                    }

                                    if (data.PaymentType == "CASH") audit.CashTotal += data.TotalAmount;
                                }
                            }

                            // [BANQUET TRACKING]
                            if (data.ActiveEventId.HasValue)
                            {
                                var banquet = audit.Banquets.FirstOrDefault(b => b.ActiveEventId == data.ActiveEventId);
                                if (banquet == null)
                                {
                                    banquet = new BanquetShiftReportDto { ActiveEventId = data.ActiveEventId };
                                    audit.Banquets.Add(banquet);
                                }

                                var items = JsonSerializer.Deserialize<List<GFC.Pos.UI.Pages.PosTerminal.ProductItem>>(data.ItemsJson, _jsonOptions);
                                if (items != null)
                                {
                                    foreach (var i in items)
                                    {
                                        if (i.Name.StartsWith("TAB DEPOSIT:"))
                                        {
                                            banquet.Deposits.Add(i.Price);
                                            // Extract event name if not set
                                            if (string.IsNullOrEmpty(banquet.EventName))
                                                banquet.EventName = i.Name.Replace("TAB DEPOSIT: ", "");
                                        }
                                        else if (data.PaymentType == "TAB")
                                        {
                                            if (!banquet.ItemSummary.ContainsKey(i.Name)) banquet.ItemSummary[i.Name] = 0;
                                            banquet.ItemSummary[i.Name] += i.Quantity;
                                            banquet.TotalSpent += (i.Price * i.Quantity);
                                            
                                            // Set event name from the first item if not set (fallback)
                                            if (string.IsNullOrEmpty(banquet.EventName))
                                                banquet.EventName = "Active Banquet"; // Will be updated by deposit if found
                                        }
                                    }
                                }
                            }

                            if (salesItems != null)
                            {
                                foreach (var i in salesItems)
                                {
                                    if (!audit.ItemSummary.ContainsKey(i.Name)) audit.ItemSummary[i.Name] = 0;
                                    if (!audit.ItemTotals.ContainsKey(i.Name)) audit.ItemTotals[i.Name] = 0;
                                    
                                    audit.ItemSummary[i.Name] += i.Quantity;
                                    audit.ItemTotals[i.Name] += (i.Price * i.Quantity);

                                    // [SEPARATION] Track regular sales vs banquet sales
                                    if (data.ActiveEventId.HasValue)
                                    {
                                        var b = audit.Banquets.FirstOrDefault(x => x.ActiveEventId == data.ActiveEventId);
                                        if (b != null)
                                        {
                                            if (!b.ItemTotals.ContainsKey(i.Name)) b.ItemTotals[i.Name] = 0;
                                            b.ItemTotals[i.Name] += (i.Price * i.Quantity);
                                        }
                                    }
                                    else
                                    {
                                        if (!audit.RegularItemSummary.ContainsKey(i.Name)) audit.RegularItemSummary[i.Name] = 0;
                                        if (!audit.RegularItemTotals.ContainsKey(i.Name)) audit.RegularItemTotals[i.Name] = 0;
                                        
                                        audit.RegularItemSummary[i.Name] += i.Quantity;
                                        audit.RegularItemTotals[i.Name] += (i.Price * i.Quantity);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return audit;
    }

    public async Task ClearShiftAsync()
    {
        try
        {
            var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            if (vaultItems.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in vaultItems.EnumerateArray())
                {
                    var key = item.GetProperty("key").GetString();
                    if (key != null && key.StartsWith(ShiftLogPrefix))
                    {
                        await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                    }
                }
            }
        }
        catch { }
    }

    private readonly SemaphoreSlim _syncLock = new(1, 1);
    private readonly Timer _syncTimer;

    public PosTerminalService(HttpClient http, IJSRuntime js, ConnectivityService connectivity)
    {
        _http = http;
        _js = js;
        _connectivity = connectivity;

        // [SYNC HEARTBEAT] Pulse every 30 seconds to flush trapped data
        _syncTimer = new Timer(async _ => await SafeFlushAsync(), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30));

        _connectivity.ConnectivityChanged += async isOnline =>
        {
            if (isOnline)
            {
                Console.WriteLine("[SYNC] Server detected! Triggering immediate vault sweep...");
                _ = SafeFlushAsync();
            }
        };

        // Initialize API Base Address from Override if present
        _ = InitializeApiUrlAsync();
    }

    private async Task InitializeApiUrlAsync()
    {
        try
        {
            var overrideUrl = await _js.InvokeAsync<string>("localStorage.getItem", "gfc_api_url_override");
            if (!string.IsNullOrEmpty(overrideUrl))
            {
                _http.BaseAddress = new Uri(overrideUrl);
                Console.WriteLine($"[POS] API OVERRIDE ACTIVE: {_http.BaseAddress}");
            }
        }
        catch { }
    }

    private async Task SafeFlushAsync()
    {
        if (!await _syncLock.WaitAsync(0)) return;
        try { await FlushAllPendingAsync(); }
        finally { _syncLock.Release(); }
    }

    // ─── READ OPERATIONS ──────────────────────────────────────────────────────────────

    public async Task<bool> CheckConnectivityAsync() => await _connectivity.CheckServerReachableAsync();

    public async Task<string> GetServerVersionAsync()
    {
        try
        {
            // [ROBUST-UPDATE] Check the static version file in the deployment root.
            var timestamp = DateTime.UtcNow.Ticks;
            var request = new HttpRequestMessage(HttpMethod.Get, $"version.txt?v={timestamp}");
            
            // Force bypass of any server-side or browser caching
            request.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue { NoCache = true };
            request.Headers.IfModifiedSince = DateTimeOffset.MinValue;

            var resp = await _http.SendAsync(request);
            if (resp.IsSuccessStatusCode)
            {
                var content = await resp.Content.ReadAsStringAsync();
                return content?.Trim() ?? "Offline";
            }
            
            // [BRIDGE-FALLBACK] If version.json is missing, check the old API
            return await _http.GetStringAsync($"api/mobile-reporting/pos-version?t={timestamp}");
        }
        catch { return "Offline"; }
    }

    public async Task<PosMenuDto?> GetCachedMenuAsync()
    {
        try
        {
            var cached = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedMenuKey);
            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<PosMenuDto>(cached, _jsonOptions);
            }
        }
        catch { }
        return null;
    }

    public async Task<PosMenuDto> GetMenuAsync(bool force = false)
    {
        Console.WriteLine($"[PosTerminalService] GetMenuAsync: Starting load (force={force})...");
        
        // 1. [INSTANT-LOAD] Try Vault FIRST for immediate UI pop (unless forced)
        if (!force)
        {
            try
            {
                var cached = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedMenuKey);
                if (!string.IsNullOrEmpty(cached))
                {
                    var menu = JsonSerializer.Deserialize<PosMenuDto>(cached, _jsonOptions);
                    if (menu != null && (menu.Items.Any() || menu.Tokens.Any() || menu.Categories.Any()))
                    {
                        Console.WriteLine($"[PosTerminalService] GetMenuAsync: Cache loaded successfully ({menu.Items.Count} items).");
                        // Background refresh
                        _ = Task.Run(async () => await RefreshMenuCacheAsync());
                        return menu;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PosTerminalService] GetMenuAsync: Cache load failed: {ex.Message}");
            }
        }

        // 2. Fallback to Server
        Console.WriteLine("[PosTerminalService] GetMenuAsync: Fetching fresh menu from server...");
        return await RefreshMenuCacheAsync();
    }

    public async Task SaveMenuToVaultAsync(PosMenuDto menu)
    {
        try
        {
            await _js.InvokeVoidAsync("window.gfcSetAsync", CachedMenuKey, menu);
        }
        catch { }
    }

    private async Task<PosMenuDto> RefreshMenuCacheAsync()
    {
        try
        {
            if (await _connectivity.GateAsync("RefreshMenu"))
            {
                Console.WriteLine("[PosTerminalService] RefreshMenuCacheAsync: Server is reachable, fetching...");
                var menu = await _http.GetFromJsonAsync<PosMenuDto>("api/pos/menu");
                
                if (menu != null)
                {
                    Console.WriteLine($"[PosTerminalService] RefreshMenuCacheAsync: Server returned {menu.Items.Count} items, {menu.Tokens.Count} tokens.");
                    
                    // We only save to vault if there's actually something to show
                    if (menu.Items.Any() || menu.Tokens.Any())
                    {
                        await _js.InvokeVoidAsync("window.gfcSetAsync", CachedMenuKey, menu);
                        Console.WriteLine("[PosTerminalService] RefreshMenuCacheAsync: Vault updated.");
                    }
                    else
                    {
                        Console.WriteLine("[PosTerminalService] RefreshMenuCacheAsync: Server returned empty menu, NOT updating vault to prevent clearing local data.");
                    }
                    return menu;
                }
            }
            else
            {
                Console.WriteLine("[PosTerminalService] RefreshMenuCacheAsync: Server unreachable or Gate closed.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PosTerminalService] RefreshMenuCacheAsync: Fetch failed: {ex.Message}");
        }
        return new PosMenuDto();
    }


    public async Task<List<UserListItemDto>> GetAuthorizedUsersAsync()
    {
        // 1. [OFFLINE-FIRST] Try Vault FIRST
        try
        {
            var cached = await _js.InvokeAsync<string>("window.gfcGetAsync", AuthorizedUsersKey);
            if (!string.IsNullOrEmpty(cached))
            {
                var users = JsonSerializer.Deserialize<List<UserListItemDto>>(cached, _jsonOptions);
                if (users != null && users.Any())
                {
                    // Trigger background refresh
                    _ = Task.Run(async () => await RefreshAuthorizedUsersCacheAsync());
                    return users;
                }
            }
        }
        catch { }

        return await RefreshAuthorizedUsersCacheAsync();
    }

    private async Task<List<UserListItemDto>> RefreshAuthorizedUsersCacheAsync()
    {
        try
        {
            if (await _connectivity.GateAsync("RefreshUsers"))
            {
                var users = await _http.GetFromJsonAsync<List<UserListItemDto>>("api/pos/users");
                if (users != null && users.Any())
                {
                    await _js.InvokeVoidAsync("window.gfcSetAsync", AuthorizedUsersKey, users);
                    return users;
                }
            }
        }
        catch { }
        return new List<UserListItemDto>();
    }

    public async Task<PosSaleDto?> GetDartsRoundTodayAsync(string terminalName)
    {
        // 1. [OFFLINE-FIRST] Check local vault for today's round first
        try
        {
            var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            if (vaultItems.ValueKind == JsonValueKind.Array)
            {
                var today = DateTime.Today;
                foreach (var item in vaultItems.EnumerateArray())
                {
                    var key = item.GetProperty("key").GetString();
                    if (key != null && key.StartsWith(VaultPrefixSales))
                    {
                        var sale = JsonSerializer.Deserialize<PosSaleDto>(item.GetProperty("data").GetRawText(), _jsonOptions);
                        if (sale != null && sale.Timestamp.Date == today && sale.ItemsJson.Contains("(DARTS)"))
                        {
                            return sale;
                        }
                    }
                }
            }
        }
        catch { }

        // 2. Fallback to Server if not in vault
        try 
        { 
            if (!await _connectivity.GateAsync("GetDartsRound")) return null;
            return await _http.GetFromJsonAsync<PosSaleDto>($"api/pos/darts-check/{terminalName}"); 
        }
        catch { return null; }
    }

    public async Task<List<PosZReportDto>> GetZReportsAsync(string terminalName)
    {
        var reports = new List<PosZReportDto>();

        // 1. Try Server
        try 
        { 
            if (await _connectivity.GateAsync("GetZReports"))
            {
                var serverReports = await _http.GetFromJsonAsync<List<PosZReportDto>>($"api/pos/z-reports/{terminalName}"); 
                if (serverReports != null) reports.AddRange(serverReports);
            }
        }
        catch { }

        // 2. Merge from Vault (ensure we don't show duplicates if they just synced)
        try
        {
            var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            if (vaultItems.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in vaultItems.EnumerateArray())
                {
                    var key = item.GetProperty("key").GetString();
                    if (key != null && key.StartsWith(VaultPrefixZ))
                    {
                        var data = JsonSerializer.Deserialize<PosZReportDto>(item.GetProperty("data").GetRawText(), _jsonOptions);
                        if (data != null && !reports.Any(r => r.Id == data.Id))
                        {
                            // Mark as pending for UI
                            data.BartenderName += " (PENDING SYNC)";
                            reports.Add(data);
                        }
                    }
                }
            }
        }
        catch { }

        return reports.OrderByDescending(r => r.Timestamp).ToList();
    }

    public async Task<PosZReportDto?> GetZReportAsync(Guid id)
    {
        try 
        { 
            if (!await _connectivity.GateAsync("GetZReport")) return null;
            return await _http.GetFromJsonAsync<PosZReportDto>($"api/pos/z-report/{id}"); 
        }
        catch { return null; }
    }

    public async Task<DateTime> GetLastZTimeAsync(string terminalName)
    {
        try 
        { 
            if (!await _connectivity.GateAsync("GetLastZ")) return DateTime.Today;
            return await _http.GetFromJsonAsync<DateTime>($"api/pos/last-z/{terminalName}"); 
        }
        catch { return DateTime.Today; }
    }

    // ─── SAVE OPERATIONS (VAULT-FIRST) ───

    public async Task SaveSaleAsync(PosSaleDto sale)
    {
        if (sale.Id == Guid.Empty) sale.Id = Guid.NewGuid();

        // 1. Instant Atomic Vault Save
        var key = $"{VaultPrefixSales}{sale.Id}";
        await _js.InvokeVoidAsync("window.gfcSetAsync", key, sale);
        
        await GetTotalPendingAsync();
        OutboxChanged?.Invoke();

        // 2. Background Attempt
        _ = SafeFlushAsync();
    }

    public async Task SaveZReportAsync(PosZReportDto report)
    {
        if (report.Id == Guid.Empty) report.Id = Guid.NewGuid();

        var key = $"{VaultPrefixZ}{report.Id}";
        await _js.InvokeVoidAsync("window.gfcSetAsync", key, report);
        
        await GetTotalPendingAsync();
        OutboxChanged?.Invoke();

        _ = SafeFlushAsync();
    }

    // ─── SYNC ENGINE (VAULT SWEEP) ───

    public async Task FlushAllPendingAsync()
    {
        if (!await _connectivity.GateAsync("OutboxSweep")) return;

        try {
            var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            if (vaultItems.ValueKind != JsonValueKind.Array) return;

            foreach (var item in vaultItems.EnumerateArray()) {
                var key = item.GetProperty("key").GetString();
                if (key == null) continue;

                if (key.StartsWith(VaultPrefixSales)) {
                    var data = JsonSerializer.Deserialize<PosSaleDto>(item.GetProperty("data").GetRawText(), _jsonOptions);
                    if (data != null) {
                        try {
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                            var resp = await _http.PostAsJsonAsync("api/pos/sale", data, cts.Token);
                            if (resp.IsSuccessStatusCode) {
                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                LastSynced = DateTime.Now;
                            } else {
                                Console.WriteLine($"[SYNC] Failed to send sale {data.Id}: {resp.StatusCode}");
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"[SYNC] Error sending sale {data.Id}: {ex.Message}");
                        }
                    }
                }
                
                if (key.StartsWith(VaultPrefixZ)) {
                    var data = JsonSerializer.Deserialize<PosZReportDto>(item.GetProperty("data").GetRawText(), _jsonOptions);
                    if (data != null) {
                        try {
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                            var resp = await _http.PostAsJsonAsync("api/pos/z-report", data, cts.Token);
                            if (resp.IsSuccessStatusCode) {
                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                LastSynced = DateTime.Now;
                            } else {
                                Console.WriteLine($"[SYNC] Failed to send Z-Report: {resp.StatusCode}");
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"[SYNC] Error sending Z-Report: {ex.Message}");
                        }
                    }
                }
            }
        } catch { }

        await GetTotalPendingAsync();
        OutboxChanged?.Invoke();
    }

    public async Task<int> GetTotalPendingAsync()
    {
        try {
            var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            int sales = 0;
            int z = 0;

            if (vaultItems.ValueKind == JsonValueKind.Array) {
                foreach (var item in vaultItems.EnumerateArray()) {
                    var key = item.GetProperty("key").GetString();
                    if (key == null) continue;
                    if (key.StartsWith(VaultPrefixSales)) sales++;
                    if (key.StartsWith(VaultPrefixZ)) z++;
                }
            }

            PendingSalesCount = sales;
            PendingZCount = z;
            return TotalPendingCount;
        } catch { return 0; }
    }
}
