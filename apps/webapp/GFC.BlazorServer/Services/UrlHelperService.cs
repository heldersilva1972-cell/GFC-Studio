// [NEW]
using GFC.Core.Interfaces;
using Microsoft.AspNetCore.Components;

namespace GFC.BlazorServer.Services;

public class UrlHelperService : IUrlHelperService
{
    private readonly IBlazorSystemSettingsService _systemSettingsService;
    private readonly NavigationManager _navigationManager;

    public UrlHelperService(IBlazorSystemSettingsService systemSettingsService, NavigationManager navigationManager)
    {
        _systemSettingsService = systemSettingsService;
        _navigationManager = navigationManager;
    }

    public async Task<string> GetBaseUrlAsync()
    {
        var settings = await _systemSettingsService.GetAsync();
        if (!string.IsNullOrEmpty(settings?.PrimaryDomain))
        {
            return $"https://{settings.PrimaryDomain}";
        }
        
        // Fallback to official domain if settings are empty (prevents IP leakage)
        return "https://gfc.lovanow.com";
    }
}
