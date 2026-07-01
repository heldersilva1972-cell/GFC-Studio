using System;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class BingoSession : BaseEntity
    {
        public int Id { get; set; }
        public DateTime SessionDate { get; set; } = DateTime.Now;
        public int AdmissionCount { get; set; }
        public decimal TotalGrossReceipts { get; set; }
        public decimal TotalPrizesPaid { get; set; }
        public decimal TotalLotteryTake { get; set; }
        public decimal TotalClubTake { get; set; }
        public decimal RoundingAdjustment { get; set; }
        public string? Category { get; set; }
        public string Status { get; set; } = "Draft";
        public string? Notes { get; set; }
        public decimal DoorPrizeAmount { get; set; }
        public int DoorPrizeCount { get; set; }

        public string? WtaColor { get; set; }
        public string? ProgColor { get; set; }
        public decimal BeanoSuppliesSales { get; set; }
        public decimal BeanoOtherReceipts { get; set; }
        public decimal BeanoUnexpendedNetProfit { get; set; }
        public decimal BeanoInterest { get; set; }
        public decimal BeanoGameBank { get; set; }
        public decimal BeanoOthers { get; set; }
        public decimal BeanoCheckbookBalance { get; set; }
        public decimal BeanoSavingsBalance { get; set; }
        public decimal BeanoCdBalance { get; set; }
        public string? BeanoTaxCheckNumber { get; set; }
        public string? BeanoOccasionTime { get; set; }
        public string? FiftyFifty1Color { get; set; }
        public string? FiftyFifty2Color { get; set; }
        public string? FiftyFifty3Color { get; set; }
        public string? BeanoDisbursementsJson { get; set; }
        public string? BeanoOtherExpensesJson { get; set; }

        public virtual ICollection<BingoGameEntry> GameEntries { get; set; } = new List<BingoGameEntry>();
        public virtual ICollection<BingoAdmissionEntry> AdmissionEntries { get; set; } = new List<BingoAdmissionEntry>();
        public virtual ICollection<PullTabGameEntry> PullTabEntries { get; set; } = new List<PullTabGameEntry>();
    }
}
