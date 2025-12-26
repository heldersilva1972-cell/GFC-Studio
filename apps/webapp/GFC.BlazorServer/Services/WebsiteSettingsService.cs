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

        private async Task HealDatabaseAsync()
        {
             var fixNullsSql = @"
                UPDATE [dbo].[WebsiteSettings] SET [MemberRate] = 0 WHERE [MemberRate] IS NULL;
                UPDATE [dbo].[WebsiteSettings] SET [NonMemberRate] = 0 WHERE [NonMemberRate] IS NULL;
                UPDATE [dbo].[WebsiteSettings] SET [IsClubOpen] = 1 WHERE [IsClubOpen] IS NULL;
                UPDATE [dbo].[WebsiteSettings] SET [MasterEmailKillSwitch] = 0 WHERE [MasterEmailKillSwitch] IS NULL;
                UPDATE [dbo].[WebsiteSettings] SET [HighAccessibilityMode] = 0 WHERE [HighAccessibilityMode] IS NULL;
            ";
            await _context.Database.ExecuteSqlRawAsync(fixNullsSql);
        }
    }
}
