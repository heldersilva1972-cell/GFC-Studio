// [NEW]
using GFC.Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace GFC.BlazorServer.Services.Camera
{
    public interface IVideoAccessService
    {
        // VPN Profile Management
        Task<List<VpnProfile>> GetVpnProfilesAsync();
        Task<VpnProfile> GetVpnProfileAsync(int id);
        Task<VpnProfile> CreateVpnProfileAsync(VpnProfile profile);
        Task UpdateVpnProfileAsync(VpnProfile profile);
        Task RevokeVpnProfileAsync(int profileId, int revokingUserId, string reason);

        // Active Session Tracking
        Task<List<VpnSession>> GetActiveVpnSessionsAsync();
        Task<VpnSession> LogVpnSessionStartAsync(int profileId, string clientIp);
        Task LogVpnSessionEndAsync(int sessionId);
        Task DisconnectSessionAsync(int sessionId);

        // Audit Logging
        Task<PagedResult<VideoAccessAudit>> GetVideoAccessAuditAsync(int page, int pageSize, string filter, DateTime? from, DateTime? to);
        Task LogVideoAccessAsync(VideoAccessAudit logEntry);

        // Security Alerts
        Task<List<SecurityAlert>> GetSecurityAlertsAsync(string status);
        Task<SecurityAlert> GetSecurityAlertAsync(int id);
        Task CreateSecurityAlertAsync(SecurityAlert alert);
        Task UpdateSecurityAlertStatusAsync(int alertId, string newStatus, int updatingUserId);
    }
}
