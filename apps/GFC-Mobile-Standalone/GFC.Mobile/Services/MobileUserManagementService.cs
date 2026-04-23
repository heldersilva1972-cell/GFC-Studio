using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.DTOs;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;

namespace GFC.Mobile.Services;

public class MobileUserManagementService : IUserManagementService
{
    private readonly HttpClient _http;
    private readonly Microsoft.JSInterop.IJSRuntime _jsRuntime;
    private const string UserCacheKey = "gfc_offline_users";
    private List<GFC.Core.DTOs.MobilePermissionDto> _cachedPermissions = new();
    private bool _hasAttemptedLocalLoad = false;
    private bool _isRevalidating = false;

    public event Action? OnUsersUpdated;

    public MobileUserManagementService(HttpClient http, Microsoft.JSInterop.IJSRuntime jsRuntime)
    {
        _http = http;
        _jsRuntime = jsRuntime;
    }

    // This method is called synchronously by the UI
    public List<GFC.Core.DTOs.MobilePermissionDto> GetUserPagePermissions(int userId)
    {
        // If we have data in memory, return it
        if (_cachedPermissions != null && _cachedPermissions.Count > 0)
        {
            return _cachedPermissions;
        }

        // Trigger background load if not already attempted
        if (!_hasAttemptedLocalLoad)
        {
            _hasAttemptedLocalLoad = true;
            _ = TryLoadFromLocalAsync();
        }

        return _cachedPermissions ?? new List<GFC.Core.DTOs.MobilePermissionDto>();
    }

