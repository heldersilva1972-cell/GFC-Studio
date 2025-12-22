// [NEW]
using GFC.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public interface IRecordingService
    {
        Task<List<Recording>> GetRecordingsForCameraAsync(int cameraId, DateTime start, DateTime end);
        Task<Recording> GetRecordingAsync(Guid recordingId);
        Task<Recording> StartRecordingAsync(int cameraId);
        Task StopRecordingAsync(Guid recordingId);
        Task DeleteRecordingAsync(Guid recordingId);
    }
}
