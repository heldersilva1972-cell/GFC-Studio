// [NEW]
namespace GFC.Core.Models
{
    public class AnimationSettings
    {
        public string Effect { get; set; } = "None"; // e.g., "FadeIn", "SlideUp"
        public double Duration { get; set; } = 1.0; // in seconds
        public double Delay { get; set; } = 0.0; // in seconds
        public string Trigger { get; set; } = "OnScroll"; // e.g., "OnLoad", "OnScroll"
        public string Easing { get; set; } = "ease-in-out"; // e.g., "linear", "ease-in-out", "cubic-bezier(0.17, 0.55, 0.55, 1)"
    }
}
