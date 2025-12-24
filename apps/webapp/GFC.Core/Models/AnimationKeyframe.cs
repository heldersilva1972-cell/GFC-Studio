// [NEW]
namespace GFC.Core.Models
{
    public class AnimationKeyframe
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Target { get; set; } // e.g., "headline", "image"
        public string Effect { get; set; } = "fadeIn";
        public double Duration { get; set; } = 1.0;
        public double Delay { get; set; } = 0.0;
        public string Easing { get; set; } = "easeOut";
    }
}
