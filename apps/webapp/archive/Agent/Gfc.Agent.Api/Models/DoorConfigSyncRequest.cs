using Gfc.ControllerClient.Models;

namespace Gfc.Agent.Api.Models;

public sealed class DoorConfigSyncRequest
{
    public ExtendedDoorConfigWriteDto? Config { get; set; }
}

