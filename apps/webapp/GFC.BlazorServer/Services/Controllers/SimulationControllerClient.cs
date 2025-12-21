using System.Text.Json;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Models;
using GFC.BlazorServer.Services.Simulation;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// Simulation controller client that maintains in-memory state without any network calls.
/// </summary>
public class SimulationControllerClient : IControllerClient
{
    private const int DefaultEventBatchSize = 100;
    private const string DefaultResultStatus = "OK";
    private readonly ISimulationStateEngine _engine;
    private readonly ISimulationTraceService _traceService;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public SimulationControllerClient(ISimulationStateEngine engine, ISimulationTraceService traceService)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        _traceService = traceService ?? throw new ArgumentNullException(nameof(traceService));
    }

    private string SerializePayload(object payload)
    {
        return JsonSerializer.Serialize(payload, _jsonOptions);
    }

    #region SimMode S2 contract

    public async Task OpenDoorAsync(int controllerId, int doorId, CancellationToken ct)
    {
        var traceId = await _traceService.CreateTraceAsync(new SimulationTraceCreateRequest
        {
            Operation = "OpenDoor",
            ControllerId = controllerId,
            DoorId = doorId,
            RequestSummary = $"OpenDoor: Controller {controllerId}, Door {doorId}",
            RequestPayloadJson = SerializePayload(new { controllerId, doorId }),
            ExpectedResponseSummary = "Expected success (no payload).",
            ExpectedResponsePayloadJson = "{}"
        }, ct);

        await _traceService.UpdateResultAsync(traceId, DefaultResultStatus, "Simulation stub executed (no-op).", ct);
    }

    public async Task AddOrUpdatePrivilegeAsync(CardPrivilegeModel model, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(model);

        var traceId = await _traceService.CreateTraceAsync(new SimulationTraceCreateRequest
        {
            Operation = "AddOrUpdatePrivilege",
            ControllerId = model.ControllerId,
            DoorId = model.DoorId,
            CardNumber = model.CardNumber,
            RequestSummary = $"AddOrUpdatePrivilege: Card {model.CardNumber} -> Controller {model.ControllerId}",
            RequestPayloadJson = SerializePayload(model),
            ExpectedResponseSummary = "Expected success (no payload).",
            ExpectedResponsePayloadJson = "{}"
        }, ct);

        await _traceService.UpdateResultAsync(traceId, DefaultResultStatus, "Simulation stub executed (no-op).", ct);
    }

    public async Task DeletePrivilegeAsync(long cardNumber, CancellationToken ct)
    {
        var traceId = await _traceService.CreateTraceAsync(new SimulationTraceCreateRequest
        {
            Operation = "DeletePrivilege",
            CardNumber = cardNumber,
            RequestSummary = $"DeletePrivilege: Card {cardNumber}",
            RequestPayloadJson = SerializePayload(new { cardNumber }),
            ExpectedResponseSummary = "Expected success (no payload).",
            ExpectedResponsePayloadJson = "{}"
        }, ct);

        await _traceService.UpdateResultAsync(traceId, DefaultResultStatus, "Simulation stub executed (no-op).", ct);
    }

    public async Task BulkUploadAsync(IEnumerable<CardPrivilegeModel> models, CancellationToken ct)
    {
        var modelsList = models?.ToList() ?? new List<CardPrivilegeModel>();
        var traceId = await _traceService.CreateTraceAsync(new SimulationTraceCreateRequest
        {
            Operation = "BulkUpload",
            RequestSummary = $"BulkUpload: {modelsList.Count} privilege(s)",
            RequestPayloadJson = SerializePayload(modelsList),
            ExpectedResponseSummary = "Expected success (no payload).",
            ExpectedResponsePayloadJson = "{}"
        }, ct);

        await _traceService.UpdateResultAsync(traceId, DefaultResultStatus, "Simulation stub executed (no-op).", ct);
    }

    public async Task ClearAllCardsAsync(int controllerId, CancellationToken ct)
    {
        var traceId = await _traceService.CreateTraceAsync(new SimulationTraceCreateRequest
        {
            Operation = "ClearAllCards",
            ControllerId = controllerId,
            RequestSummary = $"ClearAllCards: Controller {controllerId}",
            RequestPayloadJson = SerializePayload(new { controllerId }),
            ExpectedResponseSummary = "Expected success (no payload).",
            ExpectedResponsePayloadJson = "{}"
        }, ct);

        await _traceService.UpdateResultAsync(traceId, DefaultResultStatus, "Simulation stub executed (no-op).", ct);
    }

    public async Task SyncTimeAsync(int controllerId, CancellationToken ct)
    {
        var traceId = await _traceService.CreateTraceAsync(new SimulationTraceCreateRequest
        {
            Operation = "SyncTime",
            ControllerId = controllerId,
            RequestSummary = $"SyncTime: Controller {controllerId}",
            RequestPayloadJson = SerializePayload(new { controllerId }),
            ExpectedResponseSummary = "Expected success (no payload).",
            ExpectedResponsePayloadJson = "{}"
        }, ct);

        await _traceService.UpdateResultAsync(traceId, DefaultResultStatus, "Simulation stub executed (no-op).", ct);
    }

    public async Task<RunStatusModel> GetRunStatusAsync(int controllerId, CancellationToken ct)
    {
        var simulatedResponse = new RunStatusModel
        {
            IsOnline = true,
            ControllerTimeUtc = DateTime.UtcNow,
            Doors = Array.Empty<RunStatusModel.DoorStatus>()
        };

        var traceId = await _traceService.CreateTraceAsync(new SimulationTraceCreateRequest
        {
            Operation = "GetRunStatus",
            ControllerId = controllerId,
            RequestSummary = $"GetRunStatus: Controller {controllerId}",
            RequestPayloadJson = SerializePayload(new { controllerId }),
            ExpectedResponseSummary = "Expected success with run status data (simulated).",
            ExpectedResponsePayloadJson = SerializePayload(simulatedResponse)
        }, ct);

        await _traceService.UpdateResultAsync(traceId, DefaultResultStatus, "Simulation stub executed (no-op).", ct);

        return simulatedResponse;
    }

    public async Task<IReadOnlyList<EventLogModel>> GetEventsByIndexAsync(int controllerId, long fromIndex, int maxCount, CancellationToken ct)
    {
        var simulatedResponse = Array.Empty<EventLogModel>();

        var traceId = await _traceService.CreateTraceAsync(new SimulationTraceCreateRequest
        {
            Operation = "GetEventsByIndex",
            ControllerId = controllerId,
            RequestSummary = $"GetEventsByIndex: Controller {controllerId}, From {fromIndex}, Max {maxCount}",
            RequestPayloadJson = SerializePayload(new { controllerId, fromIndex, maxCount }),
            ExpectedResponseSummary = "Expected success with 0 or more events (simulated).",
            ExpectedResponsePayloadJson = SerializePayload(simulatedResponse)
        }, ct);

        await _traceService.UpdateResultAsync(traceId, DefaultResultStatus, "Simulation stub executed (no-op).", ct);

        return simulatedResponse;
    }

    #endregion

    public Task<bool> PingAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    public Task<AgentRunStatusDto?> GetRunStatusAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        return _engine.GetRunStatusAsync(controllerSn, cancellationToken);
    }

    public Task OpenDoorAsync(string controllerSn, int doorNo, int? durationSec = null, CancellationToken cancellationToken = default)
    {
        return _engine.OpenDoorAsync(controllerSn, doorNo, durationSec, "OpenDoor command", cancellationToken);
    }

    public Task<ApiResult> AddOrUpdateCardAsync(string controllerSn, AddOrUpdateCardRequestDto request, CancellationToken cancellationToken = default)
    {
        return _engine.AddOrUpdateCardAsync(controllerSn, request, cancellationToken);
    }

    public Task<ApiResult> DeleteCardAsync(string controllerSn, string cardNumber, CancellationToken cancellationToken = default)
    {
        return _engine.DeleteCardAsync(controllerSn, cardNumber, cancellationToken);
    }

    public Task<ApiResult> ClearAllCardsAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        return _engine.ClearAllCardsAsync(controllerSn, cancellationToken);
    }

    public Task<ControllerEventsResultDto> GetNewEventsAsync(string controllerSn, uint lastIndex, CancellationToken cancellationToken = default)
    {
        return _engine.GetEventsAsync(controllerSn, lastIndex, DefaultEventBatchSize, cancellationToken);
    }

    public Task<TimeScheduleDto?> GetTimeSchedulesAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        return _engine.GetTimeSchedulesAsync(controllerSn, cancellationToken);
    }

    public Task WriteTimeSchedulesAsync(string controllerSn, TimeScheduleWriteDto dto, CancellationToken cancellationToken = default)
    {
        return _engine.WriteTimeSchedulesAsync(controllerSn, dto, cancellationToken);
    }

    public Task<ExtendedConfigDto?> GetDoorConfigAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        return _engine.GetDoorConfigAsync(controllerSn, cancellationToken);
    }

    public Task WriteDoorConfigAsync(string controllerSn, ExtendedDoorConfigWriteDto dto, CancellationToken cancellationToken = default)
    {
        return _engine.WriteDoorConfigAsync(controllerSn, dto, cancellationToken);
    }

    public Task<AutoOpenConfigDto?> GetAutoOpenAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        return _engine.GetAutoOpenAsync(controllerSn, cancellationToken);
    }

    public Task WriteAutoOpenAsync(string controllerSn, AutoOpenConfigDto dto, CancellationToken cancellationToken = default)
    {
        return _engine.WriteAutoOpenAsync(controllerSn, dto, cancellationToken);
    }

    public Task<AdvancedDoorModesDto?> GetAdvancedDoorModesAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        return _engine.GetAdvancedDoorModesAsync(controllerSn, cancellationToken);
    }

    public Task WriteAdvancedDoorModesAsync(string controllerSn, AdvancedDoorModesDto dto, CancellationToken cancellationToken = default)
    {
        return _engine.WriteAdvancedDoorModesAsync(controllerSn, dto, cancellationToken);
    }

    public Task<NetworkConfigDto?> GetNetworkConfigAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        return _engine.GetNetworkConfigAsync(controllerSn, cancellationToken);
    }

    public Task SetNetworkConfigAsync(string controllerSn, NetworkConfigDto dto, CancellationToken cancellationToken = default)
    {
        return _engine.SetNetworkConfigAsync(controllerSn, dto, cancellationToken);
    }

    public Task<AllowedPcAndPasswordRequestDto?> GetAllowedPcSettingsAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        return _engine.GetAllowedPcSettingsAsync(controllerSn, cancellationToken);
    }

    public Task<ApiResult> SetAllowedPcSettingsAsync(string controllerSn, AllowedPcAndPasswordRequestDto dto, CancellationToken cancellationToken = default)
    {
        return _engine.SetAllowedPcSettingsAsync(controllerSn, dto, cancellationToken);
    }

    public Task RebootAsync(string controllerSn, CancellationToken cancellationToken = default)
    {
        return _engine.RebootAsync(controllerSn, cancellationToken);
    }
}
