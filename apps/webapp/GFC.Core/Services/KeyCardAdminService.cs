using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

/// <summary>
/// Handles administrative key card actions (assigning cards, recording history).
/// </summary>
public class KeyCardAdminService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IMemberKeycardRepository _memberKeycardRepository;
    private readonly IKeyCardRepository _keyCardRepository;
    private readonly KeyCardService _keyCardService;
    private readonly IMemberHistoryService _historyService;
    private readonly IAuditLogger _auditLogger;

    public KeyCardAdminService(
        IMemberRepository memberRepository,
        IMemberKeycardRepository memberKeycardRepository,
        IKeyCardRepository keyCardRepository,
        KeyCardService keyCardService,
        IMemberHistoryService historyService,
        IAuditLogger auditLogger)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _memberKeycardRepository = memberKeycardRepository ?? throw new ArgumentNullException(nameof(memberKeycardRepository));
        _keyCardRepository = keyCardRepository ?? throw new ArgumentNullException(nameof(keyCardRepository));
        _keyCardService = keyCardService ?? throw new ArgumentNullException(nameof(keyCardService));
        _historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
        _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
    }

    public void AssignCard(int memberId, string cardNumber, string? notes, string source)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
        {
            throw new ArgumentException("Card number is required.", nameof(cardNumber));
        }

        var member = _memberRepository.GetMemberById(memberId)
            ?? throw new InvalidOperationException("Member not found.");

        if (!_keyCardService.IsEligibleForCard(member))
        {
            throw new InvalidOperationException("Member is not eligible for a key card.");
        }

        var existingAssignment = _memberKeycardRepository.GetCurrentAssignmentForMember(memberId);
        if (existingAssignment is not null)
        {
            throw new InvalidOperationException("Member already has an active key card assignment.");
        }

        var card = _keyCardRepository.GetByCardNumber(cardNumber.Trim());
        if (card is not null)
        {
            var currentAssignment = _memberKeycardRepository.GetCurrentAssignmentForCard(card.KeyCardId);
            if (currentAssignment is not null)
            {
                throw new InvalidOperationException("That key card number is already assigned to another member.");
            }

            if (card.MemberId != memberId)
            {
                card.MemberId = memberId;
                _keyCardRepository.Update(card);
            }
        }
        else
        {
            card = _keyCardRepository.Create(cardNumber.Trim(), memberId, notes);
        }

        var assignment = new MemberKeycardAssignment
        {
            MemberId = memberId,
            KeyCardId = card.KeyCardId,
            FromDate = DateTime.Today,
            Reason = string.IsNullOrWhiteSpace(notes) ? "Assigned via WebApp" : notes.Trim(),
            ChangedBy = source
        };

        _memberKeycardRepository.AddAssignment(assignment);
        _historyService.LogChange(memberId, "KeyCard", null, $"Assigned key card {card.CardNumber}", source);
        var details = $"Assigned key card {card.CardNumber} via {source}; notes: {notes ?? "none"}";
        _auditLogger.Log(
            AuditLogActions.KeyCardAdded,
            null,
            null,
            details);
    }
}

