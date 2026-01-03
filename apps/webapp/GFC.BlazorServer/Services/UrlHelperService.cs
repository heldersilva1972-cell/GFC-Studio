// [NEW]
using GFC.Core.Interfaces;

namespace GFC.BlazorServer.Services;

public class UrlHelperService : IUrlHelperService
{
    private readonly IBlazorSystemSettingsService _systemSettingsService;

    public UrlHelperService(IBlazorSystemSettingsService systemSettingsService)
    {
        _systemSettingsService = systemSettingsService;
    }

    public async Task<string> GetBaseUrlAsync()
    {
        var settings = await _systemSettingsService.GetAsync();
        if (!string.IsNullOrEmpty(settings?.PrimaryDomain))
        {
            return $"https://{settings.PrimaryDomain}";
        }
        return "https://localhost"; // Fallback for development
    }
}
