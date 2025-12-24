using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    /// Canonical flag for controller mode selection.
    /// When true, controller operations use real hardware via the real controller client.
    /// When false, controller operations run exclusively through the simulation controller client without touching hardware.
    /// Default: false (simulation mode) for safety.
    /// </summary>

    /// <summary>
    /// The ID of the controller designated as the system's primary card reader/scanner.
    /// </summary>
    public int? ScannerControllerId { get; set; }

    /// <summary>
    /// Timestamp of last update to these settings.
    /// </summary>
    public DateTime? LastUpdatedUtc { get; set; }

    // [NEW] NVR/Camera Credentials for Auto-Discovery
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

    // Phase 2: Security Settings & Admin Control Panel

    #region Remote Access Configuration
    /// <summary>
    /// Cloudflare Tunnel Token for IP masking and DDoS protection.
    /// This should be encrypted.
    /// </summary>
    public string? CloudflareTunnelToken { get; set; }

    /// <summary>
    /// Public domain used to access the system remotely (e.g., gfc-cameras.yourclub.com).
    /// </summary>
    [StringLength(255)]
    public string? PublicDomain { get; set; }

    /// <summary>
    /// The UDP port for the WireGuard VPN server.
    /// </summary>
    [Range(1024, 65535)]
    public int WireGuardPort { get; set; } = 51820;

    /// <summary>
    /// The internal VPN subnet for remote clients.
    /// </summary>
    [Required]
    [StringLength(50)]
    [RegularExpression(@"^([0-9]{1,3}\.){3}[0-9]{1,3}\/([0-9]|[1-2][0-9]|3[0-2])$", ErrorMessage = "Invalid CIDR notation. Expected format: 10.8.0.0/24")]
    public string WireGuardSubnet { get; set; } = "10.8.0.0/24";
    #endregion

    #region User & Permission Management
    /// <summary>
    /// Global expiry date for all users with the "Director" role.
    /// </summary>
    public DateTime? DirectorAccessExpiryDate { get; set; }
    #endregion

    #region Security "Hardening" Toggles
    /// <summary>
    /// If true, requires an authenticator app code for remote access.
    /// </summary>
    public bool EnableTwoFactorAuth { get; set; } = false;

    /// <summary>
    /// If true, forces remote users to re-authenticate after a period of inactivity.
    /// </summary>
    public bool EnableSessionTimeout { get; set; } = false;

    /// <summary>
    /// The number of minutes of inactivity before a remote user's session times out.
    /// </summary>
    [Range(1, 1440)]
    public int SessionTimeoutMinutes { get; set; } = 30;

    /// <summary>
    /// If true, automatically blocks an IP address after too many incorrect password attempts.
    /// </summary>
    public bool EnableFailedLoginProtection { get; set; } = true;

    /// <summary>
    /// The maximum number of failed login attempts before an IP address is blocked.
    /// </summary>
    [Range(3, 100)]
    public int MaxFailedLoginAttempts { get; set; } = 5;

    /// <summary>
    /// The number of minutes an IP address is blocked for after too many failed login attempts.
    /// </summary>
    [Range(1, 1440)]
    public int LoginLockDurationMinutes { get; set; } = 30;

    /// <summary>
    /// If true, enables IP address filtering (whitelist or blacklist).
    /// </summary>
    public bool EnableIPFiltering { get; set; } = false;

    /// <summary>
    /// The IP filtering mode. Can be "Whitelist" or "Blacklist".
    /// </summary>
    [StringLength(20)]
    public string IPFilterMode { get; set; } = "Blacklist";

    /// <summary>
    /// If true, overlays the user's username and the current timestamp on live video streams.
    /// </summary>
    public bool EnableWatermarking { get; set; } = false;

    /// <summary>
    /// The position of the watermark on the video stream.
    /// </summary>
    [StringLength(20)]
    public string WatermarkPosition { get; set; } = "BottomRight";
    #endregion

    #region System Limits & "Mission Control"
    /// <summary>
    /// The maximum number of simultaneous remote viewers.
    /// </summary>
    [Range(1, 100)]
    public int MaxSimultaneousViewers { get; set; } = 10;

    /// <summary>
    /// The maximum bitrate (in Mbps) for remote (VPN) users.
    /// </summary>
    [Range(1, 100)]
    public int RemoteQualityMaxBitrate { get; set; } = 3;

    /// <summary>
    /// The maximum bitrate (in Mbps) for local (LAN) users. 0 for no limit.
    /// </summary>
    [Range(0, 100)]
    public int LocalQualityMaxBitrate { get; set; } = 0;
    #endregion

    #region Monitoring & Alerts
    /// <summary>
    /// If true, an alert is generated if a user connects from an unusual location.
    /// </summary>
    public bool EnableGeofencing { get; set; } = false;

    /// <summary>
    /// If true, users are notified if their connection quality is poor.
    /// </summary>
    public bool EnableConnectionQualityAlerts { get; set; } = true;

    /// <summary>
    /// The minimum bandwidth in Mbps required for a stable connection.
    /// </summary>
    [Column(TypeName = "decimal(5, 2)")]
    public decimal MinimumBandwidthMbps { get; set; } = 1.0m;
    #endregion
}

