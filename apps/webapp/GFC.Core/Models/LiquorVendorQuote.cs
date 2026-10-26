using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GFC.Core.Models
{
    [Table("LiquorVendorQuotes")]
    public class LiquorVendorQuote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LiquorItemId { get; set; }

        [ForeignKey("LiquorItemId")]
        [JsonIgnore]
        public virtual LiquorItem? LiquorItem { get; set; }

        [Required]
        public int VendorId { get; set; }

        [ForeignKey("VendorId")]
        public virtual LiquorVendor? Vendor { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BottlePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CasePrice { get; set; }

        public bool IsCaseOnly { get; set; } = false;

        public int PackSize { get; set; } = 1;

        public bool IsPreferredVendor { get; set; } = false;

        // Deal / Promotion tracking
        public bool HasDeal { get; set; } = false;

        [StringLength(50)]
        public string? DealType { get; set; } // 'TieredQty', 'BuyXGetY', 'PostOffFlat', 'PercentOff', 'FamilyMix'

        [StringLength(255)]
        public string? DealDescription { get; set; }

        public int? DealMinQty { get; set; } // e.g. Buy 3 cases

        public int? DealFreeQty { get; set; } // e.g. Get 1 free

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DealPrice { get; set; } // Discounted case/bottle price under the deal

        public DateTime? DealStartDate { get; set; }

        public DateTime? DealEndDate { get; set; }

        // Surcharges & Deposits
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SplitCaseFee { get; set; }

        public bool IncludesDeposit { get; set; } = false;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DepositAmount { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime? LastPriceChangeDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Convenience Helpers
        [NotMapped]
        public bool IsDealActive
        {
            get
            {
                if (!HasDeal) return false;
                var now = DateTime.UtcNow;
                if (DealStartDate.HasValue && now < DealStartDate.Value) return false;
                if (DealEndDate.HasValue && now > DealEndDate.Value) return false;
                return true;
            }
        }

        [NotMapped]
        public decimal EffectiveCasePrice
        {
            get
            {
                var basePrice = CasePrice ?? (BottlePrice.HasValue ? BottlePrice.Value * Math.Max(1, PackSize) : 0m);
                if (!IsDealActive) return basePrice;

                if (DealType == "BuyXGetY" && DealMinQty.HasValue && DealMinQty.Value > 0 && DealFreeQty.HasValue && DealFreeQty.Value > 0)
                {
                    // e.g. Buy 3 get 1 free: paying for 3 cases, receiving 4 cases
                    int paidCases = DealMinQty.Value;
                    int totalCases = DealMinQty.Value + DealFreeQty.Value;
                    decimal totalCost = paidCases * basePrice;
                    return Math.Round(totalCost / totalCases, 2);
                }
                else if (DealType == "PostOffFlat" && DealPrice.HasValue)
                {
                    // Post-off: DealPrice represents discounted case price or dollar reduction
                    return DealPrice.Value;
                }
                else if (DealType == "TieredQty" && DealPrice.HasValue)
                {
                    return DealPrice.Value;
                }
                else if (DealType == "PercentOff" && DealPrice.HasValue)
                {
                    return Math.Round(basePrice * (1m - (DealPrice.Value / 100m)), 2);
                }

                return basePrice;
            }
        }

        [NotMapped]
        public decimal EffectiveBottlePrice
        {
            get
            {
                if (IsCaseOnly)
                {
                    int pack = Math.Max(1, PackSize);
                    return Math.Round(EffectiveCasePrice / pack, 2);
                }
                return BottlePrice ?? (CasePrice.HasValue && PackSize > 0 ? Math.Round(CasePrice.Value / PackSize, 2) : 0m);
            }
        }

        /// <summary>
        /// What a single bottle costs when purchased as part of the case (EffectiveCasePrice / PackSize).
        /// </summary>
        [NotMapped]
        public decimal BottleCostByCase
        {
            get
            {
                int pack = Math.Max(1, PackSize);
                return Math.Round(EffectiveCasePrice / pack, 2);
            }
        }

        /// <summary>
        /// How much is saved per bottle when buying by the case vs buying loose single bottles.
        /// Positive value indicates buying by the case is cheaper per bottle.
        /// </summary>
        [NotMapped]
        public decimal CaseSavingsPerBottle
        {
            get
            {
                if (IsCaseOnly || !BottlePrice.HasValue || BottlePrice.Value <= 0 || !CasePrice.HasValue || CasePrice.Value <= 0 || PackSize <= 1)
                {
                    return 0m;
                }
                decimal singleBottleCost = BottlePrice.Value;
                decimal caseBottleCost = BottleCostByCase;
                return Math.Round(singleBottleCost - caseBottleCost, 2);
            }
        }
    }
}
