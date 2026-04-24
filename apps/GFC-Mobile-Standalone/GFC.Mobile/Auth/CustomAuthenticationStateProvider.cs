using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using GFC.Core.Models;
using GFC.Core.Interfaces;
using System.Net.Http.Json;
using GFC.Mobile.Services;

namespace GFC.Mobile.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider, ICustomAuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private readonly HttpClient _httpClient;
    private readonly IUserManagementService _userService;
    private const string LocalStorageKey = "gfc_auth_state";
    private AppUser? _currentUser;

    public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, HttpClient httpClient, IUserManagementService userService)
    {
        _jsRuntime = jsRuntime;
        _httpClient = httpClient;
        _userService = userService;
    }

    public AppUser? GetCurrentUser() => _currentUser;

    private bool _firstCall = true;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_firstCall)
        {
            _firstCall = false;
            // No more initial auto-login. Must check local storage or server.
        }

        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", LocalStorageKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                _currentUser = null;
                return CreateAnonymous();
            }

            AuthData? authData = null;
            try {
                authData = JsonSerializer.Deserialize<AuthData>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            } catch (Exception ex) {
                Console.WriteLine($"[WARN] Failed to deserialize auth state: {ex.Message}");
            }

            if (authData?.User == null || (authData.ExpiresAt.HasValue && authData.ExpiresAt < DateTime.UtcNow))
            {
                _currentUser = null;
                (_userService as MobileUserManagementService)?.ClearPermissionCache();
                return CreateAnonymous();
            }

            _currentUser = authData.User;
            
            // Restore permissions to service cache
            if (authData.Permissions != null)
            {
                var mobileService = _userService as MobileUserManagementService;
                mobileService?.UpdateCachedPermissions(authData.Permissions);

                // [SELF-HEALING] If we have permissions in the session but NOT in the vault, fix it now.
                // This ensures that even if a background sync missed the vault, the next refresh will fix it.
                _ = Task.Run(async () => {
                    try {
                        var vault = await mobileService!.GetCachedPermissionsForUserAsync(authData.User!.UserId);
                        if (vault == null || vault.Count == 0)
                        {
                            await mobileService.SavePermissionsToCacheAsync(authData.User.UserId, authData.Permissions);
                        }
                    } catch { }
                });
            }

            return CreateStateFromUser(_currentUser, "LocalStorageAuth", authData.Token, authData.CreatedAt);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AUTH ERROR] {ex.Message}");
            return CreateAnonymous();
        }
    }

    private AuthenticationState CreateStateFromUser(AppUser user, string type, string? token = null, DateTime? cachedAt = null)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim("IsAdmin", user.IsAdmin.ToString()),
            new Claim("Token", token ?? ""),
            new Claim("CachedAt", (cachedAt ?? DateTime.UtcNow).ToString("O"))
        }, type);

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task<GFC.Core.Models.GfcLoginResult> LoginAsync(string username, string password, bool rememberDevice)
    {
        var request = new { Username = username, Password = password, RememberDevice = rememberDevice };
        var response = await _httpClient.PostAsJsonAsync("/api/mobile-auth/login", request);
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<GFC.Core.Models.GfcLoginResult>();
            if (result != null && result.Success && result.User != null)
            {
                await CompleteLoginAsync(result);
                return result;
            }
            return result ?? new GFC.Core.Models.GfcLoginResult { Code = GFC.Core.Models.LoginResultCode.Error };
        }
        
        return new GFC.Core.Models.GfcLoginResult { Code = GFC.Core.Models.LoginResultCode.Error, ErrorMessageForLog = "Server error during login." };
    }

    public async Task<GFC.Core.Models.GfcLoginResult> LoginWithUserAsync(int userId)
    {
        try
        {
            // Set a short timeout for the "Online" attempt to keep the app responsive
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var response = await _httpClient.PostAsJsonAsync("/api/mobile-auth/login-user", userId, cts.Token);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GFC.Core.Models.GfcLoginResult>();
                if (result != null && result.Success)
                {
                    await CompleteLoginAsync(result);
                    return result;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AUTH] Server unreachable, attempting offline login: {ex.Message}");
        }

        // --- OFFLINE FALLBACK ---
        // If we reach here, the server is offline or the request failed.
        // We check if the user is in our cached list.
        var cachedUsers = await _userService.GetUsersAsync();
        var user = cachedUsers.FirstOrDefault(u => u.UserId == userId);

        if (user != null && user.IsActive)
        {
            // [VAULT RECOVERY] Try to get the actual permissions for this specific user from the local vault
            var mobileService = _userService as MobileUserManagementService;
            var actualPermissions = await mobileService!.GetCachedPermissionsForUserAsync(userId);

            // Simulate a successful login result using cached data
            var offlineResult = new GFC.Core.Models.GfcLoginResult
            {
                Code = GFC.Core.Models.LoginResultCode.Success,
                User = new AppUser { UserId = user.UserId, Username = user.Username, IsActive = true },
                DeviceToken = "offline-session",
                Permissions = actualPermissions
            };
            
            await CompleteLoginAsync(offlineResult);
            return offlineResult;
        }

        return new GFC.Core.Models.GfcLoginResult { Code = GFC.Core.Models.LoginResultCode.Error, ErrorMessageForLog = "Offline login failed. User not found in cache." };
    }

    private async Task CompleteLoginAsync(GFC.Core.Models.GfcLoginResult result)
    {
        var authData = new AuthData 
        { 
            User = result.User!, 
            Token = result.DeviceToken ?? "session",
            Permissions = result.Permissions,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };
        
        _currentUser = result.User;
        
        // Update user service cache and PERMANENT VAULT
        if (result.Permissions != null)
        {
            var mobileService = _userService as MobileUserManagementService;
            mobileService?.UpdateCachedPermissions(result.Permissions);
            if (result.User != null)
            {
                await mobileService!.SavePermissionsToCacheAsync(result.User.UserId, result.Permissions);
            }
        }
        
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", LocalStorageKey, JsonSerializer.Serialize(authData));
        
        if (!string.IsNullOrEmpty(result.DeviceToken))
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "gfc_device_token", result.DeviceToken);
            await _jsRuntime.InvokeVoidAsync("window.setCookie", "GFC_DeviceTrustToken", result.DeviceToken, 0); 
        }

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task LogoutAsync(string? token = null)
    {
        try
        {
            var deviceToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "gfc_device_token");
            if (!string.IsNullOrEmpty(deviceToken))
            {
                await _httpClient.PostAsJsonAsync("/api/mobile-auth/logout", deviceToken);
            }
        }
        catch
        {
            // Ignore network errors on logout. We still want to clear the local session.
        }

        _currentUser = null;
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", LocalStorageKey);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "gfc_device_token");
        await _jsRuntime.InvokeVoidAsync("window.setCookie", "GFC_DeviceTrustToken", "", -1);
        
        (_userService as MobileUserManagementService)?.ClearPermissionCache();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
    
    public async Task RefreshUserAsync()
    {
        var authJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", LocalStorageKey);
        if (!string.IsNullOrEmpty(authJson))
        {
            var authData = JsonSerializer.Deserialize<AuthData>(authJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (authData?.Token != null)
            {
                var response = await _httpClient.GetAsync($"/api/mobile-auth/user?token={authData.Token}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<GFC.Core.Models.GfcLoginResult>();
                    if (result != null && result.Success && result.User != null)
                    {
                        await CompleteLoginAsync(result);
                    }
                }
            }
        }

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task ForceReAuthAsync()
    {
        await LogoutAsync();
    }

    private AuthenticationState CreateAnonymous() => new(new ClaimsPrincipal(new ClaimsIdentity()));

    public class AuthData
    {
        public AppUser? User { get; set; }
        public string? Token { get; set; }
        public List<GFC.Core.DTOs.MobilePermissionDto>? Permissions { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
