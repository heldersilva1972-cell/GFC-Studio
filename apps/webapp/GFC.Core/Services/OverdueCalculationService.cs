using GFC.Core.BusinessRules;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

/// <summary>
/// Service for calculating member dues overdue status.
/// Determines overdue months based on first unpaid year's due date.
/// </summary>
public class OverdueCalculationService
{
    private readonly IDuesRepository _duesRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IDuesWaiverRepository _waiverRepository;
    
    // Club policy: Dues are due on January 1st of each year
    // Grace period: 0 days (members are overdue immediately after due date)
    private const int GraceDays = 0;
    
    public OverdueCalculationService(
        IDuesRepository duesRepository,
        IBoardRepository boardRepository,
        IDuesWaiverRepository waiverRepository)
    {
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
        _waiverRepository = waiverRepository ?? throw new ArgumentNullException(nameof(waiverRepository));
    }

    /// <summary>
    /// Holds pre-loaded data for bulk dues/overdue calculations to prevent N+1 queries.
    /// </summary>
    public class DuesCalculationContext
    {
        public Dictionary<int, List<DuesPayment>> DuesByMember { get; set; } = new();
        public Dictionary<int, List<DuesWaiverPeriod>> WaiversByMember { get; set; } = new();
        public Dictionary<int, HashSet<int>> BoardAssignmentsByMember { get; set; } = new();
        public DateTime Today { get; set; } = DateTime.Today;
        public DateTime? GraceEndDate { get; set; }
    }

    /// <summary>
    /// Calculates overdue information for a member using a pre-loaded context.
    /// </summary>
    public OverdueResult CalculateOverdue(Member member, DuesCalculationContext context)
    {
        if (member == null || context == null)
            return new OverdueResult { IsOverdue = false, MonthsOverdue = 0, DaysOverdue = 0 };

        var today = context.Today.Date;
        
        // Exclusions: Life members and waived members are never overdue
        if (MemberStatusHelper.IsLifeStatus(member.Status))
        {
            return new OverdueResult { IsOverdue = false, MonthsOverdue = 0, DaysOverdue = 0 };
        }

        // Get all dues records for this member from context
        context.DuesByMember.TryGetValue(member.MemberID, out var allDues);
        allDues ??= new List<DuesPayment>();
        
        // Get waivers for this member from context
        context.WaiversByMember.TryGetValue(member.MemberID, out var waivers);
        waivers ??= new List<DuesWaiverPeriod>();
        
        // Determine LastCoveredYear (highest year where member is paid or waived)
        int? lastCoveredYear = GetLastCoveredYear(member, allDues, waivers, context);
        
        // Determine FirstUnpaidYear
        int firstUnpaidYear;
        if (lastCoveredYear.HasValue)
        {
            firstUnpaidYear = lastCoveredYear.Value + 1;
        }
        else
        {
            // No payments/waivers yet - determine base dues year
            if (member.AcceptedDate.HasValue)
            {
                var acceptanceYear = member.AcceptedDate.Value.Year;
                var acceptanceMonth = member.AcceptedDate.Value.Month;
                firstUnpaidYear = acceptanceMonth < 3 ? acceptanceYear : acceptanceYear + 1;
            }
            else if (member.ApplicationDate.HasValue)
            {
                var applicationYear = member.ApplicationDate.Value.Year;
                var applicationMonth = member.ApplicationDate.Value.Month;
                firstUnpaidYear = applicationMonth < 3 ? applicationYear : applicationYear + 1;
            }
            else
            {
                firstUnpaidYear = today.Year;
            }
        }
        
        // Check if member has waiver or board status for first unpaid year or later
        bool hasActiveWaiver = HasActiveWaiverOrBoardStatus(member, firstUnpaidYear, today.Year, waivers, context);
        
        if (hasActiveWaiver)
        {
            return new OverdueResult { IsOverdue = false, MonthsOverdue = 0, DaysOverdue = 0 };
        }
        
        // Calculate due date for first unpaid year
        DateTime dueDate = new DateTime(firstUnpaidYear, 1, 1).AddDays(GraceDays);
        
        if (today < dueDate)
        {
            return new OverdueResult
            {
                IsOverdue = false,
                MonthsOverdue = 0,
                DaysOverdue = 0,
                FirstUnpaidYear = firstUnpaidYear,
                DueDate = dueDate,
                LastCoveredYear = lastCoveredYear
            };
        }

        // Check if we are within the grace period for the first unpaid year
        // Only applies if the first unpaid year matches the year of the GraceEndDate
        if (context.GraceEndDate.HasValue && 
            context.GraceEndDate.Value.Year == firstUnpaidYear && 
            today <= context.GraceEndDate.Value)
        {
            return new OverdueResult
            {
                IsOverdue = false,
                MonthsOverdue = 0,
                DaysOverdue = 0,
                FirstUnpaidYear = firstUnpaidYear,
                DueDate = context.GraceEndDate.Value,
                LastCoveredYear = lastCoveredYear,
                IsInGracePeriod = true
            };
        }
        
        int daysOverdue = (today - dueDate).Days;
        int monthsOverdue = CalculateFullMonths(dueDate, today) + 1;
        
        return new OverdueResult
        {
            IsOverdue = true,
            MonthsOverdue = monthsOverdue,
            DaysOverdue = daysOverdue,
            FirstUnpaidYear = firstUnpaidYear,
            DueDate = dueDate,
            LastCoveredYear = lastCoveredYear
        };
    }

