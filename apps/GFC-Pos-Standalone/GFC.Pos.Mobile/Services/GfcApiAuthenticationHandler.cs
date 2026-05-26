using System.Net.Http.Headers;
using Microsoft.JSInterop;
using Microsoft.Maui.Storage;

namespace GFC.Pos.Mobile.Services;

public class GfcApiAuthenticationHandler : DelegatingHandler
{
    private readonly IJSRuntime _js;

    public GfcApiAuthenticationHandler(IJSRuntime js)
    {
        _js = js;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Get Token and DeviceId from MAUI SecureStorage
            string? token = null;
            string? deviceId = null;

            try
            {
                token = await SecureStorage.Default.GetAsync("gfc_device_token");
                deviceId = await SecureStorage.Default.GetAsync("gfc_device_id");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GfcApiAuthenticationHandler] SecureStorage read error: {ex.Message}");
            }

            // 2. Fallback to localStorage via JS Interop if running in Blazor WebView environment
            if (string.IsNullOrEmpty(token))
            {
                try
                {
                    token = await _js.InvokeAsync<string>("localStorage.getItem", "gfc_device_token");
                }
                catch { }
            }
            if (string.IsNullOrEmpty(deviceId))
            {
                try
                {
                    deviceId = await _js.InvokeAsync<string>("localStorage.getItem", "gfc_device_id");
                }
                catch { }
            }

            // 3. Inject Bearer Authentication Header if not already present
            if (!string.IsNullOrEmpty(token) && !request.Headers.Contains("Authorization"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // 4. Inject Custom X-Device-ID Header if not already present
            if (!string.IsNullOrEmpty(deviceId) && !request.Headers.Contains("X-Device-ID"))
            {
                request.Headers.Add("X-Device-ID", deviceId);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[GfcApiAuthenticationHandler] Error injecting auth headers: {ex.Message}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
