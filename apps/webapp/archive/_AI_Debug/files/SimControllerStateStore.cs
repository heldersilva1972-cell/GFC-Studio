using System.Collections.Concurrent;
using GFC.BlazorServer.Models;
using GFC.BlazorServer.Services.Models;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// Singleton in-memory store for simulation controller state.
/// </summary>
public class SimControllerStateStore
{
    private readonly ConcurrentDictionary<string, SimControllerState> _controllers = new();
    private readonly object _lock = new();

    public SimControllerState GetOrCreate(string controllerSn)
    {
        return _controllers.GetOrAdd(controllerSn, _ => new SimControllerState
        {
            SerialNumber = controllerSn
        });
    }

    public SimControllerState? Get(string controllerSn)
    {
        return _controllers.TryGetValue(controllerSn, out var state) ? state : null;
    }

    public void Remove(string controllerSn)
    {
        _controllers.TryRemove(controllerSn, out _);
    }

    public IEnumerable<string> GetAllSerialNumbers()
    {
        return _controllers.Keys;
    }
}

/// <summary>
/// In-memory state for a single simulated controller.
/// </summary>
public class SimControllerState
{
    public string SerialNumber { get; set; } = "";
    public Dictionary<int, SimDoorState> Doors { get; set; } = new();
    public Dictionary<string, uint> Privileges { get; set; } = new(); // CardNo -> door mask (for future phases)
    public AllowedPcAndPasswordRequestDto? AllowedPcSettings { get; set; }
    public TimeScheduleDto? TimeSchedules { get; set; }
    public ExtendedConfigDto? DoorConfig { get; set; }
    public AutoOpenConfigDto? AutoOpen { get; set; }
    public AdvancedDoorModesDto? AdvancedDoorModes { get; set; }
    public NetworkConfigDto? NetworkConfig { get; set; }
    public uint LastEventIndex { get; set; }
    private const int MaxEventBufferSize = 2000;
    private readonly List<ControllerEventDto> _eventBuffer = new();
    public DateTime? LastConfigChangeUtc { get; set; }
    public bool IsFireAlarmActive { get; set; }
    public bool IsTamperActive { get; set; }
    public List<bool> RelayStates { get; set; } = new();
    public DateTime SimNowUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastCommUtc { get; set; } = DateTime.UtcNow;
    public bool IsOnline { get; set; } = true;

    /// <summary>
    /// Thread-safe access to event buffer.
    /// </summary>
    public List<ControllerEventDto> Events
    {
        get
        {
            lock (_eventBuffer)
            {
                return new List<ControllerEventDto>(_eventBuffer);
            }
        }
    }

    /// <summary>
    /// Adds an event to the buffer, handling indexing and trimming.
    /// All events added through this method are marked as simulated.
    /// </summary>
    public void AddEvent(ControllerEventDto evt)
    {
        lock (_eventBuffer)
        {
            LastEventIndex++;
            // Handle uint rollover (wrap around)
            if (LastEventIndex == 0)
            {
                LastEventIndex = 1; // Skip 0 to avoid confusion
            }
            
            evt.RawIndex = LastEventIndex;
            evt.TimestampUtc = SimNowUtc;
            evt.IsSimulated = true; // Mark all events from simulation as simulated
            
            _eventBuffer.Add(evt);
            
            // Trim buffer if it exceeds max size
            if (_eventBuffer.Count > MaxEventBufferSize)
            {
                var toRemove = _eventBuffer.Count - MaxEventBufferSize;
                _eventBuffer.RemoveRange(0, toRemove);
            }
        }
    }

    /// <summary>
    /// Clears all cached events and resets the event index.
    /// </summary>
    public void ResetEvents()
    {
        lock (_eventBuffer)
        {
            _eventBuffer.Clear();
            LastEventIndex = 0;
        }
    }

    /// <summary>
    /// Gets events with index greater than the specified last index.
    /// </summary>
    public List<ControllerEventDto> GetNewEvents(uint lastIndex)
    {
        lock (_eventBuffer)
        {
            return _eventBuffer
                .Where(e => e.RawIndex > lastIndex)
                .OrderBy(e => e.RawIndex)
                .ToList();
        }
    }
}

/// <summary>
/// In-memory state for a single door.
/// </summary>
public class SimDoorState
{
    public int DoorIndex { get; set; }
    public bool IsLocked { get; set; } = true;
    public bool IsDoorOpen { get; set; }
    public bool AlarmActive { get; set; }
    public bool AlarmFlags { get; set; }
    public DateTime? LastOpenCommandUtc { get; set; }
    public DateTime? LastDoorOpenUtc { get; set; }
    public DateTime? LastAutoUnlockUtc { get; set; }
    public DateTime? AutoUnlockUntilUtc { get; set; } // For auto-open windows
    public string? LastOpenCommandSource { get; set; }
    public bool IsRelayOn { get; set; }
    public bool IsSensorActive { get; set; }
    public int? UnlockDurationSeconds { get; set; } // For momentary unlock
    public DateTime? UnlockUntilUtc { get; set; } // For momentary unlock expiration
}
