using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.BlazorServer.Data.Entities;

[Table("ReimbursementItems")]
public class ReimbursementItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RequestId { get; set; }

    [ForeignKey(nameof(RequestId))]
    public ReimbursementRequest Request { get; set; } = null!;

    [Required]
    [Column(TypeName = "date")]
    public DateTime ExpenseDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public ReimbursementCategory Category { get; set; } = null!;

    [MaxLength(200)]
    public string? Vendor { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? Notes { get; set; }

    // Navigation properties
    public ICollection<ReceiptFile> ReceiptFiles { get; set; } = new List<ReceiptFile>();
}
