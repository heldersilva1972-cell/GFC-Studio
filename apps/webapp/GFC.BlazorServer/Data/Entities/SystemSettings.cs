// [MODIFIED]
using System.ComponentModel;
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

    // Cloudflare & WireGuard Remote Access Settings (Phase 1)
    public string? LanSubnet { get; set; } = "192.168.1.0/24";
    public string? WireGuardServerPublicKey { get; set; }
    public string? CloudflareTunnelToken { get; set; }
    public string? PublicDomain { get; set; }
    [DefaultValue(51820)]
    public int WireGuardPort { get; set; } = 51820;
    [DefaultValue("10.8.0.0/24")]
    public string WireGuardSubnet { get; set; } = "10.8.0.0/24";
}
