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
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var settings = await _context.WebsiteSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new WebsiteSettings
                {
                    MemberRate = 250,
                    NonMemberRate = 500,
                    NonProfitRate = 350,
                    FunctionHallNonMemberRate = 400,
                    FunctionHallMemberRate = 300,
                    CoalitionNonMemberRate = 200,
                    CoalitionMemberRate = 100,
                    YouthOrganizationNonMemberRate = 100,
                    YouthOrganizationMemberRate = 100,
                    BartenderServiceFee = 100,
                    KitchenFee = 50,
                    AvEquipmentFee = 25,
                    SecurityDepositAmount = 100,
                    BaseFunctionHours = 5,
                    AdditionalHourRate = 50,
                    MaxHallRentalDurationHours = 8,
                    IsClubOpen = true,
                    MasterEmailKillSwitch = false,
                    HighAccessibilityMode = false,
                    EnableOnlineRentalsPayment = false
                };
                _context.WebsiteSettings.Add(settings);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Apply defaults for any NULL pricing fields
                settings.FunctionHallNonMemberRate ??= 400;
                settings.FunctionHallMemberRate ??= 300;
                settings.CoalitionNonMemberRate ??= 200;
                settings.CoalitionMemberRate ??= 100;
                settings.YouthOrganizationNonMemberRate ??= 100;
                settings.YouthOrganizationMemberRate ??= 100;
                settings.BartenderServiceFee ??= 100;
                settings.BaseFunctionHours ??= 5;
                settings.AdditionalHourRate ??= 50;
                settings.KitchenFee ??= 50;
                settings.AvEquipmentFee ??= 25;
                settings.SecurityDepositAmount ??= 100;
            }
            return settings;
        }

        public async Task UpdateWebsiteSettingsAsync(WebsiteSettings settings)
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
            var existingSettings = await _context.WebsiteSettings.FirstOrDefaultAsync();
            if (existingSettings == null)
            {
                // Ensure Id is set to 1 for new settings
                settings.Id = 1;
                _context.WebsiteSettings.Add(settings);
            }
            else
            {
                // Preserve the Id from existing settings
                settings.Id = existingSettings.Id;
                
                // Update all properties
                existingSettings.FunctionHallNonMemberRate = settings.FunctionHallNonMemberRate;
                existingSettings.FunctionHallMemberRate = settings.FunctionHallMemberRate;
                existingSettings.CoalitionNonMemberRate = settings.CoalitionNonMemberRate;
                existingSettings.CoalitionMemberRate = settings.CoalitionMemberRate;
                existingSettings.YouthOrganizationNonMemberRate = settings.YouthOrganizationNonMemberRate;
                existingSettings.YouthOrganizationMemberRate = settings.YouthOrganizationMemberRate;
                existingSettings.BartenderServiceFee = settings.BartenderServiceFee;
                existingSettings.KitchenFee = settings.KitchenFee;
                existingSettings.AvEquipmentFee = settings.AvEquipmentFee;
                existingSettings.SecurityDepositAmount = settings.SecurityDepositAmount;
                existingSettings.BaseFunctionHours = settings.BaseFunctionHours;
                existingSettings.AdditionalHourRate = settings.AdditionalHourRate;
                existingSettings.MemberRate = settings.MemberRate;
                existingSettings.NonMemberRate = settings.NonMemberRate;
                existingSettings.NonProfitRate = settings.NonProfitRate;
                existingSettings.MaxHallRentalDurationHours = settings.MaxHallRentalDurationHours;
                existingSettings.EnableOnlineRentalsPayment = settings.EnableOnlineRentalsPayment;
                existingSettings.PaymentGatewayUrl = settings.PaymentGatewayUrl;
                existingSettings.PaymentGatewayApiKey = settings.PaymentGatewayApiKey;
                existingSettings.ClubPhone = settings.ClubPhone;
                existingSettings.ClubAddress = settings.ClubAddress;
                existingSettings.MasterEmailKillSwitch = settings.MasterEmailKillSwitch;
                existingSettings.PrimaryColor = settings.PrimaryColor;
                existingSettings.SecondaryColor = settings.SecondaryColor;
                existingSettings.HeadingFont = settings.HeadingFont;
                existingSettings.BodyFont = settings.BodyFont;
                existingSettings.HighAccessibilityMode = settings.HighAccessibilityMode;
                existingSettings.IsClubOpen = settings.IsClubOpen;
                existingSettings.SeoTitle = settings.SeoTitle;
                existingSettings.SeoDescription = settings.SeoDescription;
                existingSettings.SeoKeywords = settings.SeoKeywords;
            }
            await _context.SaveChangesAsync();
        }

        private async Task HealDatabaseAsync()
        {
            await using var _context = await _contextFactory.CreateDbContextAsync();
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
