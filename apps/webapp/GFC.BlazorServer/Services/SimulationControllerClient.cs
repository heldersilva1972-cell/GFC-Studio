using GFC.BlazorServer.Models;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Simulation implementation of IMengqiControllerClient.
/// NO network packets are sent. All operations use in-memory simulation state.
/// This is used when UseRealControllers = false.
/// </summary>
public class SimulationControllerClient : IMengqiControllerClient
{
    private readonly ILogger<SimulationControllerClient> _logger;
    
    // In-memory simulation state
    private static readonly Dictionary<string, SimulationControllerState> _simulationState = new();
    private static readonly object _stateLock = new();

    public SimulationControllerClient(ILogger<SimulationControllerClient> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private SimulationControllerState GetOrCreateState(string ipAddress, int port)
    {
        var key = $"{ipAddress}:{port}";
        lock (_stateLock)
        {
            if (!_simulationState.TryGetValue(key, out var state))
            {
                state = new SimulationControllerState
                {
                    IpAddress = ipAddress,
                    Port = port,
                    IsOnline = true,
                    ControllerTime = DateTime.UtcNow,
                    PowerOnCount = 1,
                    LastRecordIndex = 0,
                    CardCount = 0,
                    EventCount = 0,
                    FirmwareVersion = "SIM-1.0.0",
                    SerialNumber = (uint)(ipAddress.GetHashCode() & 0x7FFFFFFF),
                    DoorStates = new Dictionary<int, DoorSimulationState>()
                };
                _simulationState[key] = state;
            }
            return state;
        }
    }

    public async Task<ApiResult<bool>> PingAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
    {
        await Task.Delay(50, cancellationToken); // Simulate minimal delay
        
        var state = GetOrCreateState(ipAddress, port);
        _logger.LogInformation("[SIMULATION] Ping to {IpAddress}:{Port} - Online: {IsOnline}", ipAddress, port, state.IsOnline);
        
        return new ApiResult<bool>
        {
            Success = true,
            Data = state.IsOnline,
            Message = "Simulation: Ping successful"
        };
    }

    public async Task<ApiResult<ControllerRunInfoDto>> GetRunInfoAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        
        var state = GetOrCreateState(ipAddress, port);
        _logger.LogInformation("[SIMULATION] GetRunInfo from {IpAddress}:{Port}", ipAddress, port);
        
        return new ApiResult<ControllerRunInfoDto>
        {
            Success = true,
            Data = new ControllerRunInfoDto
            {
                IsOnline = state.IsOnline,
                ControllerTime = state.ControllerTime,
                LastRecordIndex = state.LastRecordIndex,
                FireAlarmActive = false,
                TamperActive = false,
                RelayStates = new bool[4],
                FirmwareVersion = state.FirmwareVersion
            },
            Message = "Simulation: Run info retrieved"
        };
    }

    public async Task<ApiResult<DateTime>> GetTimeAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
    {
        await Task.Delay(50, cancellationToken);
        
        var state = GetOrCreateState(ipAddress, port);
        _logger.LogInformation("[SIMULATION] GetTime from {IpAddress}:{Port}", ipAddress, port);
        
        return new ApiResult<DateTime>
        {
            Success = true,
            Data = state.ControllerTime,
            Message = "Simulation: Time retrieved"
        };
    }

    public async Task<ApiResult> SetTimeAsync(string ipAddress, int port, DateTime time, CancellationToken cancellationToken = default)
    {
        await Task.Delay(50, cancellationToken);
        
        var state = GetOrCreateState(ipAddress, port);
        state.ControllerTime = time;
        _logger.LogInformation("[SIMULATION] SetTime on {IpAddress}:{Port} to {Time}", ipAddress, port, time);
        
        return new ApiResult
        {
            Success = true,
            Message = "Simulation: Time set successfully"
        };
    }

    public async Task<ApiResult> OpenDoorAsync(string ipAddress, int port, int doorIndex, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        
        var state = GetOrCreateState(ipAddress, port);
        if (!state.DoorStates.TryGetValue(doorIndex, out var doorState))
        {
            doorState = new DoorSimulationState { DoorIndex = doorIndex };
            state.DoorStates[doorIndex] = doorState;
        }
        
        doorState.IsRelayOn = true;
        doorState.IsDoorOpen = true;
        _logger.LogInformation("[SIMULATION] OpenDoor {DoorIndex} on {IpAddress}:{Port}", doorIndex, ipAddress, port);
        
        return new ApiResult
        {
            Success = true,
            Message = $"Simulation: Door {doorIndex} opened"
        };
    }

    public async Task<ApiResult> CloseDoorAsync(string ipAddress, int port, int doorIndex, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        
        var state = GetOrCreateState(ipAddress, port);
        if (!state.DoorStates.TryGetValue(doorIndex, out var doorState))
        {
            doorState = new DoorSimulationState { DoorIndex = doorIndex };
            state.DoorStates[doorIndex] = doorState;
        }
        
        doorState.IsRelayOn = false;
        doorState.IsDoorOpen = false;
        _logger.LogInformation("[SIMULATION] CloseDoor {DoorIndex} on {IpAddress}:{Port}", doorIndex, ipAddress, port);
        
        return new ApiResult
        {
            Success = true,
            Message = $"Simulation: Door {doorIndex} closed"
        };
    }

