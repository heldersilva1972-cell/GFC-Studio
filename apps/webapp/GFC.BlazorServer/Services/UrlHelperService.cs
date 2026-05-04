// [NEW]
using GFC.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace GFC.BlazorServer.Services;

public class UrlHelperService : IUrlHelperService
{
    private readonly IBlazorSystemSettingsService _systemSettingsService;
    private readonly NavigationManager _navigationManager;
    private readonly IConfiguration _configuration;

    public UrlHelperService(
        IBlazorSystemSettingsService systemSettingsService, 
        NavigationManager navigationManager,
        IConfiguration configuration)
    {
        _systemSettingsService = systemSettingsService;
        _navigationManager = navigationManager;
        _configuration = configuration;
    }

    public async Task<string> GetBaseUrlAsync()
    {
        var settings = await _systemSettingsService.GetAsync();
        
        const string scheme = "https";

        // 1. Check Database Settings
        if (!string.IsNullOrEmpty(settings?.PrimaryDomain))
        {
            return $"{scheme}://{settings.PrimaryDomain}";
        }
        
        // 2. Check appsettings.json domain (e.g., gfc.lovanow.com)
        var serverDomain = _configuration["Fido2:ServerDomain"];
        if (!string.IsNullOrEmpty(serverDomain) && serverDomain != "localhost")
        {
            return $"{scheme}://{serverDomain}";
        }
        
        // 3. Fallback: extract host from current URI
        var currentUri = new Uri(_navigationManager.BaseUri);
        
        // Only force HTTPS for non-localhost domains to allow local testing
        var finalScheme = currentUri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) 
            ? currentUri.Scheme 
            : scheme;
            
        var portString = currentUri.IsDefaultPort ? "" : $":{currentUri.Port}";
        
        return $"{finalScheme}://{currentUri.Host}{portString}";
    }
}
