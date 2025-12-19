using GFC.Core.BusinessRules;
using GFC.Core.DTOs;
using GFC.Core.Enums;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

/// <summary>
/// Provides read-only member projections for UI front-ends.
/// </summary>
public class MemberQueryService : IMemberQueryService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberKeycardRepository _memberKeycardRepository;
    private readonly IHistoryRepository _historyRepository;

    public MemberQueryService(
        IMemberRepository memberRepository,
        IMemberKeycardRepository memberKeycardRepository,
        IHistoryRepository historyRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _memberKeycardRepository = memberKeycardRepository ?? throw new ArgumentNullException(nameof(memberKeycardRepository));
        _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
    }

    public async Task<IReadOnlyList<MemberListItemDto>> GetMembersAsync(
        MemberFilterOptions options,
        CancellationToken cancellationToken = default)
    {
        options ??= new MemberFilterOptions();

        var members = await Task.Run(() => _memberRepository.GetAllMembers(), cancellationToken);
        var filtered = ApplyFilters(members, options);

        var projections = new List<MemberListItemDto>();
        foreach (var member in filtered)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var hasKeyCard = _memberKeycardRepository.GetCurrentAssignmentForMember(member.MemberID) != null;

            projections.Add(new MemberListItemDto(
                member.MemberID,
                FormatMemberName(member),
                MapStatus(member),
                member.City,
                member.AcceptedDate,
                MemberStatusHelper.GetRegularSinceDate(member, _historyRepository),
                member.IsNonPortugueseOrigin,
                hasKeyCard,
                member.InactiveDate,
                member.AddressInvalid,
                member.AddressInvalidDate));
        }

        return projections;
    }

    public async Task<MemberSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        var counts = await Task.Run(() => _memberRepository.GetMemberCountsByStatus(), cancellationToken);

        int SafeGet(string status)
            => counts.TryGetValue(status, out var value) ? value : 0;

        return new MemberSummaryDto(
            TotalMembers: counts.Values.Sum(),
            RegularMembers: SafeGet("REGULAR"),
            Guests: SafeGet("GUEST"),
            RegularNonPortuguese: _memberRepository.GetNonPortugueseRegularCount(),
            LifeMembers: SafeGet("LIFE") + SafeGet("LIFE MEMBER"),
            InactiveMembers: SafeGet("INACTIVE"));
    }

    private static IEnumerable<Member> ApplyFilters(IEnumerable<Member> members, MemberFilterOptions options)
    {
        var query = members;

        if (!string.IsNullOrWhiteSpace(options.SearchText))
        {
            query = query.Where(m =>
                $"{m.FirstName} {m.LastName}".Contains(options.SearchText, StringComparison.OrdinalIgnoreCase) ||
                $"{m.LastName}, {m.FirstName}".Contains(options.SearchText, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(options.Status))
        {
            var normalized = MemberStatusHelper.NormalizeStatus(options.Status);

            if (string.Equals(normalized, "REGULAR-NP", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(m =>
                    MemberStatusHelper.NormalizeStatus(m.Status) == "REGULAR-NP" ||
                    (MemberStatusHelper.NormalizeStatus(m.Status) == "REGULAR" && m.IsNonPortugueseOrigin));
            }
            else
            {
                query = query.Where(m => MemberStatusHelper.NormalizeStatus(m.Status) == normalized);
            }
        }

        if (options.NonPortugueseOnly == true)
        {
            query = query.Where(m => m.IsNonPortugueseOrigin);
        }

        return query
            .OrderBy(m => m.LastName)
            .ThenBy(m => m.FirstName)
            .ToList();
    }

    private static MemberStatus MapStatus(Member member)
    {
        var normalized = MemberStatusHelper.NormalizeStatus(member.Status);

        if (normalized.Equals("REGULAR", StringComparison.OrdinalIgnoreCase) && member.IsNonPortugueseOrigin)
        {
            normalized = "REGULAR-NP";
        }

        return normalized switch
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

    private static string FormatMemberName(Member member)
    {
        var first = (member.FirstName ?? string.Empty).Trim();
        var last = (member.LastName ?? string.Empty).Trim();
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
}

