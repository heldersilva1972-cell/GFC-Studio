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
            try 
            {
                return await _context.WebsiteSettings.FirstOrDefaultAsync() ?? new WebsiteSettings();
            }
            catch (Exception)
            {
                // [Self-Healing] If we encounter 'Data is Null', fix the data and retry
                await HealDatabaseAsync();
                return await _context.WebsiteSettings.FirstOrDefaultAsync() ?? new WebsiteSettings();
            }
        }

        public async Task UpdateWebsiteSettingsAsync(WebsiteSettings settings)
        {
            try
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
            catch (Exception)
            {
                // [Self-Healing] Fix data and force update via raw SQL if EF fails
                await HealDatabaseAsync();
                
                // Retry the standard way first
                var retrySettings = await _context.WebsiteSettings.FirstOrDefaultAsync();
                if (retrySettings != null)
                {
                    _context.Entry(retrySettings).CurrentValues.SetValues(settings);
                    await _context.SaveChangesAsync();
                }
                else 
                {
                   // Fallback: This usually implies the table is empty or very broken
                   _context.WebsiteSettings.Add(settings);
                   await _context.SaveChangesAsync();
                }
            }
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
