using GFC.Core.Interfaces;
using GFC.Core.DTOs;
using GFC.Core.Models;
using System.Net.Http.Json;

namespace GFC.Client.Services;

public class MobileReportingService : IMobileReportingService
{
    private readonly HttpClient _http;

    public MobileReportingService(HttpClient http)
    {
        _http = http;
    }

    public int PendingCount => 0;
    public DateTime? LastSyncTime => null;
    public string? LastSyncStatus => null;
    public Task<int> GetPendingCountAsync() => Task.FromResult(0);
    public Task FlushOutboxAsync() => Task.CompletedTask;
    public event Action? OutboxChanged;

    public async Task<MobileShiftData> GetShiftReportDataAsync(DateTime date, string shiftType, bool isRental)
    {
        var response = await _http.GetFromJsonAsync<MobileShiftData>($"api/mobile-reporting/data?date={date:yyyy-MM-dd}&shiftType={shiftType}&isRental={isRental}");
        return response ?? new MobileShiftData { Date = date, ShiftType = shiftType, IsRentalHall = isRental };
    }

    public async Task<bool> IsShiftSubmittedAsync(DateTime date, string shiftType, bool isRental)
    {
        return await _http.GetFromJsonAsync<bool>($"api/mobile-reporting/is-submitted?date={date:yyyy-MM-dd}&shiftType={shiftType}&isRental={isRental}");
    }

    public async Task<decimal> GetCarryoverCashAsync(DateTime date, string shiftType)
    {
        return await _http.GetFromJsonAsync<decimal>($"api/mobile-reporting/carryover?date={date:yyyy-MM-dd}&shiftType={shiftType}");
    }

    public async Task<decimal> GetCumulativeBagDebtAsync(DateTime date)
    {
        return await _http.GetFromJsonAsync<decimal>($"api/mobile-reporting/bag-debt?date={date:yyyy-MM-dd}");
    }

    public async Task<bool> SaveShiftReportAsync(MobileShiftData data, string username)
    {
        var response = await _http.PostAsJsonAsync($"api/mobile-reporting/save?username={username}", data);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> SubmitFinalReportAsync(MobileShiftData data, string username)
    {
        var response = await _http.PostAsJsonAsync($"api/mobile-reporting/submit?username={username}", data);
        return response.IsSuccessStatusCode;
    }

    public async Task<DailyShiftSummary> GetDailySummaryAsync(DateTime date)
    {
        return await _http.GetFromJsonAsync<DailyShiftSummary>($"api/mobile-reporting/summary?date={date:yyyy-MM-dd}") 
               ?? new DailyShiftSummary();
    }
    public async Task<string> GetServerVersionAsync()
    {
        return await _http.GetStringAsync("api/mobile-reporting/version");
    }

    public async Task<LotteryCommissionRate> GetLotteryRateAsync(int year)
    {
        return await _http.GetFromJsonAsync<LotteryCommissionRate>($"api/mobile-reporting/lottery-rate?year={year}") 
               ?? new LotteryCommissionRate { Year = year };
    }

    public async Task<List<BingoSheetDefinition>> GetBingoProgramAsync()
    {
        return await _http.GetFromJsonAsync<List<BingoSheetDefinition>>("api/bingo/program") ?? new List<BingoSheetDefinition>();
    }

    public async Task<List<BingoAdmissionDefinition>> GetBingoAdmissionsAsync()
    {
        return await _http.GetFromJsonAsync<List<BingoAdmissionDefinition>>("api/bingo/admissions") ?? new List<BingoAdmissionDefinition>();
    }

    public async Task<bool> SubmitBingoSessionAsync(BingoSession session, string username)
    {
        var response = await _http.PostAsJsonAsync("api/bingo/session", session);
        return response.IsSuccessStatusCode;
    }

    public async Task<BingoSettingsDto> GetBingoSettingsAsync()
    {
        return await _http.GetFromJsonAsync<BingoSettingsDto>("api/bingo/settings") ?? new BingoSettingsDto();
    }
}
