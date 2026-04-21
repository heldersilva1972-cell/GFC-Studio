using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

[Table("TimeProfiles")]
public class TimeProfile
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    // Relational property for EF Core
    public virtual ICollection<TimeProfileInterval> Intervals { get; set; } = new List<TimeProfileInterval>();
}
