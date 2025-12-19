using GFC.Core.DTOs;

namespace GFC.Core.Interfaces;

public interface IDuesInsightService
{
    Task<IReadOnlyList<DuesListItemDto>> GetDuesAsync(int year, bool paidTab, CancellationToken cancellationToken = default);
    Task<IEnumerable<int>> GetAvailableYearsAsync(CancellationToken cancellationToken = default);
    Task<DuesSummaryDto> GetSummaryAsync(int year, CancellationToken cancellationToken = default);
}

