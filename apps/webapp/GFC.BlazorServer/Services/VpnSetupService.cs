// [MODIFIED]
using GFC.Core.Interfaces;
using GFC.BlazorServer.Services.Camera;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class VpnSetupService : IVpnSetupService
    {
        private readonly IVpnProfileRepository _vpnProfileRepository;
        private readonly ICameraPermissionService _cameraPermissionService; // Assuming this service exists

        public VpnSetupService(IVpnProfileRepository vpnProfileRepository, ICameraPermissionService cameraPermissionService)
        {
            _vpnProfileRepository = vpnProfileRepository;
            _cameraPermissionService = cameraPermissionService;
        }

        /// <summary>
        /// Determines if a user needs to complete the VPN setup process.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>True if the user has camera permissions but no active VPN profile.</returns>
        public async Task<bool> IsSetupRequiredAsync(int userId)
        {
            // 1. Check if the user has permission to view cameras in the first place.
            var hasCameraPermission = await _cameraPermissionService.UserHasAnyCameraPermissionAsync(userId);
            if (!hasCameraPermission)
            {
                return false; // No permissions, no setup required.
            }

            // 2. Check if the user already has an active (non-revoked) VPN profile.
            var existingProfile = await _vpnProfileRepository.GetByUserIdAsync(userId);

            // 3. Setup is required if they have permission BUT no active profile.
            return existingProfile == null;
        }
    }
}
