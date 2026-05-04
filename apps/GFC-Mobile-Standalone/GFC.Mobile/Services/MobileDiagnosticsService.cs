using System.Net.Http.Json;
using GFC.Core.Models;
using GFC.Core.Models.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;

namespace GFC.Mobile.Services;

public class MobileDiagnosticsService
{
    private readonly HttpClient _http;

    public MobileDiagnosticsService(HttpClient http)
    {
        _http = http;
    }

    public async Task<SystemDiagnosticsInfo> GetStatsAsync()
    {
        return await _http.GetFromJsonAsync<SystemDiagnosticsInfo>("api/mobile-diagnostics/stats") ?? new();
    }

    public async Task<DiagnosticActionResult> TestDatabaseAsync()
    {
        var response = await _http.PostAsync("api/mobile-diagnostics/test-db", null);
        return await response.Content.ReadFromJsonAsync<DiagnosticActionResult>() ?? new();
    }
}
