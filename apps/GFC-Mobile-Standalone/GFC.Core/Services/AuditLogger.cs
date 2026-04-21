using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

/// <summary>
/// Centralized logging service for tracking system-wide activity, security events, and member changes.
/// Implements IAuditLogger and provides structured constants via AuditLogActions.
/// </summary>
public class AuditLogger : IAuditLogger
{
    private readonly IAuditLogRepository _repository;

    public AuditLogger(IAuditLogRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Records a standard audit log entry.
    /// </summary>
    public void Log(string action, int? performedByUserId, int? targetUserId, string? details = null, string? ipAddress = null, string? deviceToken = null, int? targetMemberId = null)
    {
        var entry = new AuditLogEntry
        {
            Action = action,
            PerformedByUserId = performedByUserId,
            TargetUserId = targetUserId,
            TargetMemberId = targetMemberId,
            Details = details,
            IpAddress = ipAddress,
            DeviceToken = deviceToken,
            TimestampUtc = DateTime.UtcNow
        };
        _repository.Insert(entry);
    }

    /// <summary>
    /// Specialized log for administrative user creation.
    /// </summary>
    public void LogAdminCreation(int? performedByUserId, int targetUserId, string username, int? memberId, string? ipAddress = null, string? deviceToken = null)
    {
        Log(AuditLogActions.AdminUserCreated, performedByUserId, targetUserId, $"Username: {username}, MemberId: {memberId}", ipAddress, deviceToken);
    }

    /// <summary>
    /// Logs password reset events, both self-service and administrative.
    /// </summary>
    public void LogPasswordReset(int? performedByUserId, int targetUserId, bool isSelfService, string? notes = null, string? ipAddress = null, string? deviceToken = null)
    {
        Log(isSelfService ? AuditLogActions.PasswordResetSelf : AuditLogActions.PasswordResetAdmin, performedByUserId, targetUserId, notes, ipAddress, deviceToken);
    }

    /// <summary>
    /// Logs security-related login failures or suspicious patterns.
    /// </summary>
    public void LogSuspiciousLoginAttempt(string username, string? ipAddress, string reason, int? targetUserId = null, string? deviceToken = null)
    {
        Log("Suspicious Login Attempt", null, targetUserId, $"Username: {username}, Reason: {reason}", ipAddress, deviceToken);
    }

    /// <summary>
    /// Logs a page view for analytics and user behavior tracking. Returns the LogId for duration updates.
    /// </summary>
    public int LogPageView(int userId, string pageUrl, string? pageTitle = null, string? ipAddress = null, string? deviceToken = null)
    {
        // NO-OP: Disabled PageView collection to reduce audit log clutter as per user request
        return 0;
    }

    /// <summary>
    /// Async version of page view logging.
    /// </summary>
    public async Task<int> LogPageViewAsync(int userId, string pageUrl, string? pageTitle = null, string? ipAddress = null, string? deviceToken = null)
    {
        // NO-OP: Disabled PageView collection to reduce audit log clutter as per user request
        return await Task.FromResult(0);
    }

    /// <summary>
    /// Updates the time-spent-on-page metric for a previously logged page view.
    /// </summary>
    public void UpdatePageViewDuration(int userId, string pageUrl, int seconds, string? ipAddress = null, string? deviceToken = null, int? logId = null)
    {
        _repository.UpdateDuration(userId, pageUrl, seconds, ipAddress, deviceToken, logId);
    }

    /// <summary>
    /// Async version of page view duration update.
    /// </summary>
    public async Task UpdatePageViewDurationAsync(int userId, string pageUrl, int seconds, string? ipAddress = null, string? deviceToken = null, int? logId = null)
    {
        UpdatePageViewDuration(userId, pageUrl, seconds, ipAddress, deviceToken, logId);
        await Task.CompletedTask;
    }
}

/// <summary>
/// Standardized action names for Audit Logs to ensure consistency across the application.
/// </summary>
public static class AuditLogActions
{
    // Authentication & Security
    public const string LoginSuccessPassword = "Login Success (Password)";
    public const string LoginSuccessMagicLink = "Login Success (Magic Link)";
    public const string LoginSuccessPasskey = "Login Success (Passkey)";
    public const string PasswordResetSelf = "Self-Service Password Reset";
    public const string PasswordResetAdmin = "Admin Password Reset";
    public const string AdminUserCreated = "Admin User Created";
    public const string AccessRevoked = "Access Revoked";
    public const string UserManagementChanged = "User Management Changed";
    
