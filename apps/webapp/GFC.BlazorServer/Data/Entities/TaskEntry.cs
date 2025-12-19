using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("TaskEntries")]
public class TaskEntry
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Controller))]
    public int ControllerId { get; set; }

    public ControllerDevice? Controller { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = "";

    [Required]
    public int Type { get; set; }

    [Required]
    [Column(TypeName = "time")]
    public TimeOnly Time { get; set; }

    public int? Door { get; set; }

    public bool Enabled { get; set; } = true;
}
