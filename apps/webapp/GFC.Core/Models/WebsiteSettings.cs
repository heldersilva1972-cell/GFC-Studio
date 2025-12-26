// [NEW]
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class WebsiteSettings
    {
        [Key]
        public int Id { get; set; }

        public string ClubPhone { get; set; }

        public string ClubAddress { get; set; }

        public bool? MasterEmailKillSwitch { get; set; }

        public decimal? MemberRate { get; set; }
        public decimal? NonMemberRate { get; set; }
        public decimal? NonProfitRate { get; set; }
        public decimal? KitchenFee { get; set; }
        public decimal? AvEquipmentFee { get; set; }
        public decimal? SecurityDepositAmount { get; set; }

        public int? BaseFunctionHours { get; set; } = 5;
        public decimal? AdditionalHourRate { get; set; } = 50;

        public int? MaxHallRentalDurationHours { get; set; }

        public bool? EnableOnlineRentalsPayment { get; set; }
        public string? PaymentGatewayUrl { get; set; }
        public string? PaymentGatewayApiKey { get; set; }
        
        // System Settings
        public string PrimaryColor { get; set; } = "#0D1B2A"; // Midnight Blue
        public string SecondaryColor { get; set; } = "#FFD700"; // Gold
        public string HeadingFont { get; set; } = "Outfit";
        public string BodyFont { get; set; } = "Inter";
        public bool? HighAccessibilityMode { get; set; } = false;

        public bool? IsClubOpen { get; set; } = true;

        // SEO Settings
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
    }
}
