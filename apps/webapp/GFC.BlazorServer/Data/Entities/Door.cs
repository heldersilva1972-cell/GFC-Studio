using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("Doors")]
public class Door
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Controller))]
    public int ControllerId { get; set; }

    public ControllerDevice? Controller { get; set; }

    public int DoorIndex { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; } = true;

    [MaxLength(2000)]
    public string? Notes { get; set; }
}

