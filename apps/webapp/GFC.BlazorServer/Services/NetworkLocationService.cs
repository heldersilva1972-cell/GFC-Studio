// [NEW]
using System;
using System.Net;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GFC.Core.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;
using LocationType = GFC.Core.Interfaces.LocationType;

namespace GFC.BlazorServer.Services
{
    public class NetworkLocationService : INetworkLocationService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;
        private readonly ILogger<NetworkLocationService> _logger;

        public NetworkLocationService(IDbContextFactory<GfcDbContext> dbContextFactory, ILogger<NetworkLocationService> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<LocationType> DetectLocationAsync(string ipAddress)
        {
            if (await IsLanAddressAsync(ipAddress))
            {
                return LocationType.LAN;
            }
            if (await IsVpnAddressAsync(ipAddress))
            {
                return LocationType.VPN;
            }
        // In the future, these values will be loaded from SystemSettingsService.
        private const string LanSubnet = "192.168.1.0/24";
        private const string VpnSubnet = "10.8.0.0/24";

        public async Task<LocationType> DetectLocationAsync(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress) || !IPAddress.TryParse(ipAddress, out var parsedIp))
            {
                return LocationType.Unknown;
            }

            // Simulate async operation
            await Task.Delay(10);

            if (IsInSubnet(parsedIp, LanSubnet))
            {
                return LocationType.LAN;
            }

            if (IsInSubnet(parsedIp, VpnSubnet))
            {
                return LocationType.VPN;
            }

            return LocationType.Public;
        }

        public async Task<bool> IsAuthorizedForVideoAsync(string ipAddress)
        {
            var location = await DetectLocationAsync(ipAddress);
            return location == LocationType.LAN || location == LocationType.VPN;
        }

        public async Task<bool> IsLanAddressAsync(string ipAddress)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var settings = await dbContext.SystemSettings.FirstOrDefaultAsync();
            var lanSubnet = settings?.LanSubnet ?? "192.168.1.0/24";
            return IsInSubnet(ipAddress, lanSubnet);
            var location = await DetectLocationAsync(ipAddress);
            return location == LocationType.LAN;
        }

        public async Task<bool> IsVpnAddressAsync(string ipAddress)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var settings = await dbContext.SystemSettings.FirstOrDefaultAsync();
            var vpnSubnet = settings?.WireGuardSubnet ?? "10.8.0.0/24";
            return IsInSubnet(ipAddress, vpnSubnet);
        }

        private bool IsInSubnet(string ipAddress, string cidr)
        {
            try
            {
                var ipnetwork = IPNetwork.Parse(cidr);
                var ip = IPAddress.Parse(ipAddress);
                return ipnetwork.Contains(ip);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if IP {IpAddress} is in subnet {Cidr}", ipAddress, cidr);
            var location = await DetectLocationAsync(ipAddress);
            return location == LocationType.VPN;
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
