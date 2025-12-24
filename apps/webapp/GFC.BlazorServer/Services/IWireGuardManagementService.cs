// [NEW]
using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IWireGuardManagementService
    {
        /// <summary>
        /// Generates a new VPN profile for a user, including a new keypair and IP address.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="deviceName">A descriptive name for the device.</param>
        /// <param name="deviceType">The type of device (e.g., iOS, Android).</param>
        /// <returns>The newly created VpnProfile.</returns>
        Task<VpnProfile> GenerateProfileAsync(int userId, string deviceName, string deviceType);

        /// <summary>
        /// Generates the content of the .conf file for a given VPN profile.
        /// </summary>
        /// <param name="profile">The VpnProfile to generate the config for.</param>
        /// <returns>A string containing the WireGuard configuration.</returns>
        Task<string> GenerateConfigFileAsync(VpnProfile profile);

        /// <summary>
        /// Marks a VPN profile as used by setting the LastUsedAt timestamp.
        /// </summary>
        /// <param name="profileId">The ID of the VpnProfile to update.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<bool> ActivateProfileAsync(int profileId);
    }
}
