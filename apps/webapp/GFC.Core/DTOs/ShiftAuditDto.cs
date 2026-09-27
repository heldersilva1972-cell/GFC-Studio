using System.Collections.Generic;

namespace GFC.Core.DTOs;

public class ShiftAuditDto
{
    public decimal CashTotal { get; set; }
    public decimal GrossTotal { get; set; }
    public decimal TokenCredits { get; set; }
    public Dictionary<string, int> ItemSummary { get; set; } = new();
    public Dictionary<string, decimal> ItemTotals { get; set; } = new();
    public Dictionary<string, int> RegularItemSummary { get; set; } = new();
    public Dictionary<string, decimal> RegularItemTotals { get; set; } = new();
    public List<PosSaleDto> VoidedSales { get; set; } = new();
    public PosSaleDto? LatestSale { get; set; }
    public List<BanquetShiftReportDto> Banquets { get; set; } = new();
    public decimal PayoutTotal { get; set; }
    public List<PosSaleDto> Payouts { get; set; } = new();
}

public class BanquetShiftReportDto
{
    public int? ActiveEventId { get; set; }
    public string EventName { get; set; } = "";
    public List<decimal> Deposits { get; set; } = new();
    public decimal TotalSpent { get; set; }
    public Dictionary<string, int> ItemSummary { get; set; } = new();
    public Dictionary<string, decimal> ItemTotals { get; set; } = new();
    public string EventType { get; set; } = "RunningTab";
}
