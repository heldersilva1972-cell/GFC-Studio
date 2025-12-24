// [NEW]
using Microsoft.AspNetCore.Http;

namespace GFC.BlazorServer.Services
{
    public interface IDeviceDetectionService
    {
        DeviceInfo DetectDevice(HttpContext context);
    }

    public class DeviceInfo
    {
        public string Type { get; set; }
        public string OS { get; set; }
        public string AppStoreUrl { get; set; }
    }
}
