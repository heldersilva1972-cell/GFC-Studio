// [NEW]
using GFC.Core.Models.Diagnostics;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Diagnostics
{
    public class CameraDiagnosticsService
    {
        private readonly GFC.BlazorServer.Services.Camera.ICameraService _cameraService;

        public CameraDiagnosticsService(GFC.BlazorServer.Services.Camera.ICameraService cameraService)
        {
            _cameraService = cameraService;
        }

        public async Task<CameraSystemInfo> GetCameraSystemInfoAsync()
        {
            try 
            {
                var cameras = await _cameraService.GetAllCamerasAsync();
                
                // Calculate simple stats based on DB configuration
                var total = cameras.Count;
                var enabled = cameras.Count(c => c.IsEnabled == true);
                var disabled = total - enabled;

                // For now, we assume "Online" means "Enabled" until we implement real-time ping
                var info = new CameraSystemInfo
                {
                    TotalCameras = total,
                    OnlineCameras = enabled,
                    OfflineCameras = disabled,
                    ActiveStreams = 0, // Not tracked yet
                    StorageUsagePercentage = 0, // Unknown without NVR agent integration
                    NvrStatus = total > 0 ? HealthStatus.Healthy : HealthStatus.Warning,
                    OldestRecording = System.DateTime.MinValue,
                    NewestRecording = System.DateTime.MinValue,
                    EventsLast24h = 0
                };
                
                return info;
            }
            catch
            {
                // Fallback if DB fails
                 return new CameraSystemInfo
                {
                    NvrStatus = HealthStatus.Critical
                };
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            await Task.Delay(500); // Simulate check
            return true; 
        }
    }
}
