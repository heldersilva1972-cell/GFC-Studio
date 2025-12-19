using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ControllerTimeProfileLinks")]
public class ControllerTimeProfileLink
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Controller))]
    public int ControllerId { get; set; }

    [ForeignKey(nameof(TimeProfile))]
    public int TimeProfileId { get; set; }

    public ControllerDevice? Controller { get; set; }

    public TimeProfile? TimeProfile { get; set; }

    [Required]
    public int ControllerProfileIndex { get; set; }

    public bool IsEnabled { get; set; } = true;
}

