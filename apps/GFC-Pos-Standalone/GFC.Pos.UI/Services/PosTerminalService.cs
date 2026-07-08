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
public class PosTerminalService : IPosTerminalService, IDisposable
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private readonly ConnectivityService _connectivity;
    private readonly IStationSettingsService _stationSettings;

    private const string VaultPrefixSales = "gfc_pos_vault_sale_";
    private const string VaultPrefixZ     = "gfc_pos_vault_z_";
    private const string ShiftLogPrefix   = "gfc_shift_log_";
    private const string CachedMenuKey      = "gfc_pos_cached_menu";
    private const string AuthorizedUsersKey = "gfc_pos_authorized_users";
    private const string VaultPrefixLiquorReceipt = "gfc_pos_vault_liquor_receipt_";
    private const string CachedLiquorOrdersKey = "gfc_pos_cached_liquor_orders";
    private const string CachedLiquorInventoryKey = "gfc_pos_cached_liquor_inventory";
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
        try
        {
            var saleJson = await _js.InvokeAsync<string>("window.gfcGetAsync", key);
            if (!string.IsNullOrEmpty(saleJson) && saleJson != "null")
            {
                var sale = JsonSerializer.Deserialize<PosSaleDto>(saleJson, _jsonOptions);
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
        catch (Exception ex)
        {
            Console.WriteLine($"[POS] VoidSaleAsync error: {ex.Message}");
        }
    }

    public async Task<ShiftAuditDto> GetShiftAuditAsync()
    {
        var audit = new ShiftAuditDto();
        try
        {
            var menu = await GetCachedMenuAsync();
            var eventsWithExplicitInitialDeposit = new HashSet<int>();

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
                                if (data.PaymentType == "PAYOUT")
                                {
                                    audit.PayoutTotal += data.TotalAmount;
                                    audit.Payouts.Add(data);
                                    if (salesItems != null && salesItems.Any())
                                    {
                                        var pItem = salesItems.First();
                                        var parts = pItem.Name.Split(':');
                                        var category = parts.Length > 1 ? parts[1] : "OTHER";
                                        var desc = parts.Length > 2 ? parts[2] : "";
                                        var cents = (int)(data.TotalAmount * 100);
                                        var summaryKey = $"PAYOUT:{category}:{desc}:{cents}";
                                        
                                        if (!audit.ItemSummary.ContainsKey(summaryKey))
                                            audit.ItemSummary[summaryKey] = 0;
                                        audit.ItemSummary[summaryKey]++;
                                    }
                                    continue;
                                }

                                if (salesItems != null)
                                {
                                    bool isDeposit = data.ItemsJson.Contains("TAB DEPOSIT:") || 
                                                     data.ItemsJson.Contains("INITIAL DEPOSIT:") || 
                                                     data.ItemsJson.Contains("DEPOSIT CORRECTION:") ||
                                                     data.ItemsJson.Contains("RETURNED FUNDS:");
                                    
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
                                        if (i.Name.StartsWith("TAB DEPOSIT:") || i.Name.StartsWith("DEPOSIT CORRECTION:") || i.Name.StartsWith("INITIAL DEPOSIT:") || i.Name.StartsWith("RETURNED FUNDS:"))
                                        {
                                            banquet.Deposits.Add(i.Price);
                                            banquet.EventType = "PrePaid";
                                            if (i.Name.StartsWith("INITIAL DEPOSIT:") && data.ActiveEventId.HasValue)
                                            {
                                                eventsWithExplicitInitialDeposit.Add(data.ActiveEventId.Value);
                                            }
                                            // Extract event name if not set
                                            if (string.IsNullOrEmpty(banquet.EventName))
                                            {
                                                banquet.EventName = i.Name
                                                    .Replace("TAB DEPOSIT: ", "")
                                                    .Replace("DEPOSIT CORRECTION: ", "")
                                                    .Replace("INITIAL DEPOSIT: ", "")
                                                    .Replace("RETURNED FUNDS: ", "");
                                            }
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

            // Seed deposits with initial prepaid amount only if no explicit initial deposit sale is in the current shift's log.
            // This prevents double-deposits for events started offline/online during the current shift,
            // while correctly supporting events started in a previous shift that are still active.
            foreach (var banquet in audit.Banquets)
            {
                if (banquet.ActiveEventId.HasValue && banquet.EventType == "PrePaid")
                {
                    if (!eventsWithExplicitInitialDeposit.Contains(banquet.ActiveEventId.Value))
                    {
                        if (menu != null && menu.ActiveEvents != null)
                        {
                            var activeEv = menu.ActiveEvents.FirstOrDefault(e => e.Id == banquet.ActiveEventId.Value);
                            if (activeEv != null && activeEv.InitialAmount > 0)
                            {
                                banquet.Deposits.Insert(0, activeEv.InitialAmount);
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
    private Timer? _syncTimer;
    private bool _isInitialized = false;

    public PosTerminalService(HttpClient http, IJSRuntime js, ConnectivityService connectivity, IStationSettingsService stationSettings)
    {
        _http = http;
        _js = js;
        _connectivity = connectivity;
        _stationSettings = stationSettings;
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;

        System.Diagnostics.Debug.WriteLine("[PosTerminalService] Asynchronously initializing...");

        // Initialize API Base Address from Override if present
        await InitializeApiUrlAsync();

        _connectivity.ConnectivityChanged += HandleConnectivityChanged;

        // [SYNC HEARTBEAT] Pulse every 30 seconds to flush trapped data
        _syncTimer = new Timer(async _ => await SafeFlushAsync(), null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30));

        _isInitialized = true;
        System.Diagnostics.Debug.WriteLine("[PosTerminalService] Initialization complete.");
    }

    private void HandleConnectivityChanged(bool isOnline)
    {
        if (isOnline)
        {
            Console.WriteLine("[SYNC] Server detected! Triggering immediate vault sweep...");
            _ = SafeFlushAsync();
        }
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
            string? apiVersion = null;
            string? txtVersion = null;
            
            string origin = "https://pos.lovanow.com";
            try
            {
                var currentOrigin = await _js.InvokeAsync<string>("eval", "window.location.origin");
                if (!string.IsNullOrEmpty(currentOrigin) && 
                    !currentOrigin.Contains("localhost") && 
                    !currentOrigin.Contains("0.0.0.0") && 
                    !currentOrigin.StartsWith("app://"))
                {
                    origin = currentOrigin;
                }
            }
            catch { }

            // Try the live API first so we fetch instantly from the database (prevents needing to redeploy WebApp every time)
            try
            {
                var apiResponse = await _http.GetStringAsync($"api/mobile-reporting/pos-version?t={timestamp}");
                if (!string.IsNullOrEmpty(apiResponse))
                {
                    return apiResponse.Trim();
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
            return "Offline";
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

    public async Task<List<PosSaleDto>> GetUnsyncedSalesAsync()
    {
        var list = new List<PosSaleDto>();
        try
        {
            var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            if (vaultItems.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in vaultItems.EnumerateArray())
                {
                    var key = item.GetProperty("key").GetString();
                    if (key != null && key.StartsWith(VaultPrefixSales))
                    {
                        var dataElement = item.GetProperty("data");
                        PosSaleDto? sale = null;
                        if (dataElement.ValueKind == JsonValueKind.String)
                        {
                            sale = JsonSerializer.Deserialize<PosSaleDto>(dataElement.GetString()!, _jsonOptions);
                        }
                        else
                        {
                            sale = JsonSerializer.Deserialize<PosSaleDto>(dataElement.GetRawText(), _jsonOptions);
                        }
                        if (sale != null)
                        {
                            list.Add(sale);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[POS] GetUnsyncedSalesAsync Error: {ex.Message}");
        }
        return list;
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
                var terminalName = await _stationSettings.GetTerminalNameAsync();
                var url = $"api/pos/menu?terminalName={Uri.EscapeDataString(terminalName ?? string.Empty)}";
                var menu = await _http.GetFromJsonAsync<PosMenuDto>(url);
                
                if (menu != null)
                {
                    Console.WriteLine($"[PosTerminalService] RefreshMenuCacheAsync: Server returned {menu.Items.Count} items, {menu.Tokens.Count} tokens.");
                    LastSynced = DateTime.Now;
                    
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
        
        // PERFORMANCE: Run outbox count in the background to prevent blocking UI finalization
        _ = Task.Run(async () => {
            try {
                await GetTotalPendingAsync();
                OutboxChanged?.Invoke();
            } catch {}
        });

        // 2. Background Attempt
        _ = SafeFlushAsync();
    }

    public async Task SaveZReportAsync(PosZReportDto report)
    {
        if (report.Id == Guid.Empty) report.Id = Guid.NewGuid();

        var key = $"{VaultPrefixZ}{report.Id}";
        await _js.InvokeVoidAsync("window.gfcSetAsync", key, report);
        
        // PERFORMANCE: Run outbox count in the background to prevent blocking UI finalization
        _ = Task.Run(async () => {
            try {
                await GetTotalPendingAsync();
                OutboxChanged?.Invoke();
            } catch {}
        });

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

            var remappedIds = new Dictionary<int, int>();
            try {
                var persistentRemappedStr = await _js.InvokeAsync<string>("localStorage.getItem", "gfc_pos_remapped_ids");
                if (!string.IsNullOrEmpty(persistentRemappedStr)) {
                    var parsed = JsonSerializer.Deserialize<Dictionary<int, int>>(persistentRemappedStr, _jsonOptions);
                    if (parsed != null) {
                        foreach (var kvp in parsed) {
                            remappedIds[kvp.Key] = kvp.Value;
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"[SYNC] Error loading persistent remapped IDs: {ex.Message}");
            }

            // [LOGICAL ORDERING] Ensure event start requests sync before dependent added funds, closures, or sales
            itemsArray = itemsArray.OrderBy(item => {
                var k = item.GetProperty("key").GetString() ?? "";
                if (k.StartsWith("gfc_pos_vault_event_start_")) return 1;
                if (k.StartsWith("gfc_pos_vault_event_add_funds_")) return 2;
                if (k.StartsWith("gfc_pos_vault_event_tally_")) return 2.5;
                if (k.StartsWith("gfc_pos_vault_event_close_")) return 3;
                if (k.StartsWith(VaultPrefixSales) || k.StartsWith(ShiftLogPrefix)) return 4;
                if (k.StartsWith("gfc_pos_vault_z_")) return 5;
                return 10;
            }).ToList();

            foreach (var item in itemsArray) {
                var key = item.GetProperty("key").GetString();
                if (key == null) continue;

                var retryKey = $"gfc_sync_retry_{key}";
                int retryCount = 0;
                try {
                    var retryCountStr = await _js.InvokeAsync<string>("localStorage.getItem", retryKey);
                    if (!string.IsNullOrEmpty(retryCountStr) && int.TryParse(retryCountStr, out var parsedRetry))
                    {
                        retryCount = parsedRetry;
                    }
                } catch { }



                if (retryCount >= 5) {
                    if (key.StartsWith(VaultPrefixLiquorReceipt)) {
                        Console.WriteLine($"[SYNC] Retrying previously skipped liquor receipt {key}...");
                    } else {
                        Console.WriteLine($"[SYNC] Skipping poison-pill outbox item {key} due to {retryCount} consecutive server rejections.");
                        continue;
                    }
                }

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
                                     remappedIds[tempId] = createdEvent.Id;
                                     try {
                                         await _js.InvokeVoidAsync("localStorage.setItem", "gfc_pos_remapped_ids", JsonSerializer.Serialize(remappedIds));
                                     } catch {}
                                    
                                    await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                    try { await _js.InvokeVoidAsync("localStorage.removeItem", retryKey); } catch {}
                                    
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

                                            if (vKey.StartsWith("gfc_pos_vault_event_add_funds_")) {
                                                try {
                                                    AddFundsRequest? addReq = null;
                                                    var rawData = vItem.GetProperty("data");
                                                    if (rawData.ValueKind == JsonValueKind.String)
                                                        addReq = JsonSerializer.Deserialize<AddFundsRequest>(rawData.GetString()!, _jsonOptions);
                                                    else
                                                        addReq = rawData.Deserialize<AddFundsRequest>(_jsonOptions);

                                                    if (addReq != null && addReq.Id == tempId) {
                                                        addReq.Id = createdEvent.Id;
                                                        await _js.InvokeVoidAsync("window.gfcSetAsync", vKey, addReq);
                                                        await _js.InvokeVoidAsync("localStorage.setItem", $"gfc_sync_retry_{vKey}", "0");
                                                        Console.WriteLine($"[SYNC] Remapped pending add funds {vKey} Id to {createdEvent.Id}");
                                                    }
                                                } catch {}
                                            }

                                            if (vKey.StartsWith("gfc_pos_vault_event_close_")) {
                                                try {
                                                    int eventId = 0;
                                                    var rawData = vItem.GetProperty("data");
                                                    if (rawData.ValueKind == JsonValueKind.Number)
                                                        eventId = rawData.GetInt32();
                                                    else if (rawData.ValueKind == JsonValueKind.String)
                                                        int.TryParse(rawData.GetString(), out eventId);

                                                    if (eventId == tempId) {
                                                        await _js.InvokeVoidAsync("window.gfcRemoveAsync", vKey);
                                                        var newCloseKey = $"gfc_pos_vault_event_close_{createdEvent.Id}";
                                                        await _js.InvokeVoidAsync("window.gfcSetAsync", newCloseKey, createdEvent.Id);
                                                        await _js.InvokeVoidAsync("localStorage.removeItem", $"gfc_sync_retry_{vKey}");
                                                        await _js.InvokeVoidAsync("localStorage.setItem", $"gfc_sync_retry_{newCloseKey}", "0");
                                                        Console.WriteLine($"[SYNC] Remapped pending event close from negative ID {tempId} to {createdEvent.Id}");
                                                    }
                                                } catch {}
                                            }

                                            if (vKey.StartsWith("gfc_pos_vault_event_tally_")) {
                                                try {
                                                    UpdateEventTallyRequest? tallyReq = null;
                                                    var rawData = vItem.GetProperty("data");
                                                    if (rawData.ValueKind == JsonValueKind.String)
                                                        tallyReq = JsonSerializer.Deserialize<UpdateEventTallyRequest>(rawData.GetString()!, _jsonOptions);
                                                    else
                                                        tallyReq = rawData.Deserialize<UpdateEventTallyRequest>(_jsonOptions);

                                                    if (tallyReq != null && tallyReq.Id == tempId) {
                                                        tallyReq.Id = createdEvent.Id;
                                                        await _js.InvokeVoidAsync("window.gfcSetAsync", vKey, tallyReq);
                                                        await _js.InvokeVoidAsync("localStorage.setItem", $"gfc_sync_retry_{vKey}", "0");
                                                        Console.WriteLine($"[SYNC] Remapped pending event tally {vKey} Id to {createdEvent.Id}");
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
                                retryCount++;
                                try { await _js.InvokeVoidAsync("localStorage.setItem", retryKey, retryCount.ToString()); } catch {}
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
                        if (data.ActiveEventId.HasValue && data.ActiveEventId.Value < 0) {
                            if (remappedIds.TryGetValue(data.ActiveEventId.Value, out var realEventId)) {
                                Console.WriteLine($"[SYNC] Remapping negative ActiveEventId {data.ActiveEventId} to {realEventId} for sale {key}.");
                                data.ActiveEventId = realEventId;
                                try {
                                    await _js.InvokeVoidAsync("window.gfcSetAsync", key, data);
                                } catch {}
                            } else {
                                // If we still couldn't resolve the offline negative ID, set it to null so the sale can sync successfully
                                // rather than causing a database foreign key constraint crash (500) and blocking the outbox forever.
                                Console.WriteLine($"[SYNC] Could not resolve negative ActiveEventId {data.ActiveEventId} for sale {key}. Resetting to null to allow sync.");
                                data.ActiveEventId = null;
                                try {
                                    await _js.InvokeVoidAsync("window.gfcSetAsync", key, data);
                                } catch {}
                            }
                        }

                        try {
                            Console.WriteLine($"[SYNC] Sending sale {data.Id} to server ({_http.BaseAddress}api/pos/sale)...");
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                            var resp = await _http.PostAsJsonAsync("api/pos/sale", data, cts.Token);
                            if (resp.IsSuccessStatusCode) {
                                Console.WriteLine($"[SYNC] Sale {data.Id} SYNCED successfully. Removing from vault.");
                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                try { await _js.InvokeVoidAsync("localStorage.removeItem", retryKey); } catch {}
                                LastSynced = DateTime.Now;
                            } else {
                                Console.WriteLine($"[SYNC] Server REJECTED sale {data.Id}: {resp.StatusCode} at {_http.BaseAddress}");
                                var err = await resp.Content.ReadAsStringAsync();
                                Console.WriteLine($"[SYNC] Server Error Detail: {err}");
                                retryCount++;
                                try { await _js.InvokeVoidAsync("localStorage.setItem", retryKey, retryCount.ToString()); } catch {}
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
                                try { await _js.InvokeVoidAsync("localStorage.removeItem", retryKey); } catch {}
                                LastSynced = DateTime.Now;
                            } else {
                                Console.WriteLine($"[SYNC] Server REJECTED Z-Report: {resp.StatusCode}");
                                retryCount++;
                                try { await _js.InvokeVoidAsync("localStorage.setItem", retryKey, retryCount.ToString()); } catch {}
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"[SYNC] Network error sending Z-Report: {ex.Message}");
                        }
                    }
                }

                if (key.StartsWith("gfc_pos_vault_event_close_")) {
                    Console.WriteLine($"[SYNC] Processing Offline Event Closure: {key}");
                    int eventId = 0;
                    try {
                        var rawData = item.GetProperty("data");
                        if (rawData.ValueKind == JsonValueKind.Number)
                            eventId = rawData.GetInt32();
                        else if (rawData.ValueKind == JsonValueKind.String)
                            int.TryParse(rawData.GetString(), out eventId);
                    } catch {}

                    if (eventId != 0) {
                        if (eventId < 0) {
                            if (remappedIds.TryGetValue(eventId, out var realEventId)) {
                                Console.WriteLine($"[SYNC] Remapping negative close event ID {eventId} to {realEventId} using remappedIds.");
                                eventId = realEventId;
                            } else {
                                Console.WriteLine($"[SYNC] Auto-healing: Found negative Id {eventId} in event close outbox. Falling back to cached menu lookup.");
                                try {
                                    var menuJson = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedMenuKey);
                                    if (!string.IsNullOrEmpty(menuJson) && menuJson != "null") {
                                        var cachedMenu = JsonSerializer.Deserialize<PosMenuDto>(menuJson, _jsonOptions);
                                        if (cachedMenu != null && cachedMenu.ActiveEvents != null) {
                                            var matchingEvent = cachedMenu.ActiveEvents.FirstOrDefault(e => e.Id > 0);
                                            if (matchingEvent != null) {
                                                Console.WriteLine($"[SYNC] Auto-healing: Mapped negative close event Id {eventId} to real ID {matchingEvent.Id}");
                                                eventId = matchingEvent.Id;
                                                
                                                // Delete the negative close outbox key and replace with a positive close key
                                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                                key = $"gfc_pos_vault_event_close_{eventId}";
                                                await _js.InvokeVoidAsync("window.gfcSetAsync", key, eventId);
                                                
                                                retryKey = $"gfc_sync_retry_{key}";
                                                await _js.InvokeVoidAsync("localStorage.removeItem", retryKey);
                                                await _js.InvokeVoidAsync("localStorage.setItem", $"gfc_sync_retry_{key}", "0");
                                                retryCount = 0;
                                            }
                                        }
                                    }
                                } catch {}
                            }
                        }

                        try {
                            Console.WriteLine($"[SYNC] Sending offline event closure for {eventId} to server...");
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                            var resp = await _http.PostAsync($"api/pos/events/close/{eventId}", null, cts.Token);
                            if (resp.IsSuccessStatusCode) {
                                Console.WriteLine($"[SYNC] Event {eventId} closed successfully on server. Removing from outbox.");
                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                try { await _js.InvokeVoidAsync("localStorage.removeItem", retryKey); } catch {}
                                
                                // Proactively remove the closed event from the local cached menu
                                try {
                                    var menuJson = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedMenuKey);
                                    if (!string.IsNullOrEmpty(menuJson) && menuJson != "null") {
                                        var cachedMenu = JsonSerializer.Deserialize<PosMenuDto>(menuJson, _jsonOptions);
                                        if (cachedMenu != null && cachedMenu.ActiveEvents != null) {
                                            var closedEv = cachedMenu.ActiveEvents.FirstOrDefault(e => e.Id == eventId);
                                            if (closedEv != null) {
                                                cachedMenu.ActiveEvents.Remove(closedEv);
                                                await SaveMenuToVaultAsync(cachedMenu);
                                                Console.WriteLine($"[SYNC] Removed closed event {eventId} from cached menu");
                                            }
                                        }
                                    }
                                } catch {}
                                
                                LastSynced = DateTime.Now;
                            } else {
                                Console.WriteLine($"[SYNC] Server REJECTED event closure {eventId}: {resp.StatusCode}");
                                retryCount++;
                                try { await _js.InvokeVoidAsync("localStorage.setItem", retryKey, retryCount.ToString()); } catch {}
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"[SYNC] Network error closing offline event {eventId}: {ex.Message}");
                        }
                    }
                }

                if (key.StartsWith("gfc_pos_vault_event_add_funds_")) {
                    Console.WriteLine($"[SYNC] Processing Offline Event Add Funds: {key}");
                    AddFundsRequest? addReq = null;
                    try {
                        var rawData = item.GetProperty("data");
                        if (rawData.ValueKind == JsonValueKind.String)
                            addReq = JsonSerializer.Deserialize<AddFundsRequest>(rawData.GetString()!, _jsonOptions);
                        else
                            addReq = rawData.Deserialize<AddFundsRequest>(_jsonOptions);
                    } catch {}

                    if (addReq != null) {
                        if (addReq.Id < 0) {
                            if (remappedIds.TryGetValue(addReq.Id, out var realEventId)) {
                                Console.WriteLine($"[SYNC] Remapping negative add funds event ID {addReq.Id} to {realEventId} using remappedIds.");
                                addReq.Id = realEventId;
                            } else {
                                Console.WriteLine($"[SYNC] Auto-healing: Found negative Id {addReq.Id} in add funds outbox. Falling back to cached menu lookup.");
                                try {
                                    var menuJson = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedMenuKey);
                                    if (!string.IsNullOrEmpty(menuJson) && menuJson != "null") {
                                        var cachedMenu = JsonSerializer.Deserialize<PosMenuDto>(menuJson, _jsonOptions);
                                        if (cachedMenu != null && cachedMenu.ActiveEvents != null) {
                                            var matchingEvent = cachedMenu.ActiveEvents.FirstOrDefault(e => e.Type == GFC.Core.Enums.EventTabType.PrePaid && e.Id > 0);
                                            if (matchingEvent != null) {
                                                Console.WriteLine($"[SYNC] Auto-healing: Mapped negative Id {addReq.Id} to active prepaid event ID {matchingEvent.Id} ({matchingEvent.Name})");
                                                addReq.Id = matchingEvent.Id;
                                                
                                                // Update the key in IndexedDB with the corrected ID
                                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                                key = $"gfc_pos_vault_event_add_funds_{addReq.Id}";
                                                await _js.InvokeVoidAsync("window.gfcSetAsync", key, addReq);
                                                retryKey = $"gfc_sync_retry_{key}";
                                                await _js.InvokeVoidAsync("localStorage.setItem", retryKey, "0");
                                                retryCount = 0;
                                            }
                                        }
                                    }
                                } catch {}
                            }
                        }

                        try {
                            Console.WriteLine($"[SYNC] Sending offline add funds for {addReq.Id} (Amount: {addReq.Amount}) to server...");
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                            var resp = await _http.PostAsJsonAsync("api/pos/events/add-funds", addReq, cts.Token);
                            if (resp.IsSuccessStatusCode) {
                                Console.WriteLine($"[SYNC] Added funds for event {addReq.Id} synced successfully. Removing from outbox.");
                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                try { await _js.InvokeVoidAsync("localStorage.removeItem", retryKey); } catch {}
                                LastSynced = DateTime.Now;
                            } else {
                                Console.WriteLine($"[SYNC] Server REJECTED add funds {addReq.Id}: {resp.StatusCode}");
                                retryCount++;
                                try { await _js.InvokeVoidAsync("localStorage.setItem", retryKey, retryCount.ToString()); } catch {}
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"[SYNC] Network error adding offline funds to event {addReq.Id}: {ex.Message}");
                        }
                    }
                }

                if (key.StartsWith("gfc_pos_vault_event_tally_")) {
                    Console.WriteLine($"[SYNC] Processing Offline Event Tally Update: {key}");
                    UpdateEventTallyRequest? tallyReq = null;
                    try {
                        var rawData = item.GetProperty("data");
                        if (rawData.ValueKind == JsonValueKind.String)
                            tallyReq = JsonSerializer.Deserialize<UpdateEventTallyRequest>(rawData.GetString()!, _jsonOptions);
                        else
                            tallyReq = rawData.Deserialize<UpdateEventTallyRequest>(_jsonOptions);
                    } catch {}

                    if (tallyReq != null) {
                        if (tallyReq.Id < 0) {
                            if (remappedIds.TryGetValue(tallyReq.Id, out var realEventId)) {
                                Console.WriteLine($"[SYNC] Remapping negative event tally ID {tallyReq.Id} to {realEventId} using remappedIds.");
                                tallyReq.Id = realEventId;
                            } else {
                                Console.WriteLine($"[SYNC] Auto-healing: Found negative Id {tallyReq.Id} in event tally outbox. Falling back to cached menu lookup.");
                                try {
                                    var menuJson = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedMenuKey);
                                    if (!string.IsNullOrEmpty(menuJson) && menuJson != "null") {
                                        var cachedMenu = JsonSerializer.Deserialize<PosMenuDto>(menuJson, _jsonOptions);
                                        if (cachedMenu != null && cachedMenu.ActiveEvents != null) {
                                            var matchingEvent = cachedMenu.ActiveEvents.FirstOrDefault(e => e.Id > 0);
                                            if (matchingEvent != null) {
                                                Console.WriteLine($"[SYNC] Auto-healing: Mapped negative Id {tallyReq.Id} to active event ID {matchingEvent.Id} ({matchingEvent.Name})");
                                                tallyReq.Id = matchingEvent.Id;
                                                
                                                // Update the key in IndexedDB with the corrected ID
                                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                                key = $"gfc_pos_vault_event_tally_{tallyReq.Id}";
                                                await _js.InvokeVoidAsync("window.gfcSetAsync", key, tallyReq);
                                                retryKey = $"gfc_sync_retry_{key}";
                                                await _js.InvokeVoidAsync("localStorage.setItem", retryKey, "0");
                                                retryCount = 0;
                                            }
                                        }
                                    }
                                } catch {}
                            }
                        }

                        try {
                            Console.WriteLine($"[SYNC] Sending offline event tally update for {tallyReq.Id} to server...");
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                            var resp = await _http.PostAsJsonAsync("api/pos/events/update-tally", tallyReq, cts.Token);
                            if (resp.IsSuccessStatusCode) {
                                Console.WriteLine($"[SYNC] Event tally update for event {tallyReq.Id} synced successfully. Removing from outbox.");
                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                try { await _js.InvokeVoidAsync("localStorage.removeItem", retryKey); } catch {}
                                
                                // Proactively remove the closed event from the local cached menu if CloseEvent is true
                                if (tallyReq.CloseEvent) {
                                    try {
                                        var menuJson = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedMenuKey);
                                        if (!string.IsNullOrEmpty(menuJson) && menuJson != "null") {
                                            var cachedMenu = JsonSerializer.Deserialize<PosMenuDto>(menuJson, _jsonOptions);
                                            if (cachedMenu != null && cachedMenu.ActiveEvents != null) {
                                                var closedEv = cachedMenu.ActiveEvents.FirstOrDefault(e => e.Id == tallyReq.Id);
                                                if (closedEv != null) {
                                                    cachedMenu.ActiveEvents.Remove(closedEv);
                                                    await SaveMenuToVaultAsync(cachedMenu);
                                                    Console.WriteLine($"[SYNC] Removed closed tally event {tallyReq.Id} from cached menu");
                                                }
                                            }
                                        }
                                    } catch {}
                                }
                                
                                LastSynced = DateTime.Now;
                            } else {
                                Console.WriteLine($"[SYNC] Server REJECTED event tally update {tallyReq.Id}: {resp.StatusCode}");
                                retryCount++;
                                try { await _js.InvokeVoidAsync("localStorage.setItem", retryKey, retryCount.ToString()); } catch {}
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"[SYNC] Network error syncing offline event tally update to event {tallyReq.Id}: {ex.Message}");
                        }
                    }
                }

                if (key.StartsWith(VaultPrefixLiquorReceipt)) {
                    Console.WriteLine($"[SYNC] Processing Offline Liquor Receipt: {key}");
                    LiquorOrderReceiptDto? receipt = null;
                    try {
                        var rawData = item.GetProperty("data");
                        if (rawData.ValueKind == JsonValueKind.String)
                            receipt = JsonSerializer.Deserialize<LiquorOrderReceiptDto>(rawData.GetString()!, _jsonOptions);
                        else
                            receipt = rawData.Deserialize<LiquorOrderReceiptDto>(_jsonOptions);
                    } catch (Exception ex) {
                        Console.WriteLine($"[SYNC] Deserialization failed for liquor receipt {key}: {ex.Message}");
                        continue;
                    }

                    if (receipt != null) {
                        try {
                            Console.WriteLine($"[SYNC] Sending offline liquor receipt for Order #{receipt.OrderId} to server...");
                            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
                            var resp = await _http.PostAsJsonAsync($"api/liquor/orders/{receipt.OrderId}/receive", receipt, cts.Token);
                            if (resp.IsSuccessStatusCode) {
                                Console.WriteLine($"[SYNC] Liquor receipt for Order #{receipt.OrderId} synced successfully. Removing from outbox.");
                                await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                                try { await _js.InvokeVoidAsync("localStorage.removeItem", retryKey); } catch {}
                                LastSynced = DateTime.Now;
                            } else {
                                Console.WriteLine($"[SYNC] Server REJECTED liquor receipt for Order #{receipt.OrderId}: {resp.StatusCode}");
                                retryCount++;
                                try { await _js.InvokeVoidAsync("localStorage.setItem", retryKey, retryCount.ToString()); } catch {}
                            }
                        } catch (Exception ex) {
                            Console.WriteLine($"[SYNC] Network error sending offline liquor receipt for Order #{receipt.OrderId}: {ex.Message}");
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

    public async Task<List<LiquorOrder>> GetPendingLiquorOrdersAsync(bool force = false)
    {
        List<LiquorOrder> orders = null;
        bool isOnline = await CheckConnectivityAsync();
        if (isOnline)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                orders = await _http.GetFromJsonAsync<List<LiquorOrder>>("api/liquor/orders/pending", cts.Token);
                if (orders != null)
                {
                    // Cache locally in IndexedDB
                    await _js.InvokeVoidAsync("window.gfcSetAsync", CachedLiquorOrdersKey, orders);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LIQUOR SERVICE CLIENT] Failed to fetch pending orders from server: {ex.Message}. Falling back to cache.");
            }
        }

        if (orders == null)
        {
            // Offline / Fallback
            try
            {
                var cachedJson = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedLiquorOrdersKey);
                if (!string.IsNullOrEmpty(cachedJson) && cachedJson != "null")
                {
                    orders = JsonSerializer.Deserialize<List<LiquorOrder>>(cachedJson, _jsonOptions);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LIQUOR SERVICE CLIENT] Failed to read cached orders: {ex.Message}");
            }
        }

        if (orders == null)
        {
            orders = new List<LiquorOrder>();
        }

        // Filter out orders that are in the outbox waiting to sync
        try
        {
            var vaultItems = await _js.InvokeAsync<JsonElement>("window.gfcGetAllAsync");
            if (vaultItems.ValueKind == JsonValueKind.Array)
            {
                var outboxOrderIds = new HashSet<int>();
                foreach (var item in vaultItems.EnumerateArray())
                {
                    var key = item.GetProperty("key").GetString();
                    if (key != null && key.StartsWith(VaultPrefixLiquorReceipt))
                    {
                        var part = key.Substring(VaultPrefixLiquorReceipt.Length);
                        var idx = part.IndexOf('_');
                        var idStr = idx > 0 ? part.Substring(0, idx) : part;
                        if (int.TryParse(idStr, out var orderId))
                        {
                            outboxOrderIds.Add(orderId);
                        }
                    }
                }

                if (outboxOrderIds.Any())
                {
                    orders = orders.Where(o => !outboxOrderIds.Contains(o.Id)).ToList();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LIQUOR SERVICE CLIENT] Failed to filter pending orders by outbox: {ex.Message}");
        }

        return orders;
    }

    public async Task<List<LiquorItem>> GetLiquorInventoryAsync(bool force = false)
    {
        bool isOnline = await CheckConnectivityAsync();
        if (isOnline)
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                var items = await _http.GetFromJsonAsync<List<LiquorItem>>("api/liquor/items", cts.Token);
                if (items != null)
                {
                    await _js.InvokeVoidAsync("window.gfcSetAsync", CachedLiquorInventoryKey, items);
                    return items;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LIQUOR INVENTORY] Failed to fetch from server: {ex.Message}. Falling back to cache.");
            }
        }

        // Offline / Fallback — read from IndexedDB cache
        try
        {
            var cachedJson = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedLiquorInventoryKey);
            if (!string.IsNullOrEmpty(cachedJson) && cachedJson != "null")
            {
                var cached = JsonSerializer.Deserialize<List<LiquorItem>>(cachedJson, _jsonOptions);
                if (cached != null) return cached;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LIQUOR INVENTORY] Failed to read cached inventory: {ex.Message}");
        }

        return new List<LiquorItem>();
    }

    public async Task ReceiveLiquorOrderAsync(LiquorOrderReceiptDto receipt)
    {
        var key = $"{VaultPrefixLiquorReceipt}{receipt.OrderId}_{DateTime.UtcNow.Ticks}";
        await _js.InvokeVoidAsync("window.gfcSetAsync", key, receipt);
        
        // PERFORMANCE: Run outbox count in the background to prevent blocking UI finalization
        _ = Task.Run(async () => {
            try {
                await GetTotalPendingAsync();
                OutboxChanged?.Invoke();
            } catch {}
        });

        // Trigger immediate background sync flush attempt (awaited inline to update database before UI refresh)
        try {
            await FlushAllPendingAsync();
        } catch {}
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
                    if (key.StartsWith("gfc_pos_vault_event_start_")) sales++;
                    if (key.StartsWith("gfc_pos_vault_event_close_")) sales++;
                    if (key.StartsWith("gfc_pos_vault_event_add_funds_")) sales++;
                    if (key.StartsWith("gfc_pos_vault_event_tally_")) sales++;
                    if (key.StartsWith(VaultPrefixLiquorReceipt)) sales++;
                }
            }

            PendingSalesCount = sales;
            PendingZCount = z;
            return TotalPendingCount;
        } catch { return 0; }
    }

    public async Task<MemberDrawPoolDto?> GetMemberDrawPoolAsync()
    {
        try
        {
            if (await CheckConnectivityAsync())
            {
                var pool = await _http.GetFromJsonAsync<MemberDrawPoolDto>("api/pos/members/draw-pool");
                if (pool != null)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await _js.InvokeVoidAsync("window.gfcSetAsync", "gfc_member_draw_pool_cache", pool);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[POS] Failed to cache member draw pool: {ex.Message}");
                        }
                    });
                }
                return pool;
            }
            else
            {
                Console.WriteLine("[POS] Offline: retrieving member draw pool from local cache.");
                var json = await _js.InvokeAsync<string>("window.gfcGetAsync", "gfc_member_draw_pool_cache");
                if (!string.IsNullOrEmpty(json) && json != "null")
                {
                    return JsonSerializer.Deserialize<MemberDrawPoolDto>(json, _jsonOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[POS] Error fetching/retrieving member draw pool: {ex.Message}");
        }
        return null;
    }

    public async Task<MemberDrawStatusDto?> GetMemberDrawStatusAsync(int memberId)
    {
        try
        {
            if (await CheckConnectivityAsync())
            {
                var status = await _http.GetFromJsonAsync<MemberDrawStatusDto>($"api/pos/members/{memberId}/draw-status");
                if (status != null)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await _js.InvokeVoidAsync("window.gfcSetAsync", $"gfc_member_draw_status_{memberId}", status);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[POS] Failed to cache member draw status for {memberId}: {ex.Message}");
                        }
                    });
                }
                return status;
            }
            else
            {
                Console.WriteLine($"[POS] Offline: retrieving member draw status for {memberId} from local cache.");
                var json = await _js.InvokeAsync<string>("window.gfcGetAsync", $"gfc_member_draw_status_{memberId}");
                if (!string.IsNullOrEmpty(json) && json != "null")
                {
                    return JsonSerializer.Deserialize<MemberDrawStatusDto>(json, _jsonOptions);
                }

                // Fallback: Check the cached member draw pool which has the full names and eligibility
                try
                {
                    var poolJson = await _js.InvokeAsync<string>("window.gfcGetAsync", "gfc_member_draw_pool_cache");
                    if (!string.IsNullOrEmpty(poolJson) && poolJson != "null")
                    {
                        var pool = JsonSerializer.Deserialize<MemberDrawPoolDto>(poolJson, _jsonOptions);
                        if (pool != null && pool.Members != null)
                        {
                            var match = pool.Members.FirstOrDefault(m => m.MemberId == memberId);
                            if (match != null)
                            {
                                return new MemberDrawStatusDto
                                {
                                    MemberId = memberId,
                                    FirstName = match.FirstName,
                                    LastName = match.LastName,
                                    Suffix = match.Suffix,
                                    Status = "Offline Pool Cache",
                                    IsActive = true,
                                    DuesPaid = match.IsEligible,
                                    IsWaived = false,
                                    IsEligible = match.IsEligible
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[POS] Failed to query offline pool cache fallback: {ex.Message}");
                }

                return new MemberDrawStatusDto
                {
                    MemberId = memberId,
                    FirstName = "OFFLINE",
                    LastName = "RECORD",
                    Status = "Offline Cache Missing",
                    IsActive = false,
                    DuesPaid = false,
                    IsWaived = false,
                    IsEligible = false
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[POS] Error fetching/retrieving member draw status for {memberId}: {ex.Message}");
            try
            {
                var json = await _js.InvokeAsync<string>("window.gfcGetAsync", $"gfc_member_draw_status_{memberId}");
                if (!string.IsNullOrEmpty(json) && json != "null")
                {
                    return JsonSerializer.Deserialize<MemberDrawStatusDto>(json, _jsonOptions);
                }

                // Fallback: Check the cached member draw pool which has the full names and eligibility
                var poolJson = await _js.InvokeAsync<string>("window.gfcGetAsync", "gfc_member_draw_pool_cache");
                if (!string.IsNullOrEmpty(poolJson) && poolJson != "null")
                {
                    var pool = JsonSerializer.Deserialize<MemberDrawPoolDto>(poolJson, _jsonOptions);
                    if (pool != null && pool.Members != null)
                    {
                        var match = pool.Members.FirstOrDefault(m => m.MemberId == memberId);
                        if (match != null)
                        {
                            return new MemberDrawStatusDto
                            {
                                MemberId = memberId,
                                FirstName = match.FirstName,
                                LastName = match.LastName,
                                Suffix = match.Suffix,
                                Status = "Offline Pool Cache",
                                IsActive = true,
                                DuesPaid = match.IsEligible,
                                IsWaived = false,
                                IsEligible = match.IsEligible
                            };
                        }
                    }
                }
            }
            catch { }

            return new MemberDrawStatusDto
            {
                MemberId = memberId,
                FirstName = "OFFLINE",
                LastName = "RECORD",
                Status = "Error Lookup (Offline)",
                IsActive = false,
                DuesPaid = false,
                IsWaived = false,
                IsEligible = false
            };
        }
    }

    public bool IsTransactionInProgress { get; set; } = false;

    public async Task<VersionCheckResult?> CheckForUpdatesApiAsync()
    {
        try
        {
            if (IsTransactionInProgress)
            {
                Console.WriteLine("[UPDATE] Update check deferred: transaction in progress.");
                return null;
            }

            if (!await _connectivity.GateAsync("VersionCheck"))
            {
                return null;
            }

            var timestamp = DateTime.UtcNow.Ticks;
            var response = await _http.GetAsync($"api/update/version-check?t={timestamp}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<VersionCheckResult>(_jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[UPDATE] Version check failed: {ex.Message}");
        }
        return null;
    }

    public void Dispose()
    {
        try
        {
            _syncTimer?.Dispose();
        }
        catch { }

        if (_connectivity != null)
        {
            _connectivity.ConnectivityChanged -= HandleConnectivityChanged;
        }
    }
}
