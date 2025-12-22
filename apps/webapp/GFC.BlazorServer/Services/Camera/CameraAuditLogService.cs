// [NEW]
using GFC.Core.Models;
using GFC.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public class CameraAuditLogService : ICameraAuditLogService
    {
        private readonly ApplicationDbContext _context;

        public CameraAuditLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(int cameraId, int userId, string action, string details)
        {
            var logEntry = new CameraAuditLog
            {
                CameraId = cameraId,
                UserId = userId,
                Action = action,
                Details = details,
                Timestamp = DateTime.UtcNow
            };
            _context.CameraAuditLogs.Add(logEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CameraAuditLog>> GetLogsForCameraAsync(int cameraId, DateTime start, DateTime end)
        {
            return await _context.CameraAuditLogs
                .Where(l => l.CameraId == cameraId && l.Timestamp >= start && l.Timestamp <= end)
                .ToListAsync();
        }

        public async Task<List<CameraAuditLog>> GetLogsForUserAsync(int userId, DateTime start, DateTime end)
        {
            return await _context.CameraAuditLogs
                .Where(l => l.UserId == userId && l.Timestamp >= start && l.Timestamp <= end)
                .ToListAsync();
        }
    }
}
