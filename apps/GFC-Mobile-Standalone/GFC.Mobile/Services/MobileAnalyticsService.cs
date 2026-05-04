using System.Net.Http.Json;
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace GFC.Mobile.Services;

public class MobileAnalyticsService
{
    private readonly HttpClient _http;

    public MobileAnalyticsService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<int>> GetAvailableYearsAsync()
    {
        return await _http.GetFromJsonAsync<List<int>>("api/mobile-analytics/years") ?? new();
    }

    public async Task<List<FinancialDataPoint>> GetAggregatedDataAsync(FinancialAnalyticsRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/mobile-analytics/data", request);
        return await response.Content.ReadFromJsonAsync<List<FinancialDataPoint>>() ?? new();
    }

    public async Task<FinancialSummary> GetSummaryAsync(FinancialAnalyticsRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/mobile-analytics/summary", request);
        return await response.Content.ReadFromJsonAsync<FinancialSummary>() ?? new();
    }
}
