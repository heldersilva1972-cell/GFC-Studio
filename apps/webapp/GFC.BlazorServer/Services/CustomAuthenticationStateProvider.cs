using System;
using System.Security.Claims;
using GFC.BlazorServer.Auth;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace GFC.BlazorServer.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    // In-memory principal; Blazor Server app is per-connection so this is OK here.
    private ClaimsPrincipal _currentPrincipal = CreateUnauthenticatedPrincipal();
    private AppUser? _currentUser;

    public CustomAuthenticationStateProvider(
        IAuthenticationService authenticationService,
        IHttpContextAccessor httpContextAccessor)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        RefreshFromAuthenticationService();
        return Task.FromResult(new AuthenticationState(_currentPrincipal));
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

        // Add more claims (e.g., roles) later if needed.

        var identity = new ClaimsIdentity(claims, authenticationType: "GfcAuth");
        return new ClaimsPrincipal(identity);
    }

    public async Task<LoginResult> LoginAsync(string username, string password, string? ipAddress = null)
    {
        // Delegate to AuthenticationService.LoginAsync (which already does all validation + history logging)
        var result = await _authenticationService.LoginAsync(username, password, ipAddress);

        RefreshFromAuthenticationService();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));

        return result;
    }

    public void Logout()
    {
        _authenticationService.Logout();
        RefreshFromAuthenticationService();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentPrincipal)));
    }

    public async Task LogoutAsync()
    {
        Logout();
        await Task.CompletedTask;
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

        // Allow development-only auto-admin principal to flow from middleware.
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
