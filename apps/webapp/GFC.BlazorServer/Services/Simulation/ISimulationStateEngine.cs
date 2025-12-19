using System.Collections.Generic;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Models;

namespace GFC.BlazorServer.Services.Simulation;

/// <summary>
/// Internal engine that maintains simulation controller state and serves deterministic responses.
/// </summary>
public interface ISimulationStateEngine
{
    // Controllers / status
    Task<AgentRunStatusDto?> GetRunStatusAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task SyncTimeAsync(string controllerSn, DateTime serverTimeUtc, CancellationToken cancellationToken = default);

    // Doors
    Task OpenDoorAsync(string controllerSn, int doorNo, int? durationSec = null, string? reason = null, CancellationToken cancellationToken = default);

    // Privileges
    Task<ApiResult> AddOrUpdateCardAsync(string controllerSn, AddOrUpdateCardRequestDto request, CancellationToken cancellationToken = default);
    Task<ApiResult> DeleteCardAsync(string controllerSn, string cardNumber, CancellationToken cancellationToken = default);
    Task<ApiResult> BulkUploadCardsAsync(string controllerSn, IEnumerable<AddOrUpdateCardRequestDto> privileges, CancellationToken cancellationToken = default);
    Task<ApiResult> ClearAllCardsAsync(string controllerSn, CancellationToken cancellationToken = default);

    // Events
    Task<ControllerEventsResultDto> GetEventsAsync(string controllerSn, uint startIndex, int maxCount, CancellationToken cancellationToken = default);
    uint GetCurrentEventIndex(string controllerSn);

    // Time schedules
    Task<TimeScheduleDto?> GetTimeSchedulesAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task WriteTimeSchedulesAsync(string controllerSn, TimeScheduleWriteDto dto, CancellationToken cancellationToken = default);

    // Door configuration
    Task<ExtendedConfigDto?> GetDoorConfigAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task WriteDoorConfigAsync(string controllerSn, ExtendedDoorConfigWriteDto dto, CancellationToken cancellationToken = default);

    // Auto-open configuration
    Task<AutoOpenConfigDto?> GetAutoOpenAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task WriteAutoOpenAsync(string controllerSn, AutoOpenConfigDto dto, CancellationToken cancellationToken = default);

    // Advanced modes
    Task<AdvancedDoorModesDto?> GetAdvancedDoorModesAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task WriteAdvancedDoorModesAsync(string controllerSn, AdvancedDoorModesDto dto, CancellationToken cancellationToken = default);

    // Network / maintenance
    Task<NetworkConfigDto?> GetNetworkConfigAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task SetNetworkConfigAsync(string controllerSn, NetworkConfigDto dto, CancellationToken cancellationToken = default);
    Task<AllowedPcAndPasswordRequestDto?> GetAllowedPcSettingsAsync(string controllerSn, CancellationToken cancellationToken = default);
    Task<ApiResult> SetAllowedPcSettingsAsync(string controllerSn, AllowedPcAndPasswordRequestDto dto, CancellationToken cancellationToken = default);
    Task RebootAsync(string controllerSn, CancellationToken cancellationToken = default);
}

