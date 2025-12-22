// [NEW] - Camera Discovery Models
namespace GFC.BlazorServer.Services.Camera.Models
{
    /// <summary>
    /// Represents a camera discovered on the network via ONVIF or other discovery protocols
    /// </summary>
    public class DiscoveredCamera
    {
        /// <summary>
        /// IP Address of the discovered camera
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Port number (usually 80 for HTTP, 554 for RTSP)
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Manufacturer name (e.g., Hikvision, Dahua, Amcrest)
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Camera model number
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Camera's default/current name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// MAC Address for unique identification
        /// </summary>
        public string MacAddress { get; set; }

        /// <summary>
        /// Firmware version
        /// </summary>
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// Discovered RTSP URL (if available)
        /// </summary>
        public string RtspUrl { get; set; }

        /// <summary>
        /// List of available RTSP stream paths
        /// </summary>
        public List<StreamProfile> AvailableStreams { get; set; } = new();

        /// <summary>
        /// Whether the camera supports ONVIF
        /// </summary>
        public bool SupportsOnvif { get; set; }

        /// <summary>
        /// Discovery method used (ONVIF, UPnP, Manual Scan)
        /// </summary>
        public string DiscoveryMethod { get; set; }

        /// <summary>
        /// Whether this camera is already added to the system
        /// </summary>
        public bool IsAlreadyAdded { get; set; }

        /// <summary>
        /// Timestamp when camera was discovered
        /// </summary>
        public DateTime DiscoveredAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Represents a video stream profile available from a camera
    /// </summary>
    public class StreamProfile
    {
        /// <summary>
        /// Profile name (e.g., "Main Stream", "Sub Stream")
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// RTSP path for this stream
        /// </summary>
        public string RtspPath { get; set; }

        /// <summary>
        /// Resolution (e.g., "1920x1080", "640x480")
        /// </summary>
        public string Resolution { get; set; }

        /// <summary>
        /// Video codec (e.g., "H.264", "H.265")
        /// </summary>
        public string Codec { get; set; }

        /// <summary>
        /// Frame rate (e.g., 30, 15)
        /// </summary>
        public int? FrameRate { get; set; }

        /// <summary>
        /// Bitrate in kbps
        /// </summary>
        public int? Bitrate { get; set; }
    }

    /// <summary>
    /// Request to add a discovered camera to the system
    /// </summary>
    public class AddDiscoveredCameraRequest
    {
        /// <summary>
        /// IP Address of the camera
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Custom name for the camera (optional, uses discovered name if not provided)
        /// </summary>
        public string CustomName { get; set; }

        /// <summary>
        /// Selected stream profile to use
        /// </summary>
        public string SelectedStreamPath { get; set; }

        /// <summary>
        /// Username for camera authentication
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password for camera authentication
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Whether to enable the camera immediately
        /// </summary>
        public bool EnableImmediately { get; set; } = true;
    }
}
