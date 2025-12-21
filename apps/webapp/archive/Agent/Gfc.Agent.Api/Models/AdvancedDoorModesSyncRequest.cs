using Gfc.ControllerClient.Models;

namespace Gfc.Agent.Api.Models;

public sealed class AdvancedDoorModesSyncRequest
{
    public ExtendedDoorConfigWriteDto? Config { get; set; }
}

