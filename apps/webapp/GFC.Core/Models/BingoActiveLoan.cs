using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("BingoActiveLoans")]
    public class BingoActiveLoan : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Lender { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal RemainingBalance { get; set; }
    }
}
