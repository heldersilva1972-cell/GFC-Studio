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
        
        // [PWA FIX] Always use HTTPS for generated links, regardless of how admin is accessing
        // This ensures PWA installation works correctly on mobile devices
        const string scheme = "https";

        if (!string.IsNullOrEmpty(settings?.PrimaryDomain))
        {
            return $"{scheme}://{settings.PrimaryDomain}";
        }
        
        // Fallback: extract host from current URI but force HTTPS
        var currentUri = new Uri(_navigationManager.BaseUri);
        return $"{scheme}://{currentUri.Host}";
    }
}
