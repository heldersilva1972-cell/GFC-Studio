// [NEW]
namespace GFC.VideoAgent.Models
{
    public class CameraStream
    {
        public int CameraId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RtspUrl { get; set; } = string.Empty;
    }
}
