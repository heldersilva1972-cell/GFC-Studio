using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

/// <summary>
/// Represents a member's access privilege to a specific door.
/// </summary>
[Table("MemberDoorAccess")]
public class MemberDoorAccess
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MemberId { get; set; }

    [Required, MaxLength(50)]
    public string CardNumber { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(Door))]
    public int DoorId { get; set; }

    public Door? Door { get; set; }

    [ForeignKey(nameof(TimeProfile))]
    public int? TimeProfileId { get; set; }

    public TimeProfile? TimeProfile { get; set; }

    public bool IsEnabled { get; set; } = true;

    public DateTime? LastSyncedAt { get; set; }

    [MaxLength(500)]
    public string? LastSyncResult { get; set; }
}
