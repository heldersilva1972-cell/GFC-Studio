using System.Net.Http.Json;
using GFC.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace GFC.Mobile.Services;

public class MobileDuesService
{
    private readonly HttpClient _http;

    public MobileDuesService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DuesListItemDto>> GetDuesListAsync(int year, bool paidTab)
    {
        return await _http.GetFromJsonAsync<List<DuesListItemDto>>($"api/mobile-dues/list?year={year}&paidTab={paidTab}") ?? new();
    }

    public async Task<DuesSummaryDto> GetSummaryAsync(int year)
    {
        return await _http.GetFromJsonAsync<DuesSummaryDto>($"api/mobile-dues/summary?year={year}") ?? new DuesSummaryDto(year, 0, 0, 0, 0, 0);
    }
}
