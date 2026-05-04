using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.DTOs;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/mobile-auth")]
[Microsoft.AspNetCore.Authorization.AllowAnonymous]
[Microsoft.AspNetCore.Cors.EnableCors("GfcEcosystemPolicy")]
public class MobileAuthController : ControllerBase
{
    private readonly CustomAuthenticationStateProvider _authStateProvider;
    private readonly IUserManagementService _userManagementService;
    private readonly IPagePermissionRepository _pagePermissionRepository;
    private readonly ITrustedDeviceRepository _deviceTrustRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<MobileAuthController> _logger;

    public MobileAuthController(
        AuthenticationStateProvider authStateProvider,
        IUserManagementService userManagementService,
        IPagePermissionRepository pagePermissionRepository,
        ITrustedDeviceRepository deviceTrustRepository,
        IUserRepository userRepository,
        ILogger<MobileAuthController> logger)
    {
        _authStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
        _userManagementService = userManagementService;
        _pagePermissionRepository = pagePermissionRepository;
        _deviceTrustRepository = deviceTrustRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet("users")]
    [AllowAnonymous]
    public ActionResult<List<UserListItemDto>> GetUsers()
    {
        try
        {
            var allUsers = _userManagementService.GetAllUsers();
            var mobileUsers = new List<UserListItemDto>();

            foreach (var userDto in allUsers)
            {
                var user = _userManagementService.GetUser(userDto.UserId);
                if (user == null || !user.IsActive) continue;

                var permissions = _userManagementService.GetUserPagePermissions(user.UserId);
                
                // Authoritative Master-Switch Filter
                bool hasMobileAccess = permissions.Any(p => 
                    p.CanAccess && 
                    !string.IsNullOrEmpty(p.PageRoute) && 
                    (p.PageRoute.Trim('/').ToLower() == "mobile" || p.PageRoute.Trim('/').ToLower() == "hub"));

                if (hasMobileAccess)
                {
                    mobileUsers.Add(userDto);
                }
            }

            return Ok(mobileUsers.OrderBy(u => u.Username).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch mobile users");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<GFC.Core.Models.GfcLoginResult>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authStateProvider.LoginAsync(request.Username, request.Password, request.RememberDevice, request.IpAddress);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Mobile login failed for {Username}", request.Username);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("login-token")]
    public async Task<ActionResult<GFC.Core.Models.GfcLoginResult>> LoginWithToken([FromBody] string token)
    {
        try
        {
            var result = await _authStateProvider.LoginWithDeviceTokenAsync(token);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Mobile token login failed");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("login-user")]
    public async Task<ActionResult<GFC.Core.Models.GfcLoginResult>> LoginWithUser([FromBody] int userId)
    {
        try
        {
            var result = await _authStateProvider.LoginWithUserAsync(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Mobile simple user login failed for ID {UserId}", userId);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string? token)
    {
        await _authStateProvider.LogoutAsync(token);
        return Ok();
    }

    [HttpGet("user")]
    public async Task<ActionResult<GFC.Core.Models.GfcLoginResult>> GetCurrentUser([FromQuery] string? token)
    {
        // If a token is provided, we can simulate the login context for this request
        if (!string.IsNullOrEmpty(token))
        {
            // The provider handles token validation and session restoration internally
            await _authStateProvider.LoginWithDeviceTokenAsync(token);
        }
        
        // [NUCLEAR FIX] Bypass provider and fetch user directly from token if possible
        AppUser? user = null;
        if (!string.IsNullOrEmpty(token))
        {
            var session = await _deviceTrustRepository.GetByTokenAsync(token);
            if (session != null && !session.IsRevoked && session.ExpiresAtUtc > DateTime.UtcNow)
            {
                user = _userRepository.GetById(session.UserId);
            }
        }

        if (user == null) 
        {
            user = _authStateProvider.GetCurrentUser();
        }

        if (user == null) return Unauthorized();

        // Fetch DIRECTLY from repository and map to DTOs
        var rawPermissions = _pagePermissionRepository.GetUserPermissions(user.UserId).Where(p => p.CanAccess).ToList();
        var permissions = rawPermissions.Select(p => new GFC.Core.DTOs.MobilePermissionDto
        {
            PageId = p.Page?.PageId ?? 0,
            PageName = p.Page?.PageName ?? "Unknown",
            PageRoute = p.Page?.PageRoute ?? "",
            Category = p.Page?.Category,
            CanAccess = p.CanAccess,
            CanEdit = p.CanEdit,
            ReceivePush = p.ReceivePush
        }).ToList();
        
        _logger.LogInformation("Mobile permissions requested for User {UserId}. Returning {Count} active routes.", user.UserId, permissions.Count);
        
        return Ok(new GFC.Core.Models.GfcLoginResult
        {
            Code = GFC.Core.Models.LoginResultCode.Success,
            User = user,
            DeviceToken = token,
            Permissions = permissions,
            AllowedRoutes = permissions
                .Select(p => (p.PageRoute ?? "").Trim('/').ToLowerInvariant())
                .Where(r => !string.IsNullOrEmpty(r))
                .ToList()
        });
    }

    [HttpGet("permissions/{userId}")]
    public ActionResult<List<UserPagePermission>> GetPermissions(int userId)
    {
        try
        {
            // [CRITICAL FIX] Hit repository directly
            var permissions = _pagePermissionRepository.GetUserPermissions(userId).ToList();
            return Ok(permissions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get permissions for user {UserId}", userId);
            return StatusCode(500, "Internal Server Error");
        }
    }
}

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool RememberDevice { get; set; }
    public string? IpAddress { get; set; }
}