    private async Task TryLoadFromLocalAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", new object[] { "gfc_auth_state" });
            if (!string.IsNullOrEmpty(json))
            {
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("Permissions", out var permsElement))
                {
                    var perms = System.Text.Json.JsonSerializer.Deserialize<List<GFC.Core.DTOs.MobilePermissionDto>>(permsElement.GetRawText());
                    if (perms != null)
                    {
                        _cachedPermissions = perms;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Service] Local permission load failed: {ex.Message}");
        }
    }

    public void UpdateCachedPermissions(List<GFC.Core.DTOs.MobilePermissionDto> permissions)
    {
        _cachedPermissions = permissions ?? new List<GFC.Core.DTOs.MobilePermissionDto>();
    }

    public void ClearPermissionCache()
    {
        _cachedPermissions = new List<GFC.Core.DTOs.MobilePermissionDto>();
    }

    public GFC.Core.DTOs.MobilePermissionDto? GetUserPagePermission(int userId, string pageRoute)
    {
        var permissions = GetUserPagePermissions(userId);
        var normalized = pageRoute.TrimStart('/').ToLowerInvariant();
        
        return permissions.FirstOrDefault(p => 
            p.PageRoute.TrimStart('/').ToLowerInvariant() == normalized || 
            p.PageRoute.ToLowerInvariant() == pageRoute.ToLowerInvariant());
    }

    public bool UserHasPageAccess(int userId, string pageRoute)
    {
        var permissions = GetUserPagePermissions(userId);
        var normalizedRequest = pageRoute.Trim('/').ToLowerInvariant();
        
        return permissions.Any(p => 
            p.PageRoute.Trim('/').ToLowerInvariant() == normalizedRequest);
    }

    // IMPLEMENTING ALL INTERFACE MEMBERS TO SATISFY COMPILER (NO GUESSING)
    // We throw NotImplementedException for members not physically used by the mobile pages
    
    public async Task<List<UserListItemDto>> GetUsersAsync()
    {
        // 1. Load from cache immediately for instant UI response (Stale)
        List<UserListItemDto> localUsers = new();
        try {
            // [FORCE RESET] If the user is seeing the old hardcoded list, we need to wipe it.
            // We'll clear the cache once to ensure a clean transition to the new dynamic system.
            var cacheVersion = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "gfc_cache_ver");
            if (cacheVersion != "2.1")
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UserCacheKey);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "gfc_cache_ver", "2.1");
            }

            var cachedJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserCacheKey);
            if (!string.IsNullOrEmpty(cachedJson))
            {
                localUsers = JsonSerializer.Deserialize<List<UserListItemDto>>(cachedJson) ?? new List<UserListItemDto>();
            }
        } catch { }

        // 2. Trigger Revalidation in background (The Truth)
        _ = RevalidateUsersAsync();

        return localUsers;
    }

    private async Task RevalidateUsersAsync()
    {
        if (_isRevalidating) return;
        _isRevalidating = true;

        try
        {
            // Add a cache-buster timestamp to ensure we get fresh data from the server
            var timestamp = DateTime.UtcNow.Ticks;
            var freshUsers = await _http.GetFromJsonAsync<List<UserListItemDto>>($"/api/mobile-auth/users?t={timestamp}");
            
            if (freshUsers != null)
            {
                var freshJson = JsonSerializer.Serialize(freshUsers);
                
                // Get current cache to see if we actually need to update the UI
                var cachedJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", UserCacheKey);

                if (freshJson != cachedJson)
                {
                    // Update the authoritative local store
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserCacheKey, freshJson);
                    
                    // Notify the UI to re-render immediately
                    OnUsersUpdated?.Invoke();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SWR] Background revalidation failed: {ex.Message}");
        }
        finally
        {
            _isRevalidating = false;
        }
    }

    public List<UserListItemDto> GetAllUsers() => throw new NotImplementedException();
    public List<ActiveMemberDto> GetEligibleDirectorsForUserCreation() => throw new NotImplementedException();
    public List<ActiveMemberDto> GetEligibleMembersForUserCreation() => throw new NotImplementedException();
    public AppUser? GetUser(int userId) => throw new NotImplementedException();
    public Task<AppUser?> GetUserAsync(int userId) => throw new NotImplementedException();
    public int CreateUser(string username, string password, bool isAdmin, int? memberId, string? notes, string? createdBy, bool passwordChangeRequired = false, int? createdByUserId = null, bool mfaEnabled = false) => throw new NotImplementedException();
    public void UpdateUser(int userId, string username, string? password, int? memberId, string? notes, int? updatedByUserId = null, bool isAdmin = false, bool isActive = true, bool mfaEnabled = false) => throw new NotImplementedException();
    public void DeleteUser(int userId) => throw new NotImplementedException();
    public Task DeleteUserAsync(int userId) => throw new NotImplementedException();
    public void ChangePassword(int userId, string newPassword, bool clearPasswordChangeRequired = false, int? performedByUserId = null) => throw new NotImplementedException();
    public void ChangePassCode(int userId, string newPassCode, bool clearPasswordChangeRequired = false, int? performedByUserId = null) => throw new NotImplementedException();
    public void ClearPassCode(int userId, int? performedByUserId = null) => throw new NotImplementedException();
    public string GenerateUsernameFromMember(int memberId) => throw new NotImplementedException();
    public List<LoginHistoryDto> GetUserLoginHistory(int userId, int limit = 50) => throw new NotImplementedException();
    public List<LoginHistoryDto> GetAllLoginHistory(int limit = 100) => throw new NotImplementedException();
    public List<AppPage> GetAllPages() => throw new NotImplementedException();
    public List<AppPage> GetActivePages() => throw new NotImplementedException();
    public void SetUserPagePermissions(int userId, List<int> pageIds, string grantedBy) => throw new NotImplementedException();
    public void UpdateUserPushPreference(int userId, int pageId, bool receivePush) => throw new NotImplementedException();
    public void UpdateUserEditPreference(int userId, int pageId, bool canEdit) => throw new NotImplementedException();
    public void GrantAllPagePermissions(int userId, string grantedBy) => throw new NotImplementedException();
    public void CopyUserPermissions(int sourceUserId, int targetUserId, string grantedBy) => throw new NotImplementedException();
    public List<int> GetDefaultPageIds() => throw new NotImplementedException();
    public void SetDefaultPageIds(List<int> pageIds) => throw new NotImplementedException();
}
