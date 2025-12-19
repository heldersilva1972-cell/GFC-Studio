using GFC.BlazorServer.Models;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Service for managing member door access privileges.
/// </summary>
public interface IMemberAccessService
{
    Task<MemberDoorAccessResponse> GetMemberDoorAccessAsync(int memberId, CancellationToken cancellationToken = default);
    Task UpdateMemberDoorAccessAsync(int memberId, IEnumerable<MemberDoorAccessUpdateDto> updates, CancellationToken cancellationToken = default);
    Task<EffectiveAccessResult> ComputeEffectiveAccessAsync(int memberId, CancellationToken cancellationToken = default);
    Task SyncMemberPrivilegesAsync(int memberId, CancellationToken cancellationToken = default);
    Task RemoveCardAsync(int memberId, string cardNumber, string? performedByUserName = null, int? performedByUserId = null, CancellationToken cancellationToken = default);
    Task ClearAllPrivilegesAsync(int controllerId, CancellationToken cancellationToken = default);
}
