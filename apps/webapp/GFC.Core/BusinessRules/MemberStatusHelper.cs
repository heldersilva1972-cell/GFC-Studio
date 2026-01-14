using System.Collections.Generic;
using System.Linq;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.BusinessRules;

/// <summary>
/// Centralizes member status rules and life eligibility logic so all forms use the same behavior.
/// </summary>
public static class MemberStatusHelper
{
    public const int MinimumMemberAge = 21;
    /// <summary>
    /// Returns a list of statuses that the supplied member is allowed to transition to.
    /// The current status is always included (preserving the original casing/value).
    /// </summary>
    public static List<string> GetAllowedStatusesForMember(Member? member)
    {
        if (member == null)
        {
            return new List<string> { "GUEST", "REGULAR", "REJECTED" };
        }

        var normalizedCurrent = NormalizeStatus(member.Status);
        var allowed = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        void AddOption(string statusValue)
        {
            if (string.IsNullOrWhiteSpace(statusValue))
            {
                return;
            }

            var normalizedOption = NormalizeStatus(statusValue);
            if (!seen.Add(normalizedOption))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(member.Status) &&
                normalizedOption.Equals(normalizedCurrent, StringComparison.OrdinalIgnoreCase))
            {
                allowed.Add(member.Status);
            }
            else
            {
                allowed.Add(statusValue);
            }
        }

        switch (normalizedCurrent)
        {
            case "GUEST":
                AddOption("GUEST");
                AddOption("REGULAR");
                AddOption("INACTIVE");
                AddOption("REJECTED");
                break;

            case "REGULAR":
                AddOption("REGULAR");
                AddOption("INACTIVE");
                if (IsLifeEligible(member))
                {
                    AddOption("LIFE");
                }
                break;

            case "LIFE":
                AddOption("LIFE");
                AddOption("INACTIVE");
                break;

            case "INACTIVE":
                AddOption("INACTIVE");
                AddOption("REGULAR");
                break;

            case "DECEASED":
                AddOption("DECEASED");
                break;

            case "REJECTED":
                AddOption("REJECTED");
                AddOption("GUEST");
                break;

            default:
                AddOption(normalizedCurrent);
                AddOption("GUEST");
                AddOption("REGULAR");
                AddOption("INACTIVE");
                break;
        }

        if (!string.IsNullOrWhiteSpace(member.Status) &&
            !allowed.Any(s => s.Equals(member.Status, StringComparison.OrdinalIgnoreCase)))
        {
            allowed.Add(member.Status);
        }

