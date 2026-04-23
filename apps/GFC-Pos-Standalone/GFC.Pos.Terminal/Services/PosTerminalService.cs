using System.Net.Http.Json;
using GFC.Core.DTOs;
using Microsoft.JSInterop;
using System.Text.Json;

namespace GFC.Pos.Terminal.Services;

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

    private const string PendingSalesKey    = "gfc_pos_pending_sales";
    private const string PendingZKey        = "gfc_pos_pending_z_reports";
    private const string CachedMenuKey      = "gfc_pos_cached_menu";
    private const int MaxAttempts           = 5;

    /// <summary>Raised when the pending count changes — UI badges subscribe to this.</summary>
    public event Action? OutboxChanged;

    public int PendingSalesCount  { get; private set; }
    public int PendingZCount      { get; private set; }
    public int TotalPendingCount  => PendingSalesCount + PendingZCount;

    public PosTerminalService(HttpClient http, IJSRuntime js, ConnectivityService connectivity)
    {
        _http = http;
        _js = js;
        _connectivity = connectivity;

        // Auto-flush everything the moment connectivity is restored
        _connectivity.ConnectivityChanged += async isOnline =>
        {
            if (isOnline)
            {
                Console.WriteLine("[POS] Connectivity restored — flushing outbox...");
                await FlushAllPendingAsync();
            }
        };
    }

    // ─── READ OPERATIONS ──────────────────────────────────────────────────────────────

    public async Task<bool> CheckConnectivityAsync()
    {
        try
        {
            var response = await _http.GetAsync("api/pos/menu");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<PosMenuDto> GetMenuAsync()
    {
        try
        {
            var menu = await _http.GetFromJsonAsync<PosMenuDto>("api/pos/menu");
            if (menu != null)
            {
                // Cache for offline startup
                await _js.InvokeVoidAsync("localStorage.setItem", CachedMenuKey, JsonSerializer.Serialize(menu));
                return menu;
            }
        }
        catch
        {
            // Fall back to cached menu
            var cached = await _js.InvokeAsync<string>("localStorage.getItem", CachedMenuKey);
            if (!string.IsNullOrEmpty(cached))
            {
                Console.WriteLine("[POS] Loaded menu from localStorage cache.");
                return JsonSerializer.Deserialize<PosMenuDto>(cached) ?? new PosMenuDto();
            }
        }
        return new PosMenuDto();
    }

    public async Task<PosSaleDto?> GetDartsRoundTodayAsync(string terminalName)
    {
        try { return await _http.GetFromJsonAsync<PosSaleDto>($"api/pos/darts-check/{terminalName}"); }
        catch { return null; }
    }

    public async Task<List<PosZReportDto>> GetZReportsAsync(string terminalName)
    {
        try { return await _http.GetFromJsonAsync<List<PosZReportDto>>($"api/pos/z-reports/{terminalName}") ?? new(); }
        catch { return new(); }
    }

    public async Task<PosZReportDto?> GetZReportAsync(Guid id)
    {
        try { return await _http.GetFromJsonAsync<PosZReportDto>($"api/pos/z-report/{id}"); }
        catch { return null; }
    }

    public async Task<DateTime> GetLastZTimeAsync(string terminalName)
    {
        try { return await _http.GetFromJsonAsync<DateTime>($"api/pos/last-z/{terminalName}"); }
        catch { return DateTime.Today; }
    }

    // ─── WRITE OPERATIONS (local-first) ──────────────────────────────────────────────

    public async Task SaveSaleAsync(PosSaleDto sale)
    {
        // Ensure stable ID for deduplication on retry
        if (sale.Id == Guid.Empty)
            sale.Id = Guid.NewGuid();

        // 1. Save locally first (instant, always succeeds)
        await BufferSaleLocallyAsync(sale);

        // 2. Flush in background — does not block caller
        if (_connectivity.IsOnline)
            _ = FlushAllPendingAsync();
    }

    public async Task SaveZReportAsync(PosZReportDto report)
    {
        await BufferZReportLocallyAsync(report);

        if (_connectivity.IsOnline)
            _ = FlushAllPendingAsync();
    }

    // ─── FLUSH (called on reconnect + after each successful save) ────────────────────

    public async Task FlushAllPendingAsync()
    {
        await FlushPendingSalesAsync();
        await FlushPendingZReportsAsync();
    }

    public async Task<int> GetTotalPendingAsync()
    {
        var sales = await GetPendingSalesAsync();
        var zs    = await GetPendingZReportsAsync();
        PendingSalesCount = sales.Count;
        PendingZCount     = zs.Count;
        return TotalPendingCount;
    }

    // ─── INTERNAL BUFFERING ───────────────────────────────────────────────────────────

    private async Task BufferSaleLocallyAsync(PosSaleDto sale)
    {
        var pending = await GetPendingSalesAsync();
        // Replace if same ID already buffered (idempotent)
        pending.RemoveAll(s => s.Id == sale.Id);
        pending.Add(sale);
        await _js.InvokeVoidAsync("localStorage.setItem", PendingSalesKey, JsonSerializer.Serialize(pending));
        PendingSalesCount = pending.Count;
        OutboxChanged?.Invoke();
    }

    private async Task BufferZReportLocallyAsync(PosZReportDto report)
    {
        var pending = await GetPendingZReportsAsync();
        pending.RemoveAll(z => z.Id == report.Id);
        pending.Add(report);
        await _js.InvokeVoidAsync("localStorage.setItem", PendingZKey, JsonSerializer.Serialize(pending));
        PendingZCount = pending.Count;
        OutboxChanged?.Invoke();
    }

    private async Task FlushPendingSalesAsync()
    {
        var pending = await GetPendingSalesAsync();
        if (!pending.Any()) return;

        var remaining = new List<PosSaleDto>();
        foreach (var sale in pending)
        {
            try
            {
                var resp = await _http.PostAsJsonAsync("api/pos/sale", sale);
                if (!resp.IsSuccessStatusCode) remaining.Add(sale);
                else Console.WriteLine($"[POS] ✓ Synced sale {sale.Id}");
            }
            catch { remaining.Add(sale); }
        }

        await _js.InvokeVoidAsync("localStorage.setItem", PendingSalesKey, JsonSerializer.Serialize(remaining));
        PendingSalesCount = remaining.Count;
        OutboxChanged?.Invoke();
    }

    private async Task FlushPendingZReportsAsync()
    {
        var pending = await GetPendingZReportsAsync();
        if (!pending.Any()) return;

        var remaining = new List<PosZReportDto>();
        foreach (var report in pending)
        {
            try
            {
                var resp = await _http.PostAsJsonAsync("api/pos/z-report", report);
                if (!resp.IsSuccessStatusCode) remaining.Add(report);
                else Console.WriteLine($"[POS] ✓ Synced Z-report {report.Id}");
            }
            catch { remaining.Add(report); }
        }

        await _js.InvokeVoidAsync("localStorage.setItem", PendingZKey, JsonSerializer.Serialize(remaining));
        PendingZCount = remaining.Count;
        OutboxChanged?.Invoke();
    }

    private async Task<List<PosSaleDto>> GetPendingSalesAsync()
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", PendingSalesKey);
        return string.IsNullOrEmpty(json) ? new() : JsonSerializer.Deserialize<List<PosSaleDto>>(json) ?? new();
    }

    private async Task<List<PosZReportDto>> GetPendingZReportsAsync()
    {
        var json = await _js.InvokeAsync<string>("localStorage.getItem", PendingZKey);
        return string.IsNullOrEmpty(json) ? new() : JsonSerializer.Deserialize<List<PosZReportDto>>(json) ?? new();
    }
}
