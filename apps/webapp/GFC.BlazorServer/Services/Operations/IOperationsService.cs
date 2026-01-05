using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GFC.BlazorServer.Services.Operations
{
    public interface IOperationsService
    {
        Task<OperationsHealthInfo> GetHealthInfoAsync();
        Task<PublicAccessInfo> GetPublicAccessInfoAsync();
        Task<HostingInfo> GetHostingInfoAsync();
        Task<DatabaseRecoveryInfo> GetDatabaseRecoveryInfoAsync();
        Task<NetworkSecurityInfo> GetNetworkSecurityInfoAsync();
        Task<byte[]> GenerateRecoveryPackAsync();
    }

    public class OperationsHealthInfo
    {
        public bool IsHealthy { get; set; }
        public string AppVersion { get; set; }
        public DateTime BuildDate { get; set; }
        public string EnvironmentName { get; set; }
        public bool DatabaseConnected { get; set; }
        public string DiskSpaceMessage { get; set; }
        public bool IsReverseProxyDetected { get; set; }
        public bool IsHttps { get; set; }
        public bool CloudflaredRunning { get; set; }
    }

    public class PublicAccessInfo
    {
        public List<string> Domains { get; set; } = new();
        public string TunnelId { get; set; }
        public string CloudflaredVersion { get; set; }
        public bool IsCloudflaredInstalled { get; set; }
        public bool IsCloudflaredRunning { get; set; }
        public string ConfigPath { get; set; }
        public bool CanResolvePublicDns { get; set; }
    }

    public class HostingInfo
    {
        public string IisSiteName { get; set; }
        public string PhysicalPath { get; set; }
        public string Bindings { get; set; }
        public string AppPoolName { get; set; }
        public string DotNetHostingBundleVersion { get; set; }
    }

    public class DatabaseRecoveryInfo
    {
        public string ServerVersion { get; set; }
        public string InstanceName { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionStringMasked { get; set; }
        public DateTime? LastBackupTime { get; set; }
        public string BackupLocation { get; set; }
    }

    public class NetworkSecurityInfo
    {
        public List<string> OutboundPorts { get; set; } = new();
        public List<string> FirewallRules { get; set; } = new();
        public string LocalDnsResolver { get; set; }
        public bool TimeSyncStatus { get; set; }
    }
}
