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
        private readonly ConcurrentDictionary<int, StreamQuality> _streamQualities = new();
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
            var quality = _streamQualities.GetOrAdd(stream.CameraId, StreamQuality.HD);
            var ffmpegPath = _configuration.GetValue<string>("VideoAgent:FFmpegPath") ?? "ffmpeg";
            var qualityArgs = GetQualityArguments(quality);

            var arguments = $"-rtsp_transport tcp -i \"{stream.RtspUrl}\" " +
                            $"{qualityArgs} " +
                            "-c:a aac -f hls " +
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

        public async Task<byte[]?> CaptureSnapshotAsync(int cameraId)
        {
            var cameraConfig = _configuration.GetSection($"NvrSettings:Cameras").Get<List<CameraConfig>>()?.FirstOrDefault(c => c.Id == cameraId);
            if (cameraConfig == null)
            {
                _logger.LogError($"Camera configuration for camera {cameraId} not found.");
                return null;
            }

            var rtspUrl = $"rtsp://{_configuration["NvrSettings:Username"]}:{_configuration["NvrSettings:Password"]}@{_configuration["NvrSettings:IpAddress"]}:{_configuration["NvrSettings:RtspPort"]}{cameraConfig.RtspPath}";
            var ffmpegPath = _configuration.GetValue<string>("VideoAgent:FFmpegPath") ?? "ffmpeg";
            var tempFileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.jpg");

            var arguments = $"-rtsp_transport tcp -i \"{rtspUrl}\" -vframes 1 -q:v 2 \"{tempFileName}\"";

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
                }
            };

            process.Start();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                _logger.LogError($"FFmpeg snapshot process for camera {cameraId} exited with code {process.ExitCode}.");
                return null;
            }

            if (!File.Exists(tempFileName))
            {
                _logger.LogError($"Snapshot file for camera {cameraId} was not created.");
                return null;
            }

            var imageBytes = await File.ReadAllBytesAsync(tempFileName);
            File.Delete(tempFileName);

            return imageBytes;
        }

        public void SetStreamQuality(CameraStream stream, StreamQuality quality)
        {
            _streamQualities[stream.CameraId] = quality;
            StopStream(stream.CameraId);
            StartStream(stream);
            _logger.LogInformation($"Set stream quality for camera {stream.CameraId} to {quality}.");
        }

        public StreamQuality GetStreamQuality(int cameraId)
        {
            return _streamQualities.GetOrAdd(cameraId, StreamQuality.HD);
        }

        private string GetQualityArguments(StreamQuality quality)
        {
            switch (quality)
            {
                case StreamQuality.Low:
                    return "-vf scale=640:-1 -b:v 500k -c:v h264";
                case StreamQuality.SD:
                    return "-vf scale=1280:-1 -b:v 1500k -c:v h264";
                case StreamQuality.HD:
                default:
                    return "-c:v copy"; // No transcoding for HD
            }
        }
    }
}
