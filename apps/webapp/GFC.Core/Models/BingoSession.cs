using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class BingoSession : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime SessionDate { get; set; } = DateTime.Now;

        public int AdmissionCount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalGrossReceipts { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrizesPaid { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalLotteryTake { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalClubTake { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal RoundingAdjustment { get; set; }

        public string? Category { get; set; }

        public string Status { get; set; } = "Draft";

        public string? Notes { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DoorPrizeAmount { get; set; }

        public int DoorPrizeCount { get; set; }

        public string? WtaColor { get; set; }
        public string? ProgColor { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BeanoSuppliesSales { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BeanoOtherReceipts { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BeanoUnexpendedNetProfit { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BeanoInterest { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BeanoGameBank { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BeanoOthers { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BeanoCheckbookBalance { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BeanoSavingsBalance { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BeanoCdBalance { get; set; }

        public string? BeanoTaxCheckNumber { get; set; }
        public string? BeanoOccasionTime { get; set; }
        public string? FiftyFifty1Color { get; set; }
        public string? FiftyFifty2Color { get; set; }
        public string? FiftyFifty3Color { get; set; }
        public string? BeanoDisbursementsJson { get; set; }
        public string? BeanoOtherExpensesJson { get; set; }

        // Navigation properties
        public virtual ICollection<BingoGameEntry> GameEntries { get; set; } = new List<BingoGameEntry>();
        public virtual ICollection<BingoAdmissionEntry> AdmissionEntries { get; set; } = new List<BingoAdmissionEntry>();
        public virtual ICollection<PullTabGameEntry> PullTabEntries { get; set; } = new List<PullTabGameEntry>();
    }
}
