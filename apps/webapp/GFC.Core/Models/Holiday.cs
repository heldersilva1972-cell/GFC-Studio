using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

[Table("Holidays")]
public class Holiday
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public DateOnly Date { get; set; }

    public bool IsRecurring { get; set; } = false;
}
