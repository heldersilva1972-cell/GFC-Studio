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
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch consolidated dashboard metrics. Returning empty metrics.");
            return new DashboardMetricsDto();
        }
    }

    private async Task<List<ActivityFeedItem>> GetRecentActivitiesAsync(CancellationToken ct)
    {
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync(ct);
            
            var recentEvents = await db.ControllerEvents
                .Include(e => e.Door)
                .OrderByDescending(e => e.TimestampUtc)
                .ThenByDescending(e => e.RawIndex)
                .Take(5)
                .ToListAsync(ct);

            _logger.LogInformation($"[RecentActivity] Found {recentEvents.Count} recent events");

            var recentSales = await db.BarSaleEntries
                .OrderByDescending(e => e.SaleDate)
                .Take(5)
                .ToListAsync(ct);

            var activities = new List<ActivityFeedItem>();

            // Get card numbers from events to look up member names
            var cardNumbers = recentEvents
                .Where(e => e.CardNumber.HasValue && e.CardNumber.Value > 0)
                .Select(e => e.CardNumber!.Value.ToString())
                .Distinct()
                .ToList();

            _logger.LogInformation($"[RecentActivity] Card numbers to lookup: {string.Join(", ", cardNumbers)}");

            // Build a lookup dictionary: CardNumber -> Member Name
            var cardToMemberLookup = new Dictionary<string, string>();
            if (cardNumbers.Any())
            {
                var members = await Task.Run(() => _memberRepository.GetAllMembers(), ct);
                _logger.LogInformation($"[RecentActivity] Loaded {members.Count} total members");

                var keyCards = await db.KeyCards
                    .Where(kc => cardNumbers.Contains(kc.CardNumber))
                    .ToListAsync(ct);

                _logger.LogInformation($"[RecentActivity] Found {keyCards.Count} matching KeyCards");

                foreach (var card in keyCards)
                {
                    var member = members.FirstOrDefault(m => m.MemberID == card.MemberId);
                    if (member != null)
                    {
                        var memberName = $"{member.FirstName} {member.LastName}";
                        cardToMemberLookup[card.CardNumber] = memberName;
                        _logger.LogInformation($"[RecentActivity] Mapped card {card.CardNumber} -> {memberName}");
                    }
                    else
                    {
                        _logger.LogWarning($"[RecentActivity] No member found for card {card.CardNumber} (MemberId: {card.MemberId})");
                    }
                }
            }

            foreach (var e in recentEvents)
            {
                var eventTypeText = e.EventType switch {
                    1 => "Access Granted",
                    2 => "Access Denied",
                    3 => "Door Forced Open",
                    4 => "Door Held Open",
                    5 => "Button Press",
                    _ => "Security Event"
                };

                var cardInfo = "";
                if (e.CardNumber.HasValue && e.CardNumber.Value > 0)
                {
                    var cardNumStr = e.CardNumber.Value.ToString();
                    if (cardToMemberLookup.TryGetValue(cardNumStr, out var memberName))
                    {
                        cardInfo = $" - {memberName} (Card #{e.CardNumber})";
                        _logger.LogInformation($"[RecentActivity] Event {e.Id}: Found member name for card {cardNumStr}");
                    }
                    else
                    {
                        cardInfo = $" - Card #{e.CardNumber}";
                        _logger.LogWarning($"[RecentActivity] Event {e.Id}: No member name found for card {cardNumStr}");
                    }
                }

                activities.Add(new ActivityFeedItem
                {
                    Title = e.Door?.Name ?? $"Door {e.DoorOrReader}",
                    Detail = eventTypeText + cardInfo,
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
            if (MemberStatusHelper.IsPending(member))
            {
                continue;
            }

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
            var enabled = await dbContext.MemberDoorAccesses.CountAsync(da => da.IsEnabled, ct);
            var disabled = await dbContext.MemberDoorAccesses.CountAsync(da => !da.IsEnabled, ct);
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

