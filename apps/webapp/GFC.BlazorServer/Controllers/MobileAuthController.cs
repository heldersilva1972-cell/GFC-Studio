using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.DTOs;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/mobile-auth")]
[Microsoft.AspNetCore.Authorization.AllowAnonymous]
public class MobileAuthController : ControllerBase
{
    private readonly CustomAuthenticationStateProvider _authStateProvider;
    private readonly IUserManagementService _userManagementService;
    private readonly ILogger<MobileAuthController> _logger;

    public MobileAuthController(
        AuthenticationStateProvider authStateProvider,
        IUserManagementService userManagementService,
        ILogger<MobileAuthController> logger)
    {
        _authStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
        _userManagementService = userManagementService;
        _logger = logger;
    }

    [HttpGet("users")]
    public ActionResult<List<UserListItemDto>> GetUsers()
    {
        return _userManagementService.GetAllUsers();
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginRequest request)
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
    public async Task<ActionResult<LoginResult>> LoginWithToken([FromBody] string token)
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

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string? token)
    {
        await _authStateProvider.LogoutAsync(token);
        return Ok();
    }

    [HttpGet("user")]
    public async Task<ActionResult<AppUser>> GetCurrentUser([FromQuery] string? token)
    {
        // If a token is provided, we can simulate the login context for this request
        if (!string.IsNullOrEmpty(token))
        {
            // The provider handles token validation and session restoration internally
            await _authStateProvider.LoginWithDeviceTokenAsync(token);
        }
        
        var user = _authStateProvider.GetCurrentUser();
        return user != null ? Ok(user) : Unauthorized();
    }
}

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool RememberDevice { get; set; }
    public string? IpAddress { get; set; }
}
