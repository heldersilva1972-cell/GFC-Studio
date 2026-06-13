using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models.Finance
{
    [Table("FinancePayments")]
    public class FinancePayment
    {
        [Key]
        public int Id { get; set; }

        public int? BillId { get; set; }

        [ForeignKey("BillId")]
        public virtual FinanceBill? Bill { get; set; }

        public int? LoanId { get; set; }

        [ForeignKey("LoanId")]
        public virtual FinanceLoan? Loan { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? PaymentMethod { get; set; }

        public string? Note { get; set; }

        public int? ProcessedBy { get; set; }
    }
}
