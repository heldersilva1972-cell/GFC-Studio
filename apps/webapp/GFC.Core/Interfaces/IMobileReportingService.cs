using GFC.Core.Models;
using GFC.Core.DTOs;

namespace GFC.Core.Interfaces;

public interface IMobileReportingService
{
    Task<MobileShiftData> GetShiftReportDataAsync(DateTime date, string shiftType, bool isRental);
    Task<bool> SaveShiftReportAsync(MobileShiftData data, string username);
    Task<decimal> GetCarryoverCashAsync(DateTime date, string shiftType);
    Task<decimal> GetCumulativeBagDebtAsync(DateTime date);
    Task<bool> SubmitFinalReportAsync(MobileShiftData data, string username);
    Task<DailyShiftSummary> GetDailySummaryAsync(DateTime date);
    Task<string> GetServerVersionAsync();
    Task<LotteryCommissionRate> GetLotteryRateAsync(int year);
    Task FlushOutboxAsync();
    Task PurgeOutboxAsync();
    Task<int> GetPendingCountAsync();
    int PendingCount { get; }
    DateTime? LastSyncTime { get; }
    string? LastSyncStatus { get; }
    event Action? OutboxChanged;

    // BINGO
    Task<List<BingoSheetDefinition>> GetBingoProgramAsync();
    Task<List<BingoAdmissionDefinition>> GetBingoAdmissionsAsync();
    Task<BingoSettingsDto> GetBingoSettingsAsync();
    Task<bool> SubmitBingoSessionAsync(BingoSession session, string username);
    Task<BingoSession?> GetBingoSessionByDateAsync(DateTime date);
    Task<List<PullTabGameDefinition>> GetPullTabGamesAsync();
}

public class DailyShiftSummary
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
