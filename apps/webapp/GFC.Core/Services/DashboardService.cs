using System.Collections.Generic;
using System.Linq;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;

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
        private readonly IDuesRepository _duesRepository;
        private readonly IDuesWaiverRepository _waiverRepository;

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
        IPhysicalKeyService physicalKeyService,
        IDuesRepository duesRepository,
        IDuesWaiverRepository waiverRepository)
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
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _waiverRepository = waiverRepository ?? throw new ArgumentNullException(nameof(waiverRepository));
    }

    public Task<MemberSummaryDto> GetMemberSummaryAsync(CancellationToken cancellationToken = default)
        => _memberQueryService.GetSummaryAsync(cancellationToken);

    public Task<DuesSummaryDto> GetCurrentYearDuesSummaryAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.Now.Year;
        return _duesInsightService.GetSummaryAsync(year, cancellationToken);
    }

    public async Task<AlertSummaryDto> GetAlertSummaryAsync(List<Member>? members = null, CancellationToken cancellationToken = default)
    {
        // 1. Start all base data fetching tasks in parallel
        // If members are provided, we use them. If not, we fetch a MINIMAL set for alerts.
        var membersTask = members != null 
            ? Task.FromResult(members) 
            : Task.Run(() => _memberRepository.GetAllMembers().Select(m => new Member {
                MemberID = m.MemberID,
                Status = m.Status,
                AcceptedDate = m.AcceptedDate,
                ApplicationDate = m.ApplicationDate,
                LifeEligibleDate = m.LifeEligibleDate
            }).ToList(), cancellationToken);

        var npQueueTask = Task.Run(() => _memberRepository.GetNonPortugueseQueueCount(), cancellationToken);
        var activeKeyCardsTask = Task.Run(() => _keycardRepository.GetActiveAssignmentCount(), cancellationToken);
        var physicalKeysToReturnTask = Task.Run(() => _physicalKeyService.GetKeysThatShouldBeReturned().Count, cancellationToken);
        
        // PERFORMANCE: Fetch dues and waivers in bulk once instead of N+1 per member
        var duesTask = Task.Run(() => _duesRepository.GetAllDues(), cancellationToken);
        var waiversTask = Task.Run(() => _waiverRepository.GetAllWaivers(), cancellationToken);

        // 2. Wait for base data
        await Task.WhenAll(membersTask, npQueueTask, activeKeyCardsTask, physicalKeysToReturnTask, duesTask, waiversTask);
        
        var fetchedMembers = await membersTask;
        var allDues = duesTask.Result;
        
        // 3. Build optimized lookups for overdue calculations
        var duesLookup = allDues.GroupBy(d => d.MemberID).ToDictionary(g => g.Key, g => g.ToList());
        var waivers = waiversTask.Result; // This is actually List<DuesWaiverPeriod>
        var waiversLookup = waivers.GroupBy(w => w.MemberId).ToDictionary(g => g.Key, g => g.ToList());

        var today = DateTime.Today;
        var boardYearCandidate = today.Month >= 12 ? today.Year + 1 : today.Year;
        
        var boardAssignmentsTask = Task.Run(() => _boardRepository.GetAssignmentsByYear(today.Year), cancellationToken);
        await boardAssignmentsTask;
        var boardLookup = boardAssignmentsTask.Result.GroupBy(b => b.MemberID).ToDictionary(g => g.Key, g => g.Select(b => b.TermYear).ToHashSet());

        var overdueContext = new OverdueCalculationService.DuesCalculationContext
        {
            DuesByMember = duesLookup,
            WaiversByMember = waiversLookup,
            BoardAssignmentsByMember = boardLookup,
            Today = today
        };

        // 4. Start processing tasks that use the fetched data
        var lifeEligibleTask = Task.Run(() => _memberRepository.GetLifeEligibleCount(today, _historyRepository, fetchedMembers), cancellationToken);
        var overdueCountTask = Task.Run(() => _overdueService.GetOverdue15PlusMonthsCountBulk(fetchedMembers, overdueContext), cancellationToken);

        await Task.WhenAll(lifeEligibleTask, overdueCountTask);

        // 5. Finalize other metrics
        var lifeEligible = lifeEligibleTask.Result;
        var npQueue = npQueueTask.Result;
        var overdue15Plus = overdueCountTask.Result;
        var activeKeyCards = activeKeyCardsTask.Result;
        var physicalKeysToReturn = physicalKeysToReturnTask.Result;

        var boardAlertYear = 0;
        IReadOnlyList<string> boardPositionsUnfilled = Array.Empty<string>();
        var boardConfirmed = false;

        var alertStart = new DateTime(boardYearCandidate - 1, 12, 1);

        if (today >= alertStart)
        {
            boardAlertYear = boardYearCandidate;
            var confirmation = _boardTermConfirmationService.GetConfirmation(boardAlertYear);
            boardConfirmed = confirmation is not null;

            if (!boardConfirmed)
            {
                // Parallelize board positions and assignments fetching
                var positionsTask = Task.Run(() => _boardRepository.GetAllPositions(), cancellationToken);
                var assignmentsTask = Task.Run(() => _boardRepository.GetAssignmentsByYear(boardAlertYear), cancellationToken);
                
                await Task.WhenAll(positionsTask, assignmentsTask);
                
                var positions = positionsTask.Result;
                var assignments = assignmentsTask.Result;
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

