// [NEW]
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Hubs;
using GFC.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public class VideoAccessService : IVideoAccessService
    {
        private readonly GfcDbContext _context;
        private readonly IHubContext<VideoAccessHub> _hubContext;

        public VideoAccessService(GfcDbContext context, IHubContext<VideoAccessHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        #region VPN Profile Management

        public async Task<List<VpnProfile>> GetVpnProfilesAsync()
        {
            return await _context.Set<VpnProfile>().Include(p => p.User).ToListAsync();
        }

        public async Task<VpnProfile> GetVpnProfileAsync(int id)
        {
            return await _context.Set<VpnProfile>().Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<VpnProfile> CreateVpnProfileAsync(VpnProfile profile)
        {
            _context.Set<VpnProfile>().Add(profile);
            await _context.SaveChangesAsync();
            return profile;
        }

        public async Task UpdateVpnProfileAsync(VpnProfile profile)
        {
            _context.Entry(profile).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RevokeVpnProfileAsync(int profileId, int revokingUserId, string reason)
        {
            var profile = await _context.Set<VpnProfile>().FindAsync(profileId);
            if (profile != null)
            {
                profile.RevokedAt = DateTime.UtcNow;
                profile.RevokedBy = revokingUserId;
                profile.RevokedReason = reason;
                await _context.SaveChangesAsync();
            }
        }

        #endregion

        #region Active Session Tracking

        public async Task<List<VpnSession>> GetActiveVpnSessionsAsync()
        {
            return await _context.VpnSessions
                .Include(s => s.User)
                .Where(s => s.DisconnectedAt == null)
                .ToListAsync();
        }

        public async Task<VpnSession> LogVpnSessionStartAsync(int profileId, string clientIp)
        {
            var profile = await _context.Set<VpnProfile>()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == profileId);
            if (profile == null)
            {
                throw new KeyNotFoundException("VPN profile not found.");
            }

            var session = new VpnSession
            {
                VpnProfileId = profileId,
                UserId = profile.UserId,
                ClientIP = clientIp,
                ConnectedAt = DateTime.UtcNow
            };

            _context.VpnSessions.Add(session);

            profile.LastUsedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Populate user details for the real-time update
            session.User = profile.User;
            await _hubContext.Clients.All.SendAsync("SessionStarted", session);

            return session;
        }

        public async Task LogVpnSessionEndAsync(int sessionId)
        {
            var session = await _context.VpnSessions.FindAsync(sessionId);
            if (session != null && session.DisconnectedAt == null)
            {
                session.DisconnectedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("SessionEnded", sessionId);
            }
        }

        public async Task DisconnectSessionAsync(int sessionId)
        {
            await LogVpnSessionEndAsync(sessionId);
        }

        #endregion

        #region Audit Logging

        public async Task<PagedResult<VideoAccessAudit>> GetVideoAccessAuditAsync(int page, int pageSize, string filter, DateTime? from, DateTime? to)
        {
            var query = _context.VideoAccessAudits.Include(a => a.User).AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(a =>
                    a.User.Username.Contains(filter) ||
                    a.AccessType.Contains(filter) ||
                    a.CameraName.Contains(filter) ||
                    a.ClientIP.Contains(filter));
            }

            if (from.HasValue)
            {
                query = query.Where(a => a.SessionStart >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(a => a.SessionStart <= to.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(a => a.SessionStart)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedResult<VideoAccessAudit>(items, totalCount, page, pageSize);
        }

        public async Task LogVideoAccessAsync(VideoAccessAudit logEntry)
        {
            _context.VideoAccessAudits.Add(logEntry);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Security Alerts

        public async Task<List<SecurityAlert>> GetSecurityAlertsAsync(string status)
        {
            var query = _context.SecurityAlerts.Include(a => a.User).Include(a => a.ReviewedByUser).AsQueryable();

            if (!string.IsNullOrEmpty(status) && !status.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(a => a.Status == status);
            }

            return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
        }

        public async Task<SecurityAlert> GetSecurityAlertAsync(int id)
        {
            return await _context.SecurityAlerts.Include(a => a.User).Include(a => a.ReviewedByUser).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task CreateSecurityAlertAsync(SecurityAlert alert)
        {
            _context.SecurityAlerts.Add(alert);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSecurityAlertStatusAsync(int alertId, string newStatus, int updatingUserId)
        {
            var alert = await _context.SecurityAlerts.FindAsync(alertId);
            if (alert != null)
            {
                alert.Status = newStatus;
                alert.ReviewedAt = DateTime.UtcNow;
                alert.ReviewedBy = updatingUserId;
                await _context.SaveChangesAsync();
            }
        }

        #endregion
    }
}
