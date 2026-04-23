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
    private readonly ConnectivityService _connectivity;

    private const string OutboxKey = "gfc_mobile_outbox";
    private const int MaxAttempts = 5;
    private const int MaxRetentionDays = 7;

    // Raised whenever the outbox count changes so UI badges can update
    public event Action? OutboxChanged;

    public int PendingCount { get; private set; }

    public MobileReportingService(HttpClient http, IJSRuntime js, ConnectivityService connectivity)
    {
        _http = http;
        _js = js;
        _connectivity = connectivity;

        // Auto-flush when connectivity is restored
        _connectivity.ConnectivityChanged += async isOnline =>
        {
            if (isOnline) await FlushOutboxAsync();
        };
    }

    // ─── READ OPERATIONS (try server, fall back to nothing — reads don't go in outbox) ───

    public async Task<MobileShiftData> GetShiftReportDataAsync(DateTime date, string shiftType, bool isRental)
    {
        try
        {
            var url = $"/api/mobile-reporting/data?date={date:yyyy-MM-dd}&shiftType={shiftType}&isRental={isRental}";
            var data = await _http.GetFromJsonAsync<MobileShiftData>(url);
            return data ?? new MobileShiftData { Date = date, ShiftType = shiftType, IsRentalHall = isRental };
        }
        catch
        {
            return new MobileShiftData { Date = date, ShiftType = shiftType, IsRentalHall = isRental };
        }
    }

    public async Task<decimal> GetCarryoverCashAsync(DateTime date, string shiftType)
    {
        try
        {
            var url = $"/api/mobile-reporting/carryover?date={date:yyyy-MM-dd}&shiftType={shiftType}";
            return await _http.GetFromJsonAsync<decimal>(url);
        }
        catch { return 1200; }
    }

    public async Task<decimal> GetCumulativeBagDebtAsync(DateTime date)
    {
        try
        {
            var url = $"/api/mobile-reporting/bag-debt?date={date:yyyy-MM-dd}";
            return await _http.GetFromJsonAsync<decimal>(url);
        }
        catch { return 0; }
    }

    public async Task<DailyShiftSummary> GetDailySummaryAsync(DateTime date)
    {
        try
        {
            var url = $"/api/mobile-reporting/summary?date={date:yyyy-MM-dd}";
            return await _http.GetFromJsonAsync<DailyShiftSummary>(url) ?? new DailyShiftSummary();
        }
        catch { return new DailyShiftSummary(); }
    }

    public async Task<string> GetServerVersionAsync()
    {
        try { return await _http.GetStringAsync("/api/mobile-reporting/version"); }
        catch { return "GFC Mobile Revision 1.5.2 (Dynamic Sync)"; }
    }

    // ─── WRITE OPERATIONS (outbox-first) ─────────────────────────────────────────────────

    public async Task<bool> SaveShiftReportAsync(MobileShiftData data, string username)
    {
        data.ModifiedBy = username;
        await EnqueueAsync("SaveShiftReport", data);

        // Non-blocking background flush — user doesn't wait
        if (_connectivity.IsOnline)
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
        OutboxChanged?.Invoke();
    }

    public async Task FlushOutboxAsync()
    {
        var all = await LoadOutboxAsync();
        if (!all.Any()) return;

        var remaining = new List<OutboxEntry>();

        foreach (var entry in all)
        {
            // Discard entries older than MaxRetentionDays
            if ((DateTime.UtcNow - entry.SavedAt).TotalDays > MaxRetentionDays)
            {
                Console.WriteLine($"[Outbox] Discarding expired entry {entry.Id} (>{MaxRetentionDays} days old)");
                continue;
            }

            if (entry.Attempts >= MaxAttempts)
            {
                Console.WriteLine($"[Outbox] Giving up on {entry.Id} after {MaxAttempts} attempts");
                continue;
            }

            try
            {
                var data = JsonSerializer.Deserialize<MobileShiftData>(entry.Payload);
                if (data == null) continue;

                var endpoint = entry.Type == "SubmitShiftReport"
                    ? $"/api/mobile-reporting/submit?username={data.ModifiedBy}"
                    : $"/api/mobile-reporting/save?username={data.ModifiedBy}";

                var resp = await _http.PostAsJsonAsync(endpoint, data);

                if (resp.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[Outbox] ✓ Synced {entry.Type} entry {entry.Id}");
                    entry.SyncedAt = DateTime.UtcNow;
                    // Don't keep — it's been synced
                }
                else
                {
                    entry.Attempts++;
                    remaining.Add(entry);
                }
            }
            catch
            {
                entry.Attempts++;
                remaining.Add(entry);
            }
        }

        await SaveOutboxAsync(remaining);
        PendingCount = remaining.Count;
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
            var json = await _js.InvokeAsync<string>("localStorage.getItem", OutboxKey);
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
            await _js.InvokeVoidAsync("localStorage.setItem", OutboxKey, JsonSerializer.Serialize(entries));
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
