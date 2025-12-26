// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class NotificationRoutingService : INotificationRoutingService
    {
        private readonly GfcDbContext _context;

        public NotificationRoutingService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationRouting>> GetAllRoutingsAsync()
        {
            return await _context.NotificationRoutings.ToListAsync();
        }

        public async Task<NotificationRouting> GetRoutingByIdAsync(int id)
        {
            return await _context.NotificationRoutings.FindAsync(id);
        }

        public async Task CreateRoutingAsync(NotificationRouting routing)
        {
            _context.NotificationRoutings.Add(routing);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoutingAsync(NotificationRouting routing)
        {
            _context.Entry(routing).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoutingAsync(int id)
        {
            var routing = await _context.NotificationRoutings.FindAsync(id);
            if (routing != null)
            {
                _context.NotificationRoutings.Remove(routing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GetEmailForActionAsync(string actionName)
        {
            var routing = await _context.NotificationRoutings
                .FirstOrDefaultAsync(r => r.ActionName == actionName);
            return routing?.DirectorEmail;
        }
    }
}
