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

                    // Can promote if dues are paid and either they are #1 or preceding members are unpaid
                    return new NpQueueEntryDto(
                        item.Position,
                        item.MemberID,
                        BuildFullName(item.FirstName, item.MiddleName, item.LastName),
                        item.AcceptedDate,
                        isPaid,
                        isPaid,
                        monthsUnpaid,
                        item.OriginalPosition > 0 ? item.OriginalPosition : item.Position,
                        item.BumpCount);
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

    public async Task BumpDownAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var rawQueue = _memberRepository.GetNonPortugueseGuestQueue();
        var index = rawQueue.FindIndex(m => m.MemberID == memberId);
        if (index < 0 || index >= rawQueue.Count - 1)
        {
            return; // Can't bump down if not found or already at the bottom
        }

        var currentMember = rawQueue[index];
        var nextMember = rawQueue[index + 1];

        // Swap their current positions
        int tempPos = currentMember.Position;
        currentMember.Position = nextMember.Position;
        nextMember.Position = tempPos;

        // Increment bump count for the bumped member
        currentMember.BumpCount++;

        _memberRepository.SaveNpQueueAdjustment(
            currentMember.MemberID,
            currentMember.Position,
            currentMember.OriginalPosition,
            currentMember.BumpCount,
            $"Manually bumped down past {nextMember.LastName}, {nextMember.FirstName}");

        _memberRepository.SaveNpQueueAdjustment(
            nextMember.MemberID,
            nextMember.Position,
            nextMember.OriginalPosition,
            nextMember.BumpCount,
            $"Shifted up due to bump of {currentMember.LastName}, {currentMember.FirstName}");

        _auditLogger.Log(
            "NP_QUEUE_BUMP_DOWN",
            null,
            null,
            $"[{currentMember.LastName}, {currentMember.FirstName}] Bumped down from #{tempPos} to #{currentMember.Position}",
            targetMemberId: memberId);
    }

    public Task RemoveFromQueueAdjustmentsAsync(int memberId, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            _memberRepository.RemoveNpQueueAdjustment(memberId);
        }, cancellationToken);
    }

    public async Task PromoteAsync(int memberId, CancellationToken cancellationToken = default)
    {
        var queue = await GetQueueAsync(cancellationToken);
        var targetEntry = queue.FirstOrDefault(e => e.MemberId == memberId);
        if (targetEntry == null)
        {
            throw new InvalidOperationException("Member not found in queue.");
        }

        if (!targetEntry.CurrentYearPaid)
        {
            throw new InvalidOperationException("Cannot promote a member whose dues are not paid.");
        }

        // If target is not #1, any preceding members are skipped/bypassed.
        // Auto-bump preceding unpaid members down!
        var precedingMembers = queue.Where(e => e.Position < targetEntry.Position).ToList();
        foreach (var preceding in precedingMembers)
        {
            var rawItem = _memberRepository.GetNonPortugueseGuestQueue().FirstOrDefault(m => m.MemberID == preceding.MemberId);
            if (rawItem != null)
            {
                // Increment bump count for being passed over
                _memberRepository.SaveNpQueueAdjustment(
                    rawItem.MemberID,
                    rawItem.Position,
                    rawItem.OriginalPosition,
                    rawItem.BumpCount + 1,
                    $"Bypassed by promotion of {targetEntry.FullName}");
            }
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

        // Remove the promoted member from queue adjustments table
        _memberRepository.RemoveNpQueueAdjustment(memberId);

        var memberName = $"{member.LastName}, {member.FirstName}";
        var details = $"[{memberName}] Promoted from {previousStatus ?? "unknown"} to REGULAR (queue position {targetEntry.Position})";
        _auditLogger.Log(
            AuditLogActions.NPQueuePromote,
            null,
            null,
            details,
            targetMemberId: memberId);
    }
}

