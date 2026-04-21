using Microsoft.AspNetCore.Mvc;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/usage")]
public class UsageTrackingController : ControllerBase
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly ITrustedDeviceRepository _trustedDeviceRepository;
    private readonly ILogger<UsageTrackingController> _logger;

    public UsageTrackingController(
        IAuditLogRepository auditLogRepository,
        ITrustedDeviceRepository trustedDeviceRepository,
        ILogger<UsageTrackingController> logger)
    {
        _auditLogRepository = auditLogRepository;
        _trustedDeviceRepository = trustedDeviceRepository;
        _logger = logger;
    }

    [HttpPost("track")]
    public async Task<IActionResult> TrackEvent([FromBody] AnalyticsEventDto eventData)
    {
        try
        {
            // Get user from device token cookie (same as Blazor authentication)
            var userId = await GetUserIdFromDeviceToken();
            if (userId == null)
            {
                _logger.LogWarning("Analytics track called without valid device token");
                return Unauthorized();
            }

            var entry = new AuditLogEntry
            {
                TimestampUtc = DateTime.UtcNow,
                PerformedByUserId = userId.Value,
                Action = eventData.Action ?? "PageView",
                Details = eventData.Details,
                PageUrl = eventData.PageUrl,
                DurationSeconds = eventData.DurationSeconds
            };

            _auditLogRepository.Insert(entry);
            _logger.LogDebug("Tracked {Action} for user {UserId}: {PageUrl}", 
                eventData.Action, userId, eventData.PageUrl);
            
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking analytics event");
            return StatusCode(500);
        }
    }

    [HttpPost("heartbeat")]
    public async Task<IActionResult> Heartbeat([FromBody] HeartbeatDto heartbeatData)
    {
        try
        {
            // Get user from device token cookie
            var userId = await GetUserIdFromDeviceToken();
            if (userId == null)
            {
                _logger.LogWarning("Analytics heartbeat called without valid device token");
                return Unauthorized();
            }

            _auditLogRepository.UpdateDuration(
                userId.Value,
                heartbeatData.PageUrl ?? "/",
                heartbeatData.AdditionalSeconds
            );

            _logger.LogDebug("Heartbeat for user {UserId}: +{Seconds}s on {PageUrl}", 
                userId, heartbeatData.AdditionalSeconds, heartbeatData.PageUrl);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing heartbeat");
            return StatusCode(500);
        }
    }

    private async Task<int?> GetUserIdFromDeviceToken()
    {
        try
        {
            // Try to get device token from cookie (same as CustomAuthenticationStateProvider)
            var deviceToken = Request.Cookies["GFC_DeviceTrustToken"];
            if (string.IsNullOrEmpty(deviceToken))
            {
                return null;
            }

            // Validate token and get user ID
            var trustedDevice = await _trustedDeviceRepository.GetByTokenAsync(deviceToken);
            if (trustedDevice == null || trustedDevice.IsRevoked || trustedDevice.ExpiresAtUtc < DateTime.UtcNow)
            {
                return null;
            }

            return trustedDevice.UserId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating device token");
            return null;
        }
    }

    public class AnalyticsEventDto
    {
        public string? Action { get; set; }
        public string? PageUrl { get; set; }
        public string? Details { get; set; }
        public int DurationSeconds { get; set; }
    }

    public class HeartbeatDto
    {
        public string? PageUrl { get; set; }
        public int AdditionalSeconds { get; set; }
    }
}


