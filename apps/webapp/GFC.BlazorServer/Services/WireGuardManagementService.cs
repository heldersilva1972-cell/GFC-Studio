extern alias IPNetwork2Alias;

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
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;
        private readonly ILogger<WireGuardManagementService> _logger;
        private readonly IDataProtector _protector;
        private const string WgInterface = "wg0";

        public WireGuardManagementService(
            IDbContextFactory<GfcDbContext> dbContextFactory, 
            ILogger<WireGuardManagementService> logger, 
            IDataProtectionProvider provider)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
            _protector = provider.CreateProtector("WireGuardManagementService.PrivateKeys");
        }

        public async Task<VpnProfile> GenerateProfileAsync(int userId, string deviceName, string deviceType)
        {
            _logger.LogInformation("Generating new VPN profile for UserId: {UserId}", userId);

            try
            {
                var (privateKey, publicKey) = GenerateKeyPair();
                
                await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
                var assignedIp = await GetNextAvailableIpAddressAsync(dbContext);

                if (string.IsNullOrEmpty(assignedIp))
                {
                    _logger.LogError("Failed to generate profile: No available IP addresses in the subnet.");
                    throw new InvalidOperationException("Could not assign an IP address. The VPN subnet is full.");
                }

                var encryptedPrivateKey = _protector.Protect(privateKey);

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

                dbContext.VpnProfiles.Add(profile);
                await dbContext.SaveChangesAsync();

                _logger.LogInformation("Successfully created VpnProfile Id {ProfileId} for UserId {UserId} with IP {AssignedIP}", profile.Id, userId, assignedIp);

                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating WireGuard profile for user {UserId}", userId);
                throw;
            }
        }

        public async Task<string> GenerateConfigFileAsync(VpnProfile profile)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var settings = await dbContext.SystemSettings.FindAsync(1) ?? throw new InvalidOperationException("System settings not found.");

            if (string.IsNullOrWhiteSpace(settings.WireGuardServerPublicKey))
            {
                throw new InvalidOperationException("WireGuard server public key is not configured in system settings.");
            }

            var decryptedPrivateKey = _protector.Unprotect(profile.PrivateKey);

            var config = new StringBuilder();
            config.AppendLine("[Interface]");
            config.AppendLine($"PrivateKey = {decryptedPrivateKey}");
            config.AppendLine($"Address = {profile.AssignedIP}/32");
            config.AppendLine("DNS = 1.1.1.1");
            config.AppendLine();
            config.AppendLine("[Peer]");
            config.AppendLine($"PublicKey = {settings.WireGuardServerPublicKey}");
            config.AppendLine($"Endpoint = {settings.PublicDomain}:{settings.WireGuardPort}");
            config.AppendLine($"AllowedIPs = {settings.WireGuardAllowedIPs}");
            config.AppendLine("PersistentKeepalive = 25");

            return config.ToString();
        }

        public async Task<bool> ActivateProfileAsync(int profileId)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var profile = await dbContext.VpnProfiles.FindAsync(profileId);
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
                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Successfully activated profile Id {ProfileId}.", profile.Id);
                await PersistWgChangesAsync();
            }

            return success;
        }

        public async Task<bool> RevokeProfileAsync(int profileId, int revokedByUserId, string reason)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var profile = await dbContext.VpnProfiles.FindAsync(profileId);
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
                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Successfully revoked profile Id {ProfileId} in database.", profile.Id);
                await PersistWgChangesAsync();
            }

            return success;
        }

        public async Task<List<VpnProfile>> GetUserProfilesAsync(int userId)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            return await dbContext.VpnProfiles
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        private (string privateKey, string publicKey) GenerateKeyPair()
        {
            var algorithm = KeyAgreementAlgorithm.X25519;
            using var key = Key.Create(algorithm, new KeyCreationParameters { ExportPolicy = KeyExportPolicies.AllowPlaintextExport });

            var privateKeyBytes = key.Export(KeyBlobFormat.RawPrivateKey);
            var publicKeyBytes = key.PublicKey.Export(KeyBlobFormat.RawPublicKey);

            return (Convert.ToBase64String(privateKeyBytes), Convert.ToBase64String(publicKeyBytes));
        }

        private async Task<string?> GetNextAvailableIpAddressAsync(GfcDbContext dbContext)
        {
            var settings = await dbContext.SystemSettings.FindAsync(1) ?? throw new InvalidOperationException("System settings not found.");
            var subnet = IPNetwork.Parse(settings.WireGuardSubnet);

            var usedIps = (await dbContext.VpnProfiles
                .Where(p => p.RevokedAt == null)
                .Select(p => p.AssignedIP)
                .ToListAsync())
                .ToHashSet();

            foreach (var ip in subnet.ListIPAddress().Skip(2)) // Skip .0 (network) and .1 (server)
            {
                var ipString = ip.ToString();
                if (!usedIps.Contains(ipString))
                {
                    return ipString;
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
            try
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
                    _logger.LogInformation("Command '{Command} {Arguments}' executed successfully.", command, arguments);
                    return true;
                }
                else
                {
                    _logger.LogError("Command '{Command} {Arguments}' failed with exit code {ExitCode}. Error: {Error}", command, arguments, process.ExitCode, error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while executing command: {Command} {Arguments}", command, arguments);
                return false;
            }
        }
    }
}
