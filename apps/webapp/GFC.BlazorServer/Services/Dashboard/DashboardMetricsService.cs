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
        var now = DateTime.UtcNow;
        var today = DateTime.Today;
        var weekStart = today.AddDays(-7);
        var prevWeekStart = today.AddDays(-14);

        var membersTask = Task.Run(() => _memberRepository.GetAllMembers(), ct);
        var currentYearDuesTask = Task.Run(() => _duesRepository.GetDuesForYear(currentYear), ct);
        var previousYearDuesTask = Task.Run(() => _duesRepository.GetDuesForYear(currentYear - 1), ct);
        var settingsTask = Task.Run(() => _duesYearSettingsRepository.GetSettingsForYear(currentYear), ct);
        var alertSummaryTask = _dashboardService.GetAlertSummaryAsync(ct);
        var cardCountsTask = GetCardCountsAsync(ct);
        var membershipChangesTask = GetRecentMemberChangeCountAsync(ct);
        var barSalesTask = GetBarSalesMetricsAsync(weekStart, prevWeekStart, ct);
        var staffTask = GetTonightStaffAsync(today, ct);
        var activityFeedTask = GetRecentActivitiesAsync(ct);

        await Task.WhenAll(
            membersTask,
            currentYearDuesTask,
            previousYearDuesTask,
            settingsTask,
            alertSummaryTask,
            cardCountsTask,
            membershipChangesTask,
            barSalesTask,
            staffTask,
            activityFeedTask);

        var members = membersTask.Result;
        var currentYearDues = currentYearDuesTask.Result;
        var previousYearDues = previousYearDuesTask.Result;
        var graceEndDate = settingsTask.Result?.GraceEndDate?.Date;
        var (activeMembers, pastDueMembers) = CalculateMembership(members, currentYearDues, previousYearDues, graceEndDate);

        var alertSummary = alertSummaryTask.Result;
        var npQueueCount = alertSummary?.NpQueueCount ?? 0;
        var openAlerts = alertSummary is null ? 0 : CalculateOpenAlerts(alertSummary);

        var (enabledCards, disabledCards) = cardCountsTask.Result;
        var (weeklySales, weeklyTransactions, trend) = barSalesTask.Result;

        return new DashboardMetricsDto
        {
            TotalMembers = members.Count,
            ActiveMembers = activeMembers,
            PastDueMembers = pastDueMembers,
            NpQueueCount = npQueueCount,
            EnabledCards = enabledCards,
            DisabledCards = disabledCards,
            OpenAlerts = openAlerts,
            MembershipChangesLast24h = membershipChangesTask.Result,
            
            // Bar & Staff Real Data
            WeeklyBarSales = weeklySales,
            WeeklyBarTransactionCount = weeklyTransactions,
            WeeklyBarSalesTrend = trend,
            TonightBartenders = staffTask.Result,
            RecentActivities = activityFeedTask.Result
        };
    }

    private async Task<List<ActivityFeedItem>> GetRecentActivitiesAsync(CancellationToken ct)
    {
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync(ct);
            
            var recentEvents = await db.ControllerEvents
                .Include(e => e.Door)
                .OrderByDescending(e => e.TimestampUtc)
                .Take(5)
                .ToListAsync(ct);

            var recentSales = await db.BarSaleEntries
                .OrderByDescending(e => e.SaleDate)
                .Take(5)
                .ToListAsync(ct);

            var activities = new List<ActivityFeedItem>();

            foreach (var e in recentEvents)
            {
                activities.Add(new ActivityFeedItem
                {
                    Title = e.Door?.Name ?? "Security Event",
                    Detail = e.EventType switch {
                        1 => "Access Granted",
                        2 => "Access Denied",
                        3 => "Door Opened",
                        4 => "Door Closed",
                        _ => "System Event"
                    } + (e.CardNumber.HasValue ? $" (Card: {e.CardNumber})" : ""),
                    TimestampUtc = e.TimestampUtc
                });
            }

            foreach (var s in recentSales)
            {
                activities.Add(new ActivityFeedItem
                {
                    Title = "Bar Revenue Recorded",
                    Detail = $"{s.TotalSales:C} - {s.Notes ?? "General Sales"}",
                    TimestampUtc = s.SaleDate
                });
            }

            return activities
                .OrderByDescending(a => a.TimestampUtc)
                .Take(5)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching recent activities");
            return new List<ActivityFeedItem>();
        }
    }

    private async Task<(decimal sales, int transactions, double trend)> GetBarSalesMetricsAsync(DateTime weekStart, DateTime prevWeekStart, CancellationToken ct)
    {
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync(ct);
            var currentWeekSales = await db.BarSaleEntries
                .Where(e => e.SaleDate >= weekStart)
                .ToListAsync(ct);

            var prevWeekSales = await db.BarSaleEntries
                .Where(e => e.SaleDate >= prevWeekStart && e.SaleDate < weekStart)
                .ToListAsync(ct);

            var currentTotal = currentWeekSales.Sum(e => e.TotalSales);
            var prevTotal = prevWeekSales.Sum(e => e.TotalSales);
            var transactions = currentWeekSales.Count;

            double trend = 0;
            if (prevTotal > 0)
            {
                trend = (double)((currentTotal - prevTotal) / prevTotal * 100);
            }

            return (currentTotal, transactions, trend);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching bar sales metrics");
            return (0, 0, 0);
        }
    }

    private async Task<List<BartenderInfo>> GetTonightStaffAsync(DateTime today, CancellationToken ct)
    {
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync(ct);
            
            // Look for any shifts scheduled for today (Day=1 or Night=2)
            // Or shifts that are currently active (ClockOutTime is null)
            var activeShifts = await db.StaffShifts
                .Include(s => s.StaffMember)
                .Where(s => s.Date.Date == today.Date || (s.ClockInTime != null && s.ClockOutTime == null))
                .ToListAsync(ct);

            return activeShifts
                .Select(s => new BartenderInfo 
                { 
                    Name = s.StaffMember?.Name ?? "Unknown Staff",
                    Assignment = s.ShiftType switch {
                        1 => "Standard Shift (Day)",
                        2 => "Standard Shift (Night)",
                        _ => s.Status ?? "Assigned"
                    }
                    + (s.ClockOutTime == null && s.ClockInTime != null ? " [LIVE]" : "")
                })
                .DistinctBy(s => s.Name) // Avoid duplicates if someone has multiple entries
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tonight's staff");
            return new List<BartenderInfo>();
        }
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

