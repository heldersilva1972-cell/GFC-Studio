using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("NPQueueEntries")]
public class NPQueueEntry
{
    [Key]
    public int Id { get; set; }

    public int? MemberId { get; set; } // nullable for placeholder - FK to Members table (not EF entity)

    [Required]
    public int QueuePosition { get; set; }

    [Required]
    public DateTime AddedDate { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Active"; // Active, Promoted, Removed
}
