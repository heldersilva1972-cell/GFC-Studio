using GFC.Core.Interfaces;
using GFC.Core.Enums;
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
            "/login",
            "/api", 
            "/error", 
            "/_framework", 
            "/_content", 
            "/_blazor",
            "/css", 
            "/js", 
            "/images",
            "/bootstrap",
            "/app.css",
            "/favicon",
            "/manifest.json",
            "/service-worker",
            "/pwa-icons",
            "/animationhub",
            "/studiopreviewhub",
            "/videoaccesshub"
        };

        public DeviceGuardMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, 
            IUserConnectionService connectionService, 
            IBlazorSystemSettingsService settingsService,
            IDeviceTrustService deviceTrustService)
        {
            connectionService.DetectConnectionIfNeeded();
            
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // 1. Allow Public Paths (Login, Setup, Assets)
            foreach (var publicPath in PublicPaths)
            {
                if (path.StartsWith(publicPath.ToLower()))
                {
                    await _next(context);
                    return;
                }
            }

            // [NEW] 2. Honor Access Mode
            var settings = settingsService.GetSettings();
            var mode = settings?.AccessMode ?? AccessMode.Open;

            // Always allow Localhost (Server computer itself)
            if (connectionService.LocationType == LocationType.Local)
            {
                await _next(context);
                return;
            }

            // Open access: allow everyone (still subject to login)
            if (mode == AccessMode.Open)
            {
                await _next(context);
                return;
            }

            // LAN or VPN
            if (mode == AccessMode.LanOrVpn && 
               (connectionService.LocationType == LocationType.LAN || connectionService.LocationType == LocationType.VPN))
            {
                await _next(context);
                return;
            }

            // VPN Only
            if (mode == AccessMode.VpnOnly && connectionService.LocationType == LocationType.VPN)
            {
                await _next(context);
                return;
            }

            // 3. Fallback: Check for Device Token (External Users on Public networks)
            if (context.Request.Cookies.TryGetValue("GFC_DeviceTrustToken", out var token))
            {
                // For now, if the cookie exists, let them reach the login page.
                // The login page itself will then link the device once they authenticate.
                await _next(context);
                return;
            }

            // 4. Redirect Unauthorized External Users
            context.Response.Redirect("/setup/request-access");
            return;
        }
    }
}
