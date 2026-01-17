using GFC.Core.DTOs;
using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IUserManagementService
{
    List<UserListItemDto> GetAllUsers();
    List<ActiveMemberDto> GetActiveMembersForUserCreation();
    AppUser? GetUser(int userId);
    Task<AppUser?> GetUserAsync(int userId);
    int CreateUser(string username, string password, bool isAdmin, int? memberId, string? notes, string? createdBy, bool passwordChangeRequired = false, int? createdByUserId = null, bool mfaEnabled = false);
    void UpdateUser(int userId, string username, string? password, int? memberId, string? notes, int? updatedByUserId = null, bool isAdmin = false, bool isActive = true, bool mfaEnabled = false);
    void DeleteUser(int userId);
    void ChangePassword(int userId, string newPassword, bool clearPasswordChangeRequired = false, int? performedByUserId = null);
    void ChangePassCode(int userId, string newPassCode, bool clearPasswordChangeRequired = false, int? performedByUserId = null);
    string GenerateUsernameFromMember(int memberId);
    List<LoginHistoryDto> GetUserLoginHistory(int userId, int limit = 50);
    List<LoginHistoryDto> GetAllLoginHistory(int limit = 100);
    
    // Page Permission Management
    List<AppPage> GetAllPages();
    List<AppPage> GetActivePages();
    List<UserPagePermission> GetUserPagePermissions(int userId);
    bool UserHasPageAccess(int userId, string pageRoute);
    void SetUserPagePermissions(int userId, List<int> pageIds, string grantedBy);
    void GrantAllPagePermissions(int userId, string grantedBy);
    void CopyUserPermissions(int sourceUserId, int targetUserId, string grantedBy);
    
    // Default Permissions
    List<int> GetDefaultPageIds();
    void SetDefaultPageIds(List<int> pageIds);
}
