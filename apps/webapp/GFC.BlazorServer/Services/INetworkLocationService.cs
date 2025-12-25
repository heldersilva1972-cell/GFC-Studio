// [OBSOLETE - DO NOT USE]
// This file has been replaced by GFC.Core.Interfaces.INetworkLocationService
// This namespace has been changed to prevent conflicts
// TODO: DELETE THIS FILE

using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.OBSOLETE_DO_NOT_USE
{
    public enum LocationType_OBSOLETE
    {
        LAN,
        VPN,
        Public,
        Unknown
    }

    public interface INetworkLocationService_OBSOLETE
    {
        Task<LocationType_OBSOLETE> DetectLocationAsync(string ipAddress);
        Task<bool> IsAuthorizedForVideoAsync(string ipAddress);
    }
}
