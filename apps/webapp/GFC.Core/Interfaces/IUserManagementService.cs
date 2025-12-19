using GFC.Core.DTOs;
using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IUserManagementService
{
    List<UserListItemDto> GetAllUsers();
    List<ActiveMemberDto> GetActiveMembersForUserCreation();
    AppUser? GetUser(int userId);
    int CreateUser(string username, string password, bool isAdmin, int? memberId, string? notes, string? createdBy, bool passwordChangeRequired = false, int? createdByUserId = null);
    void UpdateUser(int userId, string username, string? password, int? memberId, string? notes, int? updatedByUserId = null, bool isAdmin = false, bool isActive = true);
    void DeleteUser(int userId);
    void ChangePassword(int userId, string newPassword, bool clearPasswordChangeRequired = false, int? performedByUserId = null);
    string GenerateUsernameFromMember(int memberId);
    List<LoginHistoryDto> GetUserLoginHistory(int userId, int limit = 50);
    List<LoginHistoryDto> GetAllLoginHistory(int limit = 100);
}

