using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("TimeProfileIntervals")]
public class TimeProfileInterval
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(TimeProfile))]
    public int TimeProfileId { get; set; }

    public TimeProfile? TimeProfile { get; set; }

    [Required]
    public int DayOfWeek { get; set; } // 0=Sunday, 1=Monday, ..., 6=Saturday

    [Required]
    [Column(TypeName = "time")]
    public TimeOnly StartTime { get; set; }

    [Required]
    [Column(TypeName = "time")]
    public TimeOnly EndTime { get; set; }

    public int Order { get; set; }
}

