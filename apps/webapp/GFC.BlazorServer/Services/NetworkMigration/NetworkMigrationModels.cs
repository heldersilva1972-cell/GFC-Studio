using System;
using System.Collections.Generic;

namespace GFC.BlazorServer.Services.NetworkMigration
{
    /// <summary>
    /// Request model for network migration
    /// </summary>
    public class NetworkMigrationRequest
    {
        public int ControllerId { get; set; }
        public string TargetNetworkType { get; set; } // "LAN", "VPN", "Remote"
        public string NewIpAddress { get; set; }
        public int NewPort { get; set; }
        public int? VpnProfileId { get; set; }
        public bool SkipConnectionTest { get; set; }
        public string InitiatedByUser { get; set; }
        public string Notes { get; set; }
    }

    /// <summary>
    /// Current network configuration of a controller
    /// </summary>
    public class NetworkConfiguration
    {
        public int ControllerId { get; set; }
        public string ControllerName { get; set; }
        public string NetworkType { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public int? VpnProfileId { get; set; }
        public string VpnProfileName { get; set; }
        public bool IsConnected { get; set; }
        public DateTime? LastSeenUtc { get; set; }
        public bool HasBackup { get; set; }
        public DateTime? BackupExpiresUtc { get; set; }
    }

    /// <summary>
    /// Result of connection test
    /// </summary>
    public class ConnectionTestResult
    {
        public bool Success { get; set; }
        public List<TestStep> Steps { get; set; } = new();
        public string ErrorMessage { get; set; }
        public long TotalDurationMs { get; set; }
        public List<string> Warnings { get; set; } = new();
    }

    public class TestStep
    {
        public string Name { get; set; }
        public string Status { get; set; } // "Success", "Failed", "Skipped", "InProgress"
        public string Message { get; set; }
        public long DurationMs { get; set; }
    }

    /// <summary>
    /// Result of migration operation
    /// </summary>
    public class MigrationResult
    {
        public bool Success { get; set; }
        public int MigrationId { get; set; }
        public string ErrorMessage { get; set; }
        public NetworkConfiguration NewConfiguration { get; set; }
        public NetworkConfiguration BackupConfiguration { get; set; }
        public DateTime? BackupExpiresUtc { get; set; }
        public bool CanRollback { get; set; }
        public List<string> Warnings { get; set; } = new();
    }

    /// <summary>
    /// Validation result for migration request
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }

    /// <summary>
    /// WireGuard configuration for export
    /// </summary>
    public class WireGuardConfig
    {
        public string InterfacePrivateKey { get; set; }
        public string InterfaceAddress { get; set; }
        public string PeerPublicKey { get; set; }
        public string PeerEndpoint { get; set; }
        public string PeerAllowedIPs { get; set; }
        public string FullConfigText { get; set; }
        public string QrCodeBase64 { get; set; }
    }
}
