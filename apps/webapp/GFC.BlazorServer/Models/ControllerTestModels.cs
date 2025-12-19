namespace GFC.BlazorServer.Models;

public class ControllerRunInfoDto
{
    public bool IsOnline { get; set; }
    public DateTime? ControllerTime { get; set; }
    public uint? LastRecordIndex { get; set; }
    public bool? FireAlarmActive { get; set; }
    public bool? TamperActive { get; set; }
    public bool[]? RelayStates { get; set; }
    public string? FirmwareVersion { get; set; }
}

public class LastRecordPreviewDto
{
    public uint LastRecordIndex { get; set; }
    public DateTime? LastRecordTime { get; set; }
    public int? LastRecordDoorIndex { get; set; }
    public long? LastRecordCardNumber { get; set; }
}

public class SwipeRecordDto
{
    public uint RecordIndex { get; set; }
    public DateTime Timestamp { get; set; }
    public int DoorIndex { get; set; }
    public long CardNumber { get; set; }
    public int EventType { get; set; }
    public int? ReasonCode { get; set; }
    public bool IsByCard { get; set; }
    public bool IsByButton { get; set; }
}

public class DoorStatusDto
{
    public int DoorIndex { get; set; }
    public bool IsDoorOpen { get; set; }
    public bool IsRelayOn { get; set; }
    public bool IsSensorActive { get; set; }
}

