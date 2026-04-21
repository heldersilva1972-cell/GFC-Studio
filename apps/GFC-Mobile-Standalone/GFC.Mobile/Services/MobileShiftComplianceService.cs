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
            return await _http.GetFromJsonAsync<List<MissingShiftAlert>>($"/api/sync/compliance/missing?lookbackDays={lookbackDays}") ?? new();
        }
        catch
        {
            return new List<MissingShiftAlert>();
        }
    }
}
