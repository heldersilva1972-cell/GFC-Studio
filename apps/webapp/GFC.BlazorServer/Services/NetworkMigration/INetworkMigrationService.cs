using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.NetworkMigration
{
    /// <summary>
    /// Service for managing controller network migrations (LAN ↔ VPN)
    /// </summary>
    public interface INetworkMigrationService
    {
        /// <summary>
        /// Detect current network configuration for a controller
        /// </summary>
        Task<NetworkConfiguration> DetectCurrentConfigAsync(int controllerId);

        /// <summary>
        /// Validate a migration request before execution
        /// </summary>
        Task<ValidationResult> ValidateNewConfigAsync(NetworkMigrationRequest request);

        /// <summary>
        /// Test VPN connection to target IP/port
        /// </summary>
        Task<ConnectionTestResult> TestConnectionAsync(string ipAddress, int port, int? vpnProfileId = null);

        /// <summary>
        /// Execute network migration with automatic backup
        /// </summary>
        Task<MigrationResult> ExecuteMigrationAsync(NetworkMigrationRequest request);

        /// <summary>
        /// Rollback to previous configuration
        /// </summary>
        Task<bool> RollbackMigrationAsync(int migrationId, string userName);

        /// <summary>
        /// Get migration history for a controller
        /// </summary>
        Task<List<Data.Entities.NetworkMigration>> GetMigrationHistoryAsync(int controllerId);

        /// <summary>
        /// Generate WireGuard configuration for export
        /// </summary>
        Task<WireGuardConfig> GenerateWireGuardConfigAsync(int vpnProfileId);

        /// <summary>
        /// Check if a controller has an active backup configuration
        /// </summary>
        Task<bool> HasActiveBackupAsync(int controllerId);

        /// <summary>
        /// Clean up expired backups
        /// </summary>
        Task CleanupExpiredBackupsAsync();
    }
}
