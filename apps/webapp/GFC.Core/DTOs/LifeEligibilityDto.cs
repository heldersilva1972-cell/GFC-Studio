namespace GFC.Core.DTOs;

/// <summary>
/// Provides life membership eligibility insight for a member.
/// </summary>
public record LifeEligibilityDto(
    int MemberId,
    string FullName,
    int Age,
    DateTime? RegularSince,
    DateTime? EligibilityDate,
    bool EligibleNow);

