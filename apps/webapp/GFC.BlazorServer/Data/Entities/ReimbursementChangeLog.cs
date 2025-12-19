using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ReimbursementChangeLogs")]
public class ReimbursementChangeLog
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RequestId { get; set; }

    [ForeignKey(nameof(RequestId))]
    public ReimbursementRequest Request { get; set; } = null!;

    [Required]
    public int ChangedByMemberId { get; set; }

    [Required]
    public DateTime ChangeUtc { get; set; }

    [Required, MaxLength(100)]
    public string FieldName { get; set; } = string.Empty;

    [Column(TypeName = "nvarchar(max)")]
    public string? OldValue { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? NewValue { get; set; }
}

