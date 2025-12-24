// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Text;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class WireGuardManagementService : IWireGuardManagementService
    {
        private readonly IVpnProfileRepository _vpnProfileRepository;
        private readonly ISystemSettingsService _systemSettingsService; // To get server public key and endpoint

        public WireGuardManagementService(IVpnProfileRepository vpnProfileRepository, ISystemSettingsService systemSettingsService)
        {
            _vpnProfileRepository = vpnProfileRepository;
            _systemSettingsService = systemSettingsService;
        }

        public async Task<VpnProfile> GenerateProfileAsync(int userId, string deviceName, string deviceType)
        {
            // 1. Generate Keypair
            var keyPair = GenerateKeyPair();
            var privateKey = Convert.ToBase64String(keyPair.privateKey);
            var publicKey = Convert.ToBase64String(keyPair.publicKey);

            // 2. Get next available IP
            var assignedIp = await _vpnProfileRepository.GetNextAvailableIpAddressAsync();

            // 3. Create and save profile
            var profile = new VpnProfile
            {
                UserId = userId,
                PublicKey = publicKey,
                PrivateKey = privateKey, // In a real system, this would be encrypted at rest
                AssignedIP = assignedIp,
                DeviceName = deviceName,
                DeviceType = deviceType,
                CreatedAt = DateTime.UtcNow
            };

            await _vpnProfileRepository.AddAsync(profile);

            return profile;
        }

        public async Task<string> GenerateConfigFileAsync(VpnProfile profile)
        {
            // These will be loaded from settings
            var settings = await _systemSettingsService.GetAsync();
            var serverPublicKey = settings.WireGuardPublicKey;
            var serverEndpoint = $"{settings.PublicDomain}:{settings.WireGuardPort}";
            var allowedIps = "10.8.0.0/24, 192.168.1.0/24"; // Should also be configurable

            var config = new StringBuilder();
            config.AppendLine("[Interface]");
            config.AppendLine($"PrivateKey = {profile.PrivateKey}");
            config.AppendLine($"Address = {profile.AssignedIP}/32");
            config.AppendLine("DNS = 1.1.1.1");
            config.AppendLine();
            config.AppendLine("[Peer]");
            config.AppendLine($"PublicKey = {serverPublicKey}");
            config.AppendLine($"Endpoint = {serverEndpoint}");
            config.AppendLine($"AllowedIPs = {allowedIps}");
            config.AppendLine("PersistentKeepalive = 25");

            return config.ToString();
        }

        public async Task<bool> ActivateProfileAsync(int profileId)
        {
            var profile = await _vpnProfileRepository.GetByIdAsync(profileId);
            if (profile == null) return false;

            profile.LastUsedAt = DateTime.UtcNow;
            await _vpnProfileRepository.UpdateAsync(profile);
            return true;
        }

        private (byte[] privateKey, byte[] publicKey) GenerateKeyPair()
        {
            var random = new SecureRandom();
            var privateKeyParams = new X25519PrivateKeyParameters(random);
            var publicKeyParams = privateKeyParams.GeneratePublicKey();

            return (privateKeyParams.GetEncoded(), publicKeyParams.GetEncoded());
        }
    }
}
