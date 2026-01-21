using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class LiquorTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public virtual LiquorItem? Item { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ChangeAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; } = "Checkout"; // Checkout, Restock, Adjustment

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
