// [NEW]
using System.Collections.Generic;

namespace GFC.VideoAgent.Models
{
    public class CameraCapabilities
    {
        public bool SupportsPTZ { get; set; }
        public bool SupportsZoom { get; set; }
        public bool SupportsPresets { get; set; }
        public List<int> AvailablePresets { get; set; } = new();
        public List<StreamQuality> AvailableQualities { get; set; } = new();
    }
}
