using GFC.Core.Models;

namespace GFC.Core.BusinessRules;

/// <summary>
/// Centralized helper methods for determining member visibility/eligibility in operational views.
/// </summary>
public static class MemberFilters
{
    /// <summary>
    /// True if the member should be treated as "active" for operational views
    /// (lists, queues, board assignments, etc.).
    /// </summary>
    public static bool IsActiveForOperationalViews(Member m)
    {
        if (m == null) return false;

        var status = MemberStatusHelper.NormalizeStatus(m.Status);

        return status.Equals("GUEST", StringComparison.OrdinalIgnoreCase)
            || status.Equals("REGULAR", StringComparison.OrdinalIgnoreCase)
            || status.Equals("REGULAR-NP", StringComparison.OrdinalIgnoreCase)
            || status.Equals("LIFE", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// True if the member should be included in dues views for the specified year,
    /// taking into account their inactive date (if any).
    /// </summary>
    public static bool IsActiveForDuesYear(Member m, int selectedYear)
    {
        if (m == null) return false;

        if (TryGetCutoffYear(m, out var cutoffYear))
        {
            return selectedYear <= cutoffYear;
        }

        // No cutoff dates mean the member is considered active per normal rules
        return IsActiveForOperationalViews(m);
    }

    private static bool TryGetCutoffYear(Member m, out int cutoffYear)
    {
        cutoffYear = default;

        DateTime? inactiveDate = m.InactiveDate;
        DateTime? deathDate = m.DateOfDeath;

        if (!inactiveDate.HasValue && string.Equals(m.Status, "INACTIVE", StringComparison.OrdinalIgnoreCase))
        {
            inactiveDate = m.StatusChangeDate;
        }

        if (!deathDate.HasValue && string.Equals(m.Status, "DECEASED", StringComparison.OrdinalIgnoreCase))
        {
            deathDate = m.StatusChangeDate;
        }

        DateTime? earliest = null;

        if (inactiveDate.HasValue)
        {
            earliest = inactiveDate.Value;
        }

        if (deathDate.HasValue)
        {
            earliest = !earliest.HasValue || deathDate.Value < earliest.Value
                ? deathDate.Value
                : earliest;
        }

        if (earliest.HasValue)
        {
            cutoffYear = earliest.Value.Year;
            return true;
        }

        return false;
    }
}




