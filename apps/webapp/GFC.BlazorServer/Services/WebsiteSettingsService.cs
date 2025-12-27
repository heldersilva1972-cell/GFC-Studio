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
        private readonly IDbContextFactory<GfcDbContext> _contextFactory;

        public WebsiteSettingsService(IDbContextFactory<GfcDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<WebsiteSettings> GetWebsiteSettingsAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var settings = await context.WebsiteSettings.FirstOrDefaultAsync();
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
                context.WebsiteSettings.Add(settings);
                await context.SaveChangesAsync();
            }
            return settings;
        }

        public async Task UpdateWebsiteSettingsAsync(WebsiteSettings settings)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var existingSettings = await context.WebsiteSettings.FirstOrDefaultAsync();
            if (existingSettings == null)
            {
                context.WebsiteSettings.Add(settings);
            }
            else
            {
                context.Entry(existingSettings).CurrentValues.SetValues(settings);
            }
            await context.SaveChangesAsync();
        }

        private async Task HealDatabaseAsync()
        {
             using var context = await _contextFactory.CreateDbContextAsync();
             var fixNullsSql = @"
                UPDATE [dbo].[WebsiteSettings] SET [MemberRate] = 0 WHERE [MemberRate] IS NULL;
                UPDATE [dbo].[WebsiteSettings] SET [NonMemberRate] = 0 WHERE [NonMemberRate] IS NULL;
                UPDATE [dbo].[WebsiteSettings] SET [IsClubOpen] = 1 WHERE [IsClubOpen] IS NULL;
                UPDATE [dbo].[WebsiteSettings] SET [MasterEmailKillSwitch] = 0 WHERE [MasterEmailKillSwitch] IS NULL;
                UPDATE [dbo].[WebsiteSettings] SET [HighAccessibilityMode] = 0 WHERE [HighAccessibilityMode] IS NULL;
            ";
            await context.Database.ExecuteSqlRawAsync(fixNullsSql);
        }
    }
}
