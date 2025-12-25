// [NEW]
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public enum LocationType
    {
        LAN,
        VPN,
        Public,
        Unknown
    }

    public interface INetworkLocationService
    {
        /// <summary>
        /// Detects the network location of a user based on their IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address to check.</param>
        /// <returns>The detected LocationType.</returns>
        Task<LocationType> DetectLocationAsync(string ipAddress);

        /// <summary>
        /// Checks if a user is authorized to view video streams from their current location.
        /// </summary>
        /// <param name="ipAddress">The user's IP address.</param>
        /// <returns>True if access is allowed (LAN or VPN), false otherwise.</returns>
        Task<bool> IsAuthorizedForVideoAsync(string ipAddress);
    }
}
