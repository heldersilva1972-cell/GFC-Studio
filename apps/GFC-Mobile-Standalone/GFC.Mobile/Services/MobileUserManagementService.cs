using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.DTOs;
using System.Net.Http.Json;

namespace GFC.Mobile.Services;

public class MobileUserManagementService : IUserManagementService
{
    private readonly HttpClient _http;
    private List<UserPagePermission> _cachedPermissions = new();

    public MobileUserManagementService(HttpClient http)
    {
        _http = http;
    }

    // This method is called synchronously by the UI
    public List<UserPagePermission> GetUserPagePermissions(int userId)
    {
        // RETURN CACHED OR DEFAULT
        if (_cachedPermissions.Any()) return _cachedPermissions;
        
        // Default permission for mobile shift report if nothing is cached
        return new List<UserPagePermission>
        {
            new UserPagePermission { Page = new AppPage { PageRoute = "mobile/shift-report" } },
            new UserPagePermission { Page = new AppPage { PageRoute = "widgets/shift-sales-alert" } }
        };
    }

    public bool UserHasPageAccess(int userId, string pageRoute)
    {
        var permissions = GetUserPagePermissions(userId);
        return permissions.Any(p => p.Page?.PageRoute?.ToLower() == pageRoute.ToLower().Trim('/'));
    }

    // IMPLEMENTING ALL INTERFACE MEMBERS TO SATISFY COMPILER (NO GUESSING)
    // We throw NotImplementedException for members not physically used by the mobile pages
    
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
    public UserPagePermission? GetUserPagePermission(int userId, string pageRoute) => throw new NotImplementedException();
    public void GrantAllPagePermissions(int userId, string grantedBy) => throw new NotImplementedException();
    public void CopyUserPermissions(int sourceUserId, int targetUserId, string grantedBy) => throw new NotImplementedException();
    public List<int> GetDefaultPageIds() => throw new NotImplementedException();
    public void SetDefaultPageIds(List<int> pageIds) => throw new NotImplementedException();
    public void ClearPermissionCache() => throw new NotImplementedException();
}
