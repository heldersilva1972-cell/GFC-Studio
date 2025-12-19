using System.Collections.Generic;
using System.Linq;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;

namespace GFC.Core.Services;

public class DashboardService : IDashboardService
{
    private readonly IMemberQueryService _memberQueryService;
    private readonly IDuesInsightService _duesInsightService;
    private readonly IMemberRepository _memberRepository;
    private readonly IHistoryRepository _historyRepository;
    private readonly OverdueCalculationService _overdueService;
    private readonly INpQueueService _npQueueService;
    private readonly BackupConfigService _backupConfigService;
        private readonly IMemberKeycardRepository _keycardRepository;
        private readonly IBoardRepository _boardRepository;
        private readonly IBoardTermConfirmationService _boardTermConfirmationService;
        private readonly IPhysicalKeyService _physicalKeyService;

    public DashboardService(
        IMemberQueryService memberQueryService,
        IDuesInsightService duesInsightService,
        IMemberRepository memberRepository,
        IHistoryRepository historyRepository,
        OverdueCalculationService overdueService,
        INpQueueService npQueueService,
        BackupConfigService backupConfigService,
        IMemberKeycardRepository keycardRepository,
        IBoardRepository boardRepository,
        IBoardTermConfirmationService boardTermConfirmationService,
        IPhysicalKeyService physicalKeyService)
    {
        _memberQueryService = memberQueryService ?? throw new ArgumentNullException(nameof(memberQueryService));
        _duesInsightService = duesInsightService ?? throw new ArgumentNullException(nameof(duesInsightService));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
        _overdueService = overdueService ?? throw new ArgumentNullException(nameof(overdueService));
        _npQueueService = npQueueService ?? throw new ArgumentNullException(nameof(npQueueService));
        _backupConfigService = backupConfigService ?? throw new ArgumentNullException(nameof(backupConfigService));
        _keycardRepository = keycardRepository ?? throw new ArgumentNullException(nameof(keycardRepository));
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
        _boardTermConfirmationService = boardTermConfirmationService ?? throw new ArgumentNullException(nameof(boardTermConfirmationService));
        _physicalKeyService = physicalKeyService ?? throw new ArgumentNullException(nameof(physicalKeyService));
    }

    public Task<MemberSummaryDto> GetMemberSummaryAsync(CancellationToken cancellationToken = default)
        => _memberQueryService.GetSummaryAsync(cancellationToken);

    public Task<DuesSummaryDto> GetCurrentYearDuesSummaryAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.Now.Year;
        return _duesInsightService.GetSummaryAsync(year, cancellationToken);
    }

    public async Task<AlertSummaryDto> GetAlertSummaryAsync(CancellationToken cancellationToken = default)
    {
        var lifeEligible = await Task.Run(
            () => _memberRepository.GetLifeEligibleCount(DateTime.Today, _historyRepository),
            cancellationToken);
        var npQueue = await Task.Run(() => _memberRepository.GetNonPortugueseQueueCount(), cancellationToken);
        var overdue15Plus = await Task.Run(() =>
        {
            var members = _memberRepository.GetAllMembers();
            return _overdueService.GetOverdue15PlusMonthsCount(members);
        }, cancellationToken);
        var activeKeyCards = await Task.Run(() => _keycardRepository.GetActiveAssignmentCount(), cancellationToken);
        var physicalKeysToReturn = await Task.Run(() => _physicalKeyService.GetKeysThatShouldBeReturned().Count, cancellationToken);

        var boardAlertYear = 0;
        IReadOnlyList<string> boardPositionsUnfilled = Array.Empty<string>();
        var boardConfirmed = false;

        var today = DateTime.Today;
        var boardYearCandidate = today.Month >= 12 ? today.Year + 1 : today.Year;
        var alertStart = new DateTime(boardYearCandidate - 1, 12, 1);

        if (today >= alertStart)
        {
            boardAlertYear = boardYearCandidate;
            var confirmation = _boardTermConfirmationService.GetConfirmation(boardAlertYear);
            boardConfirmed = confirmation is not null;

            if (!boardConfirmed)
            {
                var positions = await Task.Run(() => _boardRepository.GetAllPositions(), cancellationToken);
                var assignments = await Task.Run(() => _boardRepository.GetAssignmentsByYear(boardAlertYear), cancellationToken);
                var missing = new List<string>();

                foreach (var position in positions)
                {
                    var requiredSeats = Math.Max(1, position.MaxSeats);
                    var assigned = assignments.Count(a => a.PositionID == position.PositionID && a.MemberID > 0);
                    var remaining = requiredSeats - assigned;
                    if (remaining > 0)
                    {
                        missing.Add(remaining == 1
                            ? position.PositionName
                            : $"{position.PositionName} ({remaining} open)");
                    }
                }

                boardPositionsUnfilled = missing;
            }
        }

        return new AlertSummaryDto(
            physicalKeysToReturn,
            lifeEligible,
            npQueue,
            overdue15Plus,
            activeKeyCards,
            boardAlertYear,
            boardPositionsUnfilled,
            boardConfirmed);
    }

    public Task<BackupStatusDto> GetBackupStatusAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var config = _backupConfigService.Load();
            var lastBackup = _backupConfigService.GetLastBackupTimestamp(config);
            var nextRun = CalculateNextRun(config);

            return new BackupStatusDto(lastBackup, nextRun);
        }, cancellationToken);
    }

    private static DateTime? CalculateNextRun(BackupConfig config)
    {
        if (!config.IsConfigured)
        {
            return null;
        }

        var now = DateTime.Now;
        var candidate = now.Date.Add(config.DailyBackupTime);
        if (candidate <= now)
        {
            candidate = candidate.AddDays(1);
        }

        return candidate;
    }
}

