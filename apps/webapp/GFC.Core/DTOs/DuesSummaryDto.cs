namespace GFC.Core.DTOs;

/// <summary>
/// Breakdown of dues status for a target year.
/// </summary>
public record DuesSummaryDto(
    int Year,
    int PaidCount,
    int UnpaidCount,
    int WaivedCount,
    decimal AmountCollected);

