// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IWireGuardManagementService
    {
        Task<VpnProfile> GenerateProfileAsync(int userId, string deviceName, string deviceType);
        Task<string> GenerateConfigFileAsync(VpnProfile profile);
        Task<bool> ActivateProfileAsync(int profileId);
        Task<bool> RevokeProfileAsync(int profileId, int revokedByUserId, string reason);
        Task<List<VpnProfile>> GetUserProfilesAsync(int userId);
    }
}
