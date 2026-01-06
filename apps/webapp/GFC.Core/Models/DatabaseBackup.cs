using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

/// <summary>
/// Represents a database backup file tracked in the system.
/// </summary>
[Table("DatabaseBackups")]
public class DatabaseBackup
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The filename of the backup (without path).
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Full file path where the backup is stored.
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Size of the backup file in bytes.
    /// </summary>
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// When the backup was created (UTC).
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// User ID who created the backup.
    /// </summary>
    public int CreatedByUserId { get; set; }

    /// <summary>
    /// Type of backup: "Manual", "PreRestore", "Scheduled", "Auto"
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string BackupType { get; set; } = "Manual";

    /// <summary>
    /// SHA256 hash of the backup file for integrity verification.
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string FileHash { get; set; } = string.Empty;

    /// <summary>
    /// Soft delete flag - backup record kept but file may be deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Optional notes about this backup.
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    // Navigation property
    [ForeignKey(nameof(CreatedByUserId))]
    public virtual AppUser? CreatedBy { get; set; }
}
