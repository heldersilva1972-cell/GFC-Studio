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
    private readonly IJSRuntime _js;
    private readonly IServiceProvider _serviceProvider;

    public MobileAuthenticationHandler(NavigationManager navigation, IJSRuntime js, IServiceProvider serviceProvider)
    {
        _navigation = navigation;
        _js = js;
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
            string? token = null;
            var authStateJson = await _js.InvokeAsync<string>("localStorage.getItem", "gfc_auth_state");
            if (!string.IsNullOrEmpty(authStateJson))
            {
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(authStateJson);
                    if (doc.RootElement.TryGetProperty("token", out var tokenProp) || doc.RootElement.TryGetProperty("Token", out tokenProp))
                    {
                        token = tokenProp.GetString();
                    }
                }
                catch { }
            }

            if (string.IsNullOrEmpty(token))
            {
                token = await _js.InvokeAsync<string>("localStorage.getItem", "gfc_device_token");
            }
            
            if (!string.IsNullOrEmpty(token) && !request.Headers.Contains("Authorization"))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"[AUTH] Injected Bearer token for request: {request.RequestUri}");
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
            
            // Log the 401 error to localStorage for diagnostic visibility
            try
            {
                _ = _js.InvokeVoidAsync("localStorage.setItem", "gfc_last_401_error", $"URL: {url} | Time: {DateTime.UtcNow:u}");
            }
            catch { }

            // [LOOP PROTECTION] Do not trigger logout if the failure happened ON an auth endpoint
            if (url.Contains("/api/mobile-auth/login") || url.Contains("/api/mobile-auth/user"))
            {
                Console.WriteLine($"[AUTH] 401 rejected on auth endpoint: {url}. Ignoring to prevent loop.");
                return response;
            }

            Console.WriteLine($"[AUTH] 401 Unauthorized detected on: {url}. Resetting session.");

            // Bypass forced logout/redirect if running on localhost for easier development debugging
            var currentUrl = _navigation.Uri;
            if (currentUrl.Contains("localhost") || currentUrl.Contains("127.0.0.1"))
            {
                Console.WriteLine($"[DEVELOPMENT] Bypassing automatic 401 redirect for: {url}");
                return response;
            }
            
            // Avoid circular dependencies by resolving the provider via IServiceProvider
            using var resolveScope = _serviceProvider.CreateScope();
            var authProvider = resolveScope.ServiceProvider.GetService<ICustomAuthenticationStateProvider>();
            
            if (authProvider != null)
            {
                await authProvider.LogoutAsync();
            }

            // Force redirect to login if we aren't already there
            if (!currentUrl.Contains("/login"))
            {
                _navigation.NavigateTo("/login", forceLoad: true);
            }
        }

        return response;
    }
}
