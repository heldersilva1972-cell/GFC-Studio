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

        public bool MasterEmailKillSwitch { get; set; }

        public decimal MemberRate { get; set; }

        public decimal NonMemberRate { get; set; }

        // Design Tokens
        public string PrimaryColor { get; set; } = "#0D1B2A"; // Midnight Blue
        public string SecondaryColor { get; set; } = "#FFD700"; // Gold
        public string HeadingFont { get; set; } = "Outfit";
        public string BodyFont { get; set; } = "Inter";
        public bool HighAccessibilityMode { get; set; } = false;
    }
}
