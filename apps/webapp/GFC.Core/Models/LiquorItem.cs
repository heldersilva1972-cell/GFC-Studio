using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class LiquorItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? UpcCode { get; set; }

        [StringLength(50)]
        public string? BottleSize { get; set; } // e.g. "750ml", "1.75L"

        [StringLength(100)]
        public string? Category { get; set; }

        public string? ImageUrl { get; set; }

        public int? VendorId { get; set; }

        [ForeignKey("VendorId")]
        public virtual LiquorVendor? Vendor { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentPrice { get; set; } = 0;

        [Required]
        public int CurrentStock { get; set; }

        [Required]
        public int MinStockLimit { get; set; } = 2;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<LiquorOrderItem> OrderHistory { get; set; } = new List<LiquorOrderItem>();
    }
}

