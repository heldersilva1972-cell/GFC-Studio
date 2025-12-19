using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("DoorBehaviorOptions")]
public class DoorBehaviorOptions
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Door))]
    public int DoorId { get; set; }
    public Door? Door { get; set; }

    public bool FirstCardOpenEnabled { get; set; }

    public bool DoorAsSwitchEnabled { get; set; }

    public bool OpenTooLongWarnEnabled { get; set; }

    public bool Invalid3CardsWarnEnabled { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedUtc { get; set; }
}

