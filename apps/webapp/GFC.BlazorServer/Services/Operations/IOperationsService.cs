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
        Task<List<DiagnosticEntry>> RunDiagnosticsAsync();
        Task<IEnumerable<DriveDescriptor>> GetAvailableDrivesAsync();
        Task<bool> TriggerSystemImageAsync(string targetDriveLetter);
        Task<(bool Success, int RecordsProcessed, string Message)> ArchiveModulesAsync(ArchiveOptions options);
    }

    public class ArchiveOptions
    {
        public bool ArchiveDues { get; set; }
        public bool ArchiveLottery { get; set; }
        public bool ArchiveBarSales { get; set; }
        public bool ArchiveAuditLogs { get; set; }
        public bool ArchiveMemberHistory { get; set; }
        public DateTime ArchiveFromDate { get; set; } = DateTime.Today.AddYears(-5);
        public DateTime ArchiveToDate { get; set; } = DateTime.Today.AddYears(-1);
        public bool CreateExcelSnapshot { get; set; } = true;
    }

    public class DriveDescriptor
    {
        public string DriveLetter { get; set; }
        public string Label { get; set; }
        public double FreeSpaceGb { get; set; }
        public double TotalSpaceGb { get; set; }
        public bool IsSystem { get; set; }
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
        public long MemoryUsageBytes { get; set; }
        public double CpuUsagePercent { get; set; }
        public long FreeSpaceBytes { get; set; }
        public long TotalSizeBytes { get; set; }
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
        public string OSVersion { get; set; }
    }

    public class DatabaseRecoveryInfo
    {
        public string ServerVersion { get; set; }
        public string InstanceName { get; set; }
        public string DatabaseName { get; set; }
        public string ConnectionStringMasked { get; set; }
        public DateTime? LastBackupTime { get; set; }
        public string BackupLocation { get; set; }
        public double SizeMb { get; set; }
        public double UsedMb { get; set; }
    }

    public class DiagnosticEntry
    {
        public DateTime Timestamp { get; set; }
        public string Component { get; set; }
        public string Status { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public long DurationMs { get; set; }
    }

    public class NetworkSecurityInfo
    {
        public List<string> OutboundPorts { get; set; } = new();
        public List<string> FirewallRules { get; set; } = new();
        public string LocalDnsResolver { get; set; }
        public bool TimeSyncStatus { get; set; }
        public string TrustProfile { get; set; }
        public int AuthorizedIpWhitelistCount { get; set; }
        public bool IsSslActive { get; set; }
    }
}
