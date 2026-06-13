using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class PullTabGameEntry : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual BingoSession? Session { get; set; }

        [Required]
        public int GameDefinitionId { get; set; }

        [ForeignKey("GameDefinitionId")]
        public virtual PullTabGameDefinition? GameDefinition { get; set; }

        public int? SelectedPrizeOptionId { get; set; }

        [ForeignKey("SelectedPrizeOptionId")]
        public virtual PullTabPrizeOption? SelectedPrizeOption { get; set; }

        [Required]
        [MaxLength(255)]
        public string SellerName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SerialNumber { get; set; } = string.Empty;

        public int TicketsIssued { get; set; }

        public int TicketsReturned { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrizesPaid { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal StartingBank { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CashReceived { get; set; }
    }
}
