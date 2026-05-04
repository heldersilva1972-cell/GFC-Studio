using GFC.Core.DTOs;
using GFC.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace GFC.Core.Interfaces;

public interface IUserManagementService
{
    event Action? PermissionsUpdated;
    List<UserListItemDto> GetAllUsers();
    Task<List<UserListItemDto>> GetUsersAsync();
    Task<List<UserListItemDto>> GetMobileAuthorizedUsersAsync();
    Task<List<ActiveMemberDto>> GetEligibleDirectorsForUserCreationAsync();
    Task<List<ActiveMemberDto>> GetEligibleMembersForUserCreationAsync(); 
    AppUser? GetUser(int userId);
    Task<AppUser?> GetUserAsync(int userId);
    int CreateUser(string username, string password, bool isAdmin, int? memberId, string? notes, string? createdBy, bool passwordChangeRequired = false, int? createdByUserId = null, bool mfaEnabled = false);
    void UpdateUser(int userId, string username, string? password, int? memberId, string? notes, int? updatedByUserId = null, bool isAdmin = false, bool isActive = true, bool mfaEnabled = false);
    void DeleteUser(int userId);
    Task DeleteUserAsync(int userId); 
    void ChangePassword(int userId, string newPassword, bool clearPasswordChangeRequired = false, int? performedByUserId = null);
    void ChangePassCode(int userId, string newPassCode, bool clearPasswordChangeRequired = false, int? performedByUserId = null);
    void ClearPassCode(int userId, int? performedByUserId = null);
    string GenerateUsernameFromMember(int memberId);
    List<LoginHistoryDto> GetUserLoginHistory(int userId, int limit = 50);
    List<LoginHistoryDto> GetAllLoginHistory(int limit = 100);
    
    // Page Permission Management
    Task<List<AppPage>> GetAllPagesAsync();
    Task<List<AppPage>> GetActivePagesAsync();
    List<GFC.Core.DTOs.MobilePermissionDto> GetUserPagePermissions(int userId);
    bool UserHasPageAccess(int userId, string pageRoute);
    void SetUserPagePermissions(int userId, List<int> pageIds, string grantedBy);
    void UpdateUserPushPreference(int userId, int pageId, bool receivePush);
    void UpdateUserEditPreference(int userId, int pageId, bool canEdit);
    GFC.Core.DTOs.MobilePermissionDto? GetUserPagePermission(int userId, string pageRoute);
    void GrantAllPagePermissions(int userId, string grantedBy);
    void CopyUserPermissions(int sourceUserId, int targetUserId, string grantedBy);
    
    // Default Permissions
    List<int> GetDefaultPageIds();
    void SetDefaultPageIds(List<int> pageIds);
    void ClearPermissionCache();
    Task<GfcLoginResult> RefreshPermissionsAsync(string token);

    // LEGACY SYNC WRAPPERS (To be avoided in UI)
    List<ActiveMemberDto> GetEligibleDirectorsForUserCreation();
    List<ActiveMemberDto> GetEligibleMembersForUserCreation();
    List<AppPage> GetAllPages();
    List<AppPage> GetActivePages();
}
