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

            // 1a. EMERGENCY BYPASS: If a token query param is present, let it through.
            // This ensures that even if the user lands on the wrong path (e.g. /?token=...) or 
            // the path matching fails, the application layer can handle the redirection/activation.
            if (context.Request.Query.ContainsKey("token"))
            {
                await _next(context);
                return;
            }

            // [NEW] 2. Device Trust Enforcement (Invite-Only Model)
            var settings = settingsService.GetSettings();
            var mode = settings?.AccessMode ?? AccessMode.Open;
            var remoteIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // HOST BYPASS: Always allowed only from the server itself (localhost)
            if (connectionService.LocationType == LocationType.Local)
            {
                await _next(context);
                return;
            }

            // [FIX] RESPECT ACCESS MODE
            // If the system is in Open mode, allow all LAN traffic (but still block public internet if needed, 
            // though Open usually implies fully open).
            if (mode == AccessMode.Open)
            {
                await _next(context);
                return;
            }

            // ALL EXTERNAL DEVICES (LAN, VPN, PUBLIC): Must have valid device trust token
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

            // --- ALL CODE BELOW THIS POINT IS FOR UNTRUSTED ACCESS ---
            
            var logger = context.RequestServices.GetRequiredService<ILogger<DeviceGuardMiddleware>>();
            logger.LogWarning("Untrusted device access blocked. IP: {IP}, Location: {Loc}, Path: {Path}", 
                remoteIp, connectionService.LocationType, path);

            // Blocked
            context.Response.StatusCode = 403;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync($@"
                <html>
                <body style='font-family: sans-serif; display: flex; justify-content: center; align-items: center; height: 100vh; background: #f8f9fa;'>
                    <div style='max-width: 500px; text-align: center; padding: 2rem; background: white; border-radius: 8px; box-shadow: 0 4px 15px rgba(0,0,0,0.1); border-top: 5px solid #dc3545;'>
                        <h2 style='color: #dc3545;'><i class='bi bi-shield-lock'></i> Access Shield Active</h2>
                        <p>This device is not registered to access the GFC System.</p>
                        <hr style='border: 0; border-top: 1px solid #eee; margin: 1.5rem 0;' />
                        <p style='font-size: 0.9rem; color: #666;'>To access the system, an administrator must provide you with a <strong>Secure Setup Link</strong>.</p>
                        <div style='background: #fff3f3; color: #856404; padding: 1rem; border-radius: 4px; margin-top: 1rem; font-size: 0.85rem; border: 1px solid #ffeeba;'>
                             <strong>Security Policy:</strong> External devices are restricted by default, even on the local network.
                        </div>
                        <div style='margin-top: 2rem; font-size: 0.8rem; color: #aaa; text-align: left;'>
                            <div>IP: {remoteIp}</div>
                            <div>Location: {connectionService.LocationType}</div>
                            <div>Path: {path}</div>
                            <div>HasTokenParam: {context.Request.Query.ContainsKey("token")}</div>
                            <div style='margin-top: 5px; color: #ccc;'>v.2026.01.08.0150</div>
                        </div>
                    </div>
                </body>
                </html>");
            return;
        }
    }
}
