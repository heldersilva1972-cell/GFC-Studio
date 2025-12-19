using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ReceiptFiles")]
public class ReceiptFile
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RequestItemId { get; set; }

    [ForeignKey(nameof(RequestItemId))]
    public ReimbursementItem RequestItem { get; set; } = null!;

    [Required]
    [MaxLength(200)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string RelativePath { get; set; } = string.Empty;

    [Required]
    public DateTime UploadedUtc { get; set; }

    [Required]
    public int UploadedByMemberId { get; set; }
}
