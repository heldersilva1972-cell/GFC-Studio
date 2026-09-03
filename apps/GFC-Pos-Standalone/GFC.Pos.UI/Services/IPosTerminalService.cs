using GFC.Core.Models;
using GFC.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Pos.UI.Services;

public interface IPosTerminalService
{
    Task InitializeAsync();
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
    int PendingSalesCount { get; }
    int PendingZCount { get; }
    DateTime? LastSynced { get; }
    Task<int> GetTotalPendingAsync();
    Task<DateTime> GetLastZTimeAsync(string terminalName);
    Task<PosSaleDto?> GetDartsRoundTodayAsync(string terminalName);
    Task<List<PosSaleDto>> GetDartsRoundsTodayAsync(string terminalName);
    Task<bool> CheckConnectivityAsync();
    Task<List<LiquorOrder>> GetPendingLiquorOrdersAsync(bool force = false);
    Task ReceiveLiquorOrderAsync(LiquorOrderReceiptDto receipt);
    Task<List<UserListItemDto>> GetAuthorizedUsersAsync();

    // ─── LOCAL SHIFT DATABASE (PROPER ARCHITECTURE) ───
    Task AddSaleToShiftAsync(PosSaleDto sale);
    Task VoidSaleAsync(Guid saleId, string reason);
    Task<ShiftAuditDto> GetShiftAuditAsync();
    Task ClearShiftAsync();
    Task FlushAllPendingAsync();
    Task<string> GetServerVersionAsync();
    Task<BanquetMasterSummaryDto?> GetBanquetMasterSummaryAsync(int eventId);
    Task<List<PosSaleDto>> GetUnsyncedSalesAsync();
    Task<MemberDrawPoolDto?> GetMemberDrawPoolAsync();
    Task<MemberDrawStatusDto?> GetMemberDrawStatusAsync(int memberId);
    Task<List<LiquorItem>> GetLiquorInventoryAsync(bool force = false);
    Task<EmployeeMonthlyShiftsDto?> GetEmployeeShiftsAsync(string username, int year, int month);
    bool IsTransactionInProgress { get; set; }
    Task<VersionCheckResult?> CheckForUpdatesApiAsync();
}

public class VersionCheckResult
{
    public string LatestVersion { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
    public string Sha256Hash { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
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
