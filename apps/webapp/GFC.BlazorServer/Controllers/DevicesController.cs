using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GFC.BlazorServer.Auth;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/devices")]
[Microsoft.AspNetCore.Cors.EnableCors("GfcEcosystemPolicy")]
public class DevicesController : ControllerBase
{
    private readonly GfcDbContext _context;
    private readonly ILogger<DevicesController> _logger;

    public DevicesController(GfcDbContext context, ILogger<DevicesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public record DeviceRegistrationRequest(
        string DeviceToken,
        string? FcmToken,
        string? DeviceModel,
        string? OsVersion,
        string? AppVersion,
        string? Platform
    );

    [HttpPost("register")]
    [Authorize]
    public async Task<IActionResult> RegisterDevice([FromBody] DeviceRegistrationRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.DeviceToken))
        {
            return BadRequest(new { error = "DeviceToken is required." });
        }

        var userIdClaim = User.FindFirst("UserId")?.Value ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { error = "Unable to determine authenticated user identity." });
        }

        try
        {
            var device = await _context.UserDevices
                .FirstOrDefaultAsync(d => d.DeviceToken == request.DeviceToken);

            if (device == null)
            {
                device = new UserDevice
                {
                    UserId = userId,
                    DeviceToken = request.DeviceToken,
                    FcmDeviceToken = request.FcmToken,
                    DeviceModel = request.DeviceModel,
                    OsVersion = request.OsVersion,
                    AppVersion = request.AppVersion,
                    Platform = string.IsNullOrWhiteSpace(request.Platform) ? "Android" : request.Platform,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    LastActive = DateTime.UtcNow
                };
                _context.UserDevices.Add(device);
            }
            else
            {
                device.UserId = userId; // Rebind if changed
                if (!string.IsNullOrEmpty(request.FcmToken)) device.FcmDeviceToken = request.FcmToken;
                if (!string.IsNullOrEmpty(request.DeviceModel)) device.DeviceModel = request.DeviceModel;
                if (!string.IsNullOrEmpty(request.OsVersion)) device.OsVersion = request.OsVersion;
                if (!string.IsNullOrEmpty(request.AppVersion)) device.AppVersion = request.AppVersion;
                device.IsActive = true;
                device.LastActive = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("[GFC-CONNECT] Device registered for UserId={UserId}, Model={Model}", userId, request.DeviceModel);

            return Ok(new { success = true, deviceId = device.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[GFC-CONNECT] Error registering device token for UserId={UserId}", userId);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("all")]
    [Authorize]
    public async Task<IActionResult> GetAllEnrolledDevices()
    {
        // Enforce Admin policy
        var isAdminClaim = User.FindFirst("IsAdmin")?.Value?.ToLower() == "true";
        if (!isAdminClaim && !User.IsInRole(AppRoles.Admin))
        {
            return Forbid();
        }

        try
        {
            var devices = await _context.UserDevices
                .Include(d => d.User)
                .OrderByDescending(d => d.LastActive)
                .Select(d => new
                {
                    d.Id,
                    d.UserId,
                    Username = d.User != null ? d.User.Username : "Unknown",
                    MemberName = d.User != null ? (!string.IsNullOrEmpty(d.User.Email) ? $"{d.User.Username} ({d.User.Email})" : d.User.Username) : "Unknown",
                    d.DeviceToken,
                    HasFcm = !string.IsNullOrEmpty(d.FcmDeviceToken),
                    d.Platform,
                    d.DeviceModel,
                    d.OsVersion,
                    d.AppVersion,
                    d.IsActive,
                    d.CreatedAt,
                    d.LastActive
                })
                .ToListAsync();

            return Ok(devices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[GFC-CONNECT] Error fetching enrolled devices");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("revoke/{id:int}")]
    [Authorize]
    public async Task<IActionResult> RevokeDevice(int id)
    {
        var isAdminClaim = User.FindFirst("IsAdmin")?.Value?.ToLower() == "true";
        if (!isAdminClaim && !User.IsInRole(AppRoles.Admin))
        {
            return Forbid();
        }

        try
        {
            var device = await _context.UserDevices.FindAsync(id);
            if (device == null)
            {
                return NotFound(new { error = "Device not found." });
            }

            device.IsActive = false;
            await _context.SaveChangesAsync();
            _logger.LogInformation("[GFC-CONNECT] Revoked device Id={DeviceId}, User={UserId}", id, device.UserId);

            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[GFC-CONNECT] Error revoking device Id={DeviceId}", id);
            return StatusCode(500, "Internal Server Error");
        }
    }
}
