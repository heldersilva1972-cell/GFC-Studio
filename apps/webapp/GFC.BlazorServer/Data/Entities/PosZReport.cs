using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.BlazorServer.Data.Entities
{
    public class PosZReport
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        [Required]
        [MaxLength(100)]
        public string TerminalName { get; set; } = "TERMINAL 1";
        
        [Required]
        [MaxLength(100)]
        public string BartenderName { get; set; } = "";
        
        [Required]
        public decimal CashTotal { get; set; }

        [Required]
        public decimal TotalGrossSales { get; set; }
        
        [Required]
        public decimal TokenCredits { get; set; }

        [Required]
        public string InventoryPullsJson { get; set; } = "[]";

        [Required]
        public string SalesSummaryJson { get; set; } = "[]";
        
        [Required]
        public string BanquetSummaryJson { get; set; } = "[]";

        public bool IsSynced { get; set; } = false;
    }
}
