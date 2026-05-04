using GFC.Core.BusinessRules;
using GFC.Core.DTOs;
using GFC.Core.Enums;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GFC.Core.Services;

public class DuesInsightService : IDuesInsightService
{
    private readonly IDuesRepository _duesRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly OverdueCalculationService _overdueService;
    private readonly IBoardRepository _boardRepository;
    private readonly IDuesWaiverRepository _waiverRepository;
    private readonly IDuesYearSettingsRepository _settingsRepository;

    public DuesInsightService(
        IDuesRepository duesRepository,
        IMemberRepository memberRepository,
        OverdueCalculationService overdueService,
        IBoardRepository boardRepository,
        IDuesWaiverRepository waiverRepository,
        IDuesYearSettingsRepository settingsRepository)
    {
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _overdueService = overdueService ?? throw new ArgumentNullException(nameof(overdueService));
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
        _waiverRepository = waiverRepository ?? throw new ArgumentNullException(nameof(waiverRepository));
        _settingsRepository = settingsRepository ?? throw new ArgumentNullException(nameof(settingsRepository));
    }

    private static (List<DuesListItemDto> Records, DateTime Timestamp) _cache = (new(), DateTime.MinValue);
    private static readonly SemaphoreSlim _cacheLock = new(1, 1);
    private const int CacheDurationSeconds = 30;

    public async Task<IReadOnlyList<DuesListItemDto>> GetDuesAsync(int year, bool paidTab, CancellationToken cancellationToken = default)
    {
        var allRecords = await GetAllDuesInternalAsync(year, cancellationToken);
        return allRecords.Where(r => r.Satisfied == paidTab).ToList();
    }

    public async Task<DuesSummaryDto> GetSummaryAsync(int year, CancellationToken cancellationToken = default)
    {
        // [OPTIMIZATION] Call internal method once to get all data, instead of calling GetDuesAsync twice.
        // This eliminates redundant database queries and O(N) calculations.
        var allRecords = await GetAllDuesInternalAsync(year, cancellationToken);
        
        var paidCount = allRecords.Count(d => d.Satisfied && !d.IsWaived && d.Status != MemberStatus.Pending);
        var waivedCount = allRecords.Count(d => d.IsWaived && d.Status != MemberStatus.Pending);
        var unpaidCount = allRecords.Count(d => !d.Satisfied && d.Status != MemberStatus.Pending);
        var pendingCount = allRecords.Count(d => d.Status == MemberStatus.Pending);
        var amountCollected = allRecords.Where(d => d.PaidDate.HasValue && !d.IsWaived).Sum(d => d.Amount ?? 0m);

        return new DuesSummaryDto(year, paidCount, unpaidCount, waivedCount, amountCollected, pendingCount);
    }

