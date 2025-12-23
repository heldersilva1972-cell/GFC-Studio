// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class WebsiteSettingsService : IWebsiteSettingsService
    {
        private readonly GfcDbContext _context;

        public WebsiteSettingsService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<WebsiteSettings> GetWebsiteSettingsAsync()
        {
            var settings = await _context.WebsiteSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new WebsiteSettings();
                _context.WebsiteSettings.Add(settings);
                await _context.SaveChangesAsync();
            }
            return settings;
        }

        public async Task UpdateWebsiteSettingsAsync(WebsiteSettings settings)
        {
            _context.Entry(settings).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
