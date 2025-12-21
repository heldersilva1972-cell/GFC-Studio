// [NEW]
using GFC.VideoAgent.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GFC.VideoAgent.Services
{
    public class StreamManager : BackgroundService
    {
        private readonly ILogger<StreamManager> _logger;
        private readonly FFmpegService _ffmpegService;
        private readonly IConfiguration _configuration;
        private readonly List<CameraStream> _streams = new();
        private readonly ConcurrentDictionary<int, DateTime> _restartCooldowns = new();
        private static readonly TimeSpan RestartCooldown = TimeSpan.FromSeconds(30);

        public StreamManager(ILogger<StreamManager> logger, FFmpegService ffmpegService, IConfiguration configuration)
        {
            _logger = logger;
            _ffmpegService = ffmpegService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("StreamManager is starting.");

            var cameraSettings = _configuration.GetSection("NvrSettings:Cameras").Get<List<CameraConfig>>();
            if (cameraSettings == null || !cameraSettings.Any())
            {
                _logger.LogWarning("No cameras found in configuration. StreamManager will not start any streams.");
                return;
            }

            foreach (var cam in cameraSettings.Where(c => c.Enabled))
            {
                var stream = new CameraStream
                {
                    CameraId = cam.Id,
                    Name = cam.Name,
                    RtspUrl = $"rtsp://{_configuration["NvrSettings:Username"]}:{_configuration["NvrSettings:Password"]}@{_configuration["NvrSettings:IpAddress"]}:{_configuration["NvrSettings:RtspPort"]}{cam.RtspPath}"
                };
                _streams.Add(stream);
                _ffmpegService.StartStream(stream);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
                CheckStreamHealth();
            }

            _logger.LogInformation("StreamManager is stopping. Shutting down all streams.");
            foreach (var stream in _streams)
            {
                _ffmpegService.StopStream(stream.CameraId);
            }
        }

        private void CheckStreamHealth()
        {
            foreach (var stream in _streams)
            {
                var status = _ffmpegService.GetStreamStatus(stream.CameraId);
                if (status == StreamStatus.Offline)
                {
                    if (_restartCooldowns.TryGetValue(stream.CameraId, out var lastRestart) && DateTime.UtcNow - lastRestart < RestartCooldown)
                    {
                        continue;
                    }

                    _logger.LogWarning($"Stream for camera {stream.CameraId} ({stream.Name}) is offline. Restarting...");
                    _ffmpegService.StopStream(stream.CameraId);
                    _ffmpegService.StartStream(stream);
                    _restartCooldowns[stream.CameraId] = DateTime.UtcNow;
                }
            }
        }

        public StreamStatus GetStreamStatus(int cameraId)
        {
            return _ffmpegService.GetStreamStatus(cameraId);
        }
    }

    public class CameraConfig
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RtspPath { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }
}
