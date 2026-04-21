// [NEW]
namespace GFC.Core.Models
{
    public class DesignTokenModel
    {
        public ColorTokens colors { get; set; } = new ColorTokens();
        public TypographyTokens typography { get; set; } = new TypographyTokens();
        public AccessibilityTokens accessibility { get; set; } = new AccessibilityTokens();
    }

    public class ColorTokens
    {
        public string primary { get; set; } = "#0D1B2A"; // Midnight Blue
        public string secondary { get; set; } = "#E0E1DD"; // Light Gray
        public string accent { get; set; } = "#FFD700"; // Gold
    }

    public class TypographyTokens
    {
        public string headingFont { get; set; } = "Outfit";
        public string bodyFont { get; set; } = "Inter";
    }

    public class AccessibilityTokens
    {
        public bool highContrast { get; set; } = false;
    }
}
