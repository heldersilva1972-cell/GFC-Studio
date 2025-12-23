// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class StudioService : IStudioService
    {
        private readonly GfcDbContext _context;

        public StudioService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<StudioPage> GetPublishedPageAsync(int id)
        {
            return await _context.StudioPages
                .Include(p => p.Sections)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsPublished);
        }

        public async Task<IEnumerable<StudioPage>> GetPublishedPagesAsync()
        {
            return await _context.StudioPages
                .Where(p => p.IsPublished)
                .Include(p => p.Sections)
                .ToListAsync();
        }

        public async Task SaveDraftAsync(StudioDraft draft)
        {
            if (draft.Id > 0)
            {
                _context.StudioDrafts.Update(draft);
            }
            else
            {
                _context.StudioDrafts.Add(draft);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<StudioDraft> GetDraftAsync(int pageId)
        {
            return await _context.StudioDrafts.FirstOrDefaultAsync(d => d.PageId == pageId);
        }
    }
}
