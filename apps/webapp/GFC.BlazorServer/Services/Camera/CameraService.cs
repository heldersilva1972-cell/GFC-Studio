// [MODIFIED]
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GFC.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace GFC.BlazorServer.Services.Camera
{
    public class CameraService : ICameraService
    {
        private readonly GfcDbContext _context;
        private readonly GFC.Core.Interfaces.IStreamSecurityService _streamSecurityService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CameraService(GfcDbContext context, GFC.Core.Interfaces.IStreamSecurityService streamSecurityService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _streamSecurityService = streamSecurityService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<GFC.Core.Models.Camera>> GetAllCamerasAsync()
        {
            return await _context.Cameras.ToListAsync();
        }

        public async Task<GFC.Core.Models.Camera> GetCameraByIdAsync(int id)
        {
            return await _context.Cameras.FindAsync(id);
        }

        public async Task<GFC.Core.Models.Camera> CreateCameraAsync(GFC.Core.Models.Camera camera)
        {
            camera.CreatedAt = DateTime.UtcNow;
            camera.UpdatedAt = DateTime.UtcNow;
            _context.Cameras.Add(camera);
            await _context.SaveChangesAsync();
            await SyncToVideoAgentAsync();
            return camera;
        }

        public async Task UpdateCameraAsync(GFC.Core.Models.Camera camera)
        {
            var existingCamera = await _context.Cameras.FindAsync(camera.Id);
            if (existingCamera != null)
            {
                existingCamera.Name = camera.Name;
                existingCamera.RtspUrl = camera.RtspUrl;
                existingCamera.IsEnabled = camera.IsEnabled;
                existingCamera.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await SyncToVideoAgentAsync();
            }
        }

        public async Task DeleteCameraAsync(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera != null)
            {
                _context.Cameras.Remove(camera);
                await _context.SaveChangesAsync();
                await SyncToVideoAgentAsync();
            }
        }

        public async Task<bool> SendPTZCommandAsync(int cameraId, PTZCommand command)
        {
            // TODO: Implement PTZ command sending to Video Agent
            await Task.CompletedTask;
            return true;
        }

        public async Task SetStreamQualityAsync(int cameraId, StreamQuality quality)
        {
            // TODO: Implement stream quality setting via Video Agent
            await Task.CompletedTask;
        }

        public async Task<CameraCapabilities> GetCameraCapabilitiesAsync(int cameraId)
        {
            // TODO: Implement getting capabilities from Video Agent
            await Task.CompletedTask;
            return new CameraCapabilities
            {
                SupportsPTZ = false,
                SupportsZoom = false,
                SupportsPresets = false,
                AvailablePresets = new List<int>(),
                AvailableQualities = new List<StreamQuality> { StreamQuality.HD, StreamQuality.SD, StreamQuality.Low }
            };
        }

        public async Task<byte[]> CaptureSnapshotAsync(int cameraId)
        {
            // TODO: Implement snapshot capture via Video Agent
            await Task.CompletedTask;
            return new byte[0];
        }

        public async Task SyncToVideoAgentAsync()
        {
            try 
            {
                var cameras = await GetAllCamerasAsync();
                var videoAgentPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "..", "..", "services", "GFC.VideoAgent", "appsettings.json");
                
                if (!System.IO.File.Exists(videoAgentPath)) return;
                
                var configContent = await System.IO.File.ReadAllTextAsync(videoAgentPath);
                var configJson = System.Text.Json.Nodes.JsonNode.Parse(configContent);
                
                if (configJson["NvrSettings"]?["Cameras"] is System.Text.Json.Nodes.JsonArray cameraArray)
                {
                    cameraArray.Clear();
                    foreach (var cam in cameras)
                    {
                        var rtspPath = "";
                        if (!string.IsNullOrEmpty(cam.RtspUrl))
                        {
                            // Try to extract path component (after port/host)
                            // If it contains /Streaming/Channels/, use that (Hikvision)
                            if (cam.RtspUrl.Contains("/Streaming/Channels/"))
                            {
                                rtspPath = cam.RtspUrl.Substring(cam.RtspUrl.IndexOf("/Streaming/Channels/"));
                            }
                            else if (Uri.TryCreate(cam.RtspUrl, UriKind.Absolute, out var uri))
                            {
                                rtspPath = uri.PathAndQuery;
                            }
                        }

                        cameraArray.Add(new System.Text.Json.Nodes.JsonObject
                        {
                            ["Id"] = cam.Id,
                            ["Name"] = cam.Name,
                            ["RtspPath"] = rtspPath,
                            ["Enabled"] = cam.IsEnabled ?? false
                        });
                    }
                    
                    await System.IO.File.WriteAllTextAsync(videoAgentPath, configJson.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to sync cameras to Video Agent: {ex.Message}");
            }
        }

        public async Task<string> GetSecureStreamUrlAsync(int cameraId)
        {
            var camera = await GetCameraByIdAsync(cameraId);
            if (camera == null)
            {
                throw new KeyNotFoundException($"Camera with ID {cameraId} not found.");
            }

            var userIpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            var token = _streamSecurityService.GenerateStreamToken(cameraId, userIpAddress);

            var videoAgentBaseUrl = _configuration["VideoAgent:BaseUrl"] ?? "http://localhost:8888";

            // The request path should match what the Video Agent expects for HLS streams.
            // Let's assume it's `/stream/{cameraId}/index.m3u8` as seen in ViewCameras.razor
            var streamPath = $"stream/{camera.Id}/index.m3u8";

            return $"{videoAgentBaseUrl}/{streamPath}?token={Uri.EscapeDataString(token)}";
        }
    }
}
