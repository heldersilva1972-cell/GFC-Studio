using System;
using System.Collections.Generic;
using GFC.BlazorServer.Auth;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/notifications")]
[Microsoft.AspNetCore.Cors.EnableCors("GfcEcosystemPolicy")]
public class NotificationDispatchController : ControllerBase
{
    private readonly IFirebaseNotificationService _notificationService;
    private readonly ILogger<NotificationDispatchController> _logger;

    public NotificationDispatchController(
        IFirebaseNotificationService notificationService,
        ILogger<NotificationDispatchController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public record PushRequest(
        int? TargetUserId,
        List<int>? TargetUserIds,
        string Title,
        string Body,
        Dictionary<string, string>? Data
    );

    [HttpPost("send-target")]
    [Authorize]
    public async Task<IActionResult> SendTargetedNotification([FromBody] PushRequest request)
    {
        var isAdminClaim = User.FindFirst("IsAdmin")?.Value?.ToLower() == "true";
        if (!isAdminClaim && !User.IsInRole(AppRoles.Admin))
        {
            return Forbid();
        }

        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest(new { error = "Title and Body are required." });
        }

        try
        {
            if (request.TargetUserId.HasValue)
            {
                var sent = await _notificationService.SendToUserAsync(request.TargetUserId.Value, request.Title, request.Body, request.Data);
                return Ok(new { success = sent, message = sent ? "Notification sent successfully." : "No active device tokens found for target user." });
            }

            if (request.TargetUserIds != null && request.TargetUserIds.Count > 0)
            {
                var count = await _notificationService.SendToUsersAsync(request.TargetUserIds, request.Title, request.Body, request.Data);
                return Ok(new { success = count > 0, devicesNotified = count });
            }

            // Broadcast to all active devices
            var broadcastCount = await _notificationService.BroadcastAsync(request.Title, request.Body, request.Data);
            return Ok(new { success = broadcastCount > 0, devicesNotified = broadcastCount });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[GFC-CONNECT] Error sending notification");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
