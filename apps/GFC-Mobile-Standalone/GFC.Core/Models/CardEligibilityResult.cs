using System;

namespace GFC.Core.Models;

/// <summary>
/// Represents the outcome of evaluating whether a member can manage key card privileges.
/// </summary>
public sealed record CardEligibilityResult
{
    private const string DefaultReason = "Member is not eligible for key card privileges.";

    public CardEligibilityResult(bool isEligible, string? reason)
    {
        IsEligible = isEligible;
        Reason = isEligible ? null : NormalizeReason(reason);
    }

    public bool IsEligible { get; }
    public string? Reason { get; }

    public static CardEligibilityResult Eligible() => new(true, null);

    public static CardEligibilityResult Ineligible(string? reason) => new(false, reason);

    public CardEligibilityResult WithReason(string? reason) => new(IsEligible, reason);

    private static string NormalizeReason(string? reason)
        => string.IsNullOrWhiteSpace(reason) ? DefaultReason : reason.Trim();
}
