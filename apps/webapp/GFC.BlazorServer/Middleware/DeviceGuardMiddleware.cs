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
            "/setup/wizard",
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
            "/videoaccesshub",
            "/version.txt",
            "/version.json",
            "/Download/GFC_POS_Mobile.apk",
            "/Download/com.gfc.pos.mobile-Signed.apk"
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
            var path = context.Request.Path.Value?.ToLower() ?? "";

            // 1. FAST PATH: Allow Public Paths (Login, Setup, Assets)
            // Checked first to avoid hitting DB for static files
            foreach (var publicPath in PublicPaths)
            {
                if (path.StartsWith(publicPath.ToLower()))
                {
                    await _next(context);
                    return;
                }
            }

            // [CORS FIX] Explicitly bypass preflight OPTIONS requests
            if (context.Request.Method == "OPTIONS")
            {
                await _next(context);
                return;
            }

            // 1a. EMERGENCY BYPASS for tokens
            if (context.Request.Query.ContainsKey("token"))
            {
                await _next(context);
                return;
            }

            // 2. RUN SECURE DETECTION ASYNC
            await connectionService.DetectConnectionIfNeededAsync();
            
            // 3. Device Trust Enforcement
            var settings = await settingsService.GetAsync();
            var mode = settings?.AccessMode ?? AccessMode.Open;
            var remoteIp = connectionService.IpAddress ?? "unknown";

            // [FIX] RESPECT ACCESS MODE
            // 1. Local Bypass (Host PC always allowed)
            if (connectionService.LocationType == LocationType.Local)
            {
                await _next(context);
                return;
            }

            // 2. VPN-Only Mode Enforcement
            if (mode == AccessMode.VpnOnly && connectionService.LocationType != LocationType.VPN)
            {
                // Fall through to block below for non-VPN connections in VPN-only mode
            }
            else
            {
                // 3. Device Trust Check (Cookies)
                bool hasValidDeviceTrust = false;
                string? cookieToken = null;

                // Check for regular user identity token
                if (context.Request.Cookies.TryGetValue("GFC_DeviceTrustToken", out cookieToken) && 
                    !string.IsNullOrEmpty(cookieToken))
                {
                    hasValidDeviceTrust = await deviceTrustService.ValidateTokenAsync(cookieToken);
                }

                // [NEW] Check for machine-specific station identity
                if (!hasValidDeviceTrust && 
                    context.Request.Cookies.TryGetValue("GFC_StationIdentity", out var stationToken) && 
                    !string.IsNullOrEmpty(stationToken))
                {
                    hasValidDeviceTrust = await deviceTrustService.IsStationTokenAsync(stationToken);
                }

                if (hasValidDeviceTrust)
                {
                    await _next(context);
                    return;
                }

                // 4. "Open Access" Mode Bypasses (LAN/VPN Only)
                if (mode == AccessMode.Open)
                {
                    if (connectionService.LocationType == LocationType.LAN || 
                        connectionService.LocationType == LocationType.VPN)
                    {
                        await _next(context);
                        return;
                    }
                }
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
                        
                        <div style='margin: 1.5rem 0;'>
                            <a href='/setup/recovery' style='display: inline-block; padding: 12px 24px; background: #007bff; color: white; text-decoration: none; border-radius: 4px; font-weight: bold; box-shadow: 0 2px 5px rgba(0,0,0,0.1);'>
                                <i class='bi bi-key-fill'></i> Restore Access with Setup Code
                            </a>
                        </div>

                        <div style='background: #fff3f3; color: #856404; padding: 1rem; border-radius: 4px; margin-top: 1rem; font-size: 0.85rem; border: 1px solid #ffeeba;'>
                             <strong>Security Policy:</strong> External devices are restricted by default, even on the local network.
                        </div>
                        <div style='margin-top: 2rem; font-size: 0.8rem; color: #aaa; text-align: left;'>
                            <div>IP: {remoteIp}</div>
                            <div>Location: {connectionService.LocationType}</div>
                            <div>Path: {path}</div>
                            <div>HasTokenParam: {context.Request.Query.ContainsKey("token")}</div>
                            <div style='margin-top: 5px; color: #ccc;'>v.2026.01.28.0530</div>
                        </div>
                    </div>
                </body>
                </html>");
            return;
        }
    }
}
