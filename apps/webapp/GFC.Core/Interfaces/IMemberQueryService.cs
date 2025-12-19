using GFC.Core.DTOs;

namespace GFC.Core.Interfaces;

/// <summary>
/// Exposes read-only member projections for UI layers.
/// </summary>
public interface IMemberQueryService
{
    Task<IReadOnlyList<MemberListItemDto>> GetMembersAsync(MemberFilterOptions options, CancellationToken cancellationToken = default);
    Task<MemberSummaryDto> GetSummaryAsync(CancellationToken cancellationToken = default);
}

