using System;
using System.Collections.Generic;

namespace GFC.BlazorServer.Services.Models;

public class AutoOpenConfigDto
{
    public IReadOnlyList<AutoOpenTask> Tasks { get; set; } = Array.Empty<AutoOpenTask>();

    public sealed class AutoOpenTask
    {
        public int DoorNumber { get; set; }
        public int TimeZoneIndex { get; set; } // Controller profile index
        public bool IsEnabled { get; set; }
    }
}

public class AdvancedDoorModesDto
{
    public IReadOnlyList<DoorAdvancedMode> Doors { get; set; } = Array.Empty<DoorAdvancedMode>();
    public ControllerAdvancedOptions? ControllerOptions { get; set; }

    public sealed class DoorAdvancedMode
    {
        public int DoorNumber { get; set; }
        public bool FirstCardOpenEnabled { get; set; }
        public bool DoorAsSwitchEnabled { get; set; }
        public bool OpenTooLongWarnEnabled { get; set; }
        public bool Invalid3CardsWarnEnabled { get; set; }
    }

    public sealed class ControllerAdvancedOptions
    {
        public int ValidSwipeGapSeconds { get; set; }
    }
}

