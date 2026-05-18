using System.Net.Http.Json;
using GFC.Core.DTOs;
using GFC.Core.Models;
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
    public event Action<PosMenuDto>? MenuRefreshed;

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
            var menu = await GetCachedMenuAsync();

            // Retrieve persistent event name/type caches from localStorage
            var cachedNamesJson = await _js.InvokeAsync<string>("localStorage.getItem", "gfc_event_names_cache");
            var cachedTypesJson = await _js.InvokeAsync<string>("localStorage.getItem", "gfc_event_types_cache");
            var cachedNames = new Dictionary<string, string>();
            var cachedTypes = new Dictionary<string, string>();
            try
            {
                if (!string.IsNullOrEmpty(cachedNamesJson))
                    cachedNames = JsonSerializer.Deserialize<Dictionary<string, string>>(cachedNamesJson, _jsonOptions) ?? new();
            }
            catch {}
            try
            {
                if (!string.IsNullOrEmpty(cachedTypesJson))
                    cachedTypes = JsonSerializer.Deserialize<Dictionary<string, string>>(cachedTypesJson, _jsonOptions) ?? new();
            }
            catch {}

            // Update caches with any active events currently in menu
            if (menu != null && menu.ActiveEvents != null)
            {
                bool cacheUpdated = false;
                foreach (var ev in menu.ActiveEvents)
                {
                    var idStr = ev.Id.ToString();
                    if (!cachedNames.ContainsKey(idStr) || cachedNames[idStr] != ev.Name)
                    {
                        cachedNames[idStr] = ev.Name;
                        cacheUpdated = true;
                    }
                    if (!cachedTypes.ContainsKey(idStr) || cachedTypes[idStr] != ev.Type.ToString())
                    {
                        cachedTypes[idStr] = ev.Type.ToString();
                        cacheUpdated = true;
                    }
                }
                if (cacheUpdated)
                {
                    await _js.InvokeVoidAsync("localStorage.setItem", "gfc_event_names_cache", JsonSerializer.Serialize(cachedNames));
                    await _js.InvokeVoidAsync("localStorage.setItem", "gfc_event_types_cache", JsonSerializer.Serialize(cachedTypes));
                }
            }
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
                                        // Gross Total is the sum of all FULL PRICE items (Price > 0) + their modifiers
                                        audit.GrossTotal += salesItems.Where(i => i.Price > 0).Sum(i => i.Price * i.Quantity);
                                        audit.GrossTotal += salesItems.Sum(i => i.Modifiers?.Where(m => m.Price > 0).Sum(m => m.Price * m.Quantity) ?? 0);
                                        
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
                                    banquet = new BanquetShiftReportDto 
                                    { 
                                        ActiveEventId = data.ActiveEventId,
                                        EventType = "RunningTab" // Default
                                    };
                                    if (menu != null && menu.ActiveEvents != null)
                                    {
                                        var activeEv = menu.ActiveEvents.FirstOrDefault(e => e.Id == data.ActiveEventId);
                                        if (activeEv != null)
                                        {
                                            banquet.EventType = activeEv.Type.ToString();
                                            banquet.EventName = activeEv.Name;
                                            
                                            // Seed deposits with the initial prepaid amount if this is a Prepaid event
                                            if (activeEv.Type.ToString() == "PrePaid" && activeEv.InitialAmount > 0)
                                            {
                                                banquet.Deposits.Add(activeEv.InitialAmount);
                                            }
                                        }
                                    }

                                    // If we couldn't resolve from the active events (closed event), try our local persistent cache
                                    if (string.IsNullOrEmpty(banquet.EventName) && data.ActiveEventId.HasValue)
                                    {
                                        var idStr = data.ActiveEventId.Value.ToString();
                                        if (cachedNames.ContainsKey(idStr))
                                        {
                                            banquet.EventName = cachedNames[idStr];
                                        }
                                        if (cachedTypes.ContainsKey(idStr))
                                        {
                                            banquet.EventType = cachedTypes[idStr];
                                        }
                                    }
                                    audit.Banquets.Add(banquet);
                                }

                                var items = JsonSerializer.Deserialize<List<GFC.Pos.UI.Pages.PosTerminal.ProductItem>>(data.ItemsJson, _jsonOptions);
                                if (items != null)
                                {
                                    // Flatten items to include modifiers for tracking
                                    var flatList = new List<GFC.Pos.UI.Pages.PosTerminal.ProductItem>();
                                    foreach (var i in items)
                                    {
                                        flatList.Add(i);
                                        if (i.Modifiers != null)
                                        {
                                            flatList.AddRange(i.Modifiers);
                                        }
                                    }

                                    foreach (var i in flatList)
                                    {
                                        if (i.Name.StartsWith("TAB DEPOSIT:"))
                                        {
                                            banquet.Deposits.Add(i.Price);
                                            banquet.EventType = "PrePaid";
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
                                                banquet.EventName = "Active Event"; // Will be updated by deposit if found
                                        }
                                    }
                                }
                            }

                            if (salesItems != null)
                            {
                                // Flatten items to include modifiers for tracking
                                var flatList = new List<GFC.Pos.UI.Pages.PosTerminal.ProductItem>();
                                foreach (var i in salesItems)
                                {
                                    flatList.Add(i);
                                    if (i.Modifiers != null)
                                    {
                                        flatList.AddRange(i.Modifiers);
                                    }
                                }

                                foreach (var i in flatList)
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
            var timestamp = DateTime.UtcNow.Ticks;
            
            // [ROBUST-UPDATE] Fetch version.txt from the POS PWA static hosting origin rather than the API BaseAddress
            string origin = "https://localhost:7157";
            try
            {
                var currentOrigin = await _js.InvokeAsync<string>("eval", "window.location.origin");
                if (!string.IsNullOrEmpty(currentOrigin))
                {
                    origin = currentOrigin;
                }
            }
            catch { }

            var url = $"{origin.TrimEnd('/')}/version.txt?v={timestamp}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            
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

    public async Task<BanquetMasterSummaryDto?> GetBanquetMasterSummaryAsync(int eventId)
    {
        try
        {
            if (!await CheckConnectivityAsync()) return null;
            var json = await _http.GetStringAsync($"api/pos/events/summary/{eventId}");
            if (string.IsNullOrEmpty(json)) return null;
            return System.Text.Json.JsonSerializer.Deserialize<BanquetMasterSummaryDto>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch
        {
            return null;
        }
    }

    public async Task<PosMenuDto?> GetCachedMenuAsync()
    {
        try
        {
            // [OFFLINE-FIRST] Pull from LocalForage Vault (IndexedDB)
            var json = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedMenuKey);
            if (!string.IsNullOrEmpty(json) && json != "null")
            {
                return JsonSerializer.Deserialize<PosMenuDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex) 
        { 
            Console.WriteLine($"[POS] Vault Load Error: {ex.Message}");
        }
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
                var menu = await GetCachedMenuAsync();
                if (menu != null && (menu.Items.Any() || menu.Tokens.Any() || menu.Categories.Any()))
                {
                    Console.WriteLine($"[PosTerminalService] GetMenuAsync: Cache loaded successfully ({menu.Items.Count} items).");
                    // Background refresh
                    _ = Task.Run(async () => await RefreshMenuCacheAsync());
                    return menu;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PosTerminalService] GetMenuAsync Cache Error: {ex.Message}");
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
                    if (menu.Items.Any() || menu.Tokens.Any() || menu.Categories.Any())
                    {
                        await _js.InvokeVoidAsync("window.gfcSetAsync", CachedMenuKey, menu);
                        Console.WriteLine($"[POS] MENU PERSISTED TO VAULT - {menu.Items.Count} items.");
                        // Notify subscribers (e.g. PosTerminal component) that fresh menu data is available
                        MenuRefreshed?.Invoke(menu);
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

        var freshUsers = await RefreshAuthorizedUsersCacheAsync();
        if (freshUsers == null || !freshUsers.Any())
        {
            // Seed a local offline fallback operator so developers/operators are never locked out 
            // when cache is cleared and server is offline.
            Console.WriteLine("[POS] Server offline and no cached users found. Seeding local offline fallback operator...");
            var fallbackUser = new UserListItemDto(1, "GFC", true, true, null, null, null, "OFFLINE OVERRIDE", null, true, null);
            return new List<UserListItemDto> { fallbackUser };
        }
        return freshUsers;
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
        Console.WriteLine("[SYNC] FlushAllPendingAsync: Starting sweep...");
        if (!await _connectivity.GateAsync("OutboxSweep")) 
        {
            Console.WriteLine("[SYNC] FlushAllPendingAsync: Gate closed (Offline). Aborting.");
            return;
        }

        try {
            var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            Console.WriteLine($"[SYNC] Vault items retrieved. Kind: {vaultItems.ValueKind}");
            
            if (vaultItems.ValueKind != JsonValueKind.Array) 
            {
                Console.WriteLine("[SYNC] Vault is not an array. Aborting.");
                return;
            }

            var itemsArray = vaultItems.EnumerateArray().ToList();
            Console.WriteLine($"[SYNC] Found {itemsArray.Count} total items in vault.");

            foreach (var item in itemsArray) {
                var key = item.GetProperty("key").GetString();
                if (key == null) continue;

                if (key.StartsWith("gfc_pos_vault_event_start_")) {
                    Console.WriteLine($"[SYNC] Processing Offline Event Startup: {key}");
                    ActiveEvent? data = null;
                    try 
                    {
                        var dataElement = item.GetProperty("data");
                        if (dataElement.ValueKind == JsonValueKind.String)
                        {
                            data = JsonSerializer.Deserialize<ActiveEvent>(dataElement.GetString()!, _jsonOptions);
                        }
                        else
                        {
                            data = dataElement.Deserialize<ActiveEvent>(_jsonOptions);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[SYNC] Deserialization failed for offline event startup {key}: {ex.Message}");
                        continue;
                    }

                    if (data != null) {
                        try {
                            Console.WriteLine($"[SYNC] Posting offline event startup {data.Name} to server...");
                            
                            var tempId = data.Id;
                            data.Id = 0; // Let DB generate ID
                            
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                            var resp = await _http.PostAsJsonAsync("api/pos/events/start", data, cts.Token);
                            if (resp.IsSuccessStatusCode) {
                                var createdEvent = await resp.Content.ReadFromJsonAsync<ActiveEvent>(_jsonOptions, cts.Token);
                                if (createdEvent != null) {
                                    Console.WriteLine($"[SYNC] Server created event with real ID: {createdEvent.Id}");
                                    
                                    await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                    
                                    var allVault = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
                                    if (allVault.ValueKind == JsonValueKind.Array) {
                                        foreach (var vItem in allVault.EnumerateArray()) {
                                            var vKey = vItem.GetProperty("key").GetString();
                                            if (vKey == null) continue;
                                            
                                            if (vKey.StartsWith(VaultPrefixSales)) {
                                                try {
                                                    PosSaleDto? saleData = null;
                                                    var rawData = vItem.GetProperty("data");
                                                    if (rawData.ValueKind == JsonValueKind.String)
                                                        saleData = JsonSerializer.Deserialize<PosSaleDto>(rawData.GetString()!, _jsonOptions);
                                                    else
                                                        saleData = rawData.Deserialize<PosSaleDto>(_jsonOptions);

                                                    if (saleData != null && saleData.ActiveEventId == tempId) {
                                                        saleData.ActiveEventId = createdEvent.Id;
                                                        await _js.InvokeVoidAsync("window.gfcSetAsync", vKey, saleData);
                                                        Console.WriteLine($"[SYNC] Remapped pending sale {vKey} ActiveEventId to {createdEvent.Id}");
                                                    }
                                                } catch {}
                                            }
                                            
                                            if (vKey.StartsWith(ShiftLogPrefix)) {
                                                try {
                                                    PosSaleDto? saleData = null;
                                                    var rawData = vItem.GetProperty("data");
                                                    if (rawData.ValueKind == JsonValueKind.String)
                                                        saleData = JsonSerializer.Deserialize<PosSaleDto>(rawData.GetString()!, _jsonOptions);
                                                    else
                                                        saleData = rawData.Deserialize<PosSaleDto>(_jsonOptions);

                                                    if (saleData != null && saleData.ActiveEventId == tempId) {
                                                        saleData.ActiveEventId = createdEvent.Id;
                                                        await _js.InvokeVoidAsync("window.gfcSetAsync", vKey, saleData);
                                                        Console.WriteLine($"[SYNC] Remapped shift log {vKey} ActiveEventId to {createdEvent.Id}");
                                                    }
                                                } catch {}
                                            }
                                        }
                                    }

                                    try {
                                        var menuJson = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedMenuKey);
                                        if (!string.IsNullOrEmpty(menuJson) && menuJson != "null") {
                                            var cachedMenu = JsonSerializer.Deserialize<PosMenuDto>(menuJson, _jsonOptions);
                                            if (cachedMenu != null && cachedMenu.ActiveEvents != null) {
                                                var offlineEv = cachedMenu.ActiveEvents.FirstOrDefault(e => e.Id == tempId);
                                                if (offlineEv != null) {
                                                    offlineEv.Id = createdEvent.Id;
                                                    await SaveMenuToVaultAsync(cachedMenu);
                                                    Console.WriteLine($"[SYNC] Remapped cached menu active events");
                                                }
                                            }
                                        }
                                    } catch {}
                                    
                                    LastSynced = DateTime.Now;
                                }
                            } else {
                                Console.WriteLine($"[SYNC] Server rejected offline event start: {resp.StatusCode}");
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"[SYNC] Network error starting offline event {data.Name}: {ex.Message}");
                        }
                    }
                }

                if (key.StartsWith(VaultPrefixSales)) {
                    Console.WriteLine($"[SYNC] Processing sale: {key}");
                    PosSaleDto? data = null;
                    try 
                    {
                        var dataElement = item.GetProperty("data");
                        if (dataElement.ValueKind == JsonValueKind.String)
                        {
                            // Handle legacy stringified data
                            data = JsonSerializer.Deserialize<PosSaleDto>(dataElement.GetString()!, _jsonOptions);
                        }
                        else
                        {
                            // Handle raw object data
                            data = dataElement.Deserialize<PosSaleDto>(_jsonOptions);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[SYNC] Deserialization failed for {key}: {ex.Message}");
                        continue;
                    }

                    if (data != null) {
                        try {
                            Console.WriteLine($"[SYNC] Sending sale {data.Id} to server ({_http.BaseAddress}api/pos/sale)...");
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                            var resp = await _http.PostAsJsonAsync("api/pos/sale", data, cts.Token);
                            if (resp.IsSuccessStatusCode) {
                                Console.WriteLine($"[SYNC] Sale {data.Id} SYNCED successfully. Removing from vault.");
                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                LastSynced = DateTime.Now;
                            } else {
                                Console.WriteLine($"[SYNC] Server REJECTED sale {data.Id}: {resp.StatusCode} at {_http.BaseAddress}");
                                var err = await resp.Content.ReadAsStringAsync();
                                Console.WriteLine($"[SYNC] Server Error Detail: {err}");
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"[SYNC] Network error sending sale {data.Id}: {ex.Message}");
                        }
                    }
                }
                
                if (key.StartsWith(VaultPrefixZ)) {
                    Console.WriteLine($"[SYNC] Processing Z-Report: {key}");
                    PosZReportDto? data = null;
                    try 
                    {
                        var dataElement = item.GetProperty("data");
                        if (dataElement.ValueKind == JsonValueKind.String)
                        {
                            data = JsonSerializer.Deserialize<PosZReportDto>(dataElement.GetString()!, _jsonOptions);
                        }
                        else
                        {
                            data = dataElement.Deserialize<PosZReportDto>(_jsonOptions);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[SYNC] Deserialization failed for Z-Report {key}: {ex.Message}");
                        continue;
                    }

                    if (data != null) {
                        try {
                            Console.WriteLine($"[SYNC] Sending Z-Report {data.Id} to server...");
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
                            var resp = await _http.PostAsJsonAsync("api/pos/z-report", data, cts.Token);
                            if (resp.IsSuccessStatusCode) {
                                Console.WriteLine($"[SYNC] Z-Report SYNCED successfully. Removing from vault.");
                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                LastSynced = DateTime.Now;
                            } else {
                                Console.WriteLine($"[SYNC] Server REJECTED Z-Report: {resp.StatusCode}");
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"[SYNC] Network error sending Z-Report: {ex.Message}");
                        }
                    }
                }
            }
        } catch (Exception ex) {
            Console.WriteLine($"[SYNC] Global Flush Error: {ex.Message}");
        }

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