    /// <summary>
    /// Calculates overdue information for a member.
    /// </summary>
    /// <param name="member">The member to calculate overdue for.</param>
    /// <param name="asOfDate">The date to calculate overdue as of (defaults to today).</param>
    /// <returns>Overdue calculation result with months overdue, days overdue, and related dates.</returns>
    public OverdueResult CalculateOverdue(Member member, DateTime? asOfDate = null)
    {
        if (member == null)
            return new OverdueResult { IsOverdue = false, MonthsOverdue = 0, DaysOverdue = 0 };

        var today = (asOfDate ?? DateTime.Today).Date;
        
        // Exclusions: Life members and waived members are never overdue
        if (MemberStatusHelper.IsLifeStatus(member.Status))
        {
            return new OverdueResult { IsOverdue = false, MonthsOverdue = 0, DaysOverdue = 0 };
        }

        // Get all dues records for this member
        var allDues = _duesRepository.GetDuesForMember(member.MemberID);
        
        // Get waivers for this member
        var waivers = _waiverRepository.GetWaiversForMember(member.MemberID);
        
        // Determine LastCoveredYear (highest year where member is paid or waived)
        int? lastCoveredYear = GetLastCoveredYear(member, allDues, waivers, today);
        
        // Determine FirstUnpaidYear
        int firstUnpaidYear;
        if (lastCoveredYear.HasValue)
        {
            firstUnpaidYear = lastCoveredYear.Value + 1;
        }
        else
        {
            // No payments/waivers yet - determine base dues year
            // Rule: First dues year is the year after acceptance, or the year of acceptance
            // if accepted early in the year (before March)
            if (member.AcceptedDate.HasValue)
            {
                var acceptanceYear = member.AcceptedDate.Value.Year;
                var acceptanceMonth = member.AcceptedDate.Value.Month;
                
                // If accepted before March, dues start in acceptance year
                // Otherwise, dues start in the following year
                firstUnpaidYear = acceptanceMonth < 3 ? acceptanceYear : acceptanceYear + 1;
            }
            else if (member.ApplicationDate.HasValue)
            {
                var applicationYear = member.ApplicationDate.Value.Year;
                var applicationMonth = member.ApplicationDate.Value.Month;
                firstUnpaidYear = applicationMonth < 3 ? applicationYear : applicationYear + 1;
            }
            else
            {
                // Fallback: use current year
                firstUnpaidYear = today.Year;
            }
        }
        
        // Check if member has waiver or board status for first unpaid year or later
        bool hasActiveWaiver = HasActiveWaiverOrBoardStatus(member, firstUnpaidYear, today.Year, waivers, today);
        
        if (hasActiveWaiver)
        {
            return new OverdueResult { IsOverdue = false, MonthsOverdue = 0, DaysOverdue = 0 };
        }
        
        // Calculate due date for first unpaid year
        // Dues are due January 1st of the year + grace period
        DateTime dueDate = new DateTime(firstUnpaidYear, 1, 1).AddDays(GraceDays);
        
        // If today is before the due date, member is not overdue
        if (today < dueDate)
        {
            return new OverdueResult
            {
                IsOverdue = false,
                MonthsOverdue = 0,
                DaysOverdue = 0,
                FirstUnpaidYear = firstUnpaidYear,
                DueDate = dueDate,
                LastCoveredYear = lastCoveredYear
            };
        }
        
        // Calculate days overdue
        int daysOverdue = (today - dueDate).Days;
        
        // Calculate months overdue (inclusive of current month)
        int monthsOverdue = CalculateFullMonths(dueDate, today) + 1;
        
        return new OverdueResult
        {
            IsOverdue = true,
            MonthsOverdue = monthsOverdue,
            DaysOverdue = daysOverdue,
            FirstUnpaidYear = firstUnpaidYear,
            DueDate = dueDate,
            LastCoveredYear = lastCoveredYear
        };
    }

