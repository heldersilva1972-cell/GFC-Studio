using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Models;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// Abstraction for controller operations. Implementations can be real (via Agent) or simulation (in-memory).
/// </summary>
public interface IControllerClient
{
    // Access-control focused contract (SimMode S2)
    Task OpenDoorAsync(int controllerId, int doorId, CancellationToken ct);
    Task AddOrUpdatePrivilegeAsync(CardPrivilegeModel model, CancellationToken ct);
    Task DeletePrivilegeAsync(long cardNumber, CancellationToken ct);
    Task BulkUploadAsync(IEnumerable<CardPrivilegeModel> models, CancellationToken ct);
    Task ClearAllCardsAsync(int controllerId, CancellationToken ct);
    Task SyncTimeAsync(int controllerId, CancellationToken ct);
    Task<RunStatusModel> GetRunStatusAsync(int controllerId, CancellationToken ct);
    Task<IReadOnlyList<EventLogModel>> GetEventsByIndexAsync(int controllerId, long fromIndex, int maxCount, CancellationToken ct);
    Task ResetControllerAsync(int controllerId, CancellationToken ct = default);

    // Existing surface (kept for backward compatibility)
    Task<bool> PingAsync(CancellationToken cancellationToken = default);
    Task<AgentRunStatusDto?> GetRunStatusAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task OpenDoorAsync(string controllerSn, int doorNo, int? durationSec = null, CancellationToken cancellationToken = default);
    Task<ApiResult> AddOrUpdateCardAsync(string controllerSn, AddOrUpdateCardRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteCardAsync(string controllerSn, string cardNumber, CancellationToken cancellationToken = default);
    Task<ApiResult> ClearAllCardsAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task<ControllerEventsResultDto> GetNewEventsAsync(string controllerSn, uint lastIndex, CancellationToken cancellationToken = default);
    Task<TimeScheduleDto?> GetTimeSchedulesAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task WriteTimeSchedulesAsync(string controllerSn, TimeScheduleWriteDto dto, CancellationToken cancellationToken = default);
    Task<ExtendedConfigDto?> GetDoorConfigAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task WriteDoorConfigAsync(string controllerSn, ExtendedDoorConfigWriteDto dto, CancellationToken cancellationToken = default);
    Task<AutoOpenConfigDto?> GetAutoOpenAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task WriteAutoOpenAsync(string controllerSn, AutoOpenConfigDto dto, CancellationToken cancellationToken = default);
    Task<AdvancedDoorModesDto?> GetAdvancedDoorModesAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task WriteAdvancedDoorModesAsync(string controllerSn, AdvancedDoorModesDto dto, CancellationToken cancellationToken = default);
    Task<NetworkConfigDto?> GetNetworkConfigAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task SetNetworkConfigAsync(string controllerSn, NetworkConfigDto dto, CancellationToken cancellationToken = default);
    Task<AllowedPcAndPasswordRequestDto?> GetAllowedPcSettingsAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task<ApiResult> SetAllowedPcSettingsAsync(string controllerSn, AllowedPcAndPasswordRequestDto dto, CancellationToken cancellationToken = default);
    Task RebootAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task SetDoorConfigAsync(string controllerSn, int doorIndex, byte controlMode, byte relayDelay, byte doorSensor, byte interlock, CancellationToken cancellationToken = default);
    Task<GFC.BlazorServer.Connectors.Mengqi.Models.DiscoveryResult?> GetHardwareInfoAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task<IEnumerable<GFC.BlazorServer.Connectors.Mengqi.Models.DiscoveryResult>> DiscoverAsync(CancellationToken cancellationToken = default);
}
