using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.NetworkMigration
{
    /// <summary>
    /// Service for managing controller network migrations (LAN ↔ VPN)
    /// </summary>
    public class NetworkMigrationService : INetworkMigrationService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;
        private readonly ILogger<NetworkMigrationService> _logger;

        public NetworkMigrationService(
            IDbContextFactory<GfcDbContext> dbFactory,
            ILogger<NetworkMigrationService> logger)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public async Task<NetworkConfiguration> DetectCurrentConfigAsync(int controllerId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            var controller = await db.Controllers
                .Include(c => c.Events.OrderByDescending(e => e.TimestampUtc).Take(1))
                .FirstOrDefaultAsync(c => c.Id == controllerId);

            if (controller == null)
            {
                throw new ArgumentException($"Controller with ID {controllerId} not found");
            }

            var vpnProfile = controller.VpnProfileId.HasValue
                ? await db.VpnProfiles.FindAsync(controller.VpnProfileId.Value)
                : null;

            var lastEvent = controller.Events?.FirstOrDefault();
            var isConnected = lastEvent != null && 
                             (DateTime.UtcNow - lastEvent.TimestampUtc).TotalMinutes < 5;

            var hasBackup = !string.IsNullOrEmpty(controller.BackupIpAddress) &&
                           controller.BackupExpiresUtc.HasValue &&
                           controller.BackupExpiresUtc.Value > DateTime.UtcNow;

            return new NetworkConfiguration
            {
                ControllerId = controller.Id,
                ControllerName = controller.Name,
                NetworkType = controller.NetworkType ?? "LAN",
                IpAddress = controller.IpAddress,
                Port = controller.Port,
                VpnProfileId = controller.VpnProfileId,
                VpnProfileName = vpnProfile?.DeviceName,
                IsConnected = isConnected,
                LastSeenUtc = lastEvent?.TimestampUtc,
                HasBackup = hasBackup,
                BackupExpiresUtc = controller.BackupExpiresUtc
            };
        }

        public async Task<ValidationResult> ValidateNewConfigAsync(NetworkMigrationRequest request)
        {
            var result = new ValidationResult { IsValid = true };

            using var db = await _dbFactory.CreateDbContextAsync();

            // Check controller exists
            var controller = await db.Controllers.FindAsync(request.ControllerId);
            if (controller == null)
            {
                result.IsValid = false;
                result.Errors.Add($"Controller with ID {request.ControllerId} not found");
                return result;
            }

            // Validate IP address
            if (string.IsNullOrWhiteSpace(request.NewIpAddress))
            {
                result.IsValid = false;
                result.Errors.Add("IP address is required");
            }

            // Validate port
            if (request.NewPort <= 0 || request.NewPort > 65535)
            {
                result.IsValid = false;
                result.Errors.Add("Port must be between 1 and 65535");
            }

            // Validate network type
            var validNetworkTypes = new[] { "LAN", "VPN", "Remote" };
            if (!validNetworkTypes.Contains(request.TargetNetworkType))
            {
                result.IsValid = false;
                result.Errors.Add($"Network type must be one of: {string.Join(", ", validNetworkTypes)}");
            }

            // If VPN, validate VPN profile exists
            if (request.TargetNetworkType == "VPN" && request.VpnProfileId.HasValue)
            {
                var vpnProfile = await db.VpnProfiles.FindAsync(request.VpnProfileId.Value);
                if (vpnProfile == null)
                {
                    result.IsValid = false;
                    result.Errors.Add($"VPN Profile with ID {request.VpnProfileId.Value} not found");
                }
            }

            // Check if same as current config
            if (controller.IpAddress == request.NewIpAddress && 
                controller.Port == request.NewPort &&
                controller.NetworkType == request.TargetNetworkType)
            {
                result.Warnings.Add("New configuration is the same as current configuration");
            }

            return result;
        }

        public async Task<ConnectionTestResult> TestConnectionAsync(string ipAddress, int port, int? vpnProfileId = null)
        {
            var result = new ConnectionTestResult();
            var sw = Stopwatch.StartNew();

            // Step 1: Ping test
            var pingStep = new TestStep { Name = "Ping Test", Status = "InProgress" };
            result.Steps.Add(pingStep);

            try
            {
                var pingStopwatch = Stopwatch.StartNew();
                using var ping = new Ping();
                var pingReply = await ping.SendPingAsync(ipAddress, 2000);
                pingStopwatch.Stop();

                if (pingReply.Status == IPStatus.Success)
                {
                    pingStep.Status = "Success";
                    pingStep.Message = $"Ping successful ({pingReply.RoundtripTime}ms)";
                    pingStep.DurationMs = pingStopwatch.ElapsedMilliseconds;
                }
                else
                {
                    pingStep.Status = "Failed";
                    pingStep.Message = $"Ping failed: {pingReply.Status}";
                    pingStep.DurationMs = pingStopwatch.ElapsedMilliseconds;
                    result.Success = false;
                    result.ErrorMessage = $"Cannot ping {ipAddress}";
                }
            }
            catch (Exception ex)
            {
                pingStep.Status = "Failed";
                pingStep.Message = $"Ping error: {ex.Message}";
                result.Success = false;
                result.ErrorMessage = ex.Message;
                _logger.LogError(ex, "Ping test failed for {IpAddress}", ipAddress);
            }

            // Step 2: UDP Port test (basic check - just verify we can create a socket)
            var portStep = new TestStep { Name = "UDP Port Check", Status = "InProgress" };
            result.Steps.Add(portStep);

            try
            {
                var portStopwatch = Stopwatch.StartNew();
                
                // Note: UDP is connectionless, so we can't truly "test" the port without sending actual controller packets
                // This is a basic check that we can create a UDP client
                using var udpClient = new System.Net.Sockets.UdpClient();
                udpClient.Connect(ipAddress, port);
                
                portStopwatch.Stop();
                portStep.Status = "Success";
                portStep.Message = $"UDP client created for {ipAddress}:{port}";
                portStep.DurationMs = portStopwatch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                portStep.Status = "Failed";
                portStep.Message = $"UDP port check error: {ex.Message}";
                result.Success = false;
                result.ErrorMessage = ex.Message;
                _logger.LogError(ex, "UDP port test failed for {IpAddress}:{Port}", ipAddress, port);
            }

            // Step 3: VPN Profile check (if applicable)
            if (vpnProfileId.HasValue)
            {
                var vpnStep = new TestStep { Name = "VPN Profile Check", Status = "InProgress" };
                result.Steps.Add(vpnStep);

                try
                {
                    using var db = await _dbFactory.CreateDbContextAsync();
                    var vpnProfile = await db.VpnProfiles.FindAsync(vpnProfileId.Value);
                    
                    if (vpnProfile != null)
                    {
                        vpnStep.Status = "Success";
                        vpnStep.Message = $"VPN Profile '{vpnProfile.DeviceName}' found";
                    }
                    else
                    {
                        vpnStep.Status = "Failed";
                        vpnStep.Message = "VPN Profile not found";
                        result.Warnings.Add("VPN Profile not found - connection may not work");
                    }
                }
                catch (Exception ex)
                {
                    vpnStep.Status = "Failed";
                    vpnStep.Message = $"VPN check error: {ex.Message}";
                    _logger.LogError(ex, "VPN profile check failed");
                }
            }

            sw.Stop();
            result.TotalDurationMs = sw.ElapsedMilliseconds;
            
            // Overall success if no failures
            result.Success = result.Steps.All(s => s.Status != "Failed");

            return result;
        }

        public async Task<MigrationResult> ExecuteMigrationAsync(NetworkMigrationRequest request)
        {
            var result = new MigrationResult();

            try
            {
                // Step 1: Validate request
                var validation = await ValidateNewConfigAsync(request);
                if (!validation.IsValid)
                {
                    result.Success = false;
                    result.ErrorMessage = string.Join("; ", validation.Errors);
                    return result;
                }

                using var db = await _dbFactory.CreateDbContextAsync();

                // Step 2: Get current configuration
                var currentConfig = await DetectCurrentConfigAsync(request.ControllerId);
                var controller = await db.Controllers.FindAsync(request.ControllerId);

                // Step 3: Create migration record
                var migration = new Data.Entities.NetworkMigration
                {
                    ControllerId = request.ControllerId,
                    MigrationType = DetermineMigrationType(currentConfig.NetworkType, request.TargetNetworkType),
                    PreviousNetworkType = currentConfig.NetworkType,
                    PreviousIpAddress = currentConfig.IpAddress,
                    PreviousPort = currentConfig.Port,
                    PreviousVpnProfileId = currentConfig.VpnProfileId,
                    NewNetworkType = request.TargetNetworkType,
                    NewIpAddress = request.NewIpAddress,
                    NewPort = request.NewPort,
                    NewVpnProfileId = request.VpnProfileId,
                    InitiatedUtc = DateTime.UtcNow,
                    Status = "InProgress",
                    InitiatedByUser = request.InitiatedByUser,
                    Notes = request.Notes,
                    CanRollback = true
                };

                await db.NetworkMigrations.AddAsync(migration);
                await db.SaveChangesAsync();

                // Step 4: Test new connection (unless skipped)
                ConnectionTestResult testResult = null;
                if (!request.SkipConnectionTest)
                {
                    testResult = await TestConnectionAsync(request.NewIpAddress, request.NewPort, request.VpnProfileId);
                    migration.TestResultsJson = JsonSerializer.Serialize(testResult);

                    if (!testResult.Success)
                    {
                        migration.Status = "Failed";
                        migration.ErrorMessage = testResult.ErrorMessage;
                        migration.CompletedUtc = DateTime.UtcNow;
                        migration.CanRollback = false;
                        await db.SaveChangesAsync();

                        result.Success = false;
                        result.ErrorMessage = $"Connection test failed: {testResult.ErrorMessage}";
                        return result;
                    }
                }

                // Step 5: Save backup configuration
                controller.BackupIpAddress = controller.IpAddress;
                controller.BackupPort = controller.Port;
                controller.BackupExpiresUtc = DateTime.UtcNow.AddHours(24);
                migration.BackupExpiresUtc = controller.BackupExpiresUtc;

                // Step 6: Apply new configuration
                controller.IpAddress = request.NewIpAddress;
                controller.Port = request.NewPort;
                controller.NetworkType = request.TargetNetworkType;
                controller.VpnProfileId = request.VpnProfileId;
                controller.LastMigrationUtc = DateTime.UtcNow;

                // Step 7: Mark migration complete
                migration.Status = "Completed";
                migration.CompletedUtc = DateTime.UtcNow;

                await db.SaveChangesAsync();

                _logger.LogInformation(
                    "Network migration completed for controller {ControllerId}: {OldIp} -> {NewIp}",
                    request.ControllerId, currentConfig.IpAddress, request.NewIpAddress);

                // Step 8: Build result
                result.Success = true;
                result.MigrationId = migration.Id;
                result.NewConfiguration = await DetectCurrentConfigAsync(request.ControllerId);
                result.BackupConfiguration = new NetworkConfiguration
                {
                    ControllerId = currentConfig.ControllerId,
                    ControllerName = currentConfig.ControllerName,
                    NetworkType = currentConfig.NetworkType,
                    IpAddress = controller.BackupIpAddress,
                    Port = controller.BackupPort ?? currentConfig.Port,
                    VpnProfileId = currentConfig.VpnProfileId
                };
                result.BackupExpiresUtc = controller.BackupExpiresUtc;
                result.CanRollback = true;

                if (validation.Warnings.Any())
                {
                    result.Warnings.AddRange(validation.Warnings);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Network migration failed for controller {ControllerId}", request.ControllerId);
                
                result.Success = false;
                result.ErrorMessage = $"Migration failed: {ex.Message}";
                
                // Try to mark migration as failed in database
                try
                {
                    using var db = await _dbFactory.CreateDbContextAsync();
                    var migration = await db.NetworkMigrations
                        .Where(m => m.ControllerId == request.ControllerId && m.Status == "InProgress")
                        .OrderByDescending(m => m.InitiatedUtc)
                        .FirstOrDefaultAsync();

                    if (migration != null)
                    {
                        migration.Status = "Failed";
                        migration.ErrorMessage = ex.Message;
                        migration.CompletedUtc = DateTime.UtcNow;
                        migration.CanRollback = false;
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception dbEx)
                {
                    _logger.LogError(dbEx, "Failed to update migration status in database");
                }

                return result;
            }
        }

        public async Task<bool> RollbackMigrationAsync(int migrationId, string userName)
        {
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();

                var migration = await db.NetworkMigrations
                    .Include(m => m.Controller)
                    .FirstOrDefaultAsync(m => m.Id == migrationId);

                if (migration == null)
                {
                    _logger.LogWarning("Migration {MigrationId} not found for rollback", migrationId);
                    return false;
                }

                if (!migration.CanRollback)
                {
                    _logger.LogWarning("Migration {MigrationId} cannot be rolled back", migrationId);
                    return false;
                }

                if (migration.BackupExpiresUtc.HasValue && migration.BackupExpiresUtc.Value < DateTime.UtcNow)
                {
                    _logger.LogWarning("Backup for migration {MigrationId} has expired", migrationId);
                    return false;
                }

                var controller = migration.Controller;

                // Restore previous configuration
                controller.IpAddress = migration.PreviousIpAddress;
                controller.Port = migration.PreviousPort;
                controller.NetworkType = migration.PreviousNetworkType;
                controller.VpnProfileId = migration.PreviousVpnProfileId;
                controller.BackupIpAddress = null;
                controller.BackupPort = null;
                controller.BackupExpiresUtc = null;

                // Mark migration as rolled back
                migration.Status = "RolledBack";
                migration.Notes += $"\n[Rolled back by {userName} at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC]";
                migration.CanRollback = false;

                await db.SaveChangesAsync();

                _logger.LogInformation(
                    "Migration {MigrationId} rolled back by {UserName}. Controller {ControllerId} restored to {IpAddress}",
                    migrationId, userName, controller.Id, controller.IpAddress);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rollback failed for migration {MigrationId}", migrationId);
                return false;
            }
        }

        public async Task<List<Data.Entities.NetworkMigration>> GetMigrationHistoryAsync(int controllerId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            return await db.NetworkMigrations
                .Where(m => m.ControllerId == controllerId)
                .OrderByDescending(m => m.InitiatedUtc)
                .Take(50)
                .ToListAsync();
        }

        public async Task<WireGuardConfig> GenerateWireGuardConfigAsync(int vpnProfileId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            var vpnProfile = await db.VpnProfiles.FindAsync(vpnProfileId);
            if (vpnProfile == null)
            {
                throw new ArgumentException($"VPN Profile {vpnProfileId} not found");
            }

            // TODO: Implement actual WireGuard key generation
            // For now, return placeholder
            var config = new WireGuardConfig
            {
                InterfacePrivateKey = "PLACEHOLDER_PRIVATE_KEY",
                InterfaceAddress = "10.99.0.2/24",
                PeerPublicKey = "PLACEHOLDER_PUBLIC_KEY",
                PeerEndpoint = "your-server.com:51820",
                PeerAllowedIPs = "10.99.0.0/24",
                FullConfigText = @"[Interface]
PrivateKey = PLACEHOLDER_PRIVATE_KEY
Address = 10.99.0.2/24

[Peer]
PublicKey = PLACEHOLDER_PUBLIC_KEY
Endpoint = your-server.com:51820
AllowedIPs = 10.99.0.0/24
PersistentKeepalive = 25"
            };

            return config;
        }

        public async Task<bool> HasActiveBackupAsync(int controllerId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            var controller = await db.Controllers.FindAsync(controllerId);
            if (controller == null) return false;

            return !string.IsNullOrEmpty(controller.BackupIpAddress) &&
                   controller.BackupExpiresUtc.HasValue &&
                   controller.BackupExpiresUtc.Value > DateTime.UtcNow;
        }

        public async Task CleanupExpiredBackupsAsync()
        {
            try
            {
                using var db = await _dbFactory.CreateDbContextAsync();

                var expiredControllers = await db.Controllers
                    .Where(c => c.BackupExpiresUtc.HasValue && c.BackupExpiresUtc.Value < DateTime.UtcNow)
                    .ToListAsync();

                foreach (var controller in expiredControllers)
                {
                    controller.BackupIpAddress = null;
                    controller.BackupPort = null;
                    controller.BackupExpiresUtc = null;
                }

                var expiredMigrations = await db.NetworkMigrations
                    .Where(m => m.CanRollback && m.BackupExpiresUtc.HasValue && m.BackupExpiresUtc.Value < DateTime.UtcNow)
                    .ToListAsync();

                foreach (var migration in expiredMigrations)
                {
                    migration.CanRollback = false;
                    migration.Notes += $"\n[Backup expired at {migration.BackupExpiresUtc:yyyy-MM-dd HH:mm:ss} UTC]";
                }

                await db.SaveChangesAsync();

                _logger.LogInformation(
                    "Cleaned up {ControllerCount} expired controller backups and {MigrationCount} expired migration backups",
                    expiredControllers.Count, expiredMigrations.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to cleanup expired backups");
            }
        }

        private string DetermineMigrationType(string fromType, string toType)
        {
            if (fromType == toType)
            {
                return "IP_CHANGE";
            }
            else if (fromType == "LAN" && toType == "VPN")
            {
                return "LAN_TO_VPN";
            }
            else if (fromType == "VPN" && toType == "LAN")
            {
                return "VPN_TO_LAN";
            }
            else
            {
                return $"{fromType}_TO_{toType}";
            }
        }
    }
}
