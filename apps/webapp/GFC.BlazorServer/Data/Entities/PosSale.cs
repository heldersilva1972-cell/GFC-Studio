using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.BlazorServer.Data.Entities
{
    public class PosSale
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
        public decimal TotalAmount { get; set; }
        
        [MaxLength(50)]
        public string? PaymentType { get; set; } = "CASH";
        
        public string? ItemsJson { get; set; } = "[]";

        public bool IsSynced { get; set; } = false;
        public bool IsVoided { get; set; } = false;
        public bool IsCorrection { get; set; } = false;
        public Guid? OriginalSaleId { get; set; }
        public string? AdjustmentReason { get; set; }
        public decimal? AmountReceived { get; set; }
        public decimal? ChangeDue { get; set; }
        public decimal OriginalTotal { get; set; }
        public int? ActiveEventId { get; set; }
    }
}
