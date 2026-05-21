using System;

namespace GFC.Core.Models
{
    public class BingoGameDefinition : BaseEntity
    {
        public int Id { get; set; }
        public int SheetDefinitionId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
        public decimal DefaultPayout { get; set; }
        public bool IsVariablePayout { get; set; }
        public int PayoutMode { get; set; }
        public bool IsProgressive { get; set; }
        public int ProgressiveBallGoal { get; set; }
        public int CurrentProgressiveBallGoal { get; set; }
        public decimal ProgressiveJackpotAmount { get; set; }
        public decimal ProgressiveConsolationAmount { get; set; }
        public DateTime? DateLastProgressed { get; set; }
        public decimal? LotteryPercentage { get; set; }
        public decimal? ClubPercentage { get; set; }
        public virtual BingoSheetDefinition Sheet { get; set; } = null!;
    }
}
