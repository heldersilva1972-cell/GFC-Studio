using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

/// <summary>
/// Stores network configuration and security settings for a controller.
/// </summary>
[Table("ControllerNetworkConfigs")]
public class ControllerNetworkConfig
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Controller))]
    public int ControllerId { get; set; }

    public ControllerDevice? Controller { get; set; }

    [MaxLength(45)] // IPv6 max length
    public string? IpAddress { get; set; }

    [MaxLength(45)]
    public string? SubnetMask { get; set; }

    [MaxLength(45)]
    public string? Gateway { get; set; }

    [Range(1, 65535)]
    public int? Port { get; set; }

    public bool DhcpEnabled { get; set; }

    [MaxLength(45)]
    public string? AllowedPcIp { get; set; }

    /// <summary>
    /// Masked representation of the communication password (never store actual password).
    /// Format: "****" or similar to indicate password is set.
    /// </summary>
    [MaxLength(50)]
    public string? CommPasswordMasked { get; set; }

    /// <summary>
    /// Timestamp when configuration was last read from the controller.
    /// </summary>
    public DateTime? LastReadUtc { get; set; }

    /// <summary>
    /// Timestamp when configuration was last synced to the controller.
    /// </summary>
    public DateTime? LastSyncUtc { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedUtc { get; set; }
}
