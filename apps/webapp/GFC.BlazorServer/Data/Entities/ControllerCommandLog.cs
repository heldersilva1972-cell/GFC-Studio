using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ControllerCommandLogs")]
public class ControllerCommandLog
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Controller))]
    public int ControllerId { get; set; }
    public ControllerDevice? Controller { get; set; }

    [Required, MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    public bool Success { get; set; }

    [MaxLength(2000)]
    public string? Error { get; set; }

    public int? LatencyMs { get; set; }

    public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
}

