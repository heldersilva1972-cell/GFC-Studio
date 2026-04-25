using System;

namespace GFC.Core.Models
{
    // GFC Mobile Configuration Model (Synchronized with Database - Revision 2)
    public class LotteryCommissionRate
    {
        public int Year { get; set; }
        public decimal SalesRate { get; set; }
        public decimal CashingRate { get; set; }
        public decimal TicketRate { get; set; }
        
        public decimal DailySystemFee { get; set; } = 0.00m;
        public decimal DailyBondingFee { get; set; } = 1.00m; // Daily bonding fee (e.g., $1.00/day)
        public decimal TargetDrawerAmount { get; set; } = 1200.00m; // Target to place back in drawer

        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }

        // Transient helpers (not usually stored but used for math)
        public decimal SalesCommissionMultiplier => SalesRate / 100m;
        public decimal CashingBonusMultiplier => CashingRate / 100m;
        public decimal TicketBonusMultiplier => TicketRate / 100m;
    }
}
