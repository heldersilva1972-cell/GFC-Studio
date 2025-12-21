using GFC.BlazorServer.Configuration;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services;
using GFC.BlazorServer.Services.Models;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// Real controller client that delegates to AgentApiClient (Agent PC).
/// </summary>
public class RealControllerClient : IControllerClient
{
    private readonly AgentApiClient _agentApiClient;
    private readonly ILogger<RealControllerClient> _logger;
    private readonly ISimulationGuard _simulationGuard;
    private readonly ControllerRegistryService _controllerRegistry;

    public RealControllerClient(
        AgentApiClient agentApiClient,
        ILogger<RealControllerClient> logger,
        ISimulationGuard simulationGuard,
        ControllerRegistryService controllerRegistry)
    {
        _agentApiClient = agentApiClient ?? throw new ArgumentNullException(nameof(agentApiClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _simulationGuard = simulationGuard ?? throw new ArgumentNullException(nameof(simulationGuard));
        _controllerRegistry = controllerRegistry ?? throw new ArgumentNullException(nameof(controllerRegistry));
    }

    #region SimMode S2 contract

    public async Task OpenDoorAsync(int controllerId, int doorId, CancellationToken ct)
    {
        var (controller, door) = await ResolveControllerAndDoorAsync(controllerId, doorId, ct);
        if (controller == null || door == null)
        {
            return;
        }

        await OpenDoorAsync(controller.SerialNumberDisplay, door.DoorIndex, null, ct);
    }

    public async Task AddOrUpdatePrivilegeAsync(CardPrivilegeModel model, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.ControllerId == 0)
        {
            throw new ArgumentException("ControllerId is required on CardPrivilegeModel.", nameof(model));
        }

        var controller = await _controllerRegistry.GetControllerByIdAsync(model.ControllerId, ct);
        if (controller == null)
        {
            _logger.LogWarning("Controller {ControllerId} not found for AddOrUpdatePrivilege", model.ControllerId);
            return;
        }

        var doorIndex = ResolveDoorIndex(model, controller);
        if (doorIndex == null)
        {
            _logger.LogWarning("Door not resolved for controller {ControllerId} (doorId: {DoorId}, doorIndex: {DoorIndex})",
                controller.Id, model.DoorId, model.DoorIndex);
            return;
        }

        var request = new AddOrUpdateCardRequestDto
        {
            CardNumber = model.CardNumber.ToString(),
            DoorIndex = doorIndex.Value,
            TimeProfileIndex = model.TimeProfileIndex,
            Enabled = model.Enabled
        };

        var result = await AddOrUpdateCardAsync(controller.SerialNumberDisplay, request, ct);
        if (!result.Success)
        {
            throw new InvalidOperationException($"Failed to add/update privilege: {result.Message}");
        }
    }

    public async Task DeletePrivilegeAsync(long cardNumber, CancellationToken ct)
    {
        var controllers = await _controllerRegistry.GetControllersAsync(includeDoors: false, cancellationToken: ct);
        foreach (var controller in controllers)
        {
            try
            {
                var result = await DeleteCardAsync(controller.SerialNumberDisplay, cardNumber.ToString(), ct);
                if (!result.Success)
                {
                    _logger.LogWarning("DeletePrivilege failed for controller {ControllerId}: {Message}", controller.Id, result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "DeletePrivilege threw for controller {ControllerId}", controller.Id);
            }
        }
    }

    public async Task BulkUploadAsync(IEnumerable<CardPrivilegeModel> models, CancellationToken ct)
    {
        if (models == null)
        {
            return;
        }

        foreach (var model in models)
        {
            await AddOrUpdatePrivilegeAsync(model, ct);
        }
    }

    public async Task ClearAllCardsAsync(int controllerId, CancellationToken ct)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null)
        {
            _logger.LogWarning("Controller {ControllerId} not found for ClearAllCards", controllerId);
            return;
        }

        var result = await ClearAllCardsAsync(controller.SerialNumberDisplay, ct);
        if (!result.Success)
        {
            throw new InvalidOperationException($"ClearAllCards failed for controller {controller.SerialNumberDisplay}: {result.Message}");
        }
    }

