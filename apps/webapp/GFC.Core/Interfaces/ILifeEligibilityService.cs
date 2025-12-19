using GFC.Core.DTOs;

namespace GFC.Core.Interfaces;

public interface ILifeEligibilityService
{
    Task<IReadOnlyList<LifeEligibilityDto>> GetEligibleMembersAsync(bool includeUpcoming, CancellationToken cancellationToken = default);
}

