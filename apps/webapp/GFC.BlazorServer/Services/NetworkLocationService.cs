// [NEW]
using System;
using System.Net;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
                return false;
            }
        }
    }
}
