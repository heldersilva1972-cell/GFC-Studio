using System;
using System.Text.Json.Serialization;

namespace GFC.Core.Models
{
    public class BingoGameEntry : BaseEntity
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string SheetColor { get; set; } = string.Empty;
        public string GameName { get; set; } = string.Empty;
        public int SheetsSold { get; set; }
        public decimal PricePerSheet { get; set; }
        public decimal GrossReceipts { get; set; }
        public decimal PrizePaid { get; set; }
        public decimal LotteryPercent { get; set; }
        public decimal ClubPercent { get; set; }
        public decimal LotteryTake { get; set; }
        public decimal ClubTake { get; set; }
        public decimal NetProceeds { get; set; }
        public decimal RoundingAdjustment { get; set; }
        public int BallsCalled { get; set; }

        [JsonIgnore]
        public virtual BingoSession Session { get; set; } = null!;
    }
}
