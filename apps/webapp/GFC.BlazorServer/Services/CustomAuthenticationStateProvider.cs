// [REFRESHED]
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

    // PERFORMANCE CACHE: Persists across circuits (Static)
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, (AppUser User, DateTime Expiry)> _tokenCache = new();

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

    private bool _autoLoginAttempted = false;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        RefreshFromAuthenticationService();

        // [MODIFIED] HIGH-SPEED AUTO-LOGIN: Uses static cache to survive circuit drops
        if (_currentUser == null && !_autoLoginAttempted)
        {
            _autoLoginAttempted = true;
            try 
            {
                var context = _httpContextAccessor.HttpContext;
                if (context != null && context.Request.Cookies.TryGetValue("GFC_DeviceTrustToken", out var token) && !string.IsNullOrEmpty(token))
                {
                    // 1. Check Global Cache first (very fast)
                    if (_tokenCache.TryGetValue(token, out var cached) && cached.Expiry > DateTime.UtcNow)
                    {
                        var updatedUser = cached.User;
                        _currentUser = updatedUser;
                        _currentPrincipal = BuildPrincipal(updatedUser);
                        _userSessionService.SetLoginTime(DateTime.UtcNow);
                    }
                    else
                    {
                        // 2. Fallback to Database (slow but necessary if cache miss)
                        var result = await _authenticationService.LoginWithDeviceTokenAsync(token);
                        if (result.Success && result.User != null)
                        {
                            var updatedUser = result.User;
                            _currentUser = updatedUser;
                            _currentPrincipal = BuildPrincipal(updatedUser);
                            _userSessionService.SetLoginTime(DateTime.UtcNow);
                            
                            // 3. Save to Global Cache for next time (Expires in 1 hour of silence)
                            _tokenCache[token] = (updatedUser, DateTime.UtcNow.AddHours(1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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

    public async Task<LoginResult> LoginWithMfaSuccessAsync(int userId, bool rememberDevice, string? ipAddress = null)
    {
        var result = await _authenticationService.FinalizeMfaLoginAsync(userId, rememberDevice, ipAddress);
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

    public async Task<LoginResult> LoginWithPasskeyAsync(string username, string? ipAddress = null)
    {
        var result = await _authenticationService.LoginWithPasskeyAsync(username, ipAddress);
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

    public async Task RefreshUserAsync()
    {
        await _authenticationService.RefreshCurrentUserAsync();
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
            // Update if user changed, or if key flags like IsAdmin or PasswordChangeRequired have been updated
            bool stateChanged = _currentUser == null || 
                               _currentUser.UserId != serviceUser.UserId || 
                               _currentUser.IsAdmin != serviceUser.IsAdmin ||
                               _currentUser.PasswordChangeRequired != serviceUser.PasswordChangeRequired ||
                               _currentPrincipal.Identity?.IsAuthenticated != true;

            if (stateChanged)
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
