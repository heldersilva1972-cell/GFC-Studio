namespace GFC.Core.Interfaces;

public interface IAuditLogger
{
    void Log(int? performedByUserId, string action, string? details = null);
    void Log(string action, int? performedByUserId, int? targetUserId, string? details = null);
    void LogAdminCreation(int? performedByUserId, int targetUserId, string username, int? memberId);
    void LogPasswordReset(int? performedByUserId, int targetUserId, bool isSelfService, string? notes = null);
    void LogSuspiciousLoginAttempt(string username, string? ipAddress, string reason, int? targetUserId = null);
}
