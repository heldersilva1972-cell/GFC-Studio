using System;

namespace GFC.BlazorServer.Connectors.Mengqi.Models;

public sealed class ControllerEvent
{
    public DateTime TimestampUtc { get; init; }

    public int DoorOrReader { get; init; }

    public long CardNumber { get; init; }

    public ControllerEventType EventType { get; init; }

    public uint ReasonCode { get; init; }
    
    public uint RawIndex { get; init; }
    
    public string? RawData { get; init; }

    public bool IsByCard => EventType == ControllerEventType.Granted || EventType == ControllerEventType.Denied;

    public bool IsByExitButton => EventType == ControllerEventType.ButtonPressed;
}
