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

            // [NEW] 2. Device Trust Enforcement (Invite-Only Model)
            var settings = settingsService.GetSettings();
            var mode = settings?.AccessMode ?? AccessMode.Open;
            var remoteIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // LOCALHOST: Always allowed (you're on the server itself)
            if (connectionService.LocationType == LocationType.Local)
            {
                await _next(context);
                return;
            }

            // ALL OTHER CONNECTIONS: Must have valid device trust token
            bool hasValidDeviceTrust = false;
            string? token = null;

            if (context.Request.Cookies.TryGetValue("GFC_DeviceTrustToken", out token) && 
                !string.IsNullOrEmpty(token))
            {
                hasValidDeviceTrust = deviceTrustService.ValidateToken(token);
            }

            // If device is trusted, allow access
            if (hasValidDeviceTrust)
            {
                await _next(context);
                return;
            }

            // --- ALL CODE BELOW THIS POINT IS FOR UNTRUSTED DEVICES ---
            
            var logger = context.RequestServices.GetRequiredService<ILogger<DeviceGuardMiddleware>>();
            logger.LogWarning("Untrusted device access blocked. IP: {IP}, Location: {Loc}, Path: {Path}, TokenFound: {TokenFound}", 
                remoteIp, connectionService.LocationType, path, !string.IsNullOrEmpty(token));

            // NO VALID TOKEN: Block based on AccessMode
            
            // VPN: Blocked if not trusted (even VPN requires pre-approval)
            if (connectionService.LocationType == LocationType.VPN)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access Denied. This device must be approved by an administrator (VPN).");
                return;
            }

            // LAN: Blocked if not trusted OR if mode doesn't allow LAN
            if (connectionService.LocationType == LocationType.LAN)
            {
                if (mode == AccessMode.VpnOnly)
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Access Denied. VPN connection required.");
                    return;
                }
                
                // LAN is allowed by mode, but device still needs approval
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access Denied. This device must be approved by an administrator (LAN).");
                return;
            }

            // PUBLIC/UNKNOWN: Blocked unless mode is Open
            if (mode != AccessMode.Open)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access Denied. This system requires LAN or VPN connection.");
                return;
            }

            // Public connection with Open mode, but no device trust
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access Denied. This device must be approved by an administrator (Public).");
            return;
        }
    }
}
