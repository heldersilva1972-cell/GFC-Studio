using System;
using System.Collections.Generic;

namespace GFC.BlazorServer.Connectors.Mengqi.Models;

public sealed class RunStatusModel
{
    public IReadOnlyList<DoorStatus> Doors { get; init; } = Array.Empty<DoorStatus>();

    public IReadOnlyList<bool> RelayStates { get; init; } = Array.Empty<bool>();

    public bool IsFireAlarmActive { get; init; }

    public bool IsTamperActive { get; init; }

    public sealed class DoorStatus
    {
        public int DoorNumber { get; init; }

        public bool IsDoorOpen { get; init; }

        public bool IsRelayOn { get; init; }

        public bool IsSensorActive { get; init; }
    }
}


