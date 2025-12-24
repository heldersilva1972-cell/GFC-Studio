// [NEW]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSec.Cryptography;

namespace GFC.BlazorServer.Services
{
using Microsoft.AspNetCore.DataProtection;

    public class WireGuardManagementService : IWireGuardManagementService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;
        private readonly ILogger<WireGuardManagementService> _logger;
        private readonly IDataProtector _protector;

        public WireGuardManagementService(IDbContextFactory<GfcDbContext> dbContextFactory, ILogger<WireGuardManagementService> logger, IDataProtectionProvider dataProtectionProvider)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
            _protector = dataProtectionProvider.CreateProtector("WireGuard.PrivateKey");
        }

        public async Task<VpnProfile> GenerateProfileAsync(int userId, string deviceName, string deviceType)
        {
            try
            {
                using var key = Key.Create(KeyAgreementAlgorithm.X25519, new KeyCreationParameters { ExportPolicy = KeyExportPolicies.AllowPlaintextArchiving });
                var privateKeyBytes = key.Export(KeyBlobFormat.RawPrivateKey);
                var publicKeyBytes = key.Export(KeyBlobFormat.RawPublicKey);

                var privateKey = Convert.ToBase64String(privateKeyBytes);
                var publicKey = Convert.ToBase64String(publicKeyBytes);
                var encryptedPrivateKey = _protector.Protect(privateKey);

                await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
                var assignedIp = await GetNextAvailableIpAsync(dbContext);

                var newProfile = new VpnProfile
                {
                    UserId = userId,
                    PublicKey = publicKey,
                    PrivateKey = encryptedPrivateKey,
                    AssignedIP = assignedIp,
                    DeviceName = deviceName,
                    DeviceType = deviceType,
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.VpnProfiles.Add(newProfile);
                await dbContext.SaveChangesAsync();
                return newProfile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating WireGuard profile for user {UserId}", userId);
                return null;
            }
        }

        public async Task<string> GenerateConfigFileAsync(VpnProfile profile)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var settings = await dbContext.SystemSettings.FirstAsync();

            var serverPublicKey = settings.WireGuardServerPublicKey;
            var decryptedPrivateKey = _protector.Unprotect(profile.PrivateKey);

            var config = new StringBuilder();
            config.AppendLine("[Interface]");
            config.AppendLine($"PrivateKey = {decryptedPrivateKey}");
            config.AppendLine($"Address = {profile.AssignedIP}/32");
            config.AppendLine("DNS = 1.1.1.1");
            config.AppendLine();
            config.AppendLine("[Peer]");
            config.AppendLine($"PublicKey = {serverPublicKey}");
            config.AppendLine($"Endpoint = {settings.PublicDomain}:{settings.WireGuardPort}");
            config.AppendLine($"AllowedIPs = {settings.WireGuardSubnet}, {settings.LanSubnet}");
            config.AppendLine("PersistentKeepalive = 25");

            return config.ToString();
        }

        public async Task<bool> ActivateProfileAsync(int profileId)
        {
            _logger.LogInformation("Activating profile {ProfileId}", profileId);

            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var profile = await dbContext.VpnProfiles.FindAsync(profileId);
            if (profile == null) return false;

            var command = $"set wg0 peer {profile.PublicKey} allowed-ips {profile.AssignedIP}/32";
            _logger.LogInformation("Executing WireGuard command: `wg {Command}`", command);

            // In a real environment, you would execute the command like this:
            // var process = new System.Diagnostics.Process
            // {
            //     StartInfo = new System.Diagnostics.ProcessStartInfo
            //     {
            //         FileName = "wg",
            //         Arguments = command,
            //         RedirectStandardOutput = true,
            //         RedirectStandardError = true,
            //         UseShellExecute = false,
            //         CreateNoWindow = true,
            //     }
            // };
            // process.Start();
            // await process.WaitForExitAsync();
            // if (process.ExitCode != 0)
            // {
            //     var error = await process.StandardError.ReadToEndAsync();
            //     _logger.LogError("WireGuard command failed: {Error}", error);
            //     return false;
            // }

            _logger.LogWarning("WireGuard command execution is skipped in this environment.");
            return await Task.FromResult(true); // Placeholder
        }

        public async Task<bool> RevokeProfileAsync(int profileId, int revokedByUserId, string reason)
        {
            _logger.LogInformation("Revoking profile {ProfileId} by user {UserId} for reason: {Reason}", profileId, revokedByUserId, reason);

            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var profile = await dbContext.VpnProfiles.FindAsync(profileId);
            if (profile == null) return false;

            var command = $"set wg0 peer {profile.PublicKey} remove";
            _logger.LogInformation("Executing WireGuard command: `wg {Command}`", command);

            // See ActivateProfileAsync for command execution example.

            _logger.LogWarning("WireGuard command execution is skipped in this environment.");
            return await Task.FromResult(true); // Placeholder
        }

        public async Task<List<VpnProfile>> GetUserProfilesAsync(int userId)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.VpnProfiles
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public Task<List<VpnSession>> GetActiveSessionsAsync()
        {
            // TODO: Implement logic to get active sessions from WireGuard server
            // Example: `wg show wg0 dump`
            return Task.FromResult(new List<VpnSession>()); // Placeholder
        }

        public Task<bool> DisconnectSessionAsync(int sessionId)
        {
            _logger.LogInformation("Disconnecting session {SessionId}", sessionId);
            // This is complex and may not be directly possible with wg-quick.
            // It might require more advanced server management.
            return Task.FromResult(true); // Placeholder
        }

        private async Task<string> GetNextAvailableIpAsync(GfcDbContext dbContext)
        {
            var settings = await dbContext.SystemSettings.FirstAsync();
            var subnet = IPNetwork.Parse(settings.WireGuardSubnet);

            var usedIps = await dbContext.VpnProfiles
                .Select(p => p.AssignedIP)
                .ToHashSetAsync();

            var firstIp = subnet.ListIPAddress().Skip(1).First(); // Skip the network address, take the first usable (.1) as server
            var availableIps = subnet.ListIPAddress().Skip(2); // Skip .0 and .1

            foreach (var ip in availableIps)
            {
                var ipString = ip.ToString();
                if (!usedIps.Contains(ipString))
                {
                    return ipString;
                }
            }

            throw new Exception("No available IP addresses in the VPN subnet.");
        }
    }
}
