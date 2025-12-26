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
            var settings = await _context.WebsiteSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new WebsiteSettings
                {
                    MemberRate = 250,
                    NonMemberRate = 500,
                    NonProfitRate = 350,
                    KitchenFee = 50,
                    AvEquipmentFee = 25,
                    SecurityDepositAmount = 100,
                    MaxHallRentalDurationHours = 8,
                    IsClubOpen = true,
                    MasterEmailKillSwitch = false,
                    HighAccessibilityMode = false,
                    EnableOnlineRentalsPayment = false
                };
                _context.WebsiteSettings.Add(settings);
                await _context.SaveChangesAsync();
            }
            return settings;
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
