// [MODIFIED]
using System;
using System.Security.Claims;
using GFC.BlazorServer.Auth;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserSessionService _userSessionService;

    private ClaimsPrincipal _currentPrincipal = CreateUnauthenticatedPrincipal();
    private AppUser? _currentUser;

    public CustomAuthenticationStateProvider(
        IAuthenticationService authenticationService,
        IHttpContextAccessor httpContextAccessor,
        IUserSessionService userSessionService)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _userSessionService = userSessionService ?? throw new ArgumentNullException(nameof(userSessionService));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        RefreshFromAuthenticationService();

        // [MODIFIED] ASYNC AUTO-LOGIN LOGIC logic moved here to avoid blocking and deadlocks
        if (_currentUser == null)
        {
            try 
            {
                var context = _httpContextAccessor.HttpContext;
                if (context != null && context.Request.Cookies.TryGetValue("GFC_DeviceTrustToken", out var token) && !string.IsNullOrEmpty(token))
                {
                    // Asynchronously attempt to restore session from token
                    var result = await _authenticationService.LoginWithDeviceTokenAsync(token);
                    if (result.Success)
                    {
                        var updatedUser = result.User; // Capture result
                        if (updatedUser != null)
                        {
                            _currentUser = updatedUser;
                            _currentPrincipal = BuildPrincipal(updatedUser);
                            _userSessionService.SetLoginTime(DateTime.UtcNow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log but don't crash
                 System.Diagnostics.Debug.WriteLine($"Auto-login failed: {ex.Message}");
            }
        }

        return new AuthenticationState(_currentPrincipal);
    }

    private ClaimsPrincipal BuildPrincipal(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId", user.UserId.ToString()),
            new Claim("IsAdmin", user.IsAdmin ? "true" : "false")
        };

        if (user.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, AppRoles.Admin));
        }

        var identity = new ClaimsIdentity(claims, authenticationType: "GfcAuth");
        return new ClaimsPrincipal(identity);
    }

    public async Task<LoginResult> LoginAsync(string username, string password, bool rememberDevice, string? ipAddress = null)
    {
        var result = await _authenticationService.LoginAsync(username, password, ipAddress, rememberDevice);
        if (result.Success)
        {
            _userSessionService.SetLoginTime(DateTime.UtcNow);
        }
        RefreshFromAuthenticationService();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));
        return result;
    }

    public async Task<LoginResult> LoginWithDeviceTokenAsync(string token, string? ipAddress = null)
    {
        var result = await _authenticationService.LoginWithDeviceTokenAsync(token, ipAddress);
        if (result.Success)
        {
            _userSessionService.SetLoginTime(DateTime.UtcNow);
        }
        RefreshFromAuthenticationService();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));
        return result;
    }

    public async Task<LoginResult> VerifyMfaCodeAsync(int userId, string code, string? ipAddress = null)
    {
        var result = await _authenticationService.VerifyMfaCodeAsync(userId, code, ipAddress);
        if (result.Success)
        {
            _userSessionService.SetLoginTime(DateTime.UtcNow);
        }
        RefreshFromAuthenticationService();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));
        return result;
    }

    public async Task<LoginResult> LoginWithUserAsync(int userId, string? ipAddress = null)
    {
        var result = await _authenticationService.LoginMagicLinkAsync(userId, ipAddress);
        if (result.Success)
        {
            _userSessionService.SetLoginTime(DateTime.UtcNow);
        }
        RefreshFromAuthenticationService();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));
        return result;
    }

    public async Task LogoutAsync(string? deviceToken = null)
    {
        await _authenticationService.LogoutAsync(deviceToken);
        RefreshFromAuthenticationService();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));
    }

    public AppUser? GetCurrentUser()
    {
        RefreshFromAuthenticationService();
        return _currentUser;
    }

    private void RefreshFromAuthenticationService()
    {
        AppUser? serviceUser = null;
        try
        {
            serviceUser = _authenticationService.GetCurrentUser();
        }
        catch
        {
            serviceUser = null;
        }

        if (serviceUser != null && serviceUser.IsActive)
        {
            if (_currentUser == null ||
                _currentUser.UserId != serviceUser.UserId ||
                _currentPrincipal.Identity?.IsAuthenticated != true)
            {
                _currentUser = serviceUser;
                _currentPrincipal = BuildPrincipal(serviceUser);
            }
            return;
        }

        if (IsDevPrincipal(_currentPrincipal))
        {
            return;
        }

        var devPrincipal = GetDevPrincipalFromHttpContext();
        if (devPrincipal != null)
        {
            _currentPrincipal = devPrincipal;
            _currentUser = BuildUserFromPrincipal(devPrincipal);
            return;
        }

        if (_currentUser != null || _currentPrincipal.Identity?.IsAuthenticated == true)
        {
            _currentUser = null;
            _currentPrincipal = CreateUnauthenticatedPrincipal();
        }
    }

    private static ClaimsPrincipal CreateUnauthenticatedPrincipal() => new(new ClaimsIdentity());

    private ClaimsPrincipal? GetDevPrincipalFromHttpContext()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return null;
        }

        if (!httpContext.Items.TryGetValue(DevAuthDefaults.DevPrincipalItemKey, out var storedPrincipal))
        {
            return null;
        }

        if (storedPrincipal is not ClaimsPrincipal principal)
        {
            return null;
        }

        return IsDevPrincipal(principal) ? principal : null;
    }

    private static bool IsDevPrincipal(ClaimsPrincipal? principal)
    {
        return principal?.Identity?.IsAuthenticated == true &&
               principal.HasClaim(DevAuthDefaults.DevBypassClaimType, "true");
    }

    private static AppUser BuildUserFromPrincipal(ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirst("UserId")?.Value ??
                          principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        int.TryParse(userIdClaim, out var parsedUserId);

        var isAdmin = principal.IsInRole(AppRoles.Admin);
        var isAdminClaim = principal.FindFirst("IsAdmin")?.Value;
        if (!isAdmin && bool.TryParse(isAdminClaim, out var claimIsAdmin))
        {
            isAdmin = claimIsAdmin;
        }

        return new AppUser
        {
            UserId = parsedUserId == 0 ? -1 : parsedUserId,
            Username = principal.Identity?.Name ?? "Dev Admin",
            IsAdmin = isAdmin,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            PasswordChangeRequired = false
        };
    }
}
