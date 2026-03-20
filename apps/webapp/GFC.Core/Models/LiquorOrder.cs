using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("LiquorOrders")]
    public class LiquorOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int VendorId { get; set; }

        [ForeignKey("VendorId")]
        public virtual LiquorVendor? Vendor { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Placed"; // Placed, Received

        [Column(TypeName = "decimal(18,2)")]
        public decimal ItemsTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionalCosts { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; } // ItemsTotal + TaxAmount + AdditionalCosts

        [StringLength(100)]
        public string? InvoiceNumber { get; set; }

        public bool IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser? User { get; set; }

        public virtual ICollection<LiquorOrderItem> OrderItems { get; set; } = new List<LiquorOrderItem>();
    }
}
