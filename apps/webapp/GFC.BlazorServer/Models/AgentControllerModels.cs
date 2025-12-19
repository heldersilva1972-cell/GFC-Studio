using GFC.BlazorServer.Services.Models;

namespace GFC.BlazorServer.Models;

public sealed class AgentRunStatusDto
{
    public IList<AgentDoorStatusDto> Doors { get; set; } = new List<AgentDoorStatusDto>();
    public IList<bool> RelayStates { get; set; } = new List<bool>();
    public bool IsFireAlarmActive { get; set; }
    public bool IsTamperActive { get; set; }
}

public sealed class AgentDoorStatusDto
{
    public int DoorNumber { get; set; }
    public bool IsDoorOpen { get; set; }
    public bool IsRelayOn { get; set; }
    public bool IsSensorActive { get; set; }
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

