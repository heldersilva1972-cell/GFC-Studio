using Gfc.ControllerClient.Models;

namespace Gfc.Agent.Api.Models;

public sealed class TimeScheduleSyncRequest
{
    public TimeScheduleWriteDto? Schedule { get; set; }
}

