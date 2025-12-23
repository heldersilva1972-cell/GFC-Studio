// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class NavMenuService : INavMenuService
    {
        private readonly GfcDbContext _context;

        public NavMenuService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<NavMenuEntry> GetNavMenuEntryAsync(int id)
        {
            return await _context.NavMenuEntries.FindAsync(id);
        }

        public async Task<IEnumerable<NavMenuEntry>> GetNavMenuEntriesAsync()
        {
            return await _context.NavMenuEntries.ToListAsync();
        }

        public async Task CreateNavMenuEntryAsync(NavMenuEntry entry)
        {
            _context.NavMenuEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNavMenuEntryAsync(NavMenuEntry entry)
        {
            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNavMenuEntryAsync(int id)
        {
            var entry = await _context.NavMenuEntries.FindAsync(id);
            if (entry != null)
            {
                _context.NavMenuEntries.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}
