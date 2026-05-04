using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.Models.Security;
using GFC.Core.DTOs;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/mobile-users-mgmt")]
[Authorize]
public class MobileUserManagementController : ControllerBase
{
    private readonly IUserManagementService _userManagementService;
    private readonly IDeviceTrustService _deviceTrustService;
    private readonly IDeviceInviteService _deviceInviteService;
    private readonly IAuditLogger _auditLogger;

    public MobileUserManagementController(
        IUserManagementService userManagementService,
        IDeviceTrustService deviceTrustService,
        IDeviceInviteService deviceInviteService,
        IAuditLogger auditLogger)
    {
        _userManagementService = userManagementService;
        _deviceTrustService = deviceTrustService;
        _deviceInviteService = deviceInviteService;
        _auditLogger = auditLogger;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<UserListItemDto>>> GetUsers()
    {
        return Ok(await _userManagementService.GetUsersAsync());
    }

    [HttpGet("eligible-members")]
    public ActionResult<List<ActiveMemberDto>> GetEligibleMembers()
    {
        return Ok(_userManagementService.GetEligibleMembersForUserCreation());
    }

    [HttpGet("eligible-directors")]
    public ActionResult<List<ActiveMemberDto>> GetEligibleDirectors()
    {
        return Ok(_userManagementService.GetEligibleDirectorsForUserCreation());
    }

    [HttpGet("user/{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await _userManagementService.GetUserAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost("create")]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var currentUserIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? currentUserId = string.IsNullOrEmpty(currentUserIdStr) ? null : int.Parse(currentUserIdStr);
            var currentUsername = User.Identity?.Name ?? "Mobile App";

            var userId = _userManagementService.CreateUser(
                request.Username,
                request.Password,
                request.IsAdmin,
                request.MemberId,
                request.Notes,
                currentUsername,
                request.PasswordChangeRequired,
                currentUserId,
                request.MfaEnabled);

            return Ok(new { UserId = userId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("update")]
    public IActionResult UpdateUser([FromBody] UpdateUserRequest request)
    {
        try
        {
            var currentUserIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? currentUserId = string.IsNullOrEmpty(currentUserIdStr) ? null : int.Parse(currentUserIdStr);
            
            _userManagementService.UpdateUser(
                request.UserId,
                request.Username,
                request.Password,
                request.MemberId,
                request.Notes,
                currentUserId,
                request.IsAdmin,
                request.IsActive,
                request.MfaEnabled);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userManagementService.DeleteUserAsync(id);
        return Ok();
    }

    [HttpGet("devices/{userId}")]
    public async Task<ActionResult<List<TrustedDevice>>> GetUserDevices(int userId)
    {
        return Ok(await _deviceTrustService.GetDevicesForUserAsync(userId));
    }

    [HttpPost("revoke-device")]
    public async Task<IActionResult> RevokeDevice([FromBody] string token)
    {
        await _deviceTrustService.RevokeDeviceTokenAsync(token);
        return Ok();
    }

    [HttpPost("onboarding-link")]
    public async Task<ActionResult<string>> GenerateOnboardingLink([FromBody] int userId)
    {
        var token = await _deviceInviteService.CreateInviteTokenAsync(userId, "Mobile Hub");
        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var link = $"{baseUrl}/onboarding?token={token}";
        return Ok(new { Link = link });
    }

    [HttpGet("pages")]
    public ActionResult<List<AppPage>> GetPages()
    {
        return Ok(_userManagementService.GetAllPages());
    }

    [HttpGet("active-pages")]
    public ActionResult<List<AppPage>> GetActivePages()
    {
        return Ok(_userManagementService.GetActivePages());
    }

    [HttpGet("permissions/{userId}")]
    public ActionResult<List<GFC.Core.DTOs.MobilePermissionDto>> GetUserPermissions(int userId)
    {
        return Ok(_userManagementService.GetUserPagePermissions(userId));
    }

    [HttpPost("permissions")]
    public IActionResult UpdatePermissions([FromBody] UserPermissionUpdateRequest request)
    {
        try
        {
            var currentUsername = User.Identity?.Name ?? "Mobile App";
            _userManagementService.SetUserPagePermissions(request.UserId, request.PageIds, currentUsername);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
