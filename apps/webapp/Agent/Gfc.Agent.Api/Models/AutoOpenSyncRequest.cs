using Gfc.ControllerClient.Models;

namespace Gfc.Agent.Api.Models;

public sealed class AutoOpenSyncRequest
{
    public TimeScheduleWriteDto? Schedule { get; set; }
}

