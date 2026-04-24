using GFC.Core.Interfaces;
using System.Net.Http.Json;

namespace GFC.Mobile.Services;

public class MobileShiftComplianceService : IShiftComplianceService
{
    private readonly HttpClient _http;

    public MobileShiftComplianceService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<MissingShiftAlert>> GetMissingShiftsAsync(int lookbackDays = 2)
    {
        try
        {
            // [HARDENING] Check connectivity first to prevent hanging the Portal UI
            using var cts = new System.Threading.CancellationTokenSource(System.TimeSpan.FromSeconds(3));
            return await _http.GetFromJsonAsync<List<MissingShiftAlert>>($"/api/sync/compliance/missing?lookbackDays={lookbackDays}", cts.Token) ?? new();
        }
        catch { return new List<MissingShiftAlert>(); }
    }
}
