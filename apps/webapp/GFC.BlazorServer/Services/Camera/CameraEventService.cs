// [NEW]
using GFC.Core.Models;
using GFC.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public class CameraEventService : ICameraEventService
    {
        private readonly ApplicationDbContext _context;

        public CameraEventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogEventAsync(int cameraId, CameraEventType eventType, string description)
        {
            var cameraEvent = new CameraEvent
            {
                CameraId = cameraId,
                EventType = eventType,
                Description = description,
                Timestamp = DateTime.UtcNow
            };
            _context.CameraEvents.Add(cameraEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CameraEvent>> GetEventsForCameraAsync(int cameraId, DateTime start, DateTime end)
        {
            return await _context.CameraEvents
                .Where(e => e.CameraId == cameraId && e.Timestamp >= start && e.Timestamp <= end)
                .ToListAsync();
        }
    }
}
