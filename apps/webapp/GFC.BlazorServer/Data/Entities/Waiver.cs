using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("Waivers")]
public class Waiver
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MemberId { get; set; }

    [Required]
    public int Year { get; set; }

    [Required, MaxLength(100)]
    public string Reason { get; set; } = string.Empty; // e.g., Director, Life, BoardDecision

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
