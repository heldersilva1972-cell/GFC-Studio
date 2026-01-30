namespace GFC.Core.Interfaces;

public interface IAuditLogger
{
    void Log(string action, int? performedByUserId, int? targetUserId, string? details = null, string? ipAddress = null, string? deviceToken = null);
    void LogAdminCreation(int? performedByUserId, int targetUserId, string username, int? memberId, string? ipAddress = null, string? deviceToken = null);
    void LogPasswordReset(int? performedByUserId, int targetUserId, bool isSelfService, string? notes = null, string? ipAddress = null, string? deviceToken = null);
    void LogSuspiciousLoginAttempt(string username, string? ipAddress, string reason, int? targetUserId = null, string? deviceToken = null);
    int LogPageView(int userId, string pageUrl, string? pageTitle = null, string? ipAddress = null, string? deviceToken = null);
    Task<int> LogPageViewAsync(int userId, string pageUrl, string? pageTitle = null, string? ipAddress = null, string? deviceToken = null);
    void UpdatePageViewDuration(int userId, string pageUrl, int seconds, string? ipAddress = null, string? deviceToken = null, int? logId = null);
    Task UpdatePageViewDurationAsync(int userId, string pageUrl, int seconds, string? ipAddress = null, string? deviceToken = null, int? logId = null);
}
