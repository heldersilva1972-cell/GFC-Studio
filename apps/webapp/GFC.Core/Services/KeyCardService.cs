using GFC.Core.BusinessRules;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

/// <summary>
/// Centralizes business rules for key card eligibility and hooks into dues events.
/// </summary>
public class KeyCardService
{
    private readonly IMemberRepository _memberRepository;
    private readonly IDuesRepository _duesRepository;
    private readonly IDuesYearSettingsRepository _duesYearSettingsRepository;
    private readonly IBoardRepository _boardRepository;

    public KeyCardService(
        IMemberRepository memberRepository,
        IDuesRepository duesRepository,
        IDuesYearSettingsRepository duesYearSettingsRepository,
        IBoardRepository boardRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _duesYearSettingsRepository = duesYearSettingsRepository ?? throw new ArgumentNullException(nameof(duesYearSettingsRepository));
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
    }

    public KeyCardEligibilityResult GetKeyCardEligibility(int memberId, int year)
    {
        var member = _memberRepository.GetMemberById(memberId);
        if (member == null)
        {
            return new KeyCardEligibilityResult(false, false, false, false, false, null, "UNKNOWN");
        }

        // Pending members (no AcceptedDate) do not qualify for key cards
        if (MemberStatusHelper.IsPending(member))
        {
            return new KeyCardEligibilityResult(false, false, false, false, false, null, "PENDING");
        }

        // Check if member is a board member (director) for this year
        var isBoardMember = _boardRepository.IsBoardMemberForYear(memberId, year);
        var settings = _duesYearSettingsRepository.GetSettingsForYear(year);
        
        // Satisfied if: Board Member, Life Member, Paid, Waived, OR Newly Accepted this year
        var currentYearSatisfied = isBoardMember || _duesRepository.MemberHasPaidOrWaivedDuesForYear(memberId, year) || MemberStatusHelper.IsNewlyAccepted(member, year);
        var previousYearSatisfied = isBoardMember || _duesRepository.MemberHasPaidOrWaivedDuesForYear(memberId, year - 1);

        return GetEligibilityBulk(member, year, settings, currentYearSatisfied, previousYearSatisfied);
    }

    /// <summary>
    /// High-performance version that uses pre-loaded status flags to avoid N+1 database queries.
    /// </summary>
    public KeyCardEligibilityResult GetEligibilityBulk(
        Member member, 
        int year, 
        DuesYearSettings? settings, 
        bool currentYearSatisfied, 
        bool previousYearSatisfied)
    {
        var isLifeMember = string.Equals(MemberStatusHelper.NormalizeStatus(member.Status), "LIFE", StringComparison.OrdinalIgnoreCase);
        var statusAllowed = IsStatusEligible(member.Status);
        
        var currentSatisfied = isLifeMember || currentYearSatisfied || MemberStatusHelper.IsNewlyAccepted(member, year);
        var previousSatisfied = isLifeMember || previousYearSatisfied;

        return BuildEligibility(statusAllowed, currentSatisfied, previousSatisfied, member.Status, settings?.GraceEndDate?.Date);
    }

    /// <summary>
    /// Lightweight helper for callers that already have a member instance but still need to evaluate card eligibility.
    /// </summary>
    public bool IsEligibleForCard(Member? member, int? year = null)
    {
        if (member == null || MemberStatusHelper.IsPending(member))
        {
            return false;
        }

        var evaluationYear = year ?? DateTime.Today.Year;
        
        // Check if member is a board member (director) for this year
        var isBoardMember = _boardRepository.IsBoardMemberForYear(member.MemberID, evaluationYear);
        var isLifeMember = string.Equals(MemberStatusHelper.NormalizeStatus(member.Status), "LIFE", StringComparison.OrdinalIgnoreCase);
        
        var graceEndDate = _duesYearSettingsRepository.GetSettingsForYear(evaluationYear)?.GraceEndDate?.Date;

        var statusAllowed = IsStatusEligible(member.Status);
        
        // Satisfied if: Board Member, Life Member, Paid, Waived, OR Newly Accepted this year
        var currentYearSatisfied = isBoardMember || isLifeMember || _duesRepository.MemberHasPaidOrWaivedDuesForYear(member.MemberID, evaluationYear) || MemberStatusHelper.IsNewlyAccepted(member, evaluationYear);
        var previousYearSatisfied = isBoardMember || isLifeMember || _duesRepository.MemberHasPaidOrWaivedDuesForYear(member.MemberID, evaluationYear - 1);

        return BuildEligibility(statusAllowed, currentYearSatisfied, previousYearSatisfied, member.Status, graceEndDate).Eligible;
    }

