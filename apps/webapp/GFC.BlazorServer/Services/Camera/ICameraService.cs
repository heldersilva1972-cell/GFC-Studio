// [NEW]
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    // Camera models
    public class CameraInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CameraCapabilities
    {
        public bool SupportsPTZ { get; set; }
        public bool SupportsZoom { get; set; }
        public bool SupportsPresets { get; set; }
        public List<int> AvailablePresets { get; set; } = new();
        public List<StreamQuality> AvailableQualities { get; set; } = new();
    }

    public enum StreamQuality
    {
        Low = 0,
        SD = 1,
        HD = 2
    }

    public class PTZCommand
    {
        public PTZAction Action { get; set; }
        public int Speed { get; set; } = 50;
        public int? PresetId { get; set; }
    }

    public enum PTZAction
    {
        Up, Down, Left, Right, ZoomIn, ZoomOut, GotoPreset, Stop
    }

    public interface ICameraService
    {
        Task<List<CameraInfo>> GetAllCamerasAsync();
        Task<CameraCapabilities> GetCameraCapabilitiesAsync(int cameraId);
        Task<bool> SendPTZCommandAsync(int cameraId, PTZCommand command);
        Task<byte[]> CaptureSnapshotAsync(int cameraId);
        Task<StreamQuality> GetStreamQualityAsync(int cameraId);
        Task SetStreamQualityAsync(int cameraId, StreamQuality quality);
    }
}
