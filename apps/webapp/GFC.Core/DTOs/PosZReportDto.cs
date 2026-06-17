using System;

namespace GFC.Core.DTOs;

public class PosZReportDto
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string TerminalName { get; set; } = "";
    public string BartenderName { get; set; } = "";
    public decimal CashTotal { get; set; }
    public decimal TotalGrossSales { get; set; }
    public string InventoryPullsJson { get; set; } = "";
    public string SalesSummaryJson { get; set; } = "";
    public string BanquetSummaryJson { get; set; } = "[]";
    public decimal TokenCredits { get; set; }
    public decimal? HoursWorked { get; set; }
    public string? ShiftType { get; set; }
    public bool RecordSalesToBar { get; set; }
}
