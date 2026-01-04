// [MODIFIED]
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GFC.Core.Enums;

namespace GFC.BlazorServer.Data.Entities;

/// <summary>
/// System-wide settings. Only one row should exist (Id = 1).
/// </summary>
[Table("SystemSettings")]
public class SystemSettings
{
    /// <summary>
    /// Primary key. Always 1.
    /// </summary>
    [Key]
    public int Id { get; set; } = 1;

    /// <summary>
    /// The ID of the controller designated as the system's primary card reader/scanner.
    /// </summary>
    public int? ScannerControllerId { get; set; }

    /// <summary>
    /// Timestamp of last update to these settings.
    /// </summary>
    public DateTime? LastUpdatedUtc { get; set; }

    /// <summary>
    /// NVR IP Address for camera auto-discovery.
    /// </summary>
    public string? NvrIpAddress { get; set; }

    /// <summary>
    /// NVR HTTP port (default: 8000).
    /// </summary>
    public int? NvrPort { get; set; }

    /// <summary>
    /// NVR/Camera username for authentication.
    /// </summary>
    public string? NvrUsername { get; set; }

    /// <summary>
    /// NVR/Camera password for authentication. Should be encrypted in production.
    /// </summary>
    public string? NvrPassword { get; set; }

    // Phase 5: VPN and Remote Access Security
    public string? CloudflareTunnelToken { get; set; } // Encrypted
    public string? PrimaryDomain { get; set; } // e.g., gfc-cameras.yourclub.com
    public string? AllowedDomains { get; set; } // Comma-separated list of allowed domains
    public string? DomainSwitchPending { get; set; }
    public DateTime? DomainSwitchExpiryUtc { get; set; }
    public string? LastConfirmedDomain { get; set; }
    public int WireGuardPort { get; set; } = 51820;
    public string WireGuardSubnet { get; set; } = "10.20.0.0/24";
    public string? WireGuardServerPublicKey { get; set; }
    public string WireGuardAllowedIPs { get; set; } = "10.20.0.0/24";
    public int MaxSimultaneousViewers { get; set; } = 10;
    
    // Security Settings
    public bool EnableTwoFactorAuth { get; set; } = false;
    public bool EnableIPFiltering { get; set; } = false;
    public int MinimumBandwidthMbps { get; set; } = 5;
    public bool EnableSessionTimeout { get; set; } = true;
    public int SessionTimeoutMinutes { get; set; } = 30;
    public int IdleTimeoutMinutes { get; set; } = 20;
    public int AbsoluteSessionMaxMinutes { get; set; } = 1440;
    public bool EnableFailedLoginProtection { get; set; } = true;
    public int MaxFailedLoginAttempts { get; set; } = 5;
    public string IPFilterMode { get; set; } = "Whitelist"; // "Whitelist" or "Blacklist"
    public int LoginLockDurationMinutes { get; set; } = 30;
    public string WatermarkPosition { get; set; } = "BottomRight"; // TopLeft, TopRight, BottomLeft, BottomRight
    public bool EnableWatermarking { get; set; } = false;
    
    // Video Streaming Quality Settings
    public int LocalQualityMaxBitrate { get; set; } = 8000; // kbps
    public int RemoteQualityMaxBitrate { get; set; } = 2000; // kbps
    public bool EnableGeofencing { get; set; } = false;
    public bool EnableConnectionQualityAlerts { get; set; } = true;
    
    // Director Access Control
    public DateTime? DirectorAccessExpiryDate { get; set; }

    // Cloudflare & WireGuard Remote Access Settings (Phase 1)
    public string? LanSubnet { get; set; } = "192.168.1.0/24";

    // Hosting & Security Framework (Phase 2)
    [StringLength(20)]
    public string HostingEnvironment { get; set; } = "Dev"; // Dev, Staging, Prod

    [Range(1, 365)]
    public int TrustedDeviceDurationDays { get; set; } = 30;

    public bool MagicLinkEnabled { get; set; } = true;

    public bool EnforceVpn { get; set; } = false;
    public GFC.Core.Enums.AccessMode AccessMode { get; set; } = GFC.Core.Enums.AccessMode.Open;
    public bool EnableOnboarding { get; set; } = false;
    public bool SafeModeEnabled { get; set; } = false;

    // Backup & Data Protection Tracking
    public string BackupMethod { get; set; } = "External USB";
    public DateTime? LastSuccessfulBackupUtc { get; set; }
    public DateTime? LastRestoreTestUtc { get; set; }
    public int BackupFrequencyHours { get; set; } = 24;
    
}
