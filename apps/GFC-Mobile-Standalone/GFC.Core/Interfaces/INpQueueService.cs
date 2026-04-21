using GFC.Core.DTOs;

namespace GFC.Core.Interfaces;

public interface INpQueueService
{
    Task<IReadOnlyList<NpQueueEntryDto>> GetQueueAsync(CancellationToken cancellationToken = default);
    Task<int> GetSlotsAvailableAsync(CancellationToken cancellationToken = default);
    Task PromoteAsync(int memberId, CancellationToken cancellationToken = default);
}

