using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ControllerBehaviorOptions")]
public class ControllerBehaviorOptions
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Controller))]
    public int ControllerId { get; set; }
    public ControllerDevice? Controller { get; set; }

    public int ValidSwipeGapSeconds { get; set; } = 3;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedUtc { get; set; }
}

