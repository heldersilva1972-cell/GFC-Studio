// [NEW]
using GFC.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public interface ICameraEventService
    {
        Task LogEventAsync(int cameraId, CameraEventType eventType, string description);
        Task<List<CameraEvent>> GetEventsForCameraAsync(int cameraId, DateTime start, DateTime end);
    }
}
