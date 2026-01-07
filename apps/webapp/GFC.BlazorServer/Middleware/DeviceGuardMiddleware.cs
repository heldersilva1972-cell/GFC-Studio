using GFC.Core.Interfaces;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Middleware
{
    public class DeviceGuardMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly string[] PublicPaths = new[] 
        { 
            "/setup", 
            "/api/onboarding", 
            "/error", 
            "/_framework", 
            "/_content", 
            "/css", 
            "/js", 
            "/img",
            "/manifest.json",
            "/service-worker.js",
            "/pwa-icons"
        };

        public DeviceGuardMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserConnectionService connectionService, IBlazorSystemSettingsService settingsService)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // 1. Allow Public Paths (Login, Setup, Assets)
            foreach (var publicPath in PublicPaths)
            {
                if (path.StartsWith(publicPath))
                {
                    await _next(context);
                    return;
                }
            }

            // 2. Allow Local/LAN/VPN Connections automatically
            if (connectionService.LocationType != LocationType.Public)
            {
                await _next(context);
                return;
            }

            // 3. Check for Device Token (External Users)
            if (context.Request.Cookies.ContainsKey("GFC_DeviceTrustToken"))
            {
                // TODO: Validate token in DB if we want extreme security, 
                // but for "User-Friendly" we can trust the cookie for now.
                await _next(context);
                return;
            }

            // 4. Redirect Unauthorized External Users
            // Instead of showing the login page, we show a "Security Guard" message
            // or redirect to the setup page if that's what's expected.
            context.Response.Redirect("/setup/request-access");
        }
    }
}
