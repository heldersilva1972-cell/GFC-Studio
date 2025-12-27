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
        private readonly IDbContextFactory<GfcDbContext> _contextFactory;

        public NavMenuService(IDbContextFactory<GfcDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<NavMenuEntry> GetNavMenuEntryAsync(int id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            return await _context.NavMenuEntries.FindAsync(id);
        }

        public async Task<IEnumerable<NavMenuEntry>> GetNavMenuEntriesAsync()
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            return await _context.NavMenuEntries.ToListAsync();
        }

        public async Task CreateNavMenuEntryAsync(NavMenuEntry entry)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            _context.NavMenuEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNavMenuEntryAsync(NavMenuEntry entry)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            _context.Entry(entry).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNavMenuEntryAsync(int id)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var entry = await _context.NavMenuEntries.FindAsync(id);
            if (entry != null)
            {
                _context.NavMenuEntries.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}
