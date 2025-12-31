using GFC.BlazorServer.Services.Models;

namespace GFC.BlazorServer.Models;

public sealed class AgentRunStatusDto
{
    public uint SerialNumber { get; set; }
    public bool IsOnline { get; set; }
    public DateTime? ControllerTimeUtc { get; set; }
    public List<DoorRunStatus> Doors { get; set; } = new List<DoorRunStatus>();
    public List<bool> RelayStates { get; set; } = new List<bool>();
    public bool IsFireAlarmActive { get; set; }
    public bool IsTamperActive { get; set; }
    
    public uint TotalCards { get; set; }
    public uint TotalEvents { get; set; }

    public class DoorRunStatus
    {
        public int DoorNumber { get; set; }
        public bool IsDoorOpen { get; set; }
        public bool IsRelayOn { get; set; }
        public bool IsSensorActive { get; set; }
    }
}



public sealed class OpenDoorRequestDto
{
    public int? DurationSec { get; set; }
}

public sealed class TimeScheduleSyncRequest
{
    public TimeScheduleWriteDto? Schedule { get; set; }
}

public sealed class DoorConfigSyncRequest
{
    public ExtendedDoorConfigWriteDto? Config { get; set; }
}

public sealed class AutoOpenSyncRequest
{
    public AutoOpenConfigDto? Config { get; set; }
}

public sealed class AdvancedDoorModesSyncRequest
{
    public AdvancedDoorModesDto? Config { get; set; }
}

public sealed class SyncTimeRequestDto
{
    public DateTime? ServerTimeUtc { get; set; }
}

