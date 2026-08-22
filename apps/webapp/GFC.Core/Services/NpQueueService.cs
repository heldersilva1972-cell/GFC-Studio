using GFC.Core.BusinessRules;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GFC.Core.Services;

public class NpQueueService : INpQueueService
{
    private const int RegularLimit = 45;

    private readonly IMemberRepository _memberRepository;
    private readonly MemberService _memberService;
    private readonly IAuditLogger _auditLogger;
    private readonly IDuesRepository _duesRepository;
    private readonly IDuesWaiverRepository? _waiverRepository;

    public NpQueueService(
        IMemberRepository memberRepository,
        MemberService memberService,
        IAuditLogger auditLogger,
        IDuesRepository duesRepository,
        IDuesWaiverRepository? waiverRepository = null)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _waiverRepository = waiverRepository;
    }

    public Task<IReadOnlyList<NpQueueEntryDto>> GetQueueAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var queue = _memberRepository.GetNonPortugueseGuestQueue();

            var currentYear = DateTime.Today.Year;

            var allDues = _duesRepository.GetAllDues();
            var duesLookup = allDues
                .GroupBy(d => d.MemberID)
                .ToDictionary(g => g.Key, g => g.ToList());

            var allWaivers = _waiverRepository?.GetAllWaivers() ?? new List<Models.DuesWaiverPeriod>();
            var waiverLookup = allWaivers
                .GroupBy(w => w.MemberId)
                .ToDictionary(g => g.Key, g => g.ToList());

            return (IReadOnlyList<NpQueueEntryDto>)queue
                .Select(item => 
                {
                    // Check if member has paid or waived dues for the current year
                    bool isPaid = _duesRepository.MemberHasPaidOrWaivedDuesForYear(item.MemberID, currentYear);

                    if (!isPaid && waiverLookup.TryGetValue(item.MemberID, out var memberWaivers))
                    {
                        if (memberWaivers.Any(w => currentYear >= w.StartYear && currentYear <= w.EndYear))
                        {
                            isPaid = true;
                        }
                    }

                    int? monthsUnpaid = null;

                    if (!isPaid)
                    {
                        var coveredYears = new HashSet<int>();

                        if (duesLookup.TryGetValue(item.MemberID, out var mDues))
                        {
                            foreach (var d in mDues)
                            {
                                if (d.PaidDate.HasValue || string.Equals(d.PaymentType, "WAIVED", StringComparison.OrdinalIgnoreCase) || (d.PaymentType != null && !d.PaymentType.Equals("UNPAID", StringComparison.OrdinalIgnoreCase)))
                                {
                                    coveredYears.Add(d.Year);
                                }
                            }
                        }

                        if (waiverLookup.TryGetValue(item.MemberID, out var mWaivers))
                        {
                            foreach (var w in mWaivers)
                            {
                                for (int y = w.StartYear; y <= w.EndYear; y++)
                                {
                                    coveredYears.Add(y);
                                }
                            }
                        }

                        if (coveredYears.Count > 0)
                        {
                            int lastCoveredYear = coveredYears.Max();
                            monthsUnpaid = Math.Max(0, (DateTime.Today.Year - lastCoveredYear - 1) * 12 + DateTime.Today.Month);
                        }
                        else if (item.AcceptedDate.HasValue)
                        {
                            monthsUnpaid = Math.Max(0, (DateTime.Today.Year - item.AcceptedDate.Value.Year) * 12 + DateTime.Today.Month - item.AcceptedDate.Value.Month);
                        }
                    }

                    return new NpQueueEntryDto(
                        item.Position,
                        item.MemberID,
                        BuildFullName(item.FirstName, item.MiddleName, item.LastName),
                        item.AcceptedDate,
                        item.Position == 1,
                        isPaid,
                        monthsUnpaid);
                })
                .ToList();
        }, cancellationToken);
    }

    private static string BuildFullName(string? first, string? middle, string? last)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(first)) parts.Add(first.Trim());
        if (!string.IsNullOrWhiteSpace(middle)) parts.Add(middle.Trim());
        if (!string.IsNullOrWhiteSpace(last)) parts.Add(last.Trim());
        return string.Join(" ", parts);
    }

    public Task<int> GetSlotsAvailableAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var regularCount = _memberRepository.GetNonPortugueseRegularCount();
            return Math.Max(0, RegularLimit - regularCount);
        }, cancellationToken);
    }

    public async Task PromoteAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var queue = await GetQueueAsync(cancellationToken);
        var first = queue.FirstOrDefault();
        if (first == null || first.MemberId != memberId)
        {
            throw new InvalidOperationException("Only the first member in the queue can be promoted.");
        }

        var member = _memberRepository.GetMemberById(memberId)
                     ?? throw new InvalidOperationException("Member not found.");

        if (!MemberStatusHelper.NormalizeStatus(member.Status).Equals("GUEST"))
        {
            throw new InvalidOperationException("Only guests can be promoted.");
        }

        member.IsNonPortugueseOrigin = true;
        var previousStatus = member.Status;
        await _memberService.UpdateMemberStatusAsync(member, "REGULAR");

        var memberName = $"{member.LastName}, {member.FirstName}";
        var details = $"[{memberName}] Promoted from {previousStatus ?? "unknown"} to REGULAR (previous position {first.Position})";
        _auditLogger.Log(
            AuditLogActions.NPQueuePromote,
            null,
            null,
            details,
            targetMemberId: memberId);
    }
}