        return allowed;
    }

    /// <summary>
    /// Determines if a member satisfies Life membership requirements (age 65+ and 15+ years as REGULAR).
    /// Uses change history to determine when member became REGULAR for accurate calculation.
    /// </summary>
    /// <param name="member">The member to check</param>
    /// <param name="historyRepository">Optional history repository to determine when member became REGULAR</param>
    /// <returns>True if member is eligible for Life membership</returns>
    public static bool IsLifeEligible(Member? member, IHistoryRepository? historyRepository = null)
    {
        return TryCalculateLifeEligibility(member, DateTime.Today, historyRepository, out _);
    }

    /// <summary>
    /// Returns true if the provided status value represents any LIFE variant.
    /// </summary>
    public static bool IsLifeStatus(string? status)
    {
        return NormalizeStatus(status).Equals("LIFE", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Normalizes a status string for comparisons while keeping "LIFE MEMBER" compatible with "LIFE".
    /// </summary>
    public static string NormalizeStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return string.Empty;
        }

        var trimmed = status.Trim();
        if (trimmed.Equals("LIFE MEMBER", StringComparison.OrdinalIgnoreCase))
        {
            return "LIFE";
        }

        var lettersOnly = new string(trimmed
            .Where(char.IsLetterOrDigit)
            .ToArray())
            .ToUpperInvariant();

        if (lettersOnly.Equals("REGULARNP", StringComparison.OrdinalIgnoreCase) ||
            lettersOnly.StartsWith("REGULARNP", StringComparison.OrdinalIgnoreCase) ||
            lettersOnly.Equals("REGULARNONPORTUGUESE", StringComparison.OrdinalIgnoreCase) ||
            lettersOnly.StartsWith("REGULARNONPORTUGUESE", StringComparison.OrdinalIgnoreCase) ||
            lettersOnly.Equals("REGULARNONPORTUGUES", StringComparison.OrdinalIgnoreCase) ||
            lettersOnly.StartsWith("REGULARNONPORTUGUES", StringComparison.OrdinalIgnoreCase))
        {
            return "REGULAR-NP";
        }

        if (lettersOnly.Equals("GUESTNP", StringComparison.OrdinalIgnoreCase) ||
            lettersOnly.StartsWith("GUESTNP", StringComparison.OrdinalIgnoreCase) ||
            lettersOnly.Equals("GUESTNONPORTUGUESE", StringComparison.OrdinalIgnoreCase) ||
            lettersOnly.StartsWith("GUESTNONPORTUGUESE", StringComparison.OrdinalIgnoreCase))
        {
            return "GUEST";
        }

        var upper = trimmed.ToUpperInvariant();
        return upper.Replace("  ", " ");
    }

    /// <summary>
    /// A member is in a "Pending" state if they have not yet been assigned an AcceptedDate.
    /// Pending members do not qualify for key cards, cannot be board members, and are not billed for dues.
    /// </summary>
    public static bool IsPending(Member? member)
    {
        if (member == null) return false;
        return !member.AcceptedDate.HasValue;
    }

    /// <summary>
    /// A member is considered fully "Accepted" once they have an AcceptedDate.
    /// </summary>
    public static bool IsAccepted(Member? member) => !IsPending(member);

    /// <summary>
    /// Returns true if the member was accepted in the specified year.
    /// Useful for awarding initial privileges like key cards before first dues billing.
    /// </summary>
    public static bool IsNewlyAccepted(Member? member, int year)
    {
        if (member == null || !member.AcceptedDate.HasValue) return false;
        return member.AcceptedDate.Value.Year == year;
    }

    /// <summary>
    /// Determines if a member is eligible to serve on the board of directors.
    /// Must be an accepted Regular or Life member.
    /// </summary>
    public static bool IsEligibleForBoard(Member? member)
    {
        if (member == null || IsPending(member)) return false;

        var normalized = NormalizeStatus(member.Status);
        return normalized is "REGULAR" or "REGULAR-NP" or "LIFE";
    }

    public static bool TryCalculateLifeEligibility(
        Member? member,
        DateTime asOfDate,
        IHistoryRepository? historyRepository,
        out DateTime? eligibilityDate)
    {
        eligibilityDate = null;

        if (member == null)
        {
            return false;
        }

        if (!NormalizeStatus(member.Status).Equals("REGULAR", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!member.DateOfBirth.HasValue)
        {
            return false;
        }

        var regularSinceDate = GetRegularSinceDate(member, historyRepository);
        if (!regularSinceDate.HasValue)
        {
            return false;
        }

        var regularSince = regularSinceDate.Value.Date;
        var dateOfBirth = member.DateOfBirth.Value.Date;

        var minServiceDate = regularSince.AddYears(15);
        var minAgeDate = dateOfBirth.AddYears(65);
        eligibilityDate = minServiceDate > minAgeDate ? minServiceDate : minAgeDate;

        var regularYears = CalculateAge(regularSince, asOfDate);
        var age = CalculateAge(dateOfBirth, asOfDate);

        return regularYears >= 15 && age >= 65 && eligibilityDate <= asOfDate.Date;
    }

    public static DateTime? GetRegularSinceDate(Member member, IHistoryRepository? historyRepository = null)
    {
        DateTime? historyDate = null;
        if (historyRepository != null && member.MemberID > 0)
        {
            try
            {
                historyDate = historyRepository.GetEarliestRegularDate(member.MemberID);
            }
            catch
            {
                // ignore
            }
        }

        DateTime? statusDate = null;
        if (NormalizeStatus(member.Status).Equals("REGULAR", StringComparison.OrdinalIgnoreCase) &&
            member.StatusChangeDate.HasValue)
        {
            statusDate = member.StatusChangeDate.Value;
        }

        // If we have both, take the earlier one (trusting manual backdates in StatusChangeDate)
        if (historyDate.HasValue && statusDate.HasValue)
        {
            return historyDate.Value < statusDate.Value ? historyDate.Value : statusDate.Value;
        }

        return historyDate ?? statusDate ?? member.AcceptedDate;
    }

    public static int CalculateAge(DateTime startDate, DateTime endDate)
    {
        var years = endDate.Year - startDate.Year;
        if (endDate.Month < startDate.Month ||
            (endDate.Month == startDate.Month && endDate.Day < startDate.Day))
        {
            years--;
        }

        return years;
    }
}



