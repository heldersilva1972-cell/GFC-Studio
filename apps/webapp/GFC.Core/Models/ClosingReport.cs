using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

[Table("ClosingReports")]
public class ClosingReport
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual AppUser? User { get; set; }

    [Required]
    [StringLength(50)]
    public string ShiftType { get; set; } = "Day"; // 'Day', 'Night'

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal BarSales { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal LotterySales { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalSales { get; set; }

    public string? Notes { get; set; }

    [Required]
    public DateTime ReportDate { get; set; } = DateTime.Today;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public int? LastUpdatedByUserId { get; set; }

    public bool IsCorrected { get; set; }

    public string? CorrectionReason { get; set; }
}
