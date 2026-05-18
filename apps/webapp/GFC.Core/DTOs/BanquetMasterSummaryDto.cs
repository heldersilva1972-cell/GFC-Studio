using System.Collections.Generic;

namespace GFC.Core.DTOs;

public class BanquetMasterSummaryDto
{
    public string EventName { get; set; } = string.Empty;
    public decimal TotalDeposited { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal RemainingBalance => TotalDeposited - TotalSpent;
    public Dictionary<string, int> ItemQuantities { get; set; } = new();
    public Dictionary<string, decimal> ItemTotals { get; set; } = new();
}