    /// <summary>
    /// Gets the highest year where the member is covered (paid or waived).
    /// </summary>
    private int? GetLastCoveredYear(Member member, List<DuesPayment> allDues, List<DuesWaiverPeriod> waivers, object contextOrDate)
    {
        var coveredYears = new HashSet<int>();
        DateTime today;
        DuesCalculationContext? context = contextOrDate as DuesCalculationContext;

        if (context != null)
        {
            today = context.Today;
        }
        else
        {
            today = (DateTime)contextOrDate;
        }
        
        // Add years from paid dues
        foreach (var dues in allDues)
        {
            if (dues.PaidDate.HasValue && !IsWaivedPayment(dues))
            {
                coveredYears.Add(dues.Year);
            }
        }
        
        // Add years from waived dues
        foreach (var dues in allDues)
        {
            if (IsWaivedPayment(dues))
            {
                coveredYears.Add(dues.Year);
            }
        }
        
        // Add years from waiver periods
        foreach (var waiver in waivers)
        {
            for (int year = waiver.StartYear; year <= waiver.EndYear; year++)
            {
                coveredYears.Add(year);
            }
        }
        
        // Add years where member is Life (covered from life eligibility year onwards)
        if (MemberStatusHelper.IsLifeStatus(member.Status) && member.LifeEligibleDate.HasValue)
        {
            var lifeYear = member.LifeEligibleDate.Value.Year;
            for (int year = lifeYear; year <= today.Year + 1; year++)
            {
                coveredYears.Add(year);
            }
        }
        
        // Add years where member is on board (board members are auto-waived)
        // Check current year and next year for board status
        for (int year = today.Year - 1; year <= today.Year + 1; year++)
        {
            bool isBoard;
            if (context != null)
            {
                isBoard = context.BoardAssignmentsByMember.TryGetValue(member.MemberID, out var assignments) && assignments.Contains(year);
            }
            else
            {
                isBoard = _boardRepository.IsBoardMemberForYear(member.MemberID, year);
            }

            if (isBoard)
            {
                coveredYears.Add(year);
            }
        }
        
        return coveredYears.Count > 0 ? coveredYears.Max() : (int?)null;
    }

    /// <summary>
    /// Checks if member has an active waiver or board status for the given year range.
    /// </summary>
    private bool HasActiveWaiverOrBoardStatus(Member member, int startYear, int endYear, List<DuesWaiverPeriod> waivers, object contextOrToday)
    {
        DateTime today;
        DuesCalculationContext? context = contextOrToday as DuesCalculationContext;

        if (context != null)
        {
            today = context.Today;
        }
        else
        {
            today = (DateTime)contextOrToday;
        }

        // Check if Life member
        if (MemberStatusHelper.IsLifeStatus(member.Status))
        {
            if (member.LifeEligibleDate.HasValue)
            {
                var lifeYear = member.LifeEligibleDate.Value.Year;
                if (startYear >= lifeYear)
                {
                    return true;
                }
            }
        }
        
        // Check board status for any year in range
        for (int year = startYear; year <= endYear; year++)
        {
            bool isBoard;
            if (context != null)
            {
                isBoard = context.BoardAssignmentsByMember.TryGetValue(member.MemberID, out var assignments) && assignments.Contains(year);
            }
            else
            {
                isBoard = _boardRepository.IsBoardMemberForYear(member.MemberID, year);
            }

            if (isBoard)
            {
                return true;
            }
        }
        
        // Check waiver periods
        foreach (var waiver in waivers)
        {
            if (waiver.StartYear <= endYear && waiver.EndYear >= startYear)
            {
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// Calculates the number of full calendar months between two dates.
    /// </summary>
    private static int CalculateFullMonths(DateTime start, DateTime end)
    {
        if (end < start)
            return 0;
        
        // Normalize to start of day
        start = start.Date;
        end = end.Date;
        
        int months = 0;
        DateTime current = start;
        
        // Add months one at a time until we exceed the end date
        while (current <= end)
        {
            DateTime nextMonth = current.AddMonths(1);
            if (nextMonth > end)
                break;
            
            months++;
            current = nextMonth;
        }
        
        return months;
    }

    private static bool IsWaivedPayment(DuesPayment payment)
    {
        return string.Equals(payment.PaymentType, "WAIVED", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets count of members who are 15+ months overdue.
    /// </summary>
    public int GetOverdue15PlusMonthsCount(List<Member> members, DateTime? asOfDate = null)
    {
        if (members == null || members.Count == 0)
            return 0;
        
        int count = 0;
        foreach (var member in members)
        {
            // Skip inactive and deceased members
            if (string.Equals(member.Status, "INACTIVE", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(member.Status, "DECEASED", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            
            var result = CalculateOverdue(member, asOfDate);
            if (result.IsOverdue && result.MonthsOverdue >= 15)
            {
                count++;
            }
        }
        
        return count;
    }
}

/// <summary>
/// Result of overdue calculation for a member.
/// </summary>
public class OverdueResult
{
    public bool IsOverdue { get; set; }
    public int MonthsOverdue { get; set; }
    public int DaysOverdue { get; set; }
    public int FirstUnpaidYear { get; set; }
    public DateTime DueDate { get; set; }
    public int? LastCoveredYear { get; set; }
    public bool IsInGracePeriod { get; set; }
}



