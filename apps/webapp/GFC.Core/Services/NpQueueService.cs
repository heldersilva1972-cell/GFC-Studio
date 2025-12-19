using GFC.Core.BusinessRules;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;

namespace GFC.Core.Services;

public class NpQueueService : INpQueueService
{
    private const int RegularLimit = 45;

    private readonly IMemberRepository _memberRepository;
    private readonly MemberService _memberService;
    private readonly IAuditLogger _auditLogger;

    public NpQueueService(IMemberRepository memberRepository, MemberService memberService, IAuditLogger auditLogger)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
    }

    public Task<IReadOnlyList<NpQueueEntryDto>> GetQueueAsync(CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var queue = _memberRepository.GetNonPortugueseGuestQueue();
            return (IReadOnlyList<NpQueueEntryDto>)queue
                .Select(item => new NpQueueEntryDto(
                    item.Position,
                    item.MemberID,
                    $"{item.FirstName} {item.LastName}",
                    item.AcceptedDate,
                    item.Position == 1))
                .ToList();
        }, cancellationToken);
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
        _memberService.UpdateMemberStatus(member, "REGULAR");

        var details = $"Promoted from {previousStatus ?? "unknown"} to REGULAR (previous position {first.Position})";
        _auditLogger.Log(
            AuditLogActions.NPQueuePromote,
            null,
            null,
            details);
    }
}

