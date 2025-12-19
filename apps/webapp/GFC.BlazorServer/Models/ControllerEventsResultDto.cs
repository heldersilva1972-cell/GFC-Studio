namespace GFC.BlazorServer.Models;

/// <summary>
/// Result of fetching new controller events.
/// </summary>
public class ControllerEventsResultDto
{
    public IList<ControllerEventDto> Events { get; set; } = new List<ControllerEventDto>();
    public uint LastIndex { get; set; }
}

/// <summary>
/// Represents a single controller event.
/// </summary>
public class ControllerEventDto
{
    public uint RawIndex { get; set; }
    public DateTime TimestampUtc { get; set; }
    public int? DoorNumber { get; set; }
    public long? CardNumber { get; set; }
    public int EventType { get; set; }
    public int? ReasonCode { get; set; }
    public bool IsByCard { get; set; }
    public bool IsByButton { get; set; }
    public string? RawData { get; set; }
    public bool IsSimulated { get; set; } = false;
}
