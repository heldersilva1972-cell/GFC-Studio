using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

/// <summary>
/// Represents a long-running database operation (backup, restore, migration).
/// Used for operation locking and progress tracking.
/// </summary>
[Table("DatabaseOperations")]
public class DatabaseOperation
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Type of operation: "Backup", "Restore", "Migration"
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string OperationType { get; set; } = string.Empty;

    /// <summary>
    /// Current status: "Running", "Completed", "Failed", "Cancelled"
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Running";

    /// <summary>
    /// When the operation started (UTC).
    /// </summary>
    public DateTime StartedAtUtc { get; set; }

    /// <summary>
    /// When the operation completed (UTC), null if still running.
    /// </summary>
    public DateTime? CompletedAtUtc { get; set; }

    /// <summary>
    /// User ID who started the operation.
    /// </summary>
    public int StartedByUserId { get; set; }

    /// <summary>
    /// Error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Progress log with timestamped entries.
    /// </summary>
    public string? ProgressLog { get; set; }

    /// <summary>
    /// Related backup ID if this is a backup or restore operation.
    /// </summary>
    public int? RelatedBackupId { get; set; }

    /// <summary>
    /// Duration in seconds (calculated).
    /// </summary>
    [NotMapped]
    public int? DurationSeconds
    {
        get
        {
            if (CompletedAtUtc.HasValue)
            {
                return (int)(CompletedAtUtc.Value - StartedAtUtc).TotalSeconds;
            }
            return null;
        }
    }

    // Navigation property
    [ForeignKey(nameof(StartedByUserId))]
    public virtual AppUser? StartedBy { get; set; }
}
