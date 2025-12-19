namespace GFC.BlazorServer.Models;

public enum ControllerStatusState
{
    Unknown,
    InSync,
    OutOfSync
}

public class DoorAutoOpenViewModel
{
    public int DoorId { get; set; }
    public string DoorName { get; set; } = "";
    public string ControllerName { get; set; } = "";
    public int DoorIndex { get; set; }
    public int? TimeProfileId { get; set; }
    public string? TimeProfileName { get; set; }
    public bool IsActive { get; set; }
    public int? ControllerProfileIndex { get; set; }
    public ControllerStatusState ControllerStatusState { get; set; } = ControllerStatusState.Unknown;
}

public class DoorAdvancedModeViewModel
{
    public int DoorId { get; set; }
    public string DoorName { get; set; } = "";
    public string ControllerName { get; set; } = "";
    public int DoorIndex { get; set; }
    public bool FirstCardOpenEnabled { get; set; }
    public bool DoorAsSwitchEnabled { get; set; }
    public bool OpenTooLongWarnEnabled { get; set; }
    public bool Invalid3CardsWarnEnabled { get; set; }
    public ControllerStatusState ControllerStatusState { get; set; } = ControllerStatusState.Unknown;
}

public class ControllerBehaviorViewModel
{
    public int ControllerId { get; set; }
    public string Name { get; set; } = "";
    public uint SerialNumber { get; set; }
    public int ValidSwipeGapSeconds { get; set; } = 3;
    public ControllerStatusState ControllerStatusState { get; set; } = ControllerStatusState.Unknown;
}

public class SyncStatusReport
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<string> CommandKeys { get; set; } = new();
}

