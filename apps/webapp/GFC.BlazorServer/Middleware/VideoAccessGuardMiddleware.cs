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

        public VideoAccessGuardMiddleware(RequestDelegate next, ILogger<VideoAccessGuardMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, INetworkLocationService networkLocationService, IUserConnectionService userConnectionService)
        {
            var remoteIpAddress = context.Connection.RemoteIpAddress?.ToString();

            // Handle Cloudflare's X-Forwarded-For header
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            {
                remoteIpAddress = forwardedFor.ToString().Split(',').FirstOrDefault()?.Trim();
            }

            if (!string.IsNullOrEmpty(remoteIpAddress))
            {
                userConnectionService.IpAddress = remoteIpAddress;
                userConnectionService.LocationType = await networkLocationService.DetectLocationAsync(remoteIpAddress);
            }

            var path = context.Request.Path;
            if (path.StartsWithSegments("/video") || path.StartsWithSegments("/cameras"))
            {
                if (userConnectionService.LocationType == LocationType.Public)
                {
                    _logger.LogWarning("Blocked public IP {RemoteIpAddress} from accessing {Path}", remoteIpAddress, path);

                    // Log to database
                    await LogBlockedAccessAsync(context, remoteIpAddress, path);

                    context.Response.Redirect("/secure-access-wizard");
                    return;
                }
            }

            await _next(context);
        }

        private async Task LogBlockedAccessAsync(HttpContext context, string clientIp, string path)
        {
            try
            {
                using var scope = context.RequestServices.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<GfcDbContext>();

                var userId = context.User?.Identity?.IsAuthenticated == true
                    ? (int?)Convert.ToInt32(context.User.FindFirst(CustomClaimTypes.UserId)?.Value)
                    : null;

                var auditLog = new VideoAccessAudit
                {
                    UserId = userId,
                    AccessType = "Blocked",
                    ConnectionType = "Public",
                    ClientIP = clientIp,
                    SessionStart = DateTime.UtcNow,
                    Notes = $"Attempted to access {path} from a public IP address."
                };

                dbContext.VideoAccessAudits.Add(auditLog);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to log blocked video access attempt for IP {ClientIp}", clientIp);
            }
        }
    }
}
