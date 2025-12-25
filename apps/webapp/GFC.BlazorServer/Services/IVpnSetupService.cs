// [NEW]
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IVpnSetupService
    {
        /// <summary>
        /// Checks if the specified user is required to complete the VPN setup.
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <returns>True if setup is required, false otherwise.</returns>
        Task<bool> IsSetupRequiredAsync(int userId);
    }
}
