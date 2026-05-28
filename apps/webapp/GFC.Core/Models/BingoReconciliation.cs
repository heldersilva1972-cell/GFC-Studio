using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("BingoReconciliations")]
    public class BingoReconciliation : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StatementDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal EndingBalance { get; set; }

        [Required]
        public string ClearedTransactionIdsJson { get; set; } = string.Empty;
    }
}
