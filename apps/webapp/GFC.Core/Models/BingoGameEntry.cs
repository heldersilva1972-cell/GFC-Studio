using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class BingoGameEntry : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual BingoSession? Session { get; set; }

        public string SheetColor { get; set; } = string.Empty;

        [Required]
        public string GameName { get; set; } = string.Empty;

        public int SheetsSold { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PricePerSheet { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal GrossReceipts { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrizePaid { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal LotteryPercentage { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal ClubPercentage { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal LotteryTake { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal ClubTake { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal NetProceeds { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal RoundingAdjustment { get; set; }

        public int BallsCalled { get; set; }

        public string? Category { get; set; }
    }
}
