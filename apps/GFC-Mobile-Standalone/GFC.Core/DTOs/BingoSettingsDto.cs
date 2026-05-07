using System.Collections.Generic;

namespace GFC.Core.DTOs
{
    public class BingoSettingsDto
    {
        public decimal BasePrice { get; set; } = 15.00m;
        public decimal CardPrice { get; set; } = 3.00m;
        public List<BingoPayoutTierDto> PayoutTiers { get; set; } = new();
    }

    public class BingoPayoutTierDto
    {
        public int MinAdmissions { get; set; }
        public decimal PayoutPercentage { get; set; }
    }
}
