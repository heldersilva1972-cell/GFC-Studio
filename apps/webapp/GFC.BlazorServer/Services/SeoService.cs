// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class SeoService : ISeoService
    {
        private readonly GfcDbContext _context;

        public SeoService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<SeoSettings> GetSeoSettingsForPageAsync(int studioPageId)
        {
            var settings = await _context.SeoSettings.FirstOrDefaultAsync(s => s.StudioPageId == studioPageId);
            return settings ?? new SeoSettings { StudioPageId = studioPageId };
        }

        public async Task<SeoSettings> SaveSeoSettingsAsync(SeoSettings settings)
        {
            if (settings.Id > 0)
            {
                _context.SeoSettings.Update(settings);
            }
            else
            {
                _context.SeoSettings.Add(settings);
            }
            await _context.SaveChangesAsync();
            return settings;
        }
    }
}
