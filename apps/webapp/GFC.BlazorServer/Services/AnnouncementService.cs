// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GFC.BlazorServer.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly GfcDbContext _context;

        public AnnouncementService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<List<SystemNotification>> GetActiveAnnouncementsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.SystemNotifications
                .Where(n => n.IsActive && n.StartDate <= now && (n.EndDate == null || n.EndDate >= now))
                .ToListAsync();
        }
    }
}
