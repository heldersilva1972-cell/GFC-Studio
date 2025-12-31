using System;
using System.Collections.Generic;

namespace GFC.BlazorServer.Connectors.Mengqi.Models;

public sealed class ExtendedDoorConfigWriteDto
{
    public IReadOnlyList<DoorExtendedConfig> Doors { get; init; } = Array.Empty<DoorExtendedConfig>();

    public sealed class DoorExtendedConfig
    {
        public int DoorNumber { get; init; }

        public byte LockDelaySeconds { get; init; }

        public bool NormallyOpenMode { get; init; }

        public bool DoubleLock { get; init; }

        public bool AllowButtonOpen { get; init; }
    }
}


