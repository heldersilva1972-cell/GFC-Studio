namespace GFC.Core.Models
{
    public class LotteryWeeklyStat
    {
        public int Id { get; set; }
        public DateTime WeekEndingDate { get; set; }

        // Online Section
        public decimal OnlineNetSales { get; set; }
        public decimal OnlineCommission { get; set; }
        public decimal OnlineCashes { get; set; }
        public decimal OnlineCashBonus { get; set; }
        public decimal OnlineClaimsBonus { get; set; }
        public decimal OnlineAdjustments { get; set; }
        public decimal OnlineServiceFee { get; set; }
        public decimal OnlineBondingFee { get; set; }
        public decimal OnlineDue { get; set; }

        // Instant Section
        public decimal InstantGrossSales { get; set; }
        public decimal InstantReturnSales { get; set; }
        public decimal InstantCommission { get; set; }
        public decimal InstantCashes { get; set; }
        public decimal InstantCashBonus { get; set; }
        public decimal InstantClaimsBonus { get; set; }
        public decimal InstantAdjustments { get; set; }
        public decimal InstantDue { get; set; }

        // Totals
        public decimal TotalDue { get; set; }

        // Audit
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
