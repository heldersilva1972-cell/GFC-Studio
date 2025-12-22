// [NEW]
namespace GFC.VideoAgent.Models
{
    public class PTZCommand
    {
        public PTZAction Action { get; set; }
        public int Speed { get; set; } = 50; // 0-100
        public int? PresetId { get; set; }
    }

    public enum PTZAction
    {
        Up,
        Down,
        Left,
        Right,
        ZoomIn,
        ZoomOut,
        GotoPreset,
        Stop
    }
}
