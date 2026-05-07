using System;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class BingoSheetDefinition : BaseEntity
    {
        public int Id { get; set; }
        public string ColorName { get; set; } = string.Empty;
        public string DisplayColor { get; set; } = "#3b82f6";
        public decimal DefaultPrice { get; set; }
        public decimal LotteryPercentage { get; set; } = 0.05m;
        public decimal ClubPercentage { get; set; } = 0.05m;
        public bool IsFiftyFifty { get; set; }
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
        public bool IsIncludedInAdmission { get; set; } = true;
        public int PayoutMode { get; set; }
        public bool IsAdmissionOnly { get; set; }
        // 0=Fixed, 1=50/50 Sequential, 2=Gross %
        public virtual ICollection<BingoGameDefinition> Games { get; set; } = new List<BingoGameDefinition>();
    }
}
