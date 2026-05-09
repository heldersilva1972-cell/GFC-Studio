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
        public string Status { get; set; } = "Draft";
        public string? Notes { get; set; }
        public virtual ICollection<BingoGameEntry> GameEntries { get; set; } = new List<BingoGameEntry>();
        public virtual ICollection<BingoAdmissionEntry> AdmissionEntries { get; set; } = new List<BingoAdmissionEntry>();
    }
}
