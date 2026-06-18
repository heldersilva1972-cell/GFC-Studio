// [REFRESHED]
using System;
using System.Security.Claims;
using GFC.BlazorServer.Auth;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
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
    private bool _autoLoginAttempted = false;

    // [NEW] Task-Level Protection to prevent "Thundering Herd" (multiple components calling simultaneously)
    private Task<AuthenticationState>? _getAuthenticationStateTask;
    private readonly System.Threading.SemaphoreSlim _authLock = new(1, 1);
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



    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // 1. FAST PATH: Return current session if already established within this circuit
        if (_currentUser != null)
        {
            return new AuthenticationState(_currentPrincipal);
        }

        // 2. THUNDERING HERD PROTECTION: Reuse in-flight auth task if multiple components request state at once
        if (_getAuthenticationStateTask != null)
        {
            return await _getAuthenticationStateTask;
        }

        await _authLock.WaitAsync();
        try
        {
            // Re-check after acquiring lock
            if (_currentUser != null) return new AuthenticationState(_currentPrincipal);
            if (_getAuthenticationStateTask != null) return await _getAuthenticationStateTask;

            _getAuthenticationStateTask = ExecuteGetAuthenticationStateAsync();
            return await _getAuthenticationStateTask;
        }
        finally
        {
            _authLock.Release();
        }
    }

    private async Task<AuthenticationState> ExecuteGetAuthenticationStateAsync()
    {
        try
        {
            RefreshFromAuthenticationService();

            if (_currentUser == null && !_autoLoginAttempted)
            {
                _autoLoginAttempted = true;
                
                var context = _httpContextAccessor.HttpContext;
                string? token = null;
                bool isCookieToken = false;
                bool isSharedStation = false;
                bool jsAvailable = false;

                // 1. Try JS first (if interactive / browser connection active)
                try
                {
                    using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(2));
                    token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", cts.Token, "gfc_device_token");
                    isSharedStation = await _jsRuntime.InvokeAsync<bool>("window.verifyCookie", cts.Token, "GFC_StationIdentity");
                    jsAvailable = true;
                }
                catch (InvalidOperationException)
                {
                    // JS not ready yet (prerendering phase)
                    jsAvailable = false;
                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("JS auth storage lookup timed out. Failing closed to prevent hang.");
                    jsAvailable = true; // JS is supported but timed out
                }
                catch
                {
                    jsAvailable = false;
                }

                // 2. Fallback to HttpContext cookies only during initial Prerender (when JS is unavailable)
                if (!jsAvailable && context != null)
                {
                    isSharedStation = context.Request.Cookies.ContainsKey("GFC_StationIdentity");
                    if (context.Request.Cookies.TryGetValue("GFC_DeviceTrustToken", out var cookieToken) && !string.IsNullOrEmpty(cookieToken))
                    {
                        token = cookieToken;
                        isCookieToken = true;
                    }
                }

                if (!string.IsNullOrEmpty(token))
                {
                    // [SECURITY] AUTO-LOGIN POLICY
                    // 1. Never auto-login if GFC_StationIdentity is present (shared machines must use PIN)
                    // 2. Only allow auto-login for personal devices (Context exists & no station flag)
                    
                    bool shouldRestore = false;
                    
                    if (jsAvailable)
                    {
                        if (isSharedStation)
                        {
                            // On shared stations, we only restore the session if:
                            // 1. We have a token
                            // 2. The session cookie is present (we verify it by checking if it exists in document.cookie)
                            // 3. The browser says our specific window session is active (gfc_session_active)
                            // This ensures that closing the browser wipes the session, while F5 (Refresh) keeps it.
                            bool isInstanceActive = false;
                            try {
                                using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(2));
                                var flag = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", cts.Token, "gfc_session_active");
                                isInstanceActive = !string.IsNullOrEmpty(flag);
                            } catch { }

                            bool hasCookie = false;
                            try {
                                hasCookie = await _jsRuntime.InvokeAsync<bool>("window.verifyCookie", "GFC_DeviceTrustToken");
                            } catch { }

                            shouldRestore = hasCookie && isInstanceActive;
                        }
                        else
                        {
                            shouldRestore = true;
                        }
                    }
                    else if (context != null)
                    {
                        if (isSharedStation)
                        {
                            shouldRestore = isCookieToken;
                        }
                        else
                        {
                            shouldRestore = true;
                        }
                    }

                    if (shouldRestore)
                    {
                        _currentToken = token;
                        
                        // Check cache first for speed
                        if (_tokenCache.TryGetValue(token, out var cachedData) && cachedData.Expiry > DateTime.UtcNow)
                        {
                            var user = cachedData.User;
                            _currentUser = user;
                            _currentPrincipal = BuildPrincipal(user, isSharedStation);
                            _currentToken = token; // Mark it as restored
                            
                            // Propagate login to persistence service in background
                            _ = _authenticationService.LoginWithDeviceTokenAsync(token);
                            _userSessionService.SetLoginTime(DateTime.UtcNow);
                        }
                        else 
                        {
                            // DB Validation Hit
                            var result = await _authenticationService.LoginWithDeviceTokenAsync(token);
                            if (result.Success && result.User != null)
                            {
                                var updatedUser = result.User;
                                _currentUser = updatedUser;
                                _currentPrincipal = BuildPrincipal(updatedUser, isSharedStation);
                                _userSessionService.SetLoginTime(DateTime.UtcNow);
                                
                                // Cache for 1 hour to prevent DB thrashing on refreshes
                                _tokenCache[token] = (updatedUser, DateTime.UtcNow.AddHours(1));
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Auto-login failed during state resolution");
        }
        finally
        {
            // Clear the in-flight task so future refreshes can run fresh logic if needed
            _getAuthenticationStateTask = null;
        }

        return new AuthenticationState(_currentPrincipal);
    }

    private ClaimsPrincipal BuildPrincipal(AppUser user, bool isStation)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim("UserId", user.UserId.ToString()),
            new Claim("IsAdmin", user.IsAdmin ? "true" : "false")
        };
 
        // Carry station identity in claims to avoid redundant JS checks in MainLayout
        if (isStation)
        {
            claims.Add(new Claim("IsStation", "true"));
        }

        if (user.IsAdmin)
        {
            claims.Add(new Claim(ClaimTypes.Role, AppRoles.Admin));
        }
 
        var identity = new ClaimsIdentity(claims, authenticationType: "GfcAuth");
        return new ClaimsPrincipal(identity);
    }

    public async Task<GFC.Core.Models.GfcLoginResult> LoginAsync(string username, string password, bool rememberDevice, string? ipAddress = null)
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

    public async Task<GFC.Core.Models.GfcLoginResult> LoginWithDeviceTokenAsync(string token, string? ipAddress = null)
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

    public async Task<GfcLoginResult> VerifyMfaCodeAsync(int userId, string code, string? ipAddress = null)
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

    public async Task<GfcLoginResult> LoginWithMfaSuccessAsync(int userId, bool rememberDevice, string? ipAddress = null)
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

    public async Task<GfcLoginResult> LoginWithUserAsync(int userId, string? ipAddress = null)
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

    public async Task<GfcLoginResult> LoginWithPasskeyAsync(string username, string? ipAddress = null)
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
        // 0. Use current token if none provided (to ensure global cache is cleared)
        var tokenToClear = deviceToken ?? _currentToken;

        try 
        {
            // 1. Tell the server to log out (clears internal state in the AuthenticationService)
            // [FIX] Add safety timeout for the server-side logout call to prevent DB-driven hangs.
            var serverLogoutTask = _authenticationService.LogoutAsync(tokenToClear);
            if (await Task.WhenAny(serverLogoutTask, Task.Delay(3000)) != serverLogoutTask)
            {
                _logger.LogWarning("Server-side LogoutAsync timed out for token '{Token}'. Proceeding anyway.", tokenToClear?.Length > 8 ? tokenToClear.Substring(0, 8) : "null");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during server-side logout logic");
        }

        // 2. Clear global cache immediately (Synchronous/In-Memory)
        if (!string.IsNullOrEmpty(tokenToClear))
        {
            _tokenCache.TryRemove(tokenToClear, out _);
        }

        // 3. Force reset of ALL scoped state IMMEDIATELY
        _currentUser = null;
        _currentToken = null; 
        _autoLoginAttempted = false;
        _currentPrincipal = CreateUnauthenticatedPrincipal();
        
        // 4. [CRITICAL] Notify UI FIRST before waiting for browser storage cleanup.
        // This stops the "spinning" UI by allowing the layout to react to the state change.
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));

        // 5. [CRITICAL] Clean browser storage.
        // We await this to ensure the cookie/token is GONE before any redirection occurs.
        try 
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            
            // We attempt to clear localStorage and cookies. 
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "gfc_device_token");
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "gfc_session_active");
            
            // Clear cookie multiple ways to be safe
            await _jsRuntime.InvokeVoidAsync("window.setCookie", "GFC_DeviceTrustToken", "", -1);
            await _jsRuntime.InvokeVoidAsync("eval", "document.cookie = 'GFC_DeviceTrustToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';");
        }
        catch (Exception ex)
        {
            _logger.LogDebug("JS Logout cleanup failed (expected on disconnect): {Message}", ex.Message);
        }
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
                bool isStation = _currentPrincipal.HasClaim("IsStation", "true") || 
                                 (_httpContextAccessor.HttpContext?.Request?.Cookies?.ContainsKey("GFC_StationIdentity") == true);
                _currentPrincipal = BuildPrincipal(serviceUser, isStation);
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


