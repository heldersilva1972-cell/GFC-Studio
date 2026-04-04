using Microsoft.AspNetCore.Mvc;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly ITrustedDeviceRepository _trustedDeviceRepository;
    private readonly ILogger<AnalyticsController> _logger;

    public AnalyticsController(
        IAuditLogRepository auditLogRepository,
        ITrustedDeviceRepository trustedDeviceRepository,
        ILogger<AnalyticsController> logger)
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

            if (eventData.Action == "PageView" || string.IsNullOrEmpty(eventData.Action))
            {
                return Ok(); // Ignore but return success to stop retries
            }

            var entry = new AuditLogEntry
            {
                TimestampUtc = DateTime.UtcNow,
                PerformedByUserId = userId.Value,
                Action = eventData.Action,
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
        // NO-OP: Heartbeats are only useful for PageView duration, which we no longer want.
        return await Task.FromResult(Ok());
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
