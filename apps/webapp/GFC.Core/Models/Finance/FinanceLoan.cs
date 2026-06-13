using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models.Finance
{
    [Table("FinanceLoans")]
    public class FinanceLoan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string LenderName { get; set; } = string.Empty;

        [Required]
        public DateTime OriginDate { get; set; } = DateTime.Today;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalBalance { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalance { get; set; }

        public string? Notes { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<FinancePayment> Payments { get; set; } = new List<FinancePayment>();
    }
}
