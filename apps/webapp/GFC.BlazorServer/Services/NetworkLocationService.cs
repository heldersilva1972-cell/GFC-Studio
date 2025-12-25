// [NEW]
using GFC.Core.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class NetworkLocationService : INetworkLocationService
    {
        // In the future, these values will be loaded from SystemSettingsService.
        private const string LanSubnet = "192.168.1.0/24";
        private const string VpnSubnet = "10.8.0.0/24";

        public async Task<GFC.Core.Interfaces.LocationType> DetectLocationAsync(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress) || !IPAddress.TryParse(ipAddress, out var parsedIp))
            {
                return GFC.Core.Interfaces.LocationType.Unknown;
            }

            // Simulate async operation
            await Task.Delay(10);

            if (IsInSubnet(parsedIp, LanSubnet))
            {
                return GFC.Core.Interfaces.LocationType.LAN;
            }

            if (IsInSubnet(parsedIp, VpnSubnet))
            {
                return GFC.Core.Interfaces.LocationType.VPN;
            }

            return GFC.Core.Interfaces.LocationType.Public;
        }

        public async Task<bool> IsAuthorizedForVideoAsync(string ipAddress)
        {
            var location = await DetectLocationAsync(ipAddress);
            return location == GFC.Core.Interfaces.LocationType.LAN || location == GFC.Core.Interfaces.LocationType.VPN;
        }

        public async Task<bool> IsLanAddressAsync(string ipAddress)
        {
            var location = await DetectLocationAsync(ipAddress);
            return location == GFC.Core.Interfaces.LocationType.LAN;
        }

        public async Task<bool> IsVpnAddressAsync(string ipAddress)
        {
            var location = await DetectLocationAsync(ipAddress);
            return location == GFC.Core.Interfaces.LocationType.VPN;
        }

        private bool IsInSubnet(IPAddress address, string subnetCidr)
        {
            try
            {
                var parts = subnetCidr.Split('/');
                if (parts.Length != 2) return false;

                var networkAddress = IPAddress.Parse(parts[0]);
                if (networkAddress.AddressFamily != address.AddressFamily) return false;

                int cidr = int.Parse(parts[1]);
                byte[] ipBytes = address.GetAddressBytes();
                byte[] maskBytes = networkAddress.GetAddressBytes();

                int bits = cidr;
                for (int i = 0; i < ipBytes.Length && bits > 0; i++)
                {
                    int mask = bits >= 8 ? 255 : (256 - (1 << (8 - bits)));
                    if ((ipBytes[i] & mask) != (maskBytes[i] & mask))
                    {
                        return false;
                    }
                    bits -= 8;
                }

                return true;
            }
            catch
            {
                // Error parsing subnet, default to not matching.
                return false;
            }
        }
    }
}
