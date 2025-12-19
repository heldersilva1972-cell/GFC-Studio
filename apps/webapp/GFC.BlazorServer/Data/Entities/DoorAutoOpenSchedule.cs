using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("DoorAutoOpenSchedules")]
public class DoorAutoOpenSchedule
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Door))]
    public int DoorId { get; set; }
    public Door? Door { get; set; }

    [ForeignKey(nameof(TimeProfile))]
    public int? TimeProfileId { get; set; }
    public TimeProfile? TimeProfile { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(256)]
    public string? Description { get; set; }

    public int? ControllerProfileIndex { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedUtc { get; set; }
}

