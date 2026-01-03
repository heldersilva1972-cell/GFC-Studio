using System;
using System.Linq;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Services.Vpn;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    public class UserRevocationService : IUserRevocationService
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;
        private readonly IVpnConfigurationService _vpnConfigurationService;
        private readonly IAuditLogRepository _auditLogRepository; // Use Repo for custom action string if needed, or IAuditLogger if extended
        // IAuditLogger in Core might not have arbitrary string action method easily exposed without custom Action string.
        // But we added "AccessRevoked" to AuditLogActions. 
        // We likely need to use AuditLogRepository directly to insert a custom log entry if IAuditLogger doesn't have a "LogGeneric" method.
        // Checking AuditLogger... it has specific methods.
        // We will use IAuditLogRepository to insert the log manually.
        
        private readonly ILogger<UserRevocationService> _logger;

        public UserRevocationService(
            IUserManagementService userManagementService,
            IDbContextFactory<GfcDbContext> dbContextFactory,
            IVpnConfigurationService vpnConfigurationService,
            IAuditLogRepository auditLogRepository,
            ILogger<UserRevocationService> logger)
        {
            _userManagementService = userManagementService;
            _dbContextFactory = dbContextFactory;
            _vpnConfigurationService = vpnConfigurationService;
            _auditLogRepository = auditLogRepository;
            _logger = logger;
        }

        public async Task RevokeUserAccessAsync(int userId, string reason, int performedByUserId)
        {
            // 1. Safety Checks
            if (userId == performedByUserId)
            {
                throw new InvalidOperationException("You cannot revoke your own access.");
            }

            var targetUser = await _userManagementService.GetUserAsync(userId);
            if (targetUser == null)
            {
                 throw new InvalidOperationException($"User with ID {userId} not found.");
            }

            // Check if target is Super Admin (Assuming "admin" username or specific role/flag logic)
            // Implementation detail: "IsAdmin" flag is simple boolean. 
            // We should ideally prevent revoking another Admin unless we are a Super Admin, but requirement says "Cannot revoke SuperAdmin access".
            // If "IsAdmin" is the only flag, we might assume specific vital users (like ID 1 or 'admin') are protected.
            // Let's protect User ID 1 or checks if IsAdmin is true and we want to be careful.
            // Requirement: "if (targetUser.IsSuperAdmin)..." - AppUser model usually doesn't have IsSuperAdmin, just IsAdmin.
            // We will assume IsAdmin + maybe ID 1 check, or if there is a claim. 
            // For now, let's implement a heuristic: If target is Admin, warn? 
            // Prompt says: "Demote them first". So if IsAdmin is true, throw.
            if (targetUser.IsAdmin)
            {
                 throw new InvalidOperationException("Cannot revoke access for an Administrator. Please demote them first.");
            }

            // 2. Application Access: IsActive = false
            // We need to preserve other fields.
            // UpdateUser(int userId, string username, string? password, int? memberId, string? notes, int? updatedByUserId = null, bool isAdmin = false, bool isActive = true)
            // We pass null for password to keep it unchanged.
            _userManagementService.UpdateUser(
                userId, 
                targetUser.Username, 
                null, // password (no change)
                targetUser.MemberId, 
                targetUser.Notes, 
                performedByUserId, 
                targetUser.IsAdmin, 
                isActive: false // REVOKE
            );

            // 3. Database Operations (Trusted Devices & Magic Links)
            using var context = await _dbContextFactory.CreateDbContextAsync();
            
            // Delete Trusted Devices
            var devices = await context.TrustedDevices.Where(d => d.UserId == userId).ToListAsync();
            context.TrustedDevices.RemoveRange(devices);
            
            // Invalidate Magic Links (MagicLinkTokens)
            // Set IsUsed = true or delete or expire.
            // Requirement says "Invalidate".
            // I'll expire them.
            var tokens = await context.MagicLinkTokens.Where(t => t.UserId == userId && !t.IsUsed).ToListAsync();
            foreach (var t in tokens)
            {
                t.IsUsed = true;
                t.ExpiresAtUtc = DateTime.UtcNow.AddMinutes(-1);
            }
            
            // Also Invalidate Onboarding Tokens (redundant check from VpnService, but good to be sure)
            var onboardingTokens = await context.VpnOnboardingTokens.Where(t => t.UserId == userId && !t.IsUsed).ToListAsync();
            foreach (var t in onboardingTokens)
            {
                t.IsUsed = true;
            }

            await context.SaveChangesAsync();

            // 4. VPN Revocation
            try 
            {
                await _vpnConfigurationService.RevokeUserAccessAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking VPN access for user {UserId}", userId);
                // Don't block the whole flow if VPN service fails, but log it.
            }

            // 5. Audit Log
            // "AccessRevoked"
             _auditLogRepository.Insert(new GFC.Core.Models.AuditLogEntry
            {
                PerformedByUserId = performedByUserId,
                TimestampUtc = DateTime.UtcNow,
                Action = AuditLogActions.AccessRevoked,
                Details = $"User access revoked. Reason: {reason}"
            });
            
            _logger.LogInformation("User {UserId} access revoked by {PerformerId}. Reason: {Reason}", userId, performedByUserId, reason);
        }
    }
}
