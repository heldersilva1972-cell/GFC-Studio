using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for notifying admins of security events
/// </summary>
public interface ISecurityNotificationService
{
    Task NotifyBlockedAccessAsync(string ipAddress, string path, string? userAgent, int attemptCount);
    Task NotifyRateLimitAsync(string ipAddress, int attemptCount);
}

public class SecurityNotificationService : ISecurityNotificationService
{
    private readonly ILogger<SecurityNotificationService> _logger;

    public SecurityNotificationService(ILogger<SecurityNotificationService> logger)
    {
        _logger = logger;
    }

    public async Task NotifyBlockedAccessAsync(string ipAddress, string path, string? userAgent, int attemptCount)
    {
        // Log security event for admin review
        _logger.LogWarning(
            "SECURITY: Blocked access from IP {IpAddress} to {Path}. " +
            "User-Agent: {UserAgent}. Total attempts: {AttemptCount}",
            ipAddress, path, userAgent ?? "Unknown", attemptCount);
        
        // TODO: When notification system is implemented, create in-app notification here
        await Task.CompletedTask;
    }

    public async Task NotifyRateLimitAsync(string ipAddress, int attemptCount)
    {
        // Log critical security event
        _logger.LogError(
            "SECURITY ALERT: IP {IpAddress} has been rate-limited after {AttemptCount} blocked access attempts in the last hour.",
            ipAddress, attemptCount);
        
        // TODO: When notification system is implemented, create urgent notification here
        // TODO: Consider sending email to admins for critical events
        await Task.CompletedTask;
    }
}
