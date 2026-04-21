using GFC.Core.Interfaces;
using System.Net.Http.Json;

namespace GFC.Mobile.Services;

public class MobileUserUsageService : IUserUsageService
{
    private readonly HttpClient _http;

    public MobileUserUsageService(HttpClient http)
    {
        _http = http;
    }

    public async Task TrackPageUsageAsync(int userId, string pageIdentifier)
    {
        try {
            await _http.PostAsJsonAsync("/api/sync/usage/track", new { UserId = userId, PageIdentifier = pageIdentifier });
        } catch { }
    }

    public async Task<List<string>> GetTopUsedPagesAsync(int userId, int count = 3)
    {
        try
        {
            return await _http.GetFromJsonAsync<List<string>>($"/api/sync/usage/top?userId={userId}&count={count}") ?? new();
        }
        catch
        {
            return new List<string> { "End of shift sales" };
        }
    }
}
