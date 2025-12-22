// [NEW]
using GFC.VideoAgent.Models;
using GFC.VideoAgent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.VideoAgent.Controllers
{
    [ApiController]
    [Route("api")]
    public class StreamController : ControllerBase
    {
        private readonly ILogger<StreamController> _logger;
        private readonly FFmpegService _ffmpegService;
        private readonly StreamManager _streamManager;
        private readonly NvrService _nvrService;

        public StreamController(ILogger<StreamController> logger, FFmpegService ffmpegService, StreamManager streamManager, NvrService nvrService)
        {
            _logger = logger;
            _ffmpegService = ffmpegService;
            _streamManager = streamManager;
            _nvrService = nvrService;
        }

        [HttpGet("cameras/{cameraId}/capabilities")]
        public ActionResult<CameraCapabilities> GetCameraCapabilities(int cameraId)
        {
            // Placeholder implementation
            var capabilities = new CameraCapabilities
            {
                SupportsPTZ = true,
                SupportsZoom = true,
                SupportsPresets = false,
                AvailableQualities = new List<StreamQuality> { StreamQuality.HD, StreamQuality.SD, StreamQuality.Low }
            };
            return Ok(capabilities);
        }

        [HttpGet("streams/{cameraId}/quality")]
        public ActionResult<StreamQuality> GetStreamQuality(int cameraId)
        {
            var quality = _streamManager.GetStreamQuality(cameraId);
            return Ok(quality);
        }

        [HttpPost("streams/{cameraId}/quality")]
        public IActionResult SetStreamQuality(int cameraId, [FromBody] StreamQuality quality)
        {
            _logger.LogInformation($"Setting stream quality for camera {cameraId} to {quality}");
            _streamManager.SetStreamQuality(cameraId, quality);
            return Ok(new { message = $"Stream quality for camera {cameraId} set to {quality}" });
        }

        [HttpPost("cameras/{cameraId}/ptz")]
        public async Task<IActionResult> SendPTZCommand(int cameraId, [FromBody] PTZCommand command)
        {
            _logger.LogInformation($"Received PTZ command for camera {cameraId}: {command.Action}");
            var success = await _nvrService.SendPtzCommandAsync(cameraId, command);
            if (success)
            {
                return Ok(new { message = $"PTZ command {command.Action} sent to camera {cameraId}" });
            }
            else
            {
                return StatusCode(500, "Failed to send PTZ command.");
            }
        }

        [HttpGet("cameras/{cameraId}/snapshot")]
        public async Task<IActionResult> CaptureSnapshot(int cameraId)
        {
            _logger.LogInformation($"Capturing snapshot for camera {cameraId}");
            var imageBytes = await _ffmpegService.CaptureSnapshotAsync(cameraId);

            if (imageBytes == null)
            {
                return StatusCode(500, "Failed to capture snapshot.");
            }

            return File(imageBytes, "image/jpeg");
        }
    }
}
