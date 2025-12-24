// [NEW]
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
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
        Task<LocationType> DetectLocationAsync(string ipAddress);
        Task<bool> IsLanAddressAsync(string ipAddress);
        Task<bool> IsVpnAddressAsync(string ipAddress);
        Task<bool> IsAuthorizedForVideoAsync(string ipAddress);
    }
}
