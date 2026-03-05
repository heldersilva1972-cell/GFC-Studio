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

    public NpQueueService(IMemberRepository memberRepository, MemberService memberService, IAuditLogger auditLogger, IDuesRepository duesRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
    }

    public Task<IReadOnlyList<NpQueueEntryDto>> GetQueueAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var queue = _memberRepository.GetNonPortugueseGuestQueue();

            var currentYear = DateTime.Today.Year;
            var currentYearDues = _duesRepository.GetDuesForYear(currentYear);
            var paidMemberIds = currentYearDues.Where(d => d.PaymentType != "UNPAID").Select(d => d.MemberID).ToHashSet();

            var allDues = _duesRepository.GetAllDues();
            var lastPaidDictionary = allDues
                .Where(d => d.PaymentType != "UNPAID")
                .GroupBy(d => d.MemberID)
                .ToDictionary(g => g.Key, g => g.Max(d => d.Year));

            return (IReadOnlyList<NpQueueEntryDto>)queue
                .Select(item => 
                {
                    var isPaid = paidMemberIds.Contains(item.MemberID);
                    int? monthsUnpaid = null;

                    if (!isPaid)
                    {
                        if (lastPaidDictionary.TryGetValue(item.MemberID, out var lastPaidYear))
                        {
                            monthsUnpaid = Math.Max(0, (DateTime.Today.Year - lastPaidYear - 1) * 12 + DateTime.Today.Month);
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

        var details = $"Promoted from {previousStatus ?? "unknown"} to REGULAR (previous position {first.Position})";
        _auditLogger.Log(
            AuditLogActions.NPQueuePromote,
            null,
            null,
            details);
    }
}

