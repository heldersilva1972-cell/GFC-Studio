using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GFC.Mobile.Services;

public class MobileUserManagementService : IUserManagementService
{
    private readonly HttpClient _http;
    private readonly Microsoft.JSInterop.IJSRuntime _jsRuntime;
    private const string UserCacheKey = "gfc_offline_users";
    private const string PermissionCachePrefix = "gfc_perms_v2_";
    private List<GFC.Core.DTOs.MobilePermissionDto> _cachedPermissions = new();
    private bool _hasAttemptedLocalLoad = false;
    private bool _isRevalidating = false;

    public event Action? OnUsersUpdated;
    public event Action? PermissionsUpdated;

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public MobileUserManagementService(HttpClient http, Microsoft.JSInterop.IJSRuntime jsRuntime)
    {
        _http = http;
        _jsRuntime = jsRuntime;
    }

    // This method is called synchronously by the UI
    public List<GFC.Core.DTOs.MobilePermissionDto> GetUserPagePermissions(int userId)
    {
        if (!_hasAttemptedLocalLoad)
        {
            _hasAttemptedLocalLoad = true;
            // Background refresh will populate this
        }
        return _cachedPermissions;
    }

    public void UpdateCachedPermissions(List<GFC.Core.DTOs.MobilePermissionDto> permissions)
    {
        _cachedPermissions = permissions;
        PermissionsUpdated?.Invoke();
    }

