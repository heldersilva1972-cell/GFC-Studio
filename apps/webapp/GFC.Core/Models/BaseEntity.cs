using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models;

/// <summary>
/// Foundation for all "Gold Standard" database tables.
/// Provides globally unique identification, soft-delete safety, and audit timestamps.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// A globally unique fingerprint for this record. 
    /// Used for future-proofing against ID clashes during database archives or merges.
    /// </summary>
    public Guid GlobalId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Soft-delete bit. If true, the record is logically removed but remains in 
    /// the database for audit and tax compliance.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// The exact timestamp when this record was originally committed to the database.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The timestamp of the last modification to this record. 
    /// Updated automatically by the DbContext.
    /// </summary>
    public DateTime? ModifiedAt { get; set; }

    /// <summary>
    /// Audit field for the identity of the user who created this record.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Audit field for the identity of the user who last modified this record.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Preparation for mobile/tablet offline sync.
    /// </summary>
    public DateTime? SyncDate { get; set; }

    /// <summary>
    /// Safety Lock (Optimistic Concurrency). 
    /// Prevents data loss if two people edit the same record at once.
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
