// [NEW]
using GFC.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public interface ICameraAuditLogService
    {
        Task LogActionAsync(int cameraId, int userId, string action, string details);
        Task<List<CameraAuditLog>> GetLogsForCameraAsync(int cameraId, DateTime start, DateTime end);
        Task<List<CameraAuditLog>> GetLogsForUserAsync(int userId, DateTime start, DateTime end);
    }
}
