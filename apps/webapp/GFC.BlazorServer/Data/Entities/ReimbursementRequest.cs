using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ReimbursementRequests")]
public class ReimbursementRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RequestorMemberId { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public DateTime RequestDate { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved, Rejected, Paid

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? Notes { get; set; }

    [Required]
    public DateTime CreatedUtc { get; set; }

    [Required]
    public DateTime UpdatedUtc { get; set; }

    [Required]
    public bool EditedFlag { get; set; }

    public int? ApprovedByMemberId { get; set; }
    public DateTime? ApprovedDateUtc { get; set; }
    public int? RejectedByMemberId { get; set; }
    public DateTime? RejectedDateUtc { get; set; }
    [Column(TypeName = "nvarchar(max)")]
    public string? RejectReason { get; set; }
    public int? PaidByMemberId { get; set; }
    public DateTime? PaidDateUtc { get; set; }

    // Navigation properties
    public ICollection<ReimbursementItem> Items { get; set; } = new List<ReimbursementItem>();
    public ICollection<ReimbursementChangeLog> ChangeLogs { get; set; } = new List<ReimbursementChangeLog>();
}
