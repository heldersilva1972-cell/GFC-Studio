// [NEW]
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public class CameraService : ICameraService
    {
        private readonly GfcDbContext _context;

        public CameraService(GfcDbContext context)
        {
            _context = context;
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
            }
        }

        public async Task DeleteCameraAsync(int id)
        {
            var camera = await _context.Cameras.FindAsync(id);
            if (camera != null)
            {
                _context.Cameras.Remove(camera);
                await _context.SaveChangesAsync();
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
    }
}
