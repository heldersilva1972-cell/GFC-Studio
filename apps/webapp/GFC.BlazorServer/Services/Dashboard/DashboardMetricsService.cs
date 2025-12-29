using GFC.BlazorServer.Data;
using GFC.BlazorServer.Models.Dashboard;
using GFC.Core.BusinessRules;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CoreDuesPayment = GFC.Core.Models.DuesPayment;

namespace GFC.BlazorServer.Services.Dashboard;

public class DashboardMetricsService : IDashboardMetricsService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly IMemberRepository _memberRepository;
    private readonly IDuesRepository _duesRepository;
    private readonly IDuesYearSettingsRepository _duesYearSettingsRepository;
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardMetricsService> _logger;

    public DashboardMetricsService(
        IDbContextFactory<GfcDbContext> contextFactory,
        IMemberRepository memberRepository,
        IDuesRepository duesRepository,
        IDuesYearSettingsRepository duesYearSettingsRepository,
        IDashboardService dashboardService,
        ILogger<DashboardMetricsService> logger)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _duesYearSettingsRepository = duesYearSettingsRepository ?? throw new ArgumentNullException(nameof(duesYearSettingsRepository));
        _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<DashboardMetricsDto> GetMetricsAsync(CancellationToken ct = default)
    {
        var currentYear = DateTime.Today.Year;

        var membersTask = Task.Run(() => _memberRepository.GetAllMembers(), ct);
        var currentYearDuesTask = Task.Run(() => _duesRepository.GetDuesForYear(currentYear), ct);
        var previousYearDuesTask = Task.Run(() => _duesRepository.GetDuesForYear(currentYear - 1), ct);
        var settingsTask = Task.Run(() => _duesYearSettingsRepository.GetSettingsForYear(currentYear), ct);
        var alertSummaryTask = _dashboardService.GetAlertSummaryAsync(ct);
        var cardCountsTask = GetCardCountsAsync(ct);
        var membershipChangesTask = GetRecentMemberChangeCountAsync(ct);

        await Task.WhenAll(
            membersTask,
            currentYearDuesTask,
            previousYearDuesTask,
            settingsTask,
            alertSummaryTask,
            cardCountsTask,
            membershipChangesTask);

        var members = membersTask.Result;
        var currentYearDues = currentYearDuesTask.Result;
        var previousYearDues = previousYearDuesTask.Result;
        var graceEndDate = settingsTask.Result?.GraceEndDate?.Date;
        var (activeMembers, pastDueMembers) = CalculateMembership(members, currentYearDues, previousYearDues, graceEndDate);

        var alertSummary = alertSummaryTask.Result;
        var npQueueCount = alertSummary?.NpQueueCount ?? 0;
        var openAlerts = alertSummary is null ? 0 : CalculateOpenAlerts(alertSummary);

        var (enabledCards, disabledCards) = cardCountsTask.Result;

        return new DashboardMetricsDto
        {
            TotalMembers = members.Count,
            ActiveMembers = activeMembers,
            PastDueMembers = pastDueMembers,
            NpQueueCount = npQueueCount,
            EnabledCards = enabledCards,
            DisabledCards = disabledCards,
            OpenAlerts = openAlerts,
            MembershipChangesLast24h = membershipChangesTask.Result
        };
    }

    private static (int active, int pastDue) CalculateMembership(
        IEnumerable<Member> members,
        IEnumerable<CoreDuesPayment> currentYearDues,
        IEnumerable<CoreDuesPayment> previousYearDues,
        DateTime? graceEndDate)
    {
        var currentLookup = BuildLatestDuesLookup(currentYearDues);
        var previousLookup = BuildLatestDuesLookup(previousYearDues);
        var gracePeriodActive = graceEndDate.HasValue && DateTime.Today.Date <= graceEndDate.Value;

        var active = 0;
        var pastDue = 0;

        foreach (var member in members)
        {
            var normalizedStatus = MemberStatusHelper.NormalizeStatus(member.Status);
            if (!IsActiveStatus(normalizedStatus))
            {
                continue;
            }

            var currentSatisfied = IsDuesSatisfied(TryGet(currentLookup, member.MemberID));
            var previousSatisfied = IsDuesSatisfied(TryGet(previousLookup, member.MemberID));

            var inGoodStanding = currentSatisfied || (gracePeriodActive && previousSatisfied);
            if (inGoodStanding)
            {
                active++;
            }
            else
            {
                pastDue++;
            }
        }

        return (active, pastDue);
    }

    private static CoreDuesPayment? TryGet(IReadOnlyDictionary<int, CoreDuesPayment> lookup, int memberId)
        => lookup.TryGetValue(memberId, out var payment) ? payment : null;

    private static Dictionary<int, CoreDuesPayment> BuildLatestDuesLookup(IEnumerable<CoreDuesPayment> dues)
    {
        return dues
            .GroupBy(d => d.MemberID)
            .ToDictionary(
                g => g.Key,
                g => g
                    .OrderByDescending(p => p.PaidDate ?? DateTime.MinValue)
                    .ThenByDescending(p => p.Amount ?? 0)
                    .First());
    }

    private static bool IsDuesSatisfied(CoreDuesPayment? payment)
    {
        if (payment == null)
        {
            return false;
        }

        if (string.Equals(payment.PaymentType, "WAIVED", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return payment.PaidDate.HasValue;
    }

    private static bool IsActiveStatus(string normalizedStatus)
        => normalizedStatus is "REGULAR" or "REGULAR-NP" or "LIFE" or "BOARD";

    private static int CalculateOpenAlerts(AlertSummaryDto alert)
    {
        var boardPositions = alert.BoardPositionsUnfilled?.Count ?? 0;
        var boardOpen = alert.BoardAlertYear > 0 && (!alert.BoardConfirmed || boardPositions > 0)
            ? Math.Max(1, boardPositions)
            : 0;

        return Math.Max(0, alert.PhysicalKeysToReturn)
             + Math.Max(0, alert.NpQueueCount)
             + Math.Max(0, alert.OverdueMembers15Months)
             + Math.Max(0, alert.LifeEligibleCount)
             + boardOpen;
    }

    private async Task<(int enabled, int disabled)> GetCardCountsAsync(CancellationToken ct)
    {
        try
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync(ct);
            // TODO: Wire door access metrics to real MemberDoorAccess table once the table and migrations are created.
            // var enabled = await dbContext.MemberDoorAccesses.CountAsync(da => da.IsEnabled, ct);
            // var disabled = await dbContext.MemberDoorAccesses.CountAsync(da => !da.IsEnabled, ct);
            var enabled = 0; // Placeholder until migration
            var disabled = 0; // Placeholder until migration
            return (enabled, disabled);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch card counts");
            return (0, 0);
        }
    }

    private async Task<int> GetRecentMemberChangeCountAsync(CancellationToken ct)
    {
        try
        {
            await using var dbContext = await _contextFactory.CreateDbContextAsync(ct);
            // Placeholder: Wire to real tables later
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch recent member counts");
            return 0;
        }
    }
}

