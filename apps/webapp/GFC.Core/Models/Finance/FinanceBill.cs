using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GFC.Core.Models.Finance
{
    [Table("FinanceBills")]
    public class FinanceBill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int VendorId { get; set; }

        [ForeignKey("VendorId")]
        public virtual FinanceVendor? Vendor { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual FinanceCategory? Category { get; set; }

        public int? LoanId { get; set; }

        [ForeignKey("LoanId")]
        public virtual FinanceLoan? Loan { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalAmount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? InvoiceDate { get; set; }

        public int WarningDays { get; set; } = 7;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = BillStatus.Pending.ToString();

        public bool IsRecurring { get; set; }

        [StringLength(50)]
        public string? RecurringFrequency { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<FinancePayment> Payments { get; set; } = new List<FinancePayment>();

        [NotMapped]
        public decimal TotalPaid => Payments.Sum(p => p.AmountPaid);

        [NotMapped]
        public decimal BalanceRemaining => OriginalAmount - TotalPaid;

        [NotMapped]
        public bool IsOverdue => Status != BillStatus.Paid.ToString() && DueDate.Date < DateTime.Today;

        [NotMapped]
        public bool IsInWarningWindow => Status != BillStatus.Paid.ToString() && !IsOverdue && (DueDate.Date - DateTime.Today).TotalDays <= WarningDays;
    }
}
