// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class WebsiteSettingsService : GFC.Core.Interfaces.IWebsiteSettingsService
    {
        private readonly GfcDbContext _context;

        public WebsiteSettingsService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<WebsiteSettings> GetWebsiteSettingsAsync()
        {
            return await _context.WebsiteSettings.FirstOrDefaultAsync() ?? new WebsiteSettings();
        }

        public async Task UpdateWebsiteSettingsAsync(WebsiteSettings settings)
        {
            var existingSettings = await _context.WebsiteSettings.FirstOrDefaultAsync();
            if (existingSettings == null)
            {
                _context.WebsiteSettings.Add(settings);
            }
            else
            {
                _context.Entry(existingSettings).CurrentValues.SetValues(settings);
            }
            await _context.SaveChangesAsync();
        }
    }
}
