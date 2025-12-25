// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Repositories
{
    public class StudioPageRepository : IStudioPageRepository
    {
        private readonly GfcDbContext _context;

        public StudioPageRepository(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<StudioPage> GetByIdAsync(int id)
        {
            return await _context.StudioPages
                .Include(p => p.Sections)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<StudioPage> GetBySlugAsync(string slug)
        {
            return await _context.StudioPages
                .Include(p => p.Sections)
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<IEnumerable<StudioPage>> GetAllAsync()
        {
            return await _context.StudioPages.ToListAsync();
        }

        public async Task AddAsync(StudioPage page)
        {
            _context.StudioPages.Add(page);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudioPage page)
        {
            _context.Entry(page).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var page = await _context.StudioPages.FindAsync(id);
            if (page != null)
            {
                _context.StudioPages.Remove(page);
                await _context.SaveChangesAsync();
            }
        }
    }
}
