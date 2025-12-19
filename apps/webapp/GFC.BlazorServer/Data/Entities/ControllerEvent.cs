using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ControllerEvents")]
public class ControllerEvent
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Controller))]
    public int ControllerId { get; set; }

    public ControllerDevice Controller { get; set; } = null!;

    [ForeignKey(nameof(Door))]
    public int? DoorId { get; set; }

    public Door? Door { get; set; }

    public DateTime TimestampUtc { get; set; }

    public long? CardNumber { get; set; }

    public int EventType { get; set; }

    public int? ReasonCode { get; set; }

    public bool IsByCard { get; set; }

    public bool IsByButton { get; set; }

    public int RawIndex { get; set; }

    [MaxLength(4000)]
    public string? RawData { get; set; }

    public bool IsSimulated { get; set; } = false;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}

