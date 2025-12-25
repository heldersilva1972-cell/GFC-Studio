// [NEW]
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that handles the security of video stream URLs.
    /// </summary>
    public interface IStreamSecurityService
    {
        /// <summary>
        /// Generates a secure, time-limited token for a given camera stream.
        /// </summary>
        /// <param name="cameraId">The ID of the camera.</param>
        /// <param name="ipAddress">The IP address of the user requesting the stream. Can be null if IP locking is disabled.</param>
        /// <returns>A secure token string.</returns>
        string GenerateStreamToken(int cameraId, string ipAddress);

        /// <summary>
        /// Validates a secure token for a given camera stream.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        /// <param name="cameraId">The ID of the camera the token is for.</param>
        /// <param name="ipAddress">The IP address of the user making the request. Can be null if IP locking is disabled.</param>
        /// <returns>True if the token is valid, otherwise false.</returns>
        bool ValidateStreamToken(string token, int cameraId, string ipAddress);
    }
}
