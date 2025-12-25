// [MODIFIED]
using GFC.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GFC.VideoAgent.Middleware
{
    public class StreamTokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<StreamTokenValidationMiddleware> _logger;
        private static readonly Regex _streamPathRegex = new(@"^/(live|stream)/(\d+)/.+$", RegexOptions.Compiled);

        public StreamTokenValidationMiddleware(RequestDelegate next, ILogger<StreamTokenValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IStreamSecurityService streamSecurityService)
        {
            var path = context.Request.Path.Value;

            var match = _streamPathRegex.Match(path);

            if (match.Success)
            {
                if (!int.TryParse(match.Groups[2].Value, out var cameraId))
                {
                    _logger.LogWarning("Could not parse camera ID from stream path: {Path}", path);
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid camera ID in path.");
                    return;
                }

                if (!context.Request.Query.TryGetValue("token", out var token))
                {
                    _logger.LogWarning("Access denied for camera {CameraId}: Missing token.", cameraId);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Access token is missing.");
                    return;
                }

                var userIpAddress = context.Connection.RemoteIpAddress?.ToString();
                if (!streamSecurityService.ValidateStreamToken(token, cameraId, userIpAddress))
                {
                    _logger.LogWarning("Access denied for camera {CameraId}: Invalid token provided.", cameraId);
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Invalid or expired access token.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
