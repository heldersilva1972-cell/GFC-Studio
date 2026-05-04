using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using Microsoft.Extensions.DependencyInjection;
using GFC.Mobile.Services;

namespace GFC.Mobile.Auth;

public class MobileAuthenticationHandler : DelegatingHandler
{
    private readonly NavigationManager _navigation;
    private readonly IServiceProvider _serviceProvider;

    public MobileAuthenticationHandler(NavigationManager navigation, IServiceProvider serviceProvider)
    {
        _navigation = navigation;
        _serviceProvider = serviceProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // [WASM AUTH FIX] Force the browser to include cookies for cross-origin requests
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        // [STABILITY FIX] Inject token from localStorage into Authorization header
        // This is the most reliable way to authenticate cross-origin standalone mobile apps.
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var js = scope.ServiceProvider.GetRequiredService<IJSRuntime>();
            var token = await js.InvokeAsync<string>("localStorage.getItem", "gfc_device_token");
            
            if (!string.IsNullOrEmpty(token) && !request.Headers.Contains("Authorization"))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AUTH] Failed to inject auth header: {ex.Message}");
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var url = request.RequestUri?.ToString() ?? "unknown";
            
            // [LOOP PROTECTION] Do not trigger logout if the failure happened ON an auth endpoint
            if (url.Contains("/api/mobile-auth/login") || url.Contains("/api/mobile-auth/user"))
            {
                Console.WriteLine($"[AUTH] 401 rejected on auth endpoint: {url}. Ignoring to prevent loop.");
                return response;
            }

            Console.WriteLine($"[AUTH] 401 Unauthorized detected on: {url}. Resetting session.");
            
            // Avoid circular dependencies by resolving the provider via IServiceProvider
            using var scope = _serviceProvider.CreateScope();
            var authProvider = scope.ServiceProvider.GetService<ICustomAuthenticationStateProvider>();
            
            if (authProvider != null)
            {
                await authProvider.LogoutAsync();
            }

            // Force redirect to login if we aren't already there
            var currentUrl = _navigation.Uri;
            if (!currentUrl.Contains("/login"))
            {
                _navigation.NavigateTo("/login", forceLoad: true);
            }
        }

        return response;
    }
}
