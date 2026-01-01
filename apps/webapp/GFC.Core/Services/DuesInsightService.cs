using GFC.Core.BusinessRules;
using GFC.Core.DTOs;
using GFC.Core.Enums;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

public class DuesInsightService : IDuesInsightService
{
    private readonly IDuesRepository _duesRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly OverdueCalculationService _overdueService;
    private readonly IBoardRepository _boardRepository;

    public DuesInsightService(
        IDuesRepository duesRepository,
        IMemberRepository memberRepository,
        OverdueCalculationService overdueService,
        IBoardRepository boardRepository)
    {
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _overdueService = overdueService ?? throw new ArgumentNullException(nameof(overdueService));
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
    }

    public async Task<IReadOnlyList<DuesListItemDto>> GetDuesAsync(int year, bool paidTab, CancellationToken cancellationToken = default)
    {
        var membersTask = Task.Run(() => _memberRepository.GetAllMembers(), cancellationToken);
        var duesTask = Task.Run(() => _duesRepository.GetDuesForYear(year), cancellationToken);

        await Task.WhenAll(membersTask, duesTask);

        var members = membersTask.Result
            .Where(m => MemberFilters.IsActiveForDuesYear(m, year))
            .Where(m => !string.Equals(m.Status, "INACTIVE", StringComparison.OrdinalIgnoreCase)
                     && !string.Equals(m.Status, "DECEASED", StringComparison.OrdinalIgnoreCase))
            .ToList();
        var duesLookup = duesTask.Result.ToDictionary(d => d.MemberID, d => d);

        var list = new List<DuesListItemDto>();
        foreach (var member in members)
        {
            cancellationToken.ThrowIfCancellationRequested();

            duesLookup.TryGetValue(member.MemberID, out var record);
            var status = MapStatus(member.Status);
            var isBoardMember = _boardRepository.IsBoardMemberForYear(member.MemberID, year);
            var isLife = status == MemberStatus.Life;

            var recordIsWaived = record != null && string.Equals(record.PaymentType, "WAIVED", StringComparison.OrdinalIgnoreCase);
            var isWaived = recordIsWaived || isLife || isBoardMember;
            var isPaid = (record != null && record.PaidDate.HasValue) || isWaived;

            if (paidTab && !isPaid)
            {
                continue;
            }

            if (!paidTab && isPaid)
            {
                continue;
            }

            var overdueMonths = 0;
            if (!isPaid)
            {
                var overdueResult = _overdueService.CalculateOverdue(member);
                if (overdueResult.IsOverdue && overdueResult.FirstUnpaidYear <= year)
                {
                    overdueMonths = overdueResult.MonthsOverdue;
                }
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
                isWaived,
                record?.Notes ?? string.Empty,
                isBoardMember));
        }

        return list
            .OrderBy(item => item.FullName)
            .ToList();
    }

    public Task<IEnumerable<int>> GetAvailableYearsAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var years = _duesRepository.GetDistinctYears();
            var currentYear = DateTime.Now.Year;
            if (!years.Contains(currentYear))
            {
                years.Add(currentYear);
            }
            if (!years.Contains(currentYear + 1))
            {
                years.Add(currentYear + 1);
            }
            return years.OrderBy(y => y).AsEnumerable();
        }, cancellationToken);
    }

    public async Task<DuesSummaryDto> GetSummaryAsync(int year, CancellationToken cancellationToken = default)
    {
        var membersTask = Task.Run(() => _memberRepository.GetAllMembers(), cancellationToken);
        var duesTask = Task.Run(() => _duesRepository.GetDuesForYear(year), cancellationToken);
        var boardTask = Task.Run(() =>
            _boardRepository.GetAssignmentsByYear(year)
                .Select(a => a.MemberID)
                .ToHashSet(),
            cancellationToken);

        await Task.WhenAll(membersTask, duesTask, boardTask);

        var members = membersTask.Result
            .Where(m => MemberFilters.IsActiveForDuesYear(m, year))
            .ToList();
        var duesLookup = duesTask.Result.ToDictionary(d => d.MemberID, d => d);
        var boardMemberIds = boardTask.Result;

        int paidCount = 0;
        int waivedCount = 0;
        int unpaidCount = 0;
        decimal amountCollected = 0;

        foreach (var member in members)
        {
            cancellationToken.ThrowIfCancellationRequested();

            duesLookup.TryGetValue(member.MemberID, out var record);
            var status = MemberStatusHelper.NormalizeStatus(member.Status);
            var isLife = MemberStatusHelper.IsLifeStatus(status);
            var isBoard = boardMemberIds.Contains(member.MemberID);

            var recordIsWaived = record is not null && IsWaived(record);
            var recordIsPaid = record is not null && record.PaidDate.HasValue && !recordIsWaived;
            var autoWaived = isLife || isBoard;
            var isWaived = autoWaived || recordIsWaived;

            if (isWaived)
            {
                waivedCount++;
            }
            else if (recordIsPaid)
            {
                paidCount++;
            }
            else
            {
                unpaidCount++;
            }

            if (recordIsPaid && record!.Amount.HasValue)
            {
                amountCollected += record.Amount.Value;
            }
        }

        return new DuesSummaryDto(year, paidCount, unpaidCount, waivedCount, amountCollected);
    }

    private static bool IsWaived(DuesPayment payment)
        => string.Equals(payment.PaymentType, "WAIVED", StringComparison.OrdinalIgnoreCase);

    private static string FormatMemberName(Member member)
    {
        var first = member.FirstName?.Trim() ?? string.Empty;
        var last = member.LastName?.Trim() ?? string.Empty;
        var middle = string.IsNullOrWhiteSpace(member.MiddleName)
            ? string.Empty
            : $" {member.MiddleName!.Trim()}";

        if (string.IsNullOrWhiteSpace(last))
        {
            return $"{first}{middle}".Trim();
        }

        if (string.IsNullOrWhiteSpace(first))
        {
            return $"{last}{middle}".Trim();
        }

        return $"{last}, {first}{middle}";
    }

    private static MemberStatus MapStatus(string status)
    {
        return MemberStatusHelper.NormalizeStatus(status) switch
        {
            "REGULAR" => MemberStatus.Regular,
            "REGULAR-NP" => MemberStatus.RegularNonPortuguese,
            "GUEST" => MemberStatus.Guest,
            "LIFE" => MemberStatus.Life,
            "INACTIVE" => MemberStatus.Inactive,
            "DECEASED" => MemberStatus.Deceased,
            "REJECTED" => MemberStatus.Rejected,
            _ => MemberStatus.Unknown
        };
    }
}

