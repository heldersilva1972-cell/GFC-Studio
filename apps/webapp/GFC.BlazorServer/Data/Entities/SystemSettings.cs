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
}

