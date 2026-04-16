using GFC.Core.Interfaces;
using GFC.Core.DTOs;
using System.Net.Http.Json;

namespace GFC.Client.Services;

public class MobileReportingService : IMobileReportingService
{
    private readonly HttpClient _http;

    public MobileReportingService(HttpClient http)
    {
        _http = http;
    }

    public async Task<MobileShiftData> GetShiftReportDataAsync(DateTime date, string shiftType, bool isRental)
    {
        var response = await _http.GetFromJsonAsync<MobileShiftData>($"api/mobile-reporting/data?date={date:yyyy-MM-dd}&shiftType={shiftType}&isRental={isRental}");
        return response ?? new MobileShiftData { Date = date, ShiftType = shiftType, IsRentalHall = isRental };
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
}
