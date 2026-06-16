using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        public int MinimumOrderQuantity { get; set; } = 1;

        public int PackSize { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        public decimal RetailPrice { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PourSize { get; set; } = 1.5m;

        public bool IsUnitBased { get; set; } = false; // Sold by unit (Can, Bottle, Pack) vs By Ounce (Liquor)

        public int InventoryTrackType { get; set; } = 0; // 0 = Inherit Category, 1 = None, 2 = Liquor, 3 = Food/General

        public bool IsActive { get; set; } = true;
        public bool IsBeer { get; set; } = false;
        public bool ShowInPos { get; set; } = false;
        public bool AllowLooseReconciliation { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;

        public int? ParentItemId { get; set; }
        [ForeignKey("ParentItemId")]
        public virtual LiquorItem? ParentItem { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PourVolumeOunces { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BottleVolumeOunces { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OuncesAccumulator { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public virtual ICollection<LiquorOrderItem> OrderHistory { get; set; } = new List<LiquorOrderItem>();

        [NotMapped]
        public string StockSummary
        {
            get
            {
                if (PackSize > 1)
                {
                    int cases = CurrentStock / PackSize;
                    int bottles = CurrentStock % PackSize;
                    string casesLabel = cases == 1 ? "case" : "cases";
                    string bottlesLabel = bottles == 1 ? "bottle" : "bottles";
                    return $"{cases} {casesLabel}, {bottles} {bottlesLabel}";
                }
                else
                {
                    string unitsLabel = CurrentStock == 1 ? "unit" : "units";
                    return $"{CurrentStock} {unitsLabel}";
                }
            }
        }
    }
}

