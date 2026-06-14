using GFC.Core.Models;
using GFC.Core.DTOs;

namespace GFC.Core.Interfaces;

// GFC Mobile Service Contract (Revision 2.1.35)
public interface IMobileReportingService
{
    Task<MobileShiftData> GetShiftReportDataAsync(DateTime date, string shiftType, bool isRental);
    Task<bool> IsShiftSubmittedAsync(DateTime date, string shiftType, bool isRental);
    Task<bool> SaveShiftReportAsync(MobileShiftData data, string username);
    Task<decimal> GetCarryoverCashAsync(DateTime date, string shiftType);
    Task<decimal> GetCumulativeBagDebtAsync(DateTime date);
    Task<bool> SubmitFinalReportAsync(MobileShiftData data, string username);
    Task<DailyShiftSummary> GetDailySummaryAsync(DateTime date);
    Task<string> GetServerVersionAsync();
    Task<LotteryCommissionRate> GetLotteryRateAsync(int year);
    Task<List<BingoSheetDefinition>> GetBingoProgramAsync();
    Task<List<BingoAdmissionDefinition>> GetBingoAdmissionsAsync();
    Task<BingoSettingsDto> GetBingoSettingsAsync();
    Task<bool> SubmitBingoSessionAsync(BingoSession session, string username);
    Task FlushOutboxAsync();
    Task PurgeOutboxAsync();
    Task<int> GetPendingCountAsync();
    int PendingCount { get; }
    DateTime? LastSyncTime { get; }
    string? LastSyncStatus { get; }
    event Action? OutboxChanged;
    Task<BingoSession?> GetBingoSessionByDateAsync(DateTime date);
    Task<bool> CancelBingoSessionAsync(DateTime date);
    Task<List<PullTabGameDefinition>> GetPullTabGamesAsync();
    Task<List<ProgressiveHistoryDto>> GetProgressiveHistoryAsync(string gameName, string sheetColor);
}

public class DailyShiftSummary // Revision 2.1.35
{
    public ShiftStatus Day { get; set; } = new();
    public ShiftStatus Night { get; set; } = new();
    public ShiftStatus Hall { get; set; } = new();
}

public class ShiftStatus
{
    public bool Submitted { get; set; }
    public bool HasData { get; set; }
    public string Closer { get; set; }
    public bool Modified { get; set; }
}
