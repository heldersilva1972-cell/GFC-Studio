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
        
        [MaxLength(100)]
        public string? TerminalName { get; set; } = "TERMINAL 1";
        
        [MaxLength(100)]
        public string? BartenderName { get; set; } = "";
        
        [Required]
        public decimal CashTotal { get; set; }

        [Required]
        public decimal TotalGrossSales { get; set; }
        
        public decimal? TokenCredits { get; set; } = 0;

        public string? InventoryPullsJson { get; set; } = "[]";

        public string? SalesSummaryJson { get; set; } = "[]";
        
        public string? BanquetSummaryJson { get; set; } = "[]";

        public decimal? HoursWorked { get; set; }
        
        [MaxLength(50)]
        public string? ShiftType { get; set; }

        public bool RecordSalesToBar { get; set; } = false;

        public bool IsSynced { get; set; } = false;
    }
}