    // Member Operations
    public const string MemberAdded = "Member Added";
    public const string MemberUpdated = "Member Updated";
    public const string MemberDeleted = "Member Deleted";
    public const string MemberArchived = "Member Archived";
    public const string MemberStatusChanged = "Member Status Changed";
    public const string LifeStatusChanged = "Life Status Changed";
    public const string DirectorRoleChanged = "Director Role Changed";
    
    // Financial & Dues
    public const string DuesPaymentAdded = "Dues Payment Added";
    public const string DuesPaymentUpdated = "Dues Payment Updated";
    public const string DuesAdvancedAdded = "Dues Advanced Added";
    public const string DuesChanged = "Dues Changed";
    public const string DuesWaiverAdded = "Dues Waiver Added";
    public const string DuesWaiverDeleted = "Dues Waiver Deleted";
    public const string DuesWaiverRemoved = "Dues Waiver Removed";
    
    // Physical Assets & Key Cards
    public const string PhysicalKeyAssigned = "Physical Key Assigned";
    public const string PhysicalKeyReturned = "Physical Key Returned";
    public const string KeyCardAdded = "Key Card Added";
    public const string KeyCardDeactivated = "KeyCardDeactivated"; 
    public const string KeyCardActivated = "KeyCardActivated";
    
    // System & Network
    public const string VpnConfigChanged = "VPN Configuration Changed";
    public const string VpnConfigDownloaded = "VPN Config Downloaded";
    public const string VpnProfileCreated = "VPN Profile Created";
    public const string VpnProfileRevoked = "VPN Profile Revoked";
    public const string VpnKeyRotated = "VPN Key Rotated";
    public const string VpnOnboardingStarted = "VPN Onboarding Started";
    public const string VpnAppleProfileDownloaded = "VPN Apple Profile Downloaded";
    public const string VpnWindowsSetupDownloaded = "VPN Windows Setup Downloaded";
    public const string VpnCaCertDownloaded = "VPN Root CA Downloaded";
    public const string VpnOnboardingCompleted = "VPN Onboarding Completed";
    public const string ControllerSyncInitiated = "Controller Sync Initiated";
    public const string DataExported = "Data Exported";
    public const string ShiftReportSubmitted = "Shift Report Submitted";
    public const string ShiftReportCorrected = "Shift Report Corrected";
    public const string NPQueuePromote = "Non-Portuguese Queue Member Promoted";

    /// <summary>
    /// Gets all defined audit action strings for filtering/UI purposes.
    /// </summary>
    public static string[] All => new[]
    {
        LoginSuccessPassword, LoginSuccessMagicLink, LoginSuccessPasskey,
        PasswordResetSelf, PasswordResetAdmin, AdminUserCreated, AccessRevoked, UserManagementChanged,
        MemberAdded, MemberUpdated, MemberDeleted, MemberArchived, MemberStatusChanged, LifeStatusChanged, DirectorRoleChanged,
        DuesPaymentAdded, DuesPaymentUpdated, DuesAdvancedAdded, DuesChanged, DuesWaiverAdded, DuesWaiverDeleted, DuesWaiverRemoved,
        PhysicalKeyAssigned, PhysicalKeyReturned, KeyCardAdded, KeyCardDeactivated, KeyCardActivated,
        VpnConfigChanged, VpnConfigDownloaded, VpnProfileCreated, VpnProfileRevoked, VpnKeyRotated,
        VpnOnboardingStarted, VpnAppleProfileDownloaded, VpnWindowsSetupDownloaded, VpnCaCertDownloaded, VpnOnboardingCompleted,
        ControllerSyncInitiated, DataExported, ShiftReportSubmitted, ShiftReportCorrected, NPQueuePromote,
        "Suspicious Login Attempt"
    };
}