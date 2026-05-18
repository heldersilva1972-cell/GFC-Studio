using System;
using System.Collections.Generic;

namespace GFC.Core.DTOs;

public class BanquetDepositDetailDto
{
    public DateTime Timestamp { get; set; }
    public decimal Amount { get; set; }
    public bool IsInitial { get; set; }
}

public class BanquetMasterSummaryDto
{
    public string EventName { get; set; } = string.Empty;
    public decimal TotalDeposited { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal RemainingBalance => TotalDeposited - TotalSpent;
    public Dictionary<string, int> ItemQuantities { get; set; } = new();
    public Dictionary<string, decimal> ItemTotals { get; set; } = new();
    public List<BanquetDepositDetailDto> Deposits { get; set; } = new();
}
