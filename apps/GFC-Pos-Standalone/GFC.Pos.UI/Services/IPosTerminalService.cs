using GFC.Core.Models;
using GFC.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Pos.UI.Services;

public interface IPosTerminalService
{
    Task<PosMenuDto> GetMenuAsync();
    Task SaveSaleAsync(PosSaleDto sale);
    Task<PosZReportDto?> GetZReportAsync(Guid id);
    Task<List<PosZReportDto>> GetZReportsAsync(string terminalName);
    Task SaveZReportAsync(PosZReportDto report);
    event Action? OutboxChanged;
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
}

public class ShiftAuditDto
{
    public decimal CashTotal { get; set; }
    public decimal GrossTotal { get; set; }
    public Dictionary<string, int> ItemSummary { get; set; } = new();
    public List<PosSaleDto> VoidedSales { get; set; } = new();
    public PosSaleDto? LatestSale { get; set; }
}
