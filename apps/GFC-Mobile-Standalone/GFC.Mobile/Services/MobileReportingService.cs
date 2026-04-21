using System.Net.Http.Json;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Mobile.Services;

public class MobileReportingService : IMobileReportingService
{
    private readonly HttpClient _http;

    public MobileReportingService(HttpClient http)
    {
        _http = http;
    }

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

    public async Task<bool> SaveShiftReportAsync(MobileShiftData data, string username)
    {
        data.ModifiedBy = username;
        var response = await _http.PostAsJsonAsync($"/api/mobile-reporting/save?username={username}", data);
        return response.IsSuccessStatusCode;
    }

    public async Task<decimal> GetCarryoverCashAsync(DateTime date, string shiftType)
    {
        try
        {
            var url = $"/api/mobile-reporting/carryover?date={date:yyyy-MM-dd}&shiftType={shiftType}";
            return await _http.GetFromJsonAsync<decimal>(url);
        }
        catch
        {
            return 1200; 
        }
    }

    public async Task<decimal> GetCumulativeBagDebtAsync(DateTime date)
    {
        try
        {
            var url = $"/api/mobile-reporting/bag-debt?date={date:yyyy-MM-dd}";
            return await _http.GetFromJsonAsync<decimal>(url);
        }
        catch
        {
            return 0;
        }
    }

    public async Task<bool> SubmitFinalReportAsync(MobileShiftData data, string username)
    {
        data.Status = "Submitted";
        var response = await _http.PostAsJsonAsync($"/api/mobile-reporting/submit?username={username}", data);
        return response.IsSuccessStatusCode;
    }

    public async Task<DailyShiftSummary> GetDailySummaryAsync(DateTime date)
    {
        try
        {
            var url = $"/api/mobile-reporting/summary?date={date:yyyy-MM-dd}";
            return await _http.GetFromJsonAsync<DailyShiftSummary>(url) ?? new DailyShiftSummary();
        }
        catch
        {
            return new DailyShiftSummary();
        }
    }

    public async Task<string> GetServerVersionAsync()
    {
        try
        {
            return await _http.GetStringAsync("/api/mobile-reporting/version");
        }
        catch
        {
            return "1.5.2-Production";
        }
    }
}
