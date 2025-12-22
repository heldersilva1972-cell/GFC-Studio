// [NEW] - Camera Discovery Service Interface
using GFC.BlazorServer.Services.Camera.Models;

namespace GFC.BlazorServer.Services.Camera
{
    /// <summary>
    /// Service for discovering IP cameras on the network
    /// </summary>
    public interface ICameraDiscoveryService
    {
        /// <summary>
        /// Scans the network for available IP cameras using ONVIF, UPnP, and port scanning
        /// </summary>
        /// <param name="networkRange">Optional network range to scan (e.g., "192.168.1.0/24"). If null, scans local subnet.</param>
        /// <param name="timeoutSeconds">Timeout for discovery operation in seconds</param>
        /// <returns>List of discovered cameras</returns>
        Task<List<DiscoveredCamera>> DiscoverCamerasAsync(string networkRange = null, int timeoutSeconds = 30, Action<string> onStatusUpdate = null);

        /// <summary>
        /// Tests connection to a specific camera and retrieves its details
        /// </summary>
        /// <param name="ipAddress">IP address of the camera</param>
        /// <param name="username">Username for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <returns>Camera details if successful, null otherwise</returns>
        Task<DiscoveredCamera> TestCameraConnectionAsync(string ipAddress, string username, string password);

        /// <summary>
        /// Adds a discovered camera to the system
        /// </summary>
        /// <param name="request">Request containing camera details and credentials</param>
        /// <returns>The created camera entity</returns>
        Task<GFC.Core.Models.Camera> AddDiscoveredCameraAsync(AddDiscoveredCameraRequest request);

        /// <summary>
        /// Gets the default credentials for common camera manufacturers
        /// </summary>
        /// <param name="manufacturer">Manufacturer name</param>
        /// <returns>Dictionary of common username/password combinations</returns>
        Dictionary<string, string> GetDefaultCredentials(string manufacturer);
    }
}