    public async Task<ApiResult> HoldDoorAsync(string ipAddress, int port, int doorIndex, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        
        var state = GetOrCreateState(ipAddress, port);
        if (!state.DoorStates.TryGetValue(doorIndex, out var doorState))
        {
            doorState = new DoorSimulationState { DoorIndex = doorIndex };
            state.DoorStates[doorIndex] = doorState;
        }
        
        doorState.IsRelayOn = true;
        doorState.IsDoorOpen = true;
        doorState.IsHeld = true;
        _logger.LogInformation("[SIMULATION] HoldDoor {DoorIndex} on {IpAddress}:{Port}", doorIndex, ipAddress, port);
        
        return new ApiResult
        {
            Success = true,
            Message = $"Simulation: Door {doorIndex} held open"
        };
    }

    public async Task<ApiResult<LastRecordPreviewDto>> GetLastRecordPreviewAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        
        var state = GetOrCreateState(ipAddress, port);
        _logger.LogInformation("[SIMULATION] GetLastRecordPreview from {IpAddress}:{Port}", ipAddress, port);
        
        return new ApiResult<LastRecordPreviewDto>
        {
            Success = true,
            Data = new LastRecordPreviewDto
            {
                LastRecordIndex = state.LastRecordIndex,
                LastRecordTime = state.LastRecordIndex > 0 ? state.ControllerTime.AddMinutes(-5) : null,
                LastRecordDoorIndex = state.LastRecordIndex > 0 ? 1 : null,
                LastRecordCardNumber = state.LastRecordIndex > 0 ? 1234567890L : null
            },
            Message = "Simulation: Last record preview retrieved"
        };
    }

    public async Task<ApiResult<SwipeRecordDto>> GetSingleSwipeAsync(string ipAddress, int port, uint recordIndex, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        
        var state = GetOrCreateState(ipAddress, port);
        _logger.LogInformation("[SIMULATION] GetSingleSwipe {RecordIndex} from {IpAddress}:{Port}", recordIndex, ipAddress, port);
        
        if (recordIndex > state.LastRecordIndex)
        {
            return new ApiResult<SwipeRecordDto>
            {
                Success = false,
                Message = $"Simulation: Record index {recordIndex} does not exist (last index: {state.LastRecordIndex})"
            };
        }
        
        return new ApiResult<SwipeRecordDto>
        {
            Success = true,
            Data = new SwipeRecordDto
            {
                RecordIndex = recordIndex,
                Timestamp = state.ControllerTime.AddMinutes(-(int)(state.LastRecordIndex - recordIndex) * 5),
                DoorIndex = 1,
                CardNumber = 1234567890L,
                EventType = 1,
                ReasonCode = 0,
                IsByCard = true,
                IsByButton = false
            },
            Message = $"Simulation: Swipe record {recordIndex} retrieved"
        };
    }

    public async Task<ApiResult<DoorStatusDto[]>> GetAllDoorsAsync(string ipAddress, int port, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);
        
        var state = GetOrCreateState(ipAddress, port);
        _logger.LogInformation("[SIMULATION] GetAllDoors from {IpAddress}:{Port}", ipAddress, port);
        
        // Return simulation state for up to 4 doors
        var doors = new List<DoorStatusDto>();
        for (int i = 1; i <= 4; i++)
        {
            if (state.DoorStates.TryGetValue(i, out var doorState))
            {
                doors.Add(new DoorStatusDto
                {
                    DoorIndex = i,
                    IsDoorOpen = doorState.IsDoorOpen,
                    IsRelayOn = doorState.IsRelayOn,
                    IsSensorActive = doorState.IsSensorActive
                });
            }
            else
            {
                doors.Add(new DoorStatusDto
                {
                    DoorIndex = i,
                    IsDoorOpen = false,
                    IsRelayOn = false,
                    IsSensorActive = false
                });
            }
        }
        
        return new ApiResult<DoorStatusDto[]>
        {
            Success = true,
            Data = doors.ToArray(),
            Message = "Simulation: All doors status retrieved"
        };
    }

    private class SimulationControllerState
    {
        public string IpAddress { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool IsOnline { get; set; }
        public DateTime ControllerTime { get; set; }
        public uint PowerOnCount { get; set; }
        public uint LastRecordIndex { get; set; }
        public uint CardCount { get; set; }
        public uint EventCount { get; set; }
        public string FirmwareVersion { get; set; } = string.Empty;
        public uint SerialNumber { get; set; }
        public Dictionary<int, DoorSimulationState> DoorStates { get; set; } = new();
    }

    private class DoorSimulationState
    {
        public int DoorIndex { get; set; }
        public bool IsDoorOpen { get; set; }
        public bool IsRelayOn { get; set; }
        public bool IsSensorActive { get; set; }
        public bool IsHeld { get; set; }
    }
}