    public async Task RefreshPermissionsInBackgroundAsync(int userId)
    {
        try {
            using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(3));
            var permissions = await _http.GetFromJsonAsync<List<GFC.Core.DTOs.MobilePermissionDto>>($"api/mobile-users-mgmt/permissions/{userId}", cts.Token);
            if (permissions != null)
            {
                UpdateCachedPermissions(permissions);
                await SavePermissionsToCacheAsync(userId, permissions);
            }
        } catch { }
    }

    public async Task SavePermissionsToCacheAsync(int userId, List<GFC.Core.DTOs.MobilePermissionDto> permissions)
    {
        try {
            var key = PermissionCachePrefix + userId;
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(permissions));
        } catch { }
    }

    public async Task<List<GFC.Core.DTOs.MobilePermissionDto>> GetCachedPermissionsForUserAsync(int userId)
    {
        try {
            var key = PermissionCachePrefix + userId;
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonSerializer.Deserialize<List<GFC.Core.DTOs.MobilePermissionDto>>(json, JsonOptions) ?? new();
            }
        } catch { }
        return new();
    }

    public bool UserHasPageAccess(int userId, string pageRoute)
    {
        var permissions = GetUserPagePermissions(userId);
        var normalizedRequest = pageRoute.Trim('/').ToLowerInvariant();
        
        return permissions.Any(p => 
            p.PageRoute.Trim('/').ToLowerInvariant() == normalizedRequest);
    }

    public async Task<List<UserListItemDto>> GetMobileAuthorizedUsersAsync()
    {
        try {
            var timestamp = DateTime.UtcNow.Ticks;
            using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(10));
            var users = await _http.GetFromJsonAsync<List<UserListItemDto>>($"api/mobile-auth/users?t={timestamp}", cts.Token);
            
            // [REFINED CACHE PROTECTION] 
            if (users != null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserCacheKey, JsonSerializer.Serialize(users));
                return users;
            }
        } catch { }

        // FALLBACK: If server is unreachable OR returned an empty list, try the local vault.
        try {
            var cachedJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserCacheKey);
            if (!string.IsNullOrEmpty(cachedJson))
            {
                return JsonSerializer.Deserialize<List<UserListItemDto>>(cachedJson, JsonOptions) ?? new List<UserListItemDto>();
            }
        } catch { }

        return new List<UserListItemDto>();
    }

    public async Task<List<UserListItemDto>> GetUsersAsync()
    {
        List<UserListItemDto> localUsers = new();
        try {
            var cachedJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserCacheKey);
            if (!string.IsNullOrEmpty(cachedJson))
            {
                localUsers = JsonSerializer.Deserialize<List<UserListItemDto>>(cachedJson, JsonOptions) ?? new List<UserListItemDto>();
            }
        } catch { }

        _ = RevalidateUsersAsync();
        return localUsers;
    }

    private async Task RevalidateUsersAsync()
    {
        if (_isRevalidating) return;
        _isRevalidating = true;
        try
        {
            var timestamp = DateTime.UtcNow.Ticks;
            using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(10));
            var freshUsers = await _http.GetFromJsonAsync<List<UserListItemDto>>($"api/mobile-auth/users?t={timestamp}", cts.Token);
            
            if (freshUsers != null)
            {
                var freshJson = JsonSerializer.Serialize(freshUsers);
                var cachedJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserCacheKey);

                if (freshJson != cachedJson)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserCacheKey, freshJson);
                    OnUsersUpdated?.Invoke();
                }
            }
        }
        catch { }
        finally
        {
            _isRevalidating = false;
        }
    }

    public List<UserListItemDto> GetAllUsers()
    {
        // Never block in Blazor WASM. Return cached or empty.
        return new List<UserListItemDto>();
    }

    public async Task<List<ActiveMemberDto>> GetEligibleDirectorsForUserCreationAsync()
    {
        return await _http.GetFromJsonAsync<List<ActiveMemberDto>>("api/mobile-users-mgmt/eligible-directors") ?? new();
    }
    public List<ActiveMemberDto> GetEligibleDirectorsForUserCreation() => new();

    public async Task<List<ActiveMemberDto>> GetEligibleMembersForUserCreationAsync()
    {
        return await _http.GetFromJsonAsync<List<ActiveMemberDto>>("api/mobile-users-mgmt/eligible-members") ?? new();
    }
    public List<ActiveMemberDto> GetEligibleMembersForUserCreation() => new();

    public async Task<AppUser?> GetUserAsync(int userId)
    {
        return await _http.GetFromJsonAsync<AppUser>($"api/mobile-users-mgmt/user/{userId}");
    }
    public AppUser? GetUser(int userId) => null;

    public async Task<List<AppPage>> GetAllPagesAsync()
    {
        return await _http.GetFromJsonAsync<List<AppPage>>("api/mobile-users-mgmt/pages") ?? new();
    }
    public List<AppPage> GetAllPages() => new();

    public async Task<List<AppPage>> GetActivePagesAsync()
    {
        return await _http.GetFromJsonAsync<List<AppPage>>("api/mobile-users-mgmt/pages/active") ?? new();
    }
    public List<AppPage> GetActivePages() => new();

    public int CreateUser(string username, string password, bool isAdmin, int? memberId, string? notes, string? createdBy, bool passwordChangeRequired = false, int? createdByUserId = null, bool mfaEnabled = false)
    {
        var request = new {
            Username = username,
            Password = password,
            IsAdmin = isAdmin,
            MemberId = memberId,
            Notes = notes,
            PasswordChangeRequired = passwordChangeRequired,
            MfaEnabled = mfaEnabled
        };
        _ = _http.PostAsJsonAsync("api/mobile-users-mgmt/create", request);
        return 1;
    }

    public void UpdateUser(int userId, string username, string? password, int? memberId, string? notes, int? updatedByUserId = null, bool isAdmin = false, bool isActive = true, bool mfaEnabled = false)
    {
        var request = new {
            UserId = userId,
            Username = username,
            Password = password,
            MemberId = memberId,
            Notes = notes,
            IsAdmin = isAdmin,
            IsActive = isActive,
            MfaEnabled = mfaEnabled
        };
        _ = _http.PostAsJsonAsync("api/mobile-users-mgmt/update", request);
    }

    public void DeleteUser(int userId) => _ = _http.DeleteAsync($"api/mobile-users-mgmt/delete/{userId}");
    public async Task DeleteUserAsync(int userId) => await _http.DeleteAsync($"api/mobile-users-mgmt/delete/{userId}");

    public void ChangePassword(int userId, string newPassword, bool clearPasswordChangeRequired = false, int? performedByUserId = null) { }
    public void ChangePassCode(int userId, string newPassCode, bool clearPasswordChangeRequired = false, int? performedByUserId = null) { }
    public void ClearPassCode(int userId, int? performedByUserId = null) { }

    public string GenerateUsernameFromMember(int memberId) => "";
    public List<LoginHistoryDto> GetUserLoginHistory(int userId, int limit = 50) => new();
    public List<LoginHistoryDto> GetAllLoginHistory(int limit = 100) => new();

    public void SetUserPagePermissions(int userId, List<int> pageIds, string grantedBy)
    {
        var request = new { UserId = userId, PageIds = pageIds };
        _ = _http.PostAsJsonAsync("api/mobile-users-mgmt/permissions", request);
    }

    public void UpdateUserPushPreference(int userId, int pageId, bool receivePush) { }
    public void UpdateUserEditPreference(int userId, int pageId, bool canEdit) { }
    public GFC.Core.DTOs.MobilePermissionDto? GetUserPagePermission(int userId, string pageRoute) => null;
    public void GrantAllPagePermissions(int userId, string grantedBy) { }
    public void CopyUserPermissions(int sourceUserId, int targetUserId, string grantedBy) { }
    public List<int> GetDefaultPageIds() => new();
    public void SetDefaultPageIds(List<int> pageIds) { }
    
    public void ClearPermissionCache() 
    {
        _cachedPermissions = new();
        PermissionsUpdated?.Invoke();
    }

    public async Task<GfcLoginResult> RefreshPermissionsAsync(string token)
    {
        try
        {
            // Standalone PWA uses token-based background refresh logic
            // Fetch fresh permissions and user context from the server
            Console.WriteLine($"[SYNC] Refreshing permissions for token: {token.Substring(0, Math.Min(token.Length, 8))}...");
            var response = await _http.GetAsync($"api/mobile-auth/user?token={Uri.EscapeDataString(token)}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GfcLoginResult>();
                Console.WriteLine($"[SYNC] Server response Success: {result?.Success}, Permissions Count: {result?.Permissions?.Count ?? 0}");
                if (result != null && result.Success && result.Permissions != null)
                {
                    // 1. Update internal memory cache
                    UpdateCachedPermissions(result.Permissions);
                    
                    // 2. Update the permanent user-specific vault (for offline login fallback)
                    if (result.User != null)
                    {
                        await SavePermissionsToCacheAsync(result.User.UserId, result.Permissions);
                    }

                    // 3. Update the global session state in localStorage (gfc_auth_state)
                    // This ensures fresh permissions are picked up by the AuthProvider on next load
                    try
                    {
                        var authJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "gfc_auth_state");
                        if (!string.IsNullOrEmpty(authJson))
                        {
                            using var doc = JsonDocument.Parse(authJson);
                            var root = doc.RootElement;
                            
                            // Reconstruct the full AuthData object to ensure all fields are preserved
                            var updatedData = new Dictionary<string, object>();
                            foreach (var prop in root.EnumerateObject())
                            {
                                if (prop.Name.Equals("Permissions", StringComparison.OrdinalIgnoreCase))
                                {
                                    updatedData["Permissions"] = result.Permissions;
                                }
                                else
                                {
                                    // Use GetRawText and re-deserialize to avoid JsonElement serialization issues
                                    updatedData[prop.Name] = JsonSerializer.Deserialize<object>(prop.Value.GetRawText())!;
                                }
                            }
                            
                            // Ensure Permissions is present even if it wasn't there before
                            if (!updatedData.Keys.Any(k => k.Equals("Permissions", StringComparison.OrdinalIgnoreCase)))
                            {
                                updatedData["Permissions"] = result.Permissions;
                            }

                            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "gfc_auth_state", JsonSerializer.Serialize(updatedData));
                            Console.WriteLine("[SYNC] Global auth state updated in localStorage.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[SYNC ERROR] Failed to update global auth state: {ex.Message}");
                    }

                    return result;
                }
            }
            return new GfcLoginResult { Code = LoginResultCode.Error, ErrorMessageForLog = "Server sync failed." };
        }
        catch (Exception ex)
        {
            return new GfcLoginResult { Code = LoginResultCode.Error, ErrorMessageForLog = ex.Message };
        }
    }
}
