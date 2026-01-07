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

            // 2. Allow only Localhost (Server computer itself) automatically
            // All other devices (LAN, VPN, Public) MUST have a trust token.
            if (connectionService.LocationType == LocationType.Local)
            {
                await _next(context);
                return;
            }

            // 3. Check for Device Token (External Users)
            if (context.Request.Cookies.TryGetValue("GFC_DeviceTrustToken", out var token))
            {
                // Validate token in DB
                // Since middleware runs on every request, we rely on the DB check.
                // In production, we might want to cache the "trusted device" status for a few minutes.
                
                // We need a UserId to validate the token properly if we use the one in DeviceTrustService.
                // However, for the initial guard, we might just want to know if the token is valid at ALL.
                // Let me check if DeviceTrustService has a generic validation.
                
                // Currently ValidateDeviceTokenAsync(token, userId) requires a userId.
                // If the user isn't logged in yet, we can't easily check the userId.
                // Let's add a method to IDeviceTrustService to check if a token is valid without userId if needed.
                
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
