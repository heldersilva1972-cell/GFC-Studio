namespace GFC.Core.DTOs;

/// <summary>
/// Aggregated alert counters displayed in dashboards.
/// </summary>
public record AlertSummaryDto(
    int PhysicalKeysToReturn,
    int LifeEligibleCount,
    int NpQueueCount,
    int OverdueMembers15Months,
    int ActiveKeyCardCount,
    int BoardAlertYear,
    IReadOnlyList<string> BoardPositionsUnfilled,
    bool BoardConfirmed);

