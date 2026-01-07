// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using GFC.BlazorServer.Auth;

namespace GFC.BlazorServer.Middleware
{
    public class VideoAccessGuardMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<VideoAccessGuardMiddleware> _logger;
        
        // Paths that should NEVER be blocked (critical for app functionality)
        private static readonly string[] ExcludedPaths = new[]
        {
            "/_blazor",           // Blazor SignalR hub
            "/_framework",        // Blazor framework files
            "/_content",          // Static content
            "/css",               // Stylesheets
            "/js",                // JavaScript
            "/images",            // Images
            "/favicon",           // Favicon
            "/manifest.json",     // PWA manifest
            "/service-worker",    // Service worker
            "/pwa-icons",         // PWA icons
            "/animationhub",      // SignalR hubs
            "/studiopreviewhub",
            "/videoaccesshub",
            "/cameras/secure-access", // The access request page itself
            "/setup",             // Setup pages
            "/login",             // Login page
            "/api"                // API endpoints
        };

        public VideoAccessGuardMiddleware(RequestDelegate next, ILogger<VideoAccessGuardMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, GFC.Core.Interfaces.INetworkLocationService networkLocationService, IUserConnectionService userConnectionService)
        {
            var remoteIpAddress = context.Connection.RemoteIpAddress?.ToString();

            // Prefer Cloudflare's CF-Connecting-IP header (more secure than X-Forwarded-For)
            if (context.Request.Headers.TryGetValue("CF-Connecting-IP", out var cfIp))
            {
                remoteIpAddress = cfIp.ToString();
            }
            // Fallback to X-Forwarded-For if CF-Connecting-IP not present
            else if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                remoteIpAddress = forwardedFor.ToString().Split(',').FirstOrDefault()?.Trim();
            }

            // Set connection info in UserConnectionService (single source of truth)
            if (!string.IsNullOrEmpty(remoteIpAddress))
            {
                var locationType = await networkLocationService.DetectLocationAsync(remoteIpAddress);
                userConnectionService.SetConnectionInfo(remoteIpAddress, locationType);
            }

            var path = context.Request.Path.Value?.ToLower() ?? "";
            
            // CRITICAL: Check excluded paths first (including WebSocket requests)
            if (context.WebSockets.IsWebSocketRequest)
            {
                await _next(context);
                return;
            }
            
            foreach (var excludedPath in ExcludedPaths)
            {
                if (path.StartsWith(excludedPath.ToLower()))
                {
                    await _next(context);
                    return;
                }
            }
            
            // Only check /video and /cameras paths
            if (path.StartsWith("/video") || path.StartsWith("/cameras"))
            {
                if (userConnectionService.LocationType == GFC.Core.Interfaces.LocationType.Public)
                {
                    _logger.LogWarning("Blocked public IP {RemoteIpAddress} from accessing {Path}", remoteIpAddress, path);

                    // Log to database
                    await LogBlockedAccessAsync(context, remoteIpAddress, path);

                    context.Response.Redirect("/cameras/secure-access");
                    return;
                }
            }

            await _next(context);
        }

        private async Task LogBlockedAccessAsync(HttpContext context, string? clientIp, string path)
        {
            try
            {
                using var scope = context.RequestServices.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<GfcDbContext>();

                var userId = context.User?.Identity?.IsAuthenticated == true
                    ? (int?)Convert.ToInt32(context.User.FindFirst("UserId")?.Value)
                    : null;

                var userAgent = context.Request.Headers["User-Agent"].ToString();
                
                var auditLog = new VideoAccessAudit
                {
                    UserId = userId ?? 0,
                    AccessType = "Blocked",
                    ConnectionType = "Public",
                    ClientIP = clientIp?.Length > 45 ? clientIp.Substring(0, 45) : clientIp,
                    SessionStart = DateTime.UtcNow,
                    Notes = $"Blocked public IP system access to {path}. User-Agent: {(userAgent.Length > 200 ? userAgent.Substring(0, 200) : userAgent)}"
                };

                dbContext.VideoAccessAudits.Add(auditLog);
                await dbContext.SaveChangesAsync();
                
                _logger.LogInformation("Logged blocked system access attempt from {ClientIp} to {Path}", clientIp, path);
            }
            catch (Exception ex)
            {
                // Don't let logging failures break the security check
                _logger.LogError(ex, "Failed to log blocked system access attempt for IP {ClientIp}. Continuing with block.", clientIp);
            }
        }
    }
}
