using GFC.BlazorServer.Models;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// A dynamic proxy that delegates to either the Real or Simulation Mengqi controller client
/// based on the current system mode at the time of the call.
/// </summary>
public class DynamicMengqiControllerClient : IMengqiControllerClient
{
    private readonly SimulationControllerClient _simClient;
    private readonly RealControllerClient _realClient;
    private readonly IControllerModeProvider _modeProvider;
    private readonly ILogger<DynamicMengqiControllerClient> _logger;

    public DynamicMengqiControllerClient(
        SimulationControllerClient simClient,
        RealControllerClient realClient,
        IControllerModeProvider modeProvider,
        ILogger<DynamicMengqiControllerClient> logger)
    {
        _simClient = simClient ?? throw new ArgumentNullException(nameof(simClient));
        _realClient = realClient ?? throw new ArgumentNullException(nameof(realClient));
        _modeProvider = modeProvider ?? throw new ArgumentNullException(nameof(modeProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private IMengqiControllerClient CurrentClient
    {
        get
        {
            var useReal = _modeProvider.UseRealControllers;
            return useReal ? _realClient : _simClient;
        }
    }

    public Task<ApiResult<bool>> PingAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
        => CurrentClient.PingAsync(ipAddress, port, cancellationToken);

    public Task<ApiResult<ControllerRunInfoDto>> GetRunInfoAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
        => CurrentClient.GetRunInfoAsync(ipAddress, port, cancellationToken);

    public Task<ApiResult<DateTime>> GetTimeAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
        => CurrentClient.GetTimeAsync(ipAddress, port, cancellationToken);

    public Task<ApiResult> SetTimeAsync(string ipAddress, int port, DateTime time, CancellationToken cancellationToken = default)
        => CurrentClient.SetTimeAsync(ipAddress, port, time, cancellationToken);

    public Task<ApiResult> OpenDoorAsync(string ipAddress, int port, int doorIndex, CancellationToken cancellationToken = default)
        => CurrentClient.OpenDoorAsync(ipAddress, port, doorIndex, cancellationToken);

    public Task<ApiResult> CloseDoorAsync(string ipAddress, int port, int doorIndex, CancellationToken cancellationToken = default)
        => CurrentClient.CloseDoorAsync(ipAddress, port, doorIndex, cancellationToken);

    public Task<ApiResult> HoldDoorAsync(string ipAddress, int port, int doorIndex, CancellationToken cancellationToken = default)
        => CurrentClient.HoldDoorAsync(ipAddress, port, doorIndex, cancellationToken);

    public Task<ApiResult<LastRecordPreviewDto>> GetLastRecordPreviewAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
        => CurrentClient.GetLastRecordPreviewAsync(ipAddress, port, cancellationToken);

    public Task<ApiResult<SwipeRecordDto>> GetSingleSwipeAsync(string ipAddress, int port, uint recordIndex, CancellationToken cancellationToken = default)
        => CurrentClient.GetSingleSwipeAsync(ipAddress, port, recordIndex, cancellationToken);

    public Task<ApiResult<DoorStatusDto[]>> GetAllDoorsAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
        => CurrentClient.GetAllDoorsAsync(ipAddress, port, cancellationToken);
}
