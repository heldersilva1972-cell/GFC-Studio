// [NEW]
using System.Threading.Tasks;
using GFC.VideoAgent.Models;
using Microsoft.Extensions.Logging;

namespace GFC.VideoAgent.Services
{
    public class NvrService
    {
        private readonly ILogger<NvrService> _logger;

        public NvrService(ILogger<NvrService> logger)
        {
            _logger = logger;
        }

        public Task<bool> SendPtzCommandAsync(int cameraId, PTZCommand command)
        {
            _logger.LogInformation($"Simulating PTZ command '{command.Action}' for camera {cameraId} with speed {command.Speed}.");
            // In a real implementation, this would send a command to the NVR/camera via its API (e.g., ONVIF)
            return Task.FromResult(true);
        }
    }
}
