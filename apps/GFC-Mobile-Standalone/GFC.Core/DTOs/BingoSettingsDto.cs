using System.Collections.Generic;

namespace GFC.Core.DTOs
{
    public class BingoSettingsDto
    {
        public decimal BasePrice { get; set; } = 15.00m;
        public decimal CardPrice { get; set; } = 3.00m;
        public int PayoutRoundingMode { get; set; } = 0; // 0: None, 1: Round Up, 2: Round Down
        public List<BingoPayoutTierDto> PayoutTiers { get; set; } = new();
    }

    public class BingoPayoutTierDto
    {
        public int MinAdmissions { get; set; }
        public decimal PayoutPercentage { get; set; }
    }
}
