using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models.Finance
{
    [Table("FinanceVendors")]
    public class FinanceVendor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? ContactInfo { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? Phone { get; set; }

        [StringLength(500)]
        public string? Website { get; set; }

        [StringLength(100)]
        public string? AccountNumber { get; set; }

        public int? DefaultCategoryId { get; set; }

        [ForeignKey("DefaultCategoryId")]
        public virtual FinanceCategory? DefaultCategory { get; set; }

        public int Priority { get; set; } = 3; // 1 = High, 2 = Medium, 3 = Low

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public virtual ICollection<FinanceBill> Bills { get; set; } = new List<FinanceBill>();
    }
}
