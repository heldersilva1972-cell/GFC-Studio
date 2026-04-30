using GFC.BlazorServer.Data;
using GFC.BlazorServer.Models.Dashboard;
using GFC.Core.BusinessRules;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GFC.BlazorServer.Services;
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
            var today = DateTime.Today;
            var weekStart = today.AddDays(-7);
            var prevWeekStart = today.AddDays(-14);

            // 1. Start ALL data-fetching tasks in parallel
            var membersTask = Task.Run(() => _memberRepository.GetAllMembers(), ct);
            
            // This task waits for membersTask internally but allows other tasks to start immediately
            var alertSummaryTask = Task.Run(async () => {
                var membersResult = await membersTask;
                return await _dashboardService.GetAlertSummaryAsync(membersResult, ct);
            }, ct);

            var currentYearDuesTask = Task.Run(() => _duesRepository.GetDuesForYear(currentYear), ct);
            var previousYearDuesTask = Task.Run(() => _duesRepository.GetDuesForYear(currentYear - 1), ct);
            var duesSettingsTask = Task.Run(() => _duesYearSettingsRepository.GetSettingsForYear(currentYear), ct);
            var systemSettingsTask = _settingsService.GetAsync();
            var cardCountsTask = GetCardCountsAsync(ct);
            var membershipChangesTask = GetRecentMemberChangeCountAsync(ct);
            var barSalesTask = GetBarSalesMetricsAsync(weekStart, prevWeekStart, ct);
            var staffTask = GetTonightStaffAsync(today, ct);
            var entryCountsTask = GetTodaysEntryCountsAsync(ct);
            var boardAssignmentsTask = Task.Run(() => _boardRepository.GetAssignmentsByYear(currentYear), ct);
            var unacknowledgedNotesTask = GetUnacknowledgedNotesAsync(ct);

            // 2. Start dependent processing tasks as early as possible
            var drawStatusTask = Task.Run(async () => {
                await Task.WhenAll(membersTask, currentYearDuesTask, previousYearDuesTask, systemSettingsTask, duesSettingsTask, boardAssignmentsTask);
                return await GetSignInDrawStatusAsync(
                    membersTask.Result, 
                    currentYearDuesTask.Result, 
                    previousYearDuesTask.Result, 
                    systemSettingsTask.Result, 
                    duesSettingsTask.Result, 
                    boardAssignmentsTask.Result, 
                    ct);
            }, ct);

            var recentActivitiesTask = Task.Run(async () => {
                var members = await membersTask;
                return await GetRecentActivitiesAsync(members, ct);
            }, ct);

            // 3. Wait for everything
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
                entryCountsTask,
                boardAssignmentsTask,
                unacknowledgedNotesTask,
                drawStatusTask,
                recentActivitiesTask);

            var members = membersTask.Result;
            var currentYearDues = currentYearDuesTask.Result;
            var previousYearDues = previousYearDuesTask.Result;
            var systemSettings = systemSettingsTask.Result;
            var duesSettings = duesSettingsTask.Result;
            var alertSummary = alertSummaryTask.Result;
            var boardAssignments = boardAssignmentsTask.Result;

            // 4. Perform final synchronous calculations
            var (activeMembers, pastDueMembers) = CalculateMembership(members, currentYearDues, previousYearDues, duesSettings?.GraceEndDate?.Date);
            var openAlerts = alertSummary == null ? 0 : CalculateOpenAlerts(alertSummary);
            var (enabledCards, disabledCards) = cardCountsTask.Result;
            var (weeklySales, weeklyTransactions, trend) = barSalesTask.Result;
            var (recommended, lastExport, lastChange, reasons, drawTotal) = drawStatusTask.Result;

            return new DashboardMetricsDto
            {
                AlertSummary = alertSummary,
                TotalMembers = members.Count,
                ActiveMembers = activeMembers,
                PastDueMembers = pastDueMembers,
                NpQueueCount = alertSummary?.NpQueueCount ?? 0,
                EnabledCards = enabledCards,
                DisabledCards = disabledCards,
                OpenAlerts = openAlerts,
                MembershipChangesLast24h = membershipChangesTask.Result,
                
                WeeklyBarSales = weeklySales,
                WeeklyBarTransactionCount = weeklyTransactions,
                WeeklyBarSalesTrend = trend,
                TodaysMemberEntryCount = entryCountsTask.Result.memberCount,
                TodaysBuzzedInCount = entryCountsTask.Result.buzzedInCount,
                TonightBartenders = staffTask.Result,
                RecentActivities = recentActivitiesTask.Result,
                
                SignInDrawReprintRecommended = recommended,
                LastSignInDrawExportDate = lastExport,
                LastSignInDrawChangeDate = lastChange,
                SignInDrawChangeReasons = reasons,
                SignInDrawTotalCount = drawTotal,
                UnacknowledgedNotes = unacknowledgedNotesTask.Result
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch consolidated dashboard metrics");
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

            var recentSales = await db.BarSaleEntries
                .OrderByDescending(e => e.SaleDate)
                .Take(5)
                .ToListAsync(ct);

            var activities = new List<ActivityFeedItem>();
            var cardNumbers = recentEvents
                .Where(e => e.CardNumber.HasValue && e.CardNumber.Value > 0)
                .Select(e => e.CardNumber!.Value.ToString())
                .Distinct()
                .ToList();

            var memberLookup = members.ToDictionary(m => m.MemberID);
            var cardToMemberLookup = new Dictionary<string, string>();
            
            if (cardNumbers.Any())
            {
                var keyCards = await db.KeyCards
                    .Where(kc => cardNumbers.Contains(kc.CardNumber))
                    .ToListAsync(ct);

                foreach (var card in keyCards)
                {
                    if (memberLookup.TryGetValue(card.MemberId, out var member))
                    {
                        cardToMemberLookup[card.CardNumber] = $"{member.LastName}, {member.FirstName}";
                    }
                }
            }

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

            // CardNumber == 1 is "Buzzed In", CardNumber > 1 is a Member card
            int memberCount = 0;
            int buzzedInCount = 0;

            // O(M) Debounce Logic: Group by key and use last timestamp
            var lastEventPerKey = new Dictionary<string, DateTime>();
            
            foreach (var e in eventsToday)
            {
                var key = $"{e.CardNumber}_{e.DoorId}_{e.EventType}";
                if (lastEventPerKey.TryGetValue(key, out var lastTime))
                {
                    if ((e.TimestampUtc - lastTime).TotalSeconds < 6)
                    {
                        continue; // Skip burst
                    }
                }

                lastEventPerKey[key] = e.TimestampUtc;
                if (e.CardNumber == 1) buzzedInCount++;
                else if (e.CardNumber > 1) memberCount++;
            }

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
            
            // Optimization: Use SumAsync and CountAsync to avoid downloading all records
            var currentTotal = await db.BarSaleEntries
                .Where(e => e.SaleDate >= weekStart)
                .SumAsync(e => e.TotalSales, ct);

            var transactions = await db.BarSaleEntries
                .Where(e => e.SaleDate >= weekStart)
                .CountAsync(ct);

            var prevTotal = await db.BarSaleEntries
                .Where(e => e.SaleDate >= prevWeekStart && e.SaleDate < weekStart)
                .SumAsync(e => (decimal?)e.TotalSales, ct) ?? 0; // Use nullable to handle empty set

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
        DuesYearSettings? duesSettings,
        List<BoardAssignment>? boardAssignments,
        CancellationToken ct)
    {
        try
        {
            if (settings == null) settings = await _settingsService.GetAsync();
            var lastExportRaw = settings.LastSignInDrawExportUtc ?? DateTime.MinValue;
            
            var lastExportBuffered = lastExportRaw.AddSeconds(60);
            
            var reasons = new List<string>();

            var currentYear = DateTime.Today.Year;
            
            // Grace Period Handling (Use pre-fetched if available)
            if (duesSettings == null) duesSettings = await Task.Run(() => _duesYearSettingsRepository.GetSettingsForYear(currentYear), ct);
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

            // Use pre-fetched board assignments if available
            if (boardAssignments == null) boardAssignments = await Task.Run(() => _boardRepository.GetAssignmentsByYear(currentYear), ct);
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

            // Use a dictionary for fast lookup in loops
            var currentYearDuesLookup = currentYearDues
                .GroupBy(d => d.MemberID)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(d => d.PaidDate ?? DateTime.MinValue).First());

            // 1. Check for anyone who meets the criteria and changed recently (ADD)
            var additions = members
                .Where(m => {
                    if (!IsIncluded(m, isGracePeriodActive)) return false;
                    
                    // Was there a status change since last print?
                    if (m.StatusChangeDate.HasValue && m.StatusChangeDate.Value > lastExportBuffered) return true;
                    
                    // Was there a payment since last print?
                    if (currentYearDuesLookup.TryGetValue(m.MemberID, out var dues))
                    {
                        if (dues?.PaidDate != null && dues.PaidDate.Value > lastExportBuffered) return true;
                    }
                    
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