    public KeyCardEligibilityResult EvaluateEligibilityForRow(KeyCardMemberRow row, int year, DateTime? graceEndDate)
    {
        var statusAllowed = IsStatusEligible(row.MemberStatus);
        var isLifeMember = string.Equals(MemberStatusHelper.NormalizeStatus(row.MemberStatus), "LIFE", StringComparison.OrdinalIgnoreCase);
        
        var currentYearSatisfied = row.IsDirectorCurrent || isLifeMember || IsDuesSatisfied(row.DuesPaymentType, row.DuesPaidDate);
        var previousYearSatisfied = row.IsDirectorPrevious || isLifeMember || IsDuesSatisfied(row.PreviousYearPaymentType, row.PreviousYearPaidDate);
        
        return BuildEligibility(statusAllowed, currentYearSatisfied, previousYearSatisfied, row.MemberStatus, graceEndDate?.Date);
    }

    public bool IsMemberEligibleForKeyCard(int memberId, int year)
        => GetKeyCardEligibility(memberId, year).Eligible;

    /// <summary>
    /// Hook that can be called whenever dues are marked as paid/waived.
    /// Currently a placeholder for future door-controller integration.
    /// </summary>
    public void OnDuesPaid(int memberId, int year)
    {
        // Future enhancement: automatically sync active cards to external readers.
    }
    /// <summary>
    /// Determines if the provided member status is allowed to receive a key card assignment.
    /// Only REGULAR and LIFE members are currently permitted.
    /// To extend eligibility to guests, include:
    ///     status.StartsWith("GUEST", StringComparison.OrdinalIgnoreCase)
    /// in the allowed-status check below.
    /// </summary>
    public static bool IsStatusEligible(string? status)
    {
        var normalized = MemberStatusHelper.NormalizeStatus(status);
        return string.Equals(normalized, "REGULAR", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(normalized, "LIFE", StringComparison.OrdinalIgnoreCase) ||
               normalized.StartsWith("GUEST", StringComparison.OrdinalIgnoreCase);
    }

    public static string GetDuesState(string? paymentType, DateTime? paidDate)
    {
        if (string.IsNullOrWhiteSpace(paymentType) && !paidDate.HasValue)
        {
            return "No record";
        }

        if (IsWaived(paymentType))
        {
            return "Waived";
        }

        if (IsPaid(paymentType, paidDate))
        {
            return "Paid";
        }

        return "Unpaid";
    }

    public static bool IsDuesSatisfied(string? paymentType, DateTime? paidDate)
        => IsWaived(paymentType) || IsPaid(paymentType, paidDate);

    private static bool IsWaived(string? paymentType)
        => string.Equals(paymentType, "WAIVED", StringComparison.OrdinalIgnoreCase);

    private static bool IsPaid(string? paymentType, DateTime? paidDate)
        => paidDate.HasValue && !IsWaived(paymentType);

    private static KeyCardEligibilityResult BuildEligibility(
        bool statusAllowed,
        bool currentYearSatisfied,
        bool previousYearSatisfied,
        string? memberStatus,
        DateTime? graceEndDate)
    {
        var graceDate = graceEndDate?.Date;
        var gracePeriodActive = graceDate.HasValue && DateTime.Today <= graceDate.Value;
        var eligible = statusAllowed && (currentYearSatisfied || (gracePeriodActive && previousYearSatisfied));

        return new KeyCardEligibilityResult(
            eligible,
            statusAllowed,
            currentYearSatisfied,
            previousYearSatisfied,
            gracePeriodActive,
            graceDate,
            memberStatus ?? string.Empty);
    }
}

public sealed record KeyCardEligibilityResult(
    bool Eligible,
    bool StatusAllowed,
    bool CurrentYearSatisfied,
    bool PreviousYearSatisfied,
    bool GracePeriodActive,
    DateTime? GraceEndDate,
    string MemberStatus)
{
    public bool GracePeriodDefined => GraceEndDate.HasValue;
}



