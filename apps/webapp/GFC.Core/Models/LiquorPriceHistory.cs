using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GFC.Core.Models
{
    [Table("LiquorPriceHistories")]
    public class LiquorPriceHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LiquorVendorQuoteId { get; set; }

        [ForeignKey("LiquorVendorQuoteId")]
        [JsonIgnore]
        public virtual LiquorVendorQuote? LiquorVendorQuote { get; set; }

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
        public decimal? OldBottlePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewBottlePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? OldCasePrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewCasePrice { get; set; }

        [Required]
        [StringLength(50)]
        public string ChangeType { get; set; } = "StandardChange"; // 'StandardChange', 'PromoStart', 'PromoEnd'

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ChangeAmount { get; set; }

        [Column(TypeName = "decimal(9,4)")]
        public decimal? ChangePercentage { get; set; }

        [StringLength(255)]
        public string? ChangeReason { get; set; }

        public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? RecordedByUserId { get; set; }
    }
}
