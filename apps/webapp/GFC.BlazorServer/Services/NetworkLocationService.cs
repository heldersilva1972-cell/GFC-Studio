// [NEW]
using GFC.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Threading.Tasks;
using IPNetwork2;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services
{
    public class NetworkLocationService : INetworkLocationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<NetworkLocationService> _logger;
        private IPNetwork _lanSubnet;
        private IPNetwork _vpnSubnet;

        public NetworkLocationService(IConfiguration configuration, ILogger<NetworkLocationService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Load subnets from configuration
            var lanSubnetString = _configuration.GetValue<string>("NetworkSettings:LanSubnet");
            var vpnSubnetString = _configuration.GetValue<string>("NetworkSettings:VpnSubnet");

            if (!IPNetwork.TryParse(lanSubnetString, out _lanSubnet))
            {
                _logger.LogError("Invalid LAN subnet configured: {LanSubnet}", lanSubnetString);
            }

            if (!IPNetwork.TryParse(vpnSubnetString, out _vpnSubnet))
            {
                _logger.LogError("Invalid VPN subnet configured: {VpnSubnet}", vpnSubnetString);
            }
        }

        public Task<LocationType> DetectLocationAsync(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress) || !IPAddress.TryParse(ipAddress, out var ip))
            {
                return Task.FromResult(LocationType.Unknown);
            }

            if (_lanSubnet != null && _lanSubnet.Contains(ip))
            {
                return Task.FromResult(LocationType.LAN);
            }

            if (_vpnSubnet != null && _vpnSubnet.Contains(ip))
            {
                return Task.FromResult(LocationType.VPN);
            }

            return Task.FromResult(LocationType.Public);
        }

        public async Task<bool> IsLanAddressAsync(string ipAddress)
        {
            var location = await DetectLocationAsync(ipAddress);
            return location == LocationType.LAN;
        }

        public async Task<bool> IsVpnAddressAsync(string ipAddress)
        {
            var location = await DetectLocationAsync(ipAddress);
            return location == LocationType.VPN;
        }

        public async Task<bool> IsAuthorizedForVideoAsync(string ipAddress)
        {
            var location = await DetectLocationAsync(ipAddress);
            return location == LocationType.LAN || location == LocationType.VPN;
        }
    }
}
