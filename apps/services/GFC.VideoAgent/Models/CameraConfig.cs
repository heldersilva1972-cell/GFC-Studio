// [NEW]
namespace GFC.VideoAgent.Models
{
    public class CameraConfig
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RtspPath { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }
}
