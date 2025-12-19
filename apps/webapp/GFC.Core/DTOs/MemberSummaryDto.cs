namespace GFC.Core.DTOs;

/// <summary>
/// Aggregate member metrics for dashboards and list views.
/// </summary>
public record MemberSummaryDto(
    int TotalMembers,
    int RegularMembers,
    int Guests,
    int RegularNonPortuguese,
    int LifeMembers,
    int InactiveMembers);

