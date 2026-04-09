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
        
        [Required]
        [MaxLength(100)]
        public string TerminalName { get; set; } = "TERMINAL 1";
        
        [Required]
        [MaxLength(100)]
        public string BartenderName { get; set; } = "";
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string PaymentType { get; set; } = "CASH";
        
        [Required]
        public string ItemsJson { get; set; } = "[]";

        public bool IsSynced { get; set; } = false;
    }
}