    private async Task<List<DuesListItemDto>> GetAllDuesInternalAsync(int year, CancellationToken cancellationToken)
    {
        // [CACHE CHECK] Rapid-fire mobile syncs shouldn't hit the DB every time
        await _cacheLock.WaitAsync(cancellationToken);
        try
        {
            if (_cache.Records.Any() && (DateTime.Now - _cache.Timestamp).TotalSeconds < CacheDurationSeconds)
            {
                return _cache.Records;
            }
        }
        finally { _cacheLock.Release(); }

        // [PERFORMANCE] Fetch all required data in parallel to minimize latency
        var membersTask = Task.Run(() => _memberRepository.GetAllMembers(), cancellationToken);
        var duesHistoryTask = Task.Run(() => _duesRepository.GetAllDues(), cancellationToken);
        var waiversTask = Task.Run(() => _waiverRepository.GetAllWaivers(), cancellationToken);
        var boardAssignmentsTask = Task.Run(() => _boardRepository.GetAllAssignments(), cancellationToken);
        var settingsTask = Task.Run(() => _settingsRepository.GetSettingsForYear(year), cancellationToken);

        await Task.WhenAll(membersTask, duesHistoryTask, waiversTask, boardAssignmentsTask, settingsTask);

        var members = await membersTask ?? new List<Member>();
        var duesHistory = await duesHistoryTask ?? new List<DuesPayment>();
        var waivers = await waiversTask ?? new List<DuesWaiverPeriod>();
        var boardAssignments = await boardAssignmentsTask ?? new List<BoardAssignment>();
        var settings = await settingsTask;

        var context = new OverdueCalculationService.DuesCalculationContext
        {
            DuesByMember = duesHistory.GroupBy(d => d.MemberID).ToDictionary(g => g.Key, g => g.ToList()),
            WaiversByMember = waivers.GroupBy(w => w.MemberId).ToDictionary(g => g.Key, g => g.ToList()),
            BoardAssignmentsByMember = boardAssignments.GroupBy(a => a.MemberID).ToDictionary(g => g.Key, g => g.Select(a => a.TermYear).ToHashSet()),
            Today = DateTime.Today,
            GraceEndDate = settings?.GraceEndDate
        };

        var duesLookup = duesHistory.Where(d => d.Year == year).ToLookup(d => d.MemberID);
        var waiverLookup = waivers.Where(w => year >= w.StartYear && year <= w.EndYear).ToLookup(w => w.MemberId);

        var list = new List<DuesListItemDto>();
        foreach (var member in members)
        {
            if (!MemberFilters.IsActiveForDuesYear(member, year)) continue;

            var record = duesLookup[member.MemberID].FirstOrDefault();
            var status = MapStatus(member);
            var isBoard = context.BoardAssignmentsByMember.TryGetValue(member.MemberID, out var boardYears) && boardYears.Contains(year);
            var isLife = status == MemberStatus.Life;
            
            var isWaived = isLife || isBoard || waiverLookup.Contains(member.MemberID);
            var isPaid = record != null && (record.PaidDate.HasValue || string.Equals(record.PaymentType, "WAIVED", StringComparison.OrdinalIgnoreCase));
            var isSatisfied = isPaid || isWaived;

            var overdueMonths = 0;
            var overdueDays = 0;
            DateTime? dueDate = null;
            var isInGracePeriod = false;
            
            if (!isSatisfied)
            {
                var overdueResult = _overdueService.CalculateOverdue(member, context);
                if (overdueResult.IsOverdue && overdueResult.FirstUnpaidYear <= year)
                {
                    overdueMonths = overdueResult.MonthsOverdue;
                    overdueDays = overdueResult.DaysOverdue;
                    dueDate = overdueResult.DueDate;
                }
                isInGracePeriod = overdueResult.IsInGracePeriod;
            }

            string? waiverReason = null;
            if (isWaived)
            {
                if (isLife) waiverReason = "Life Member";
                else if (isBoard) waiverReason = "Board Member";
                else waiverReason = waiverLookup[member.MemberID].FirstOrDefault()?.Reason ?? "Waived";
            }

            List<int>? advanceYears = null;
            if (context.DuesByMember.TryGetValue(member.MemberID, out var allMemberDues))
            {
                advanceYears = allMemberDues
                    .Where(d => d.Year > year && d.PaidDate.HasValue && !string.Equals(d.PaymentType, "WAIVED", StringComparison.OrdinalIgnoreCase))
                    .Select(d => d.Year)
                    .OrderBy(y => y)
                    .ToList();
                
                if (advanceYears.Count == 0) advanceYears = null;
            }

            string? pendingReason = null;
            if (status == MemberStatus.Pending)
            {
                if (!member.ApplicationDate.HasValue) pendingReason = "Missing Application Date";
                else if (!member.AcceptedDate.HasValue) pendingReason = "Waiting for Acceptance Date";
                else pendingReason = "Incomplete Setup";
            }

            list.Add(new DuesListItemDto(
                member.MemberID,
                FormatMemberName(member),
                status,
                year,
                record?.Amount,
                record?.PaidDate,
                record?.PaymentType ?? string.Empty,
                overdueMonths,
                overdueDays,
                dueDate,
                isSatisfied,
                isWaived,
                waiverReason,
                record?.Notes ?? string.Empty,
                isBoard,
                isInGracePeriod,
                member.IsNonPortugueseOrigin,
                record?.RecordedBy,
                advanceYears,
                pendingReason));
        }

        var result = list.OrderBy(i => i.FullName).ToList();

        // [CACHE UPDATE]
        await _cacheLock.WaitAsync(cancellationToken);
        try
        {
            _cache = (result, DateTime.Now);
        }
        finally { _cacheLock.Release(); }

        return result;
    }



    public async Task<IEnumerable<int>> GetAvailableYearsAsync(CancellationToken cancellationToken = default)
    {
        var years = await Task.Run(() => _duesRepository.GetDistinctYears(), cancellationToken);
        var currentYear = DateTime.Now.Year;
        if (!years.Contains(currentYear)) years.Add(currentYear);
        if (!years.Contains(currentYear + 1)) years.Add(currentYear + 1);
        return years.OrderBy(y => y);
    }

    private static string FormatMemberName(Member member)
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

    private static MemberStatus MapStatus(Member member)
    {
        if (MemberStatusHelper.IsPending(member))
        {
            return MemberStatus.Pending;
        }

        return MemberStatusHelper.NormalizeStatus(member.Status) switch
        {
            "REGULAR" => MemberStatus.Regular,
            "REGULAR-NP" => MemberStatus.RegularNonPortuguese,
            "GUEST" => MemberStatus.Guest,
            "LIFE" => MemberStatus.Life,
            "INACTIVE" => MemberStatus.Inactive,
            _ => MemberStatus.Unknown
        };
    }
}
