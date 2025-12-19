using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IUserRepository
{
    AppUser? GetByUsername(string username);
    AppUser? GetById(int userId);
    AppUser? GetByMemberId(int memberId);
    List<AppUser> GetAllUsers();
    int CreateUser(AppUser user);
    void UpdateUser(AppUser user);
    void DeleteUser(int userId);
    void UpdateLastLogin(int userId, DateTime loginDate);
    bool UsernameExists(string username, int? excludeUserId = null);
    void ClearPasswordChangeRequired(int userId);
}

