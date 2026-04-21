using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface ILoginHistoryRepository
{
    void LogLogin(LoginHistory history);
    List<LoginHistory> GetUserLoginHistory(int userId, int limit = 50);
    List<LoginHistory> GetAllLoginHistory(int limit = 100);
}

