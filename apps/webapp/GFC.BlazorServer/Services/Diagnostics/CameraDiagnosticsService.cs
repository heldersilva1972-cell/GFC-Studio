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
                var enabledCount = cameras.Count(c => c.IsEnabled == true);
                
                // Check actual online status for enabled cameras
                var onlineCount = 0;
                var checkTasks = cameras
                    .Where(c => c.IsEnabled == true)
                    .Select(async c => 
                    {
                        if (await IsCameraOnlineAsync(c))
                        {
                            System.Threading.Interlocked.Increment(ref onlineCount);
                        }
                    });
                
                await Task.WhenAll(checkTasks);

                var disabled = total - enabledCount;
                // Offline count includes enabled cameras that are offline + disabled cameras (effectively offline)
                // However, usually "Offline" in dashboards implies "Should be online but isn't"
                // So let's count "Offline" as "Enabled but unreachable"
                var offlineCount = enabledCount - onlineCount;

                var info = new CameraSystemInfo
                {
                    TotalCameras = total,
                    OnlineCameras = onlineCount,
                    OfflineCameras = offlineCount, // Reporting failures
                    ActiveStreams = 0, // Not tracked yet
                    StorageUsagePercentage = 0, // Unknown without NVR agent integration
                    NvrStatus = offlineCount > 0 ? HealthStatus.Warning : HealthStatus.Healthy,
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

        public async Task<Dictionary<int, bool>> GetCameraOnlineStatusesAsync()
        {
             var cameras = await _cameraService.GetAllCamerasAsync();
             var results = new Dictionary<int, bool>();
             
             var tasks = cameras.Select(async c => 
             {
                 var isOnline = await IsCameraOnlineAsync(c);
                 lock(results)
                 {
                     results[c.Id] = isOnline;
                 }
             });

             await Task.WhenAll(tasks);
             return results;
        }

        private async Task<bool> IsCameraOnlineAsync(GFC.Core.Models.Camera camera)
        {
            if (camera.IsEnabled != true) return false;
            
            string host = camera.IpAddress;
            if (string.IsNullOrEmpty(host))
            {
                // Try to parse from RTSP URL
                if (!string.IsNullOrEmpty(camera.RtspUrl))
                {
                    try 
                    {
                        var uri = new Uri(camera.RtspUrl.Replace("rtsp://", "http://")); // Mock as http to parse host
                        host = uri.Host;
                    }
                    catch { return false; }
                }
                else 
                {
                    return false;
                }
            }

            try
            {
                using (var client = new System.Net.Sockets.TcpClient())
                {
                    // Try connecting to RTSP port (554) with short timeout
                    var connectTask = client.ConnectAsync(host, 554);
                    var timeoutTask = Task.Delay(2000);
                    
                    var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                    if (completedTask == timeoutTask) return false;
                    
                    return client.Connected;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
