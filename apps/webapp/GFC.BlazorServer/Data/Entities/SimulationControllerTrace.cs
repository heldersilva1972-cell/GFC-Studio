using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("SimulationControllerTraces")]
public class SimulationControllerTrace
{
    [Key]
    public long Id { get; set; }

    [Required]
    public DateTime TimestampUtc { get; set; }

    public int? UserId { get; set; }

    [Required, MaxLength(100)]
    public string Operation { get; set; } = string.Empty;

    public int? ControllerId { get; set; }

    public int? DoorId { get; set; }

    public long? CardNumber { get; set; }

    public int? MemberId { get; set; }

    [MaxLength(1000)]
    public string? RequestSummary { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? RequestPayloadJson { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? RequestPayloadRaw { get; set; }

    [MaxLength(1000)]
    public string? ExpectedResponseSummary { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? ExpectedResponsePayloadJson { get; set; }

    [Required, MaxLength(50)]
    public string ResultStatus { get; set; } = string.Empty;

    [Column(TypeName = "nvarchar(max)")]
    public string? ResultDetails { get; set; }

    [MaxLength(100)]
    public string? TriggerPage { get; set; }

    public bool IsSimulation { get; set; } = true;
}
