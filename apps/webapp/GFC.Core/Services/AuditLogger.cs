using System;
using System.Collections.Generic;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.Extensions.Logging;

namespace GFC.Core.Services;

public class AuditLogger : IAuditLogger
{
    private readonly IAuditLogRepository _repository;
    private readonly ILogger<AuditLogger> _logger;

    public AuditLogger(IAuditLogRepository repository, ILogger<AuditLogger> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        Log(AuditLogActions.AdminCreated, performedByUserId, targetUserId, details);
    }

    public void LogPasswordReset(int? performedByUserId, int targetUserId, bool isSelfService, string? notes = null)
    {
        var details = $"Password reset (self-service: {isSelfService}, notes: {notes ?? "none"})";
        Log(AuditLogActions.PasswordReset, performedByUserId, targetUserId, details);
    }

    public void LogSuspiciousLoginAttempt(string username, string? ipAddress, string reason, int? targetUserId = null)
    {
        var details = $"Suspicious login attempt for '{username}' from {ipAddress ?? "unknown"}: {reason}";
        Log(AuditLogActions.SuspiciousLoginAttempt, null, targetUserId, details);
    }

    private static bool IsUserAccountAction(string action)
    {
        return action is AuditLogActions.AdminCreated
            or AuditLogActions.PasswordReset
            or AuditLogActions.SuspiciousLoginAttempt;
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

public static class AuditLogActions
{
    public const string AdminCreated = "AdminCreated";
    public const string PasswordReset = "PasswordReset";
    public const string SuspiciousLoginAttempt = "SuspiciousLoginAttempt";
    public const string DuesChanged = "DuesChanged";
    public const string DuesWaiverChanged = "DuesWaiverChanged";
    public const string NPQueueAdd = "NPQueueAdd";
    public const string NPQueuePromote = "NPQueuePromote";
    public const string NPQueueRemove = "NPQueueRemove";
    public const string LifeStatusChanged = "LifeStatusChanged";
    public const string DirectorRoleChanged = "DirectorRoleChanged";
    public const string KeyCardAdded = "KeyCardAdded";
    public const string KeyCardReplaced = "KeyCardReplaced";
    public const string KeyCardDisabled = "KeyCardDisabled";
    public const string KeyCardEnabled = "KeyCardEnabled";
    public const string PhysicalKeyAssigned = "PhysicalKeyAssigned";
    public const string PhysicalKeyReturned = "PhysicalKeyReturned";
    
    // Auth & Session Actions
    public const string MagicLinkSent = "MagicLinkSent";
    public const string LoginSuccessPassword = "LoginSuccess (Password)";
    public const string LoginSuccessMagicLink = "LoginSuccess (Magic Link)";
    public const string LogoutIdle = "LogoutIdle";
    public const string LogoutAbsolute = "LogoutAbsolute";
    public const string SessionInvalidatedVpnLost = "SessionInvalidatedVpnLost";
    public const string AccessRevoked = "AccessRevoked";
    
    // VPN & Onboarding Actions
    public const string VpnOnboardingStarted = "VpnOnboardingStarted";
    public const string VpnConfigDownloaded = "VpnConfigDownloaded";
    public const string VpnAppleProfileDownloaded = "VpnAppleProfileDownloaded";
    public const string VpnWindowsSetupDownloaded = "VpnWindowsSetupDownloaded";
    public const string VpnCaCertDownloaded = "VpnCaCertDownloaded";
    public const string VpnProfileCreated = "VpnProfileCreated";
    public const string VpnProfileRevoked = "VpnProfileRevoked";
    public const string VpnKeyRotated = "VpnKeyRotated";
    public const string VpnOnboardingCompleted = "VpnOnboardingCompleted";
    
    // Database Maintenance Actions
    public const string DbBackupCreated = "DbBackupCreated";
    public const string DbBackupDownloaded = "DbBackupDownloaded";
    public const string DbMigrationsApplied = "DbMigrationsApplied";
    public const string DbRestoreStarted = "DbRestoreStarted";
    public const string DbRestoreCompleted = "DbRestoreCompleted";
    public const string DbRestoreFailed = "DbRestoreFailed";
    public const string DbMaintenanceModeEnabled = "DbMaintenanceModeEnabled";
    public const string DbMaintenanceModeDisabled = "DbMaintenanceModeDisabled";


    public static readonly IReadOnlyList<string> All = new[]
    {
        AdminCreated,
        PasswordReset,
        SuspiciousLoginAttempt,
        DuesChanged,
        DuesWaiverChanged,
        NPQueueAdd,
        NPQueuePromote,
        NPQueueRemove,
        LifeStatusChanged,
        DirectorRoleChanged,
        KeyCardAdded,
        KeyCardReplaced,
        KeyCardDisabled,
        KeyCardEnabled,
        PhysicalKeyAssigned,
        PhysicalKeyReturned,
        MagicLinkSent,
        LoginSuccessPassword,
        LoginSuccessMagicLink,
        LogoutIdle,
        LogoutAbsolute,
        SessionInvalidatedVpnLost,
        AccessRevoked,
        VpnOnboardingStarted,
        VpnConfigDownloaded,
        VpnAppleProfileDownloaded,
        VpnWindowsSetupDownloaded,
        VpnCaCertDownloaded,
        VpnProfileCreated,
        VpnProfileRevoked,
        VpnKeyRotated,
        VpnOnboardingCompleted,
        DbBackupCreated,
        DbBackupDownloaded,
        DbMigrationsApplied,
        DbRestoreStarted,
        DbRestoreCompleted,
        DbRestoreFailed,
        DbMaintenanceModeEnabled,
        DbMaintenanceModeDisabled
    };
}
