using System.Net.Http.Json;
using GFC.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

namespace GFC.Mobile.Services;

public class MobileKeyCardService
{
    private readonly HttpClient _http;

    public MobileKeyCardService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<MobileKeyCardDashboardRow>> GetDashboardDataAsync(int year)
    {
        return await _http.GetFromJsonAsync<List<MobileKeyCardDashboardRow>>($"api/mobile-keycards/dashboard?year={year}") ?? new();
    }

    public async Task<bool> ToggleCardStatusAsync(int cardId)
    {
        var response = await _http.PostAsJsonAsync("api/mobile-keycards/toggle", new ToggleCardRequest { CardId = cardId });
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AssignCardAsync(AssignCardRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/mobile-keycards/assign", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ReactivateMemberAsync(int memberId)
    {
        var response = await _http.PostAsJsonAsync("api/mobile-keycards/reactivate-member", memberId);
        return response.IsSuccessStatusCode;
    }
}
