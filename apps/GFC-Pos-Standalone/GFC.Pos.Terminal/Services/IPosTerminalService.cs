using GFC.Core.Models;
using GFC.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Pos.Terminal.Services;

public interface IPosTerminalService
{
    Task<PosMenuDto> GetMenuAsync();
    Task SaveSaleAsync(PosSaleDto sale);
    Task<PosZReportDto?> GetZReportAsync(Guid id);
    Task<List<PosZReportDto>> GetZReportsAsync(string terminalName);
    Task SaveZReportAsync(PosZReportDto report);
    Task<DateTime> GetLastZTimeAsync(string terminalName);
    Task<PosSaleDto?> GetDartsRoundTodayAsync(string terminalName);
    Task<bool> CheckConnectivityAsync();
}
