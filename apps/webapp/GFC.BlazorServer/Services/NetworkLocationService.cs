// [NEW]
using System;
using System.Net;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GFC.Core.Interfaces;

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
            if (string.IsNullOrEmpty(ipAddress))
            {
                return LocationType.Unknown;
            }

            if (await IsLanAddressAsync(ipAddress))
            {
                return LocationType.LAN;
            }

            if (await IsVpnAddressAsync(ipAddress))
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
            try
            {
                await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
                var settings = await dbContext.SystemSettings.FirstOrDefaultAsync();
                var lanSubnet = settings?.LanSubnet ?? "192.168.1.0/24";
                return IsInSubnet(ipAddress, lanSubnet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if IP {IpAddress} is in LAN subnet", ipAddress);
                return false;
            }
        }

        public async Task<bool> IsVpnAddressAsync(string ipAddress)
        {
            try
            {
                await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
                var settings = await dbContext.SystemSettings.FirstOrDefaultAsync();
                var vpnSubnet = settings?.WireGuardSubnet ?? "10.8.0.0/24";
                return IsInSubnet(ipAddress, vpnSubnet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if IP {IpAddress} is in VPN subnet", ipAddress);
                return false;
            }
        }

        private bool IsInSubnet(string ipAddress, string subnetCidr)
        {
            if (!IPAddress.TryParse(ipAddress, out var address))
                return false;

            try
            {
                var parts = subnetCidr.Split('/');
                if (parts.Length != 2) return false;

                var networkAddress = IPAddress.Parse(parts[0]);
                if (networkAddress.AddressFamily != address.AddressFamily) return false;

                int cidr = int.Parse(parts[1]);
                byte[] ipBytes = address.GetAddressBytes();
                byte[] maskBytes = networkAddress.GetAddressBytes();

                // Normalize
                if (address.IsIPv4MappedToIPv6) ipBytes = address.MapToIPv4().GetAddressBytes();
                if (networkAddress.IsIPv4MappedToIPv6) maskBytes = networkAddress.MapToIPv4().GetAddressBytes();

                if (ipBytes.Length != maskBytes.Length) return false;

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
                return false;
            }
        }
    }
}
