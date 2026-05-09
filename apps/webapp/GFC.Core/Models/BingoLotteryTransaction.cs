using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("BingoLotteryTransactions")]
    public class BingoLotteryTransaction : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; // "Income", "Prize", "LotteryFee", "Expense", "Rounding"

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public int? CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public virtual BingoExpenseCategory? Category { get; set; }

        public int? SessionId { get; set; }
        
        [ForeignKey("SessionId")]
        public virtual BingoSession? Session { get; set; }
    }
}
