using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

[Table("SpecialEvents")]
public class SpecialEvent
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateOnly Date { get; set; }

    public bool IsRecurring { get; set; } = false;

    [ForeignKey(nameof(TimeProfile))]
    public int TimeProfileId { get; set; }
    public TimeProfile? TimeProfile { get; set; }
}
