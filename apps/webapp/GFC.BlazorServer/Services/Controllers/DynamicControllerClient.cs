using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Models;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// A dynamic proxy that delegates to either the Real or Simulation controller client
/// based on the current system mode at the time of the call.
/// This prevents the implementation from being locked for the lifetime of the scope.
/// </summary>
public class DynamicControllerClient : IControllerClient
{
    private readonly SimulationControllerClient _simClient;
    private readonly RealControllerClient _realClient;
    private readonly IControllerModeProvider _modeProvider;
    private readonly ILogger<DynamicControllerClient> _logger;

    public DynamicControllerClient(
        SimulationControllerClient simClient,
        RealControllerClient realClient,
        IControllerModeProvider modeProvider,
        ILogger<DynamicControllerClient> logger)
    {
        _simClient = simClient ?? throw new ArgumentNullException(nameof(simClient));
        _realClient = realClient ?? throw new ArgumentNullException(nameof(realClient));
        _modeProvider = modeProvider ?? throw new ArgumentNullException(nameof(modeProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private IControllerClient CurrentClient
    {
        get
        {
            var useReal = _modeProvider.UseRealControllers;
            // Logger is verbose, good for debugging the switch
            // _logger.LogTrace("DynamicControllerClient resolving. Mode: {Mode}", useReal ? "REAL" : "SIMULATION");
            return useReal ? _realClient : _simClient;
        }
    }

    // Access-control focused contract
    public Task OpenDoorAsync(int controllerId, int doorId, CancellationToken ct) 
        => CurrentClient.OpenDoorAsync(controllerId, doorId, ct);
    
    public Task AddOrUpdatePrivilegeAsync(CardPrivilegeModel model, CancellationToken ct) 
        => CurrentClient.AddOrUpdatePrivilegeAsync(model, ct);

    public Task DeletePrivilegeAsync(long cardNumber, CancellationToken ct) 
        => CurrentClient.DeletePrivilegeAsync(cardNumber, ct);

    public Task BulkUploadAsync(IEnumerable<CardPrivilegeModel> models, CancellationToken ct) 
        => CurrentClient.BulkUploadAsync(models, ct);

    public Task ClearAllCardsAsync(int controllerId, CancellationToken ct) 
        => CurrentClient.ClearAllCardsAsync(controllerId, ct);

    public Task SyncTimeAsync(int controllerId, CancellationToken ct) 
        => CurrentClient.SyncTimeAsync(controllerId, ct);

    public Task<RunStatusModel> GetRunStatusAsync(int controllerId, CancellationToken ct) 
        => CurrentClient.GetRunStatusAsync(controllerId, ct);

    public Task<IReadOnlyList<EventLogModel>> GetEventsByIndexAsync(int controllerId, long fromIndex, int maxCount, CancellationToken ct) 
        => CurrentClient.GetEventsByIndexAsync(controllerId, fromIndex, maxCount, ct);

    // Legacy/Generic contract
    public Task<bool> PingAsync(CancellationToken cancellationToken = default) 
        => CurrentClient.PingAsync(cancellationToken);

    public Task<AgentRunStatusDto?> GetRunStatusAsync(string controllerSn, CancellationToken cancellationToken = default) 
        => CurrentClient.GetRunStatusAsync(controllerSn, cancellationToken);

    public Task OpenDoorAsync(string controllerSn, int doorNo, int? durationSec = null, CancellationToken cancellationToken = default) 
        => CurrentClient.OpenDoorAsync(controllerSn, doorNo, durationSec, cancellationToken);

    public Task<ApiResult> AddOrUpdateCardAsync(string controllerSn, AddOrUpdateCardRequestDto request, CancellationToken cancellationToken = default) 
        => CurrentClient.AddOrUpdateCardAsync(controllerSn, request, cancellationToken);

    public Task<ApiResult> DeleteCardAsync(string controllerSn, string cardNumber, CancellationToken cancellationToken = default) 
        => CurrentClient.DeleteCardAsync(controllerSn, cardNumber, cancellationToken);

    public Task<ApiResult> ClearAllCardsAsync(string controllerSn, CancellationToken cancellationToken = default) 
        => CurrentClient.ClearAllCardsAsync(controllerSn, cancellationToken);

    public Task<ControllerEventsResultDto> GetNewEventsAsync(string controllerSn, uint lastIndex, CancellationToken cancellationToken = default) 
        => CurrentClient.GetNewEventsAsync(controllerSn, lastIndex, cancellationToken);

    public Task<TimeScheduleDto?> GetTimeSchedulesAsync(string controllerSn, CancellationToken cancellationToken = default) 
        => CurrentClient.GetTimeSchedulesAsync(controllerSn, cancellationToken);

    public Task WriteTimeSchedulesAsync(string controllerSn, TimeScheduleWriteDto dto, CancellationToken cancellationToken = default) 
        => CurrentClient.WriteTimeSchedulesAsync(controllerSn, dto, cancellationToken);

    public Task<ExtendedConfigDto?> GetDoorConfigAsync(string controllerSn, CancellationToken cancellationToken = default) 
        => CurrentClient.GetDoorConfigAsync(controllerSn, cancellationToken);

    public Task WriteDoorConfigAsync(string controllerSn, ExtendedDoorConfigWriteDto dto, CancellationToken cancellationToken = default) 
        => CurrentClient.WriteDoorConfigAsync(controllerSn, dto, cancellationToken);

    public Task<AutoOpenConfigDto?> GetAutoOpenAsync(string controllerSn, CancellationToken cancellationToken = default) 
        => CurrentClient.GetAutoOpenAsync(controllerSn, cancellationToken);

    public Task WriteAutoOpenAsync(string controllerSn, AutoOpenConfigDto dto, CancellationToken cancellationToken = default) 
        => CurrentClient.WriteAutoOpenAsync(controllerSn, dto, cancellationToken);

    public Task<AdvancedDoorModesDto?> GetAdvancedDoorModesAsync(string controllerSn, CancellationToken cancellationToken = default) 
        => CurrentClient.GetAdvancedDoorModesAsync(controllerSn, cancellationToken);

    public Task WriteAdvancedDoorModesAsync(string controllerSn, AdvancedDoorModesDto dto, CancellationToken cancellationToken = default) 
        => CurrentClient.WriteAdvancedDoorModesAsync(controllerSn, dto, cancellationToken);

    public Task<NetworkConfigDto?> GetNetworkConfigAsync(string controllerSn, CancellationToken cancellationToken = default) 
        => CurrentClient.GetNetworkConfigAsync(controllerSn, cancellationToken);

    public Task SetNetworkConfigAsync(string controllerSn, NetworkConfigDto dto, CancellationToken cancellationToken = default) 
        => CurrentClient.SetNetworkConfigAsync(controllerSn, dto, cancellationToken);

    public Task<AllowedPcAndPasswordRequestDto?> GetAllowedPcSettingsAsync(string controllerSn, CancellationToken cancellationToken = default) 
        => CurrentClient.GetAllowedPcSettingsAsync(controllerSn, cancellationToken);

    public Task<ApiResult> SetAllowedPcSettingsAsync(string controllerSn, AllowedPcAndPasswordRequestDto dto, CancellationToken cancellationToken = default) 
        => CurrentClient.SetAllowedPcSettingsAsync(controllerSn, dto, cancellationToken);

    public Task RebootAsync(string controllerSn, CancellationToken cancellationToken = default) 
        => CurrentClient.RebootAsync(controllerSn, cancellationToken);
}
