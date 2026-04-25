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
                            audit.GrossTotal += data.TotalAmount;
                            if (data.PaymentType == "CASH") audit.CashTotal += data.TotalAmount;

                            var items = JsonSerializer.Deserialize<List<GFC.Pos.UI.Pages.PosTerminal.ProductItem>>(data.ItemsJson, _jsonOptions);
                            if (items != null)
                            {
                                foreach (var i in items)
                                {
                                    if (!audit.ItemSummary.ContainsKey(i.Name)) audit.ItemSummary[i.Name] = 0;
                                    audit.ItemSummary[i.Name] += i.Quantity;
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
                Console.WriteLine("[SYNC TRACE] Connectivity restored — sweeping vault...");
                await SafeFlushAsync();
            }
        };
    }

    private async Task SafeFlushAsync()
    {
        if (!await _syncLock.WaitAsync(0)) return;
        try { await FlushAllPendingAsync(); }
        finally { _syncLock.Release(); }
    }

    // ─── READ OPERATIONS ──────────────────────────────────────────────────────────────

    public async Task<bool> CheckConnectivityAsync() => await _connectivity.CheckServerReachableAsync();

    public async Task<PosMenuDto> GetMenuAsync()
    {
        // 1. Try Server (if online)
        try
        {
            if (await _connectivity.GateAsync("GetMenu"))
            {
                var menu = await _http.GetFromJsonAsync<PosMenuDto>("api/pos/menu");
                if (menu != null && menu.Items.Any())
                {
                    await _js.InvokeVoidAsync("window.gfcSetAsync", CachedMenuKey, menu);
                    return menu;
                }
            }
        }
        catch { }

        // 2. Fallback to Vault
        try
        {
            var cached = await _js.InvokeAsync<string>("window.gfcGetAsync", CachedMenuKey);
            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<PosMenuDto>(cached, _jsonOptions) ?? new PosMenuDto();
            }
        }
        catch { }

        return new PosMenuDto();
    }

    public async Task<List<UserListItemDto>> GetAuthorizedUsersAsync()
    {
        // 1. Try Server
        try
        {
            if (await _connectivity.GateAsync("GetUsers"))
            {
                var users = await _http.GetFromJsonAsync<List<UserListItemDto>>("api/pos/users");
                if (users != null)
                {
                    await _js.InvokeVoidAsync("window.gfcSetAsync", AuthorizedUsersKey, users);
                    return users;
                }
            }
        }
        catch { }

        // 2. Fallback to Vault
        try
        {
            var cached = await _js.InvokeAsync<string>("window.gfcGetAsync", AuthorizedUsersKey);
            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<List<UserListItemDto>>(cached, _jsonOptions) ?? new();
            }
        }
        catch { }

        return new();
    }

    public async Task<PosSaleDto?> GetDartsRoundTodayAsync(string terminalName)
    {
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
                        var resp = await _http.PostAsJsonAsync("api/pos/sale", data);
                        if (resp.IsSuccessStatusCode) {
                            await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                            LastSynced = DateTime.Now;
                        }
                    }
                }
                
                if (key.StartsWith(VaultPrefixZ)) {
                    var data = JsonSerializer.Deserialize<PosZReportDto>(item.GetProperty("data").GetRawText(), _jsonOptions);
                    if (data != null) {
                        var resp = await _http.PostAsJsonAsync("api/pos/z-report", data);
                        if (resp.IsSuccessStatusCode) {
                            await _js.InvokeVoidAsync("window.gfcRemoveAsync", key);
                            LastSynced = DateTime.Now;
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
