using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("KeyHistory")]
public class KeyHistory
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MemberId { get; set; }

    [Required]
    public long CardNumber { get; set; }

    [Required, MaxLength(50)]
    public string Action { get; set; } = string.Empty; // Assigned, Revoked

    [Required]
    public DateTime Date { get; set; }

    [MaxLength(500)]
    public string? Reason { get; set; }

    [MaxLength(255)]
    public string? PerformedBy { get; set; }

    [MaxLength(50)]
    public string? KeyType { get; set; } // Card, Fob
}
