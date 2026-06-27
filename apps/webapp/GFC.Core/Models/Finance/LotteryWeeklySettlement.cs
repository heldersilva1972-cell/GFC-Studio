using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models.Finance;

[Table("LotteryWeeklySettlements")]
public class LotteryWeeklySettlement
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime WeekStartDate { get; set; }

    [Required]
    public DateTime WeekEndDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal NetDueAmount { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Pending";

    public DateTime? SettleDate { get; set; }

    [StringLength(256)]
    public string? SettleBy { get; set; }

    [StringLength(256)]
    public string? ReferenceNumber { get; set; }

    [Required]
    public int RecordedShiftsCount { get; set; } = 0;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal EnvelopeDropAmount { get; set; } = 0;

    public int? LinkedBillId { get; set; }

    [ForeignKey("LinkedBillId")]
    public virtual FinanceBill? LinkedBill { get; set; }
}
