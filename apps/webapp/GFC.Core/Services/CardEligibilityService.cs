using GFC.Core.BusinessRules;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

/// <summary>
/// Central point for determining whether a member can add, replace, or update card privileges.
/// </summary>
public class CardEligibilityService : ICardEligibilityService
{
    private readonly IDuesRepository _duesRepository;
    private readonly IDuesYearSettingsRepository _duesYearSettingsRepository;
    private readonly IBoardRepository _boardRepository;

    public CardEligibilityService(
        IDuesRepository duesRepository,
        IDuesYearSettingsRepository duesYearSettingsRepository,
        IBoardRepository boardRepository)
    {
        _duesRepository = duesRepository ?? throw new ArgumentNullException(nameof(duesRepository));
        _duesYearSettingsRepository = duesYearSettingsRepository ?? throw new ArgumentNullException(nameof(duesYearSettingsRepository));
        _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
    }

    public CardEligibilityResult Evaluate(Member member)
    {
        if (member == null)
        {
            return CardEligibilityResult.Ineligible("Member was not found.");
        }

        if (member.MemberID <= 0)
        {
            return CardEligibilityResult.Ineligible("Member record is incomplete.");
        }

        var normalizedStatus = MemberStatusHelper.NormalizeStatus(member.Status);
        if (string.IsNullOrWhiteSpace(normalizedStatus))
        {
            return CardEligibilityResult.Ineligible("Member status is missing.");
        }

        if (IsRestrictedStatus(normalizedStatus))
        {
            return CardEligibilityResult.Ineligible(GetStatusRestrictionReason(normalizedStatus));
        }

        if (normalizedStatus.Equals("LIFE", StringComparison.OrdinalIgnoreCase))
        {
            return CardEligibilityResult.Eligible();
        }

        if (!normalizedStatus.Equals("REGULAR", StringComparison.OrdinalIgnoreCase))
        {
            return CardEligibilityResult.Ineligible("Only Regular or Life members can manage key card privileges.");
        }

        return EvaluateDues(member.MemberID);
    }

    private CardEligibilityResult EvaluateDues(int memberId)
    {
        var evaluationYear = DateTime.Today.Year;
        
        // Check if member is a board member (director) for this year
        // Directors automatically have their dues waived and are eligible
        if (_boardRepository.IsBoardMemberForYear(memberId, evaluationYear))
        {
            return CardEligibilityResult.Eligible();
        }
        
        var currentYearDues = _duesRepository.GetDuesForMemberYear(memberId, evaluationYear);
        var previousYearDues = _duesRepository.GetDuesForMemberYear(memberId, evaluationYear - 1);

        var graceEndDate = _duesYearSettingsRepository.GetSettingsForYear(evaluationYear)?.GraceEndDate?.Date;
        var gracePeriodActive = graceEndDate.HasValue && DateTime.Today.Date <= graceEndDate.Value;

        var currentYearSatisfied = IsDuesSatisfied(currentYearDues?.PaymentType, currentYearDues?.PaidDate);
        if (currentYearSatisfied)
        {
            return CardEligibilityResult.Eligible();
        }

        var previousYearSatisfied = IsDuesSatisfied(previousYearDues?.PaymentType, previousYearDues?.PaidDate);
        if (gracePeriodActive && previousYearSatisfied)
        {
            return CardEligibilityResult.Eligible();
        }

        if (gracePeriodActive && !previousYearSatisfied)
        {
            return CardEligibilityResult.Ineligible("Previous-year dues must be satisfied before the grace period ends.");
        }

        return CardEligibilityResult.Ineligible("Current-year dues must be paid or waived before card privileges can change.");
    }

    private static bool IsRestrictedStatus(string normalizedStatus)
        => normalizedStatus is "REGULAR-NP" or "GUEST" or "SUSPENDED" or "INACTIVE" or "REJECTED" or "DECEASED";

    private static string GetStatusRestrictionReason(string normalizedStatus)
        => normalizedStatus switch
        {
            "REGULAR-NP" => "Regular (Non-Portuguese) members require board approval before card privileges can be managed.",
            "GUEST" => "Guest members are not eligible for key card privileges.",
            "SUSPENDED" => "Suspended members cannot manage key card privileges.",
            "INACTIVE" => "Inactive members cannot manage key card privileges.",
            "REJECTED" => "Rejected members cannot manage key card privileges.",
            "DECEASED" => "Deceased members cannot manage key card privileges.",
            _ => "Member is not eligible for key card privileges."
        };

    private static bool IsDuesSatisfied(string? paymentType, DateTime? paidDate)
        => string.Equals(paymentType, "WAIVED", StringComparison.OrdinalIgnoreCase) || paidDate.HasValue;
}
