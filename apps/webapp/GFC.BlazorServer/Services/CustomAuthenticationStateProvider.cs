// [REFRESHED]
using System;
using System.Security.Claims;
using GFC.BlazorServer.Auth;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GFC.BlazorServer.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserSessionService _userSessionService;
    private readonly IJSRuntime _jsRuntime;
    private readonly Microsoft.Extensions.Logging.ILogger<CustomAuthenticationStateProvider> _logger;

    // [NEW] Real-Time Multi-Circuit Invalidation
    // Allows an admin action in one circuit (e.g. deleting a user) to instantly terminate
    // the sessions in ALL other active circuits for that user.
    private static event Action<int>? OnUserInvalidated;
    private static event Action<string>? OnTokenInvalidated;

    // [NEW] High-Speed Global Session Cache
    // This persists across all user circuits and prevents redundant DB calls during reloads.
    private static readonly ConcurrentDictionary<string, (AppUser User, DateTime Expiry)> _tokenCache = new();

    // Scoped state for the current circuit
    private ClaimsPrincipal _currentPrincipal = CreateUnauthenticatedPrincipal();
    private AppUser? _currentUser;
    private string? _currentToken;
 
    /// <summary>
    /// Clears the global session cache for a specific user to force a database re-validation.
    /// Used when a device is revoked or setup is reset.
    /// </summary>
    public static void InvalidateUser(int userId)
    {
        var tokensToInvalidate = _tokenCache
            .Where(kvp => kvp.Value.User?.UserId == userId)
            .Select(kvp => kvp.Key)
            .ToList();
 
        foreach (var token in tokensToInvalidate)
        {
            _tokenCache.TryRemove(token, out _);
        }

        // [NEW] Notify all active circuits to self-destruct if they belong to this user
        OnUserInvalidated?.Invoke(userId);
    }
 
    /// <summary>
    /// Clears the global session cache for a specific token.
    /// Used when a single device is revoked.
    /// </summary>
    public static void InvalidateToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return;
        _tokenCache.TryRemove(token, out _);
        
        // [NEW] Notify all active circuits to self-destruct if they are using this token
        OnTokenInvalidated?.Invoke(token);
    }

    /// <summary>
    /// Clears the entire global session cache, forcing all users to re-validate against the database.
    /// </summary>
    public static void InvalidateAll()
    {
        _tokenCache.Clear();
    }

    public CustomAuthenticationStateProvider(
        IAuthenticationService authenticationService,
        IHttpContextAccessor httpContextAccessor,
        IUserSessionService userSessionService,
        IJSRuntime jsRuntime,
        Microsoft.Extensions.Logging.ILogger<CustomAuthenticationStateProvider> logger)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _userSessionService = userSessionService ?? throw new ArgumentNullException(nameof(userSessionService));
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // [NEW] Subscribe to global invalidation events
        OnUserInvalidated += HandleUserInvalidated;
        OnTokenInvalidated += HandleTokenInvalidated;
    }

    private async void HandleUserInvalidated(int userId)
    {
        if (_currentUser != null && _currentUser.UserId == userId)
        {
            _logger.LogInformation("Circuit for User {UserId} invalidated by administrative action.", userId);
            await LogoutAsync(); 
        }
    }

    private async void HandleTokenInvalidated(string token)
    {
        if (_currentToken == token)
        {
            _logger.LogInformation("Circuit Token invalidated by administrative action.");
            await LogoutAsync(token);
        }
    }

    public void Dispose()
    {
        OnUserInvalidated -= HandleUserInvalidated;
        OnTokenInvalidated -= HandleTokenInvalidated;
    }

    private bool _autoLoginAttempted = false;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        RefreshFromAuthenticationService();

        // [FIX] Standard Auto-Login: Check cookie and validate against DB
        if (_currentUser == null)
        {
            _autoLoginAttempted = true;
            try 
            {
                var context = _httpContextAccessor.HttpContext;
                string? token = null;

                // [STATION GUARD] 
                // If this machine is tagged as a Shared Station, we ABSOLUTELY FORBID auto-login.
                // This is the machine authorization token (Station Identity).
                bool isSharedStation = context?.Request.Cookies.ContainsKey("GFC_StationIdentity") == true;
                
                if (isSharedStation)
                {
                    _logger?.LogDebug("Auto-login: Station Mode Detected. User auto-login is forbidden.");
                }
                else
                {
                    // 1. Try Cookies (Initial load / Prerendering)
                    if (context != null && context.Request.Cookies.TryGetValue("GFC_DeviceTrustToken", out token) && !string.IsNullOrEmpty(token))
                    {
                        // Got token from cookie
                    }
                    else 
                    {
                        // 2. Try LocalStorage (Interactive circuit reconnection)
                        try 
                        {
                            token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "gfc_device_token");
                        }
                        catch { /* Not interactive yet or JS not ready */ }
                    }

                    if (!string.IsNullOrEmpty(token))
                    {
                        _currentToken = token; // Store for revocation monitoring
                        
                        // 1. Check Global Cache First (High speed memory hit)
                        if (_tokenCache.TryGetValue(token, out var cachedData) && cachedData.Expiry > DateTime.UtcNow)
                        {
                            var user = cachedData.User;
                            _currentUser = user;
                            _currentPrincipal = BuildPrincipal(user);
                            
                            // [FIX] Hydrate the scoped AuthenticationService so it's ready for sub-services
                            await _authenticationService.LoginWithDeviceTokenAsync(token);
                            
                            _userSessionService.SetLoginTime(DateTime.UtcNow);
                            _logger?.LogDebug("Auto-login: Restored user {Username} from global token cache.", user.Username);
                        }
                        else 
                        {
                            // 2. Fallback to Database
                            var result = await _authenticationService.LoginWithDeviceTokenAsync(token);
                            if (result.Success && result.User != null)
                            {
                                var updatedUser = result.User;
                                _currentUser = updatedUser;
                                _currentPrincipal = BuildPrincipal(updatedUser);
                                _userSessionService.SetLoginTime(DateTime.UtcNow);

                                // Cache for 1 hour to prevent constant DB pressure during mobile flickers
                                _tokenCache[token] = (updatedUser, DateTime.UtcNow.AddHours(1));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                 _logger?.LogError(ex, "Auto-login failed during state resolution");
            }
        }

        return new AuthenticationState(_currentPrincipal);
    }

    private ClaimsPrincipal BuildPrincipal(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
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
        try 
        {
            // 1. Tell the server to log out (clears internal state)
            await _authenticationService.LogoutAsync(deviceToken);
            
            // 2. Clear browser tokens to prevent immediate auto-login loop
            try 
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "gfc_device_token");
                await _jsRuntime.InvokeVoidAsync("window.setCookie", "GFC_DeviceTrustToken", "", -1);
            }
            catch { /* Not interactive or JS not ready */ }

            // 3. Clear global cache to ensure this specific token isn't reused immediately
            if (!string.IsNullOrEmpty(deviceToken))
            {
                _tokenCache.TryRemove(deviceToken, out _);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during LogoutAsync");
        }

        _currentUser = null;
        _currentPrincipal = CreateUnauthenticatedPrincipal();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));
    }

    public async Task RefreshUserAsync()
    {
        try 
        {
            // [FIX] Add a safety timeout to prevent circuit hangs during refresh
            var refreshTask = _authenticationService.RefreshCurrentUserAsync();
            if (await Task.WhenAny(refreshTask, Task.Delay(3000)) == refreshTask)
            {
                await refreshTask;
            }
            else 
            {
                _logger.LogWarning("RefreshCurrentUserAsync timed out in Provider.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during RefreshUserAsync");
        }
        
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
