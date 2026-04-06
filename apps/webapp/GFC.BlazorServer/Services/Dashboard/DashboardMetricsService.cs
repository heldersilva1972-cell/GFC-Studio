using GFC.BlazorServer.Data;
using GFC.BlazorServer.Models.Dashboard;
using GFC.Core.BusinessRules;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GFC.BlazorServer.Services;
using GFC.BlazorServer.Data.Entities;
using CoreDuesPayment = GFC.Core.Models.DuesPayment;

namespace GFC.BlazorServer.Services.Dashboard;

public class DashboardMetricsService : IDashboardMetricsService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly IMemberRepository _memberRepository;
    private readonly IDuesRepository _duesRepository;
    private readonly IDuesYearSettingsRepository _duesYearSettingsRepository;
    private readonly IDashboardService _dashboardService;
    private readonly IBlazorSystemSettingsService _settingsService;
    private readonly IBoardRepository _boardRepository;
    private readonly ILogger<DashboardMetricsService> _logger;

    public DashboardMetricsService(
        IDbContextFactory<GfcDbContext> contextFactory,
        IMemberRepository memberRepository,
        IDuesRepository duesRepository,
        IDuesYearSettingsRepository duesYearSettingsRepository,
        IDashboardService dashboardService,
        IBlazorSystemSettingsService settingsService,
        IBoardRepository boardRepository,
        ILogger<DashboardMetricsService> logger)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _duesYearSettingsRepository = duesYearSettingsRepository ?? throw new ArgumentNullException(nameof(duesYearSettingsRepository));
        _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
        _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
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
            var duesSettingsTask = Task.Run(() => _duesYearSettingsRepository.GetSettingsForYear(currentYear), ct);
            var systemSettingsTask = _settingsService.GetAsync();
            var alertSummaryTask = _dashboardService.GetAlertSummaryAsync(ct);
            var cardCountsTask = GetCardCountsAsync(ct);
            var membershipChangesTask = GetRecentMemberChangeCountAsync(ct);
            var barSalesTask = GetBarSalesMetricsAsync(weekStart, prevWeekStart, ct);
            var staffTask = GetTonightStaffAsync(today, ct);
            var entryCountsTask = GetTodaysEntryCountsAsync(ct);
            // These will now be handled sequentially or with shared data
            // var activityFeedTask = GetRecentActivitiesAsync(ct);
            // var drawStatusTask = GetSignInDrawStatusAsync(ct);

            await Task.WhenAll(
                membersTask,
                currentYearDuesTask,
                previousYearDuesTask,
                duesSettingsTask,
                systemSettingsTask,
                alertSummaryTask,
                cardCountsTask,
                membershipChangesTask,
                barSalesTask,
                staffTask,
                entryCountsTask);

            var members = membersTask.Result;
            var currentYearDues = currentYearDuesTask.Result;
            var previousYearDues = previousYearDuesTask.Result;
            var graceEndDate = duesSettingsTask.Result?.GraceEndDate?.Date;
            var (activeMembers, pastDueMembers) = CalculateMembership(members, currentYearDues, previousYearDues, graceEndDate);

            // Now perform sub-calculations using the already fetched data
            var drawStatus = await GetSignInDrawStatusAsync(members, currentYearDues, previousYearDues, systemSettingsTask.Result, ct);
            var activityFeed = await GetRecentActivitiesAsync(members, ct);

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
                TodaysMemberEntryCount = entryCountsTask.Result.memberCount,
                TodaysBuzzedInCount = entryCountsTask.Result.buzzedInCount,
                TonightBartenders = staffTask.Result,
                RecentActivities = activityFeed,
                SignInDrawReprintRecommended = drawStatus.recommended,
                LastSignInDrawExportDate = drawStatus.lastExport,
                LastSignInDrawChangeDate = drawStatus.lastChange,
                SignInDrawChangeReasons = drawStatus.reasons,
                SignInDrawTotalCount = drawStatus.totalCount,
                UnacknowledgedNotes = await GetUnacknowledgedNotesAsync(ct)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch consolidated dashboard metrics. Returning empty metrics.");
            return new DashboardMetricsDto();
        }
    }

    private async Task<List<ShiftNoteAlert>> GetUnacknowledgedNotesAsync(CancellationToken ct)
    {
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync(ct);
            
            var barNotes = await db.BarSaleEntries
                .AsNoTracking()
                .Where(e => e.Status == "Submitted" && e.Notes != null && e.Notes != "" && e.AcknowledgedAt == null)
                .Select(e => new ShiftNoteAlert
                {
                    RecordId = e.Id,
                    NoteType = "Bar",
                    ShiftDate = e.AdjustedSaleDate ?? e.SaleDate,
                    ShiftType = e.Shift,
                    Author = e.CreatedBy ?? "Unknown",
                    NoteText = e.Notes!,
                    IsRentalHall = e.IsRentalHall
                })
                .ToListAsync(ct);

            var lottoNotes = await db.LotteryShifts
                .AsNoTracking()
                .Where(e => e.Status == "Submitted" && e.Notes != null && e.Notes != "" && e.AcknowledgedAt == null)
                .Select(e => new ShiftNoteAlert
                {
                    RecordId = e.ShiftId,
                    NoteType = "Lottery",
                    ShiftDate = e.ShiftDate,
                    ShiftType = e.ShiftType ?? "Unknown",
                    Author = e.CreatedBy ?? "Unknown",
                    NoteText = e.Notes!
                })
                .ToListAsync(ct);

            return barNotes.Concat(lottoNotes)
                .OrderByDescending(n => n.ShiftDate)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching unacknowledged shift notes");
            return new List<ShiftNoteAlert>();
        }
    }

    private async Task<List<ActivityFeedItem>> GetRecentActivitiesAsync(List<Member> members, CancellationToken ct)
    {
        try
        {
            await using var db = await _contextFactory.CreateDbContextAsync(ct);
            
            // Take more than we need so we can filter duplicates and still have 5 left
            var recentEvents = await db.ControllerEvents
                .Include(e => e.Door)
                .OrderByDescending(e => e.TimestampUtc)
                .ThenByDescending(e => e.RawIndex)
                .Take(30) 
                .ToListAsync(ct);

            _logger.LogInformation($"[RecentActivity] Fetched {recentEvents.Count} candidates for recent events");

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

            // Build a lookup dictionary: CardNumber -> Member Name
            var cardToMemberLookup = new Dictionary<string, string>();
            if (cardNumbers.Any())
            {
                // members is now passed in
                var keyCards = await db.KeyCards
                    .Where(kc => cardNumbers.Contains(kc.CardNumber))
                    .ToListAsync(ct);

                foreach (var card in keyCards)
                {
                    var member = members.FirstOrDefault(m => m.MemberID == card.MemberId);
                    if (member != null)
                    {
                        cardToMemberLookup[card.CardNumber] = FormatMemberName(member);
                    }
                }
            }

            // DEBOUNCE LOGIC for Feed
            var filteredEvents = new List<GFC.BlazorServer.Data.Entities.ControllerEvent>();
            foreach (var e in recentEvents)
            {
                var isDuplicate = filteredEvents.Any(existing => 
                    existing.CardNumber == e.CardNumber && 
                    existing.DoorId == e.DoorId && 
                    existing.EventType == e.EventType &&
                    Math.Abs((e.TimestampUtc - existing.TimestampUtc).TotalSeconds) < 6);

                if (!isDuplicate)
                {
                    filteredEvents.Add(e);
                    if (filteredEvents.Count >= 10) break; // Keep up to 10 events for the combined feed
                }
            }

            foreach (var e in filteredEvents)
            {
                var eventTypeText = e.EventType switch {
                    1 => "Access Granted",
                    2 => "Access Denied",
                    3 => "Door Forced",
                    4 => "Held Open",
                    5 => "Button",
                    _ => "Security Event"
                };

                var cardInfo = "";
                if (e.CardNumber.HasValue && e.CardNumber.Value > 0)
                {
                    var cardNumStr = e.CardNumber.Value.ToString();
                    if (cardToMemberLookup.TryGetValue(cardNumStr, out var memberName))
                        cardInfo = $" - {memberName}";
                    else
                        cardInfo = $" - Card #{e.CardNumber}";
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
                    Title = "Bar Revenue",
                    Detail = $"{s.TotalSales:C} - {s.Notes ?? "General"}",
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
    
    private async Task<(int memberCount, int buzzedInCount)> GetTodaysEntryCountsAsync(CancellationToken ct)
    {
        try
        {
            var settings = await _settingsService.GetAsync();
            var timeZoneId = settings.SystemTimeZoneId ?? "Eastern Standard Time";
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            
            var nowUtc = DateTime.UtcNow;
            var clubNow = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, timeZone);
            var startOfTodayUtc = TimeZoneInfo.ConvertTimeToUtc(clubNow.Date, timeZone);
            
            await using var db = await _contextFactory.CreateDbContextAsync(ct);
            
            var eventsToday = await db.ControllerEvents
                .Where(e => e.TimestampUtc >= startOfTodayUtc)
                .OrderBy(e => e.TimestampUtc)
                .Select(e => new { e.CardNumber, e.DoorId, e.EventType, e.TimestampUtc })
                .ToListAsync(ct);

            // DEBOUNCE LOGIC: Calculate intent entries (collapse hardware bursts)
            var debouncedEvents = new List<GFC.BlazorServer.Data.Entities.ControllerEvent>();
            foreach (var e in eventsToday)
            {
                var isDuplicate = debouncedEvents.Any(existing => 
                    existing.CardNumber == e.CardNumber && 
                    existing.DoorId == e.DoorId && 
                    existing.EventType == e.EventType &&
                    Math.Abs((e.TimestampUtc - existing.TimestampUtc).TotalSeconds) < 6);

                if (!isDuplicate)
                {
                    // Convert the anonymous type back to an entity for the debounced list
                    debouncedEvents.Add(new GFC.BlazorServer.Data.Entities.ControllerEvent
                    {
                        CardNumber = e.CardNumber,
                        DoorId = e.DoorId,
                        EventType = e.EventType,
                        TimestampUtc = e.TimestampUtc
                    });
                }
            }

            // CardNumber == 1 is "Buzzed In"
            var buzzedInCount = debouncedEvents.Count(e => e.CardNumber == 1);
            // CardNumber > 1 is a Member card
            var memberCount = debouncedEvents.Count(e => e.CardNumber > 1);

            return (memberCount, buzzedInCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating today's entry counts");
            return (0, 0);
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
        var gracePeriodActive = graceEndDate.HasValue && DateTime.Today.Date < graceEndDate.Value;

        var statusActiveCount = 0;
        var unpaidCount = 0;

        foreach (var member in members)
        {
            if (MemberStatusHelper.IsPending(member)) continue;
            var normalizedStatus = MemberStatusHelper.NormalizeStatus(member.Status);
            if (!IsActiveStatus(normalizedStatus)) continue;

            // This member is status-active (Regular/Life/NP-Reg/Board)
            statusActiveCount++;

            var currentSatisfied = IsDuesSatisfied(TryGet(currentLookup, member.MemberID));
            var previousSatisfied = IsDuesSatisfied(TryGet(previousLookup, member.MemberID));

            var inGoodStanding = currentSatisfied || (gracePeriodActive && previousSatisfied);
            if (!inGoodStanding)
            {
                unpaidCount++;
            }
        }

        return (statusActiveCount, unpaidCount);
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

    private async Task<(bool recommended, DateTime? lastExport, DateTime? lastChange, List<string> reasons, int totalCount)> GetSignInDrawStatusAsync(
        List<Member> members,
        List<GFC.Core.Models.DuesPayment> currentYearDues,
        List<GFC.Core.Models.DuesPayment> previousYearDues,
        SystemSettings? settings,
        CancellationToken ct)
    {
        try
        {
            if (settings == null) settings = await _settingsService.GetAsync();
            var lastExportRaw = settings.LastSignInDrawExportUtc ?? DateTime.MinValue;
            
            // Use a 60-second buffer to prevent "Reprint Required" from showing up immediately 
            // after a print due to slight timestamp drifts or database clock differences.
            var lastExportBuffered = lastExportRaw.AddSeconds(60);
            
            var reasons = new List<string>();

            var currentYear = DateTime.Today.Year;
            
            // Grace Period Handling
            var duesSettings = await Task.Run(() => _duesYearSettingsRepository.GetSettingsForYear(currentYear), ct);
            var graceEndDate = duesSettings?.GraceEndDate?.Date;
            var isGracePeriodActive = graceEndDate.HasValue && DateTime.Today.Date < graceEndDate.Value;
            
            // Use pre-fetched previous year dues
            var prevPaidIds = previousYearDues
                .Where(d => d.PaidDate.HasValue && !string.Equals(d.PaymentType, "UNPAID", StringComparison.OrdinalIgnoreCase))
                .Select(d => d.MemberID)
                .ToHashSet();

            var paidMemberIds = currentYearDues
                .Where(d => !string.Equals(d.PaymentType, "UNPAID", StringComparison.OrdinalIgnoreCase))
                .Select(d => d.MemberID)
                .ToHashSet();


            var boardAssignments = await Task.Run(() => _boardRepository.GetAssignmentsByYear(currentYear), ct);
            var boardMemberIds = boardAssignments
                .Select(a => a.MemberID)
                .Where(id => id != 0)
                .ToHashSet();

            bool IsIncluded(Member m, bool forGraceCheck)
            {
                var normalized = MemberStatusHelper.NormalizeStatus(m.Status);
                if (normalized is "INACTIVE" or "DECEASED") return false;
                
                bool paidCurrent = paidMemberIds.Contains(m.MemberID);
                bool paidPrev = prevPaidIds.Contains(m.MemberID);
                bool isOnBoard = boardMemberIds.Contains(m.MemberID);

                // If forGraceCheck is true, we assume the grace period is/was active
                return normalized is "LIFE" || isOnBoard || paidCurrent || (forGraceCheck && paidPrev);
            }

            // 1. Check for anyone who meets the criteria and changed recently (ADD)
            var additions = members
                .Where(m => {
                    if (!IsIncluded(m, isGracePeriodActive)) return false;
                    
                    // Was there a status change since last print?
                    if (m.StatusChangeDate.HasValue && m.StatusChangeDate.Value > lastExportBuffered) return true;
                    
                    // Was there a payment since last print?
                    var dues = currentYearDues.FirstOrDefault(d => d.MemberID == m.MemberID);
                    if (dues?.PaidDate != null && dues.PaidDate.Value > lastExportBuffered) return true;
                    
                    return false;
                })
                .ToList();

            foreach (var m in additions)
            {
                reasons.Add($"Add: [{m.MemberID}] {FormatMemberName(m)}");
            }

            // 2. Check for anyone who does NOT meet criteria but changed recently (REMOVE)
            // Or people who lost eligibility because the grace period ended since last print.
            var removals = members
                .Where(m => {
                    bool currentlyIncluded = IsIncluded(m, isGracePeriodActive);
                    if (currentlyIncluded) return false;
                    
                    // If they were included in the last print, they need to be removed now.
                    // Case A: Status changed since last print
                    if (m.StatusChangeDate.HasValue && m.StatusChangeDate.Value > lastExportBuffered) return true;
                    
                    // Case B: Grace period was active during last print, but is not now
                    if (!isGracePeriodActive && graceEndDate.HasValue && lastExportRaw.Date < graceEndDate.Value.Date)
                    {
                        // Were they only in because of last year's dues?
                        if (prevPaidIds.Contains(m.MemberID) && !paidMemberIds.Contains(m.MemberID)) return true;
                    }
                    
                    return false;
                })
                .ToList();

            foreach (var m in removals)
            {
                reasons.Add($"Remove: [{m.MemberID}] {FormatMemberName(m)}");
            }

            // Overall change date for display
            var statusDates = members.Where(m => m.StatusChangeDate.HasValue).Select(m => m.StatusChangeDate!.Value).ToList();
            var paymentDates = currentYearDues.Where(d => d.PaidDate.HasValue).Select(d => d.PaidDate!.Value).ToList();
            var allDates = statusDates.Concat(paymentDates).ToList();
            var lastChange = allDates.Any() ? (DateTime?)allDates.Max() : null;

            var totalCount = members.Count(m => IsIncluded(m, isGracePeriodActive));

            return (reasons.Any(), settings.LastSignInDrawExportUtc, lastChange, reasons.Distinct().OrderBy(r => r).Take(10).ToList(), totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error calculating Sign-in Draw status");
            return (false, null, null, new List<string>(), 0);
        }
    }
    private string FormatMemberName(Member member)
    {
        var first = member.FirstName?.Trim() ?? "";
        var last = member.LastName?.Trim() ?? "";
        var middle = member.MiddleName?.Trim() ?? "";
        var suffix = member.Suffix?.Trim() ?? "";

        var mFirst = first;
        var mLast = last;
        var mMiddle = string.IsNullOrWhiteSpace(middle) ? "" : " " + middle[0] + ".";
        var mSuffix = string.IsNullOrWhiteSpace(suffix) ? "" : " " + suffix;

        if (string.IsNullOrWhiteSpace(mLast)) return (mFirst + mMiddle + mSuffix).Trim();
        return $"{mLast}, {mFirst}{mMiddle}{mSuffix}".Trim().Replace("  ", " ");
    }
}

