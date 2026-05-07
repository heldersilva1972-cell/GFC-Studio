using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class BingoSheetDefinition : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ColorName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string DisplayColor { get; set; } = "#3b82f6";

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DefaultPrice { get; set; }
        public decimal LotteryPercentage { get; set; } = 0.05m;
        public decimal ClubPercentage { get; set; } = 0.05m;
        public bool IsFiftyFifty { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public bool IsIncludedInAdmission { get; set; } = true;

        public int PayoutMode { get; set; } // 0: Fixed, 1: 50/50 Sequential, 2: Gross % Additive
        public bool IsAdmissionOnly { get; set; }

        // Navigation property for games under this sheet
        public virtual ICollection<BingoGameDefinition> Games { get; set; } = new List<BingoGameDefinition>();
    }
}
