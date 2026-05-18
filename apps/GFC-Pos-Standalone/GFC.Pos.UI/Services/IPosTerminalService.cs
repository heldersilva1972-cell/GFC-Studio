using GFC.Core.Models;
using GFC.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Pos.UI.Services;

public interface IPosTerminalService
{
    Task<PosMenuDto> GetMenuAsync(bool force = false);
    Task<PosMenuDto?> GetCachedMenuAsync();
    Task SaveMenuToVaultAsync(PosMenuDto menu);
    Task SaveSaleAsync(PosSaleDto sale);
    Task<PosZReportDto?> GetZReportAsync(Guid id);
    Task<List<PosZReportDto>> GetZReportsAsync(string terminalName);
    Task SaveZReportAsync(PosZReportDto report);
    event Action? OutboxChanged;
    event Action<PosMenuDto>? MenuRefreshed;
    int TotalPendingCount { get; }
    DateTime? LastSynced { get; }
    Task<int> GetTotalPendingAsync();
    Task<DateTime> GetLastZTimeAsync(string terminalName);
    Task<PosSaleDto?> GetDartsRoundTodayAsync(string terminalName);
    Task<bool> CheckConnectivityAsync();
    Task<List<UserListItemDto>> GetAuthorizedUsersAsync();

    // ─── LOCAL SHIFT DATABASE (PROPER ARCHITECTURE) ───
    Task AddSaleToShiftAsync(PosSaleDto sale);
    Task VoidSaleAsync(Guid saleId, string reason);
    Task<ShiftAuditDto> GetShiftAuditAsync();
    Task ClearShiftAsync();
    Task FlushAllPendingAsync();
    Task<string> GetServerVersionAsync();
    Task<BanquetMasterSummaryDto?> GetBanquetMasterSummaryAsync(int eventId);
}

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
