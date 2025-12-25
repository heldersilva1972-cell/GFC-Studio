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
extern alias IPNetwork2Alias;
// [MODIFIED]
using GFC.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSec.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using System.Net;
using IPNetwork = IPNetwork2Alias::System.Net.IPNetwork;

namespace GFC.BlazorServer.Services
{
    public class WireGuardManagementService : IWireGuardManagementService
    {
        private readonly GfcDbContext _dbContext;
        private readonly ILogger<WireGuardManagementService> _logger;
        private readonly IDataProtector _protector;
        private const string WgInterface = "wg0"; // The default WireGuard interface name

        public WireGuardManagementService(GfcDbContext dbContext, ILogger<WireGuardManagementService> logger, IDataProtectionProvider provider)
        {
            _dbContext = dbContext;
            _logger = logger;
            _protector = provider.CreateProtector("WireGuardManagementService.PrivateKeys");
        }

        /// <summary>
        /// Generates a new VPN profile for a user, including a unique keypair and IP address.
        /// </summary>
        public async Task<VpnProfile> GenerateProfileAsync(int userId, string deviceName, string deviceType)
        {
            _logger.LogInformation("Generating new VPN profile for UserId: {UserId}", userId);

            var (privateKey, publicKey) = GenerateKeyPair();
            var assignedIp = await GetNextAvailableIpAddressAsync();

            if (string.IsNullOrEmpty(assignedIp))
            {
                _logger.LogError("Failed to generate profile: No available IP addresses in the subnet.");
                throw new InvalidOperationException("Could not assign an IP address. The VPN subnet is full.");
            }

            var encryptedPrivateKey = EncryptPrivateKey(privateKey);

            var profile = new VpnProfile
            {
                UserId = userId,
                PublicKey = publicKey,
                PrivateKey = encryptedPrivateKey,
                AssignedIP = assignedIp,
                DeviceName = deviceName,
                DeviceType = deviceType,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.VpnProfiles.Add(profile);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully created VpnProfile Id {ProfileId} for UserId {UserId} with IP {AssignedIP}", profile.Id, userId, assignedIp);

            return profile;
        }

        /// <summary>
        /// Generates the content for a .conf file based on a user's VPN profile.
        /// </summary>
        public async Task<string> GenerateConfigFileAsync(VpnProfile profile)
        {
            var settings = await _dbContext.SystemSettings.FindAsync(1) ?? throw new InvalidOperationException("System settings not found.");

            if (string.IsNullOrWhiteSpace(settings.WireGuardServerPublicKey))
            {
                throw new InvalidOperationException("WireGuard server public key is not configured in system settings.");
            }

            var decryptedPrivateKey = DecryptPrivateKey(profile.PrivateKey);

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
            config.AppendLine($"PublicKey = {settings.WireGuardServerPublicKey}");
            config.AppendLine($"Endpoint = {settings.PublicDomain}:{settings.WireGuardPort}");
            config.AppendLine($"AllowedIPs = {settings.WireGuardAllowedIPs}");
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
        /// <summary>
        /// Activates a profile by adding it as a peer to the WireGuard server.
        /// </summary>
        public async Task<bool> ActivateProfileAsync(int profileId)
        {
            var profile = await _dbContext.VpnProfiles.FindAsync(profileId);
            if (profile == null)
            {
                _logger.LogError("ActivateProfileAsync failed: Profile with Id {ProfileId} not found.", profileId);
                return false;
            }

            if (profile.RevokedAt.HasValue)
            {
                _logger.LogWarning("ActivateProfileAsync skipped: Profile with Id {ProfileId} has been revoked.", profileId);
                return false;
            }

            _logger.LogInformation("Activating profile Id {ProfileId} for user {UserId}", profile.Id, profile.UserId);

            var command = $"set {WgInterface} peer {profile.PublicKey} allowed-ips {profile.AssignedIP}/32";
            var success = await ExecuteWgCommandAsync(command);

            if (success)
            {
                profile.LastUsedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Successfully activated profile Id {ProfileId}.", profile.Id);
                await PersistWgChangesAsync();
            }

            return success;
        }

        /// <summary>
        /// Revokes a profile, removing it as a peer from the WireGuard server and marking it as revoked in the DB.
        /// </summary>
        public async Task<bool> RevokeProfileAsync(int profileId, int revokedByUserId, string reason)
        {
            var profile = await _dbContext.VpnProfiles.FindAsync(profileId);
            if (profile == null)
            {
                _logger.LogError("RevokeProfileAsync failed: Profile with Id {ProfileId} not found.", profileId);
                return false;
            }

            _logger.LogInformation("Revoking profile Id {ProfileId} by user {RevokedByUserId} for reason: {Reason}", profile.Id, revokedByUserId, reason);

            var command = $"set {WgInterface} peer {profile.PublicKey} remove";
            var success = await ExecuteWgCommandAsync(command);

            if (success)
            {
                profile.RevokedAt = DateTime.UtcNow;
                profile.RevokedBy = revokedByUserId;
                profile.RevokedReason = reason;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Successfully revoked profile Id {ProfileId} in database.", profile.Id);
                await PersistWgChangesAsync();
            }

            return success;
        }

        public async Task<List<VpnProfile>> GetUserProfilesAsync(int userId)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.VpnProfiles
                .AsNoTracking()
            return await _dbContext.VpnProfiles
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
        private (string privateKey, string publicKey) GenerateKeyPair()
        {
            var algorithm = KeyAgreementAlgorithm.X25519;
            using var key = Key.Create(algorithm, new KeyCreationParameters { ExportPolicy = KeyExportPolicies.AllowPlaintextExport });

            var privateKeyBytes = key.Export(KeyBlobFormat.RawPrivateKey);
            var publicKeyBytes = key.PublicKey.Export(KeyBlobFormat.RawPublicKey);

            return (Convert.ToBase64String(privateKeyBytes), Convert.ToBase64String(publicKeyBytes));
        }

        private async Task<string?> GetNextAvailableIpAddressAsync()
        {
            var settings = await _dbContext.SystemSettings.FindAsync(1) ?? throw new InvalidOperationException("System settings not found.");
            var subnet = IPNetwork.Parse(settings.WireGuardSubnet);

            var usedIps = (await _dbContext.VpnProfiles
                .Where(p => p.RevokedAt == null)
                .Select(p => p.AssignedIP)
                .ToListAsync())
                .ToHashSet();

            foreach (var ip in subnet.ListIPAddress().Skip(2))
            {
                if (!usedIps.Contains(ip.ToString()))
                {
                    return ip.ToString();
                }
            }

            return null;
        }

        private async Task<bool> PersistWgChangesAsync()
        {
            return await ExecuteCommandAsync("wg-quick", $"save {WgInterface}");
        }

        private async Task<bool> ExecuteWgCommandAsync(string arguments)
        {
            return await ExecuteCommandAsync("wg", arguments);
        }

        private async Task<bool> ExecuteCommandAsync(string command, string arguments)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            _logger.LogDebug("Executing command: {Command} {Arguments}", command, arguments);

            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode == 0)
            {
                _logger.LogInformation("Command '{Command} {Arguments}' executed successfully. Output: {Output}", command, arguments, output);
                return true;
            }
            else
            {
                _logger.LogError("Command '{Command} {Arguments}' failed with exit code {ExitCode}. Error: {Error}", command, arguments, process.ExitCode, error);
                return false;
            }
        }

        private string EncryptPrivateKey(string privateKey)
        {
            return _protector.Protect(privateKey);
        }

        private string DecryptPrivateKey(string encryptedKey)
        {
            return _protector.Unprotect(encryptedKey);
        }
    }
}
