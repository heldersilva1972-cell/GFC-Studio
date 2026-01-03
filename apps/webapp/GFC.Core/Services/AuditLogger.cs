// [MODIFIED]
using System;
using System.Collections.Generic;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Extensions.Logging;

namespace GFC.Core.Services
{
    public class AuditLogger : IAuditLogger
    {
        private readonly IAuditLogRepository _repository;
        private readonly ILogger<AuditLogger> _logger;

        public AuditLogger(IAuditLogRepository repository, ILogger<AuditLogger> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Log(int? performedByUserId, string action, string? details = null)
        {
            Log(action, performedByUserId, null, details);
        }

        public void Log(string action, int? performedByUserId, int? targetUserId, string? details = null)
        {
            var sanitizedTargetUserId = IsUserAccountAction(action) ? targetUserId : null;

            WriteEntry(new AuditLogEntry
            {
                PerformedByUserId = performedByUserId,
                TargetUserId = sanitizedTargetUserId,
                Action = action,
                Details = details
            });
        }

        public void LogAdminCreation(int? performedByUserId, int targetUserId, string username, int? memberId)
        {
            var details = $"Admin created for '{username}' (memberId: {memberId?.ToString() ?? "none"}; roles: Admin)";
            Log(AuditLogActions.UserCreated, performedByUserId, targetUserId, details);
        }

        public void LogPasswordReset(int? performedByUserId, int targetUserId, bool isSelfService, string? notes = null)
        {
            var details = $"Password reset (self-service: {isSelfService}, notes: {notes ?? "none"})";
            Log(AuditLogActions.PasswordChanged, performedByUserId, targetUserId, details);
        }

        public void LogSuspiciousLoginAttempt(string username, string? ipAddress, string reason, int? targetUserId = null)
        {
            var details = $"Suspicious login attempt for '{username}' from {ipAddress ?? "unknown"}: {reason}";
            Log(AuditLogActions.LoginFailed, null, targetUserId, details);
        }

        private static bool IsUserAccountAction(string action)
        {
            return action is AuditLogActions.UserCreated
                or AuditLogActions.PasswordChanged
                or AuditLogActions.LoginFailed;
        }

        private void WriteEntry(AuditLogEntry entry)
        {
            try
            {
                if (entry.TimestampUtc == default)
                {
                    entry.TimestampUtc = DateTime.UtcNow;
                }

                _repository.Insert(entry);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to write audit log entry for action {Action}", entry.Action);
            }
        }
    }
}