    public async Task SyncTimeAsync(int controllerId, CancellationToken ct)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null)
        {
            _logger.LogWarning("Controller {ControllerId} not found for SyncTime", controllerId);
            return;
        }

        await EnsureRealControllersAsync("SyncTime", controller.SerialNumber);
        await _agentApiClient.SyncTimeAsync(controller.SerialNumber, DateTime.UtcNow, ct);
    }

    public async Task<RunStatusModel> GetRunStatusAsync(int controllerId, CancellationToken ct)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null)
        {
            _logger.LogWarning("Controller {ControllerId} not found for GetRunStatus", controllerId);
            return new RunStatusModel { IsOnline = false };
        }

        var status = await GetRunStatusAsync(controller.SerialNumberDisplay, ct);
        if (status == null)
        {
            return new RunStatusModel { IsOnline = false, ControllerTimeUtc = null, LastRecordIndex = null };
        }

        return new RunStatusModel
        {
            IsOnline = true,
            Doors = status.Doors
                .Select(d => new RunStatusModel.DoorStatus
                {
                    DoorIndex = d.DoorNumber,
                    IsDoorOpen = d.IsDoorOpen,
                    IsRelayOn = d.IsRelayOn,
                    IsSensorActive = d.IsSensorActive
                })
                .ToList(),
            LastRecordIndex = null,
            ControllerTimeUtc = null
        };
    }

    public async Task<IReadOnlyList<EventLogModel>> GetEventsByIndexAsync(int controllerId, long fromIndex, int maxCount, CancellationToken ct)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null)
        {
            _logger.LogWarning("Controller {ControllerId} not found for GetEventsByIndex", controllerId);
            return Array.Empty<EventLogModel>();
        }

        var result = await _agentApiClient.GetEventsAsync(controller.SerialNumber, (uint)fromIndex, ct);
        if (result?.Events == null || result.Events.Count == 0)
        {
            return Array.Empty<EventLogModel>();
        }

        var doors = controller.Doors?.ToDictionary(d => d.DoorIndex, d => d.Id) ?? new Dictionary<int, int>();
        return result.Events
            .OrderBy(e => e.RawIndex)
            .SkipWhile(e => e.RawIndex < fromIndex)
            .Take(maxCount > 0 ? maxCount : int.MaxValue)
            .Select(e => new EventLogModel
            {
                Index = e.RawIndex,
                ControllerId = controller.Id,
                DoorNumber = e.DoorNumber,
                DoorId = e.DoorNumber.HasValue && doors.TryGetValue(e.DoorNumber.Value, out var doorId) ? doorId : null,
                TimestampUtc = e.TimestampUtc,
                CardNumber = e.CardNumber,
                EventType = e.EventType,
                ReasonCode = e.ReasonCode,
                IsByCard = e.IsByCard,
                IsByButton = e.IsByButton
            })
            .ToList();
    }

    #endregion

    public async Task<bool> PingAsync(CancellationToken cancellationToken = default)
    {
        await EnsureRealControllersAsync("Ping");
        return await _agentApiClient.PingAsync(cancellationToken);
    }

    public async Task<AgentRunStatusDto?> GetRunStatusAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return null;
        }
        await EnsureRealControllersAsync("GetRunStatus", sn);
        return await _agentApiClient.GetRunStatusAsync(sn, cancellationToken);
    }

    public async Task OpenDoorAsync(string controllerSn, int doorNo, int? durationSec = null, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return;
        }
        await EnsureRealControllersAsync("OpenDoor", sn);
        var success = await _agentApiClient.OpenDoorAsync(sn, doorNo, durationSec, cancellationToken);
        if (!success)
        {
            throw new InvalidOperationException("Agent rejected the open door request.");
        }
    }

    public async Task<ApiResult> AddOrUpdateCardAsync(string controllerSn, AddOrUpdateCardRequestDto request, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return new ApiResult { Success = false, Message = "Invalid controller serial number." };
        }
        await EnsureRealControllersAsync("AddOrUpdateCard", sn);
        return await _agentApiClient.AddOrUpdateCardAsync(sn, request, cancellationToken);
    }

    public async Task<ApiResult> DeleteCardAsync(string controllerSn, string cardNumber, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return new ApiResult { Success = false, Message = "Invalid controller serial number." };
        }
        await EnsureRealControllersAsync("DeleteCard", sn);
        return await _agentApiClient.DeleteCardAsync(sn, cardNumber, cancellationToken);
    }

    public async Task<ApiResult> ClearAllCardsAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return new ApiResult { Success = false, Message = "Invalid controller serial number." };
        }
        await EnsureRealControllersAsync("ClearAllCards", sn);
        return await _agentApiClient.ClearAllCardsAsync(sn, cancellationToken);
    }

    public Task<ControllerEventsResultDto> GetNewEventsAsync(string controllerSn, uint lastIndex, CancellationToken cancellationToken = default)
    {
        // Real implementation would call Agent API endpoint for events
        // For now, return empty result as events are typically handled via ControllerEventService
        _logger.LogWarning("GetNewEventsAsync not yet implemented for RealControllerClient");
        return Task.FromResult(new ControllerEventsResultDto { LastIndex = lastIndex });
    }

    public async Task<TimeScheduleDto?> GetTimeSchedulesAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return null;
        }
        await EnsureRealControllersAsync("GetTimeSchedules", sn);
        return await _agentApiClient.GetTimeSchedulesAsync(sn, cancellationToken);
    }

    public async Task WriteTimeSchedulesAsync(string controllerSn, TimeScheduleWriteDto dto, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return;
        }
        await EnsureRealControllersAsync("WriteTimeSchedules", sn);
        var result = await _agentApiClient.SyncTimeSchedulesAsync(sn, dto, cancellationToken);
        if (!result.Success)
        {
            throw new InvalidOperationException($"Failed to write time schedules: {result.Message}");
        }
    }

    public async Task<ExtendedConfigDto?> GetDoorConfigAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return null;
        }
        await EnsureRealControllersAsync("GetDoorConfig", sn);
        return await _agentApiClient.GetDoorConfigAsync(sn, cancellationToken);
    }

    public async Task WriteDoorConfigAsync(string controllerSn, ExtendedDoorConfigWriteDto dto, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return;
        }
        await EnsureRealControllersAsync("WriteDoorConfig", sn);
        var result = await _agentApiClient.SyncDoorConfigAsync(sn, dto, cancellationToken);
        if (!result.Success)
        {
            throw new InvalidOperationException($"Failed to write door config: {result.Message}");
        }
    }

    public async Task<AutoOpenConfigDto?> GetAutoOpenAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return null;
        }
        await EnsureRealControllersAsync("GetAutoOpen", sn);
        return await _agentApiClient.GetAutoOpenAsync(sn, cancellationToken);
    }

    public async Task WriteAutoOpenAsync(string controllerSn, AutoOpenConfigDto dto, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return;
        }
        await EnsureRealControllersAsync("WriteAutoOpen", sn);
        var result = await _agentApiClient.SyncAutoOpenAsync(sn, dto, cancellationToken);
        if (!result.Success)
        {
            throw new InvalidOperationException($"Failed to write auto-open config: {result.Message}");
        }
    }

    public async Task<AdvancedDoorModesDto?> GetAdvancedDoorModesAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return null;
        }
        await EnsureRealControllersAsync("GetAdvancedDoorModes", sn);
        return await _agentApiClient.GetAdvancedDoorModesAsync(sn, cancellationToken);
    }

    public async Task WriteAdvancedDoorModesAsync(string controllerSn, AdvancedDoorModesDto dto, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return;
        }
        await EnsureRealControllersAsync("WriteAdvancedDoorModes", sn);
        var result = await _agentApiClient.SyncAdvancedDoorModesAsync(sn, dto, cancellationToken);
        if (!result.Success)
        {
            throw new InvalidOperationException($"Failed to write advanced door modes: {result.Message}");
        }
    }

    public async Task<NetworkConfigDto?> GetNetworkConfigAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return null;
        }
        await EnsureRealControllersAsync("GetNetworkConfig", sn);
        return await _agentApiClient.GetNetworkConfigAsync(sn, cancellationToken);
    }

    public async Task SetNetworkConfigAsync(string controllerSn, NetworkConfigDto dto, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return;
        }
        await EnsureRealControllersAsync("SetNetworkConfig", sn);
        var requestDto = new NetworkConfigRequestDto
        {
            IpAddress = dto.IpAddress,
            SubnetMask = dto.SubnetMask,
            Gateway = dto.Gateway,
            Port = dto.Port,
            DhcpEnabled = dto.DhcpEnabled
        };
        var result = await _agentApiClient.SetNetworkConfigAsync(sn, requestDto, cancellationToken);
        if (!result.Success)
        {
            throw new InvalidOperationException($"Failed to set network config: {result.Message}");
        }
    }

    public async Task<AllowedPcAndPasswordRequestDto?> GetAllowedPcSettingsAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return null;
        }
        await EnsureRealControllersAsync("GetAllowedPcSettings", sn);
        return await _agentApiClient.GetAllowedPcAndPasswordAsync(sn, cancellationToken);
    }

    public async Task<ApiResult> SetAllowedPcSettingsAsync(string controllerSn, AllowedPcAndPasswordRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (!uint.TryParse(controllerSn, out var sn))
        {
            _logger.LogWarning("Invalid controller serial number: {Sn}", controllerSn);
            return new ApiResult { Success = false, Message = "Invalid controller serial number." };
        }
        await EnsureRealControllersAsync("SetAllowedPcSettings", sn);
        return await _agentApiClient.SetAllowedPcAndCommPasswordAsync(sn, dto, cancellationToken);
    }

    public Task RebootAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        // Reboot not yet implemented in Agent API
        _logger.LogWarning("RebootAsync not yet implemented");
        return Task.CompletedTask;
    }

    private Task EnsureRealControllersAsync(string operationName, uint? serial = null) =>
        _simulationGuard.EnsureNotSimulationAsync(operationName, controllerSerialNumber: serial);

    private async Task<(ControllerDevice? Controller, Door? Door)> ResolveControllerAndDoorAsync(int controllerId, int doorId, CancellationToken ct)
    {
        var controller = await _controllerRegistry.GetControllerByIdAsync(controllerId, ct);
        if (controller == null)
        {
            _logger.LogWarning("Controller {ControllerId} not found.", controllerId);
            return (null, null);
        }

        var door = controller.Doors.FirstOrDefault(d => d.Id == doorId);
        if (door == null)
        {
            _logger.LogWarning("Door {DoorId} not found for controller {ControllerId}.", doorId, controllerId);
            return (controller, null);
        }

        return (controller, door);
    }

    private static int? ResolveDoorIndex(CardPrivilegeModel model, ControllerDevice controller)
    {
        if (model.DoorIndex.HasValue)
        {
            return model.DoorIndex.Value;
        }

        if (model.DoorId.HasValue)
        {
            var door = controller.Doors.FirstOrDefault(d => d.Id == model.DoorId.Value);
            return door?.DoorIndex;
        }

        return null;
    }
}
