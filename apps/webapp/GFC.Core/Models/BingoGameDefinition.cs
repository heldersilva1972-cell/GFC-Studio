using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class BingoGameDefinition : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SheetDefinitionId { get; set; }

        [ForeignKey("SheetDefinitionId")]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual BingoSheetDefinition? Sheet { get; set; }

        [Required]
        [MaxLength(255)]
        public string GameName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DefaultPayout { get; set; }

        public bool IsVariablePayout { get; set; }

        public int PayoutMode { get; set; } // 0: Fixed, 1: 50/50 Sequential, 2: Gross % Additive
        public bool IsProgressive { get; set; }
        public int ProgressiveBallGoal { get; set; }
        public int CurrentProgressiveBallGoal { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ProgressiveJackpotAmount { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ProgressiveConsolationAmount { get; set; }

        public DateTime? DateLastProgressed { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal? LotteryPercentage { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal? ClubPercentage { get; set; }

        public bool IncludeInTargetProfit { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal MinimumPayout { get; set; }
    }
}
