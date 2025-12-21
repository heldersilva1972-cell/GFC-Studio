// [NEW]
using GFC.VideoAgent.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;

namespace GFC.VideoAgent.Services
{
    public class FFmpegService
    {
        private readonly ILogger<FFmpegService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<int, Process> _activeProcesses = new();
        private readonly string _outputDirectory;

        public FFmpegService(ILogger<FFmpegService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _outputDirectory = _configuration.GetValue<string>("VideoAgent:OutputDirectory") ?? "hls-streams";

            if (!Directory.Exists(_outputDirectory))
            {
                Directory.CreateDirectory(_outputDirectory);
            }
        }

        public void StartStream(CameraStream stream)
        {
            var ffmpegPath = _configuration.GetValue<string>("VideoAgent:FFmpegPath") ?? "ffmpeg";
            var arguments = $"-rtsp_transport tcp -i \"{stream.RtspUrl}\" " +
                            "-c:v copy -c:a aac -f hls " +
                            "-hls_time 2 -hls_list_size 5 -hls_flags delete_segments " +
                            $"-hls_segment_filename \"{Path.Combine(_outputDirectory, $"camera_{stream.CameraId}_%03d.ts")}\" " +
                            $"\"{Path.Combine(_outputDirectory, $"camera{stream.CameraId}.m3u8")}\"";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
                EnableRaisingEvents = true
            };

            process.Exited += (sender, e) =>
            {
                _logger.LogWarning($"FFmpeg process for camera {stream.CameraId} exited.");
                _activeProcesses.TryRemove(stream.CameraId, out _);
            };

            process.OutputDataReceived += (sender, e) => _logger.LogInformation(e.Data);
            process.ErrorDataReceived += (sender, e) => _logger.LogError(e.Data);

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                _activeProcesses[stream.CameraId] = process;
                _logger.LogInformation($"Started FFmpeg process for camera {stream.CameraId} ({stream.Name}).");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to start FFmpeg process for camera {stream.CameraId}.");
            }
        }

        public void StopStream(int cameraId)
        {
            if (_activeProcesses.TryRemove(cameraId, out var process))
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                        _logger.LogInformation($"Stopped FFmpeg process for camera {cameraId}.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error stopping FFmpeg process for camera {cameraId}.");
                }
            }
        }

        public StreamStatus GetStreamStatus(int cameraId)
        {
            if (_activeProcesses.TryGetValue(cameraId, out var process) && !process.HasExited)
            {
                // A more robust health check would involve checking the output files
                var playlistPath = Path.Combine(_outputDirectory, $"camera{cameraId}.m3u8");
                if (File.Exists(playlistPath) && (DateTime.UtcNow - File.GetLastWriteTimeUtc(playlistPath)) < TimeSpan.FromSeconds(10))
                {
                    return StreamStatus.Live;
                }
                return StreamStatus.Buffering;
            }
            return StreamStatus.Offline;
        }
    }
}
