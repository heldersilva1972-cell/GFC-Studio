using GFC.BlazorServer.Services.Models;

namespace GFC.BlazorServer.Models;

/// <summary>
/// Compiled time schedule data ready for Agent API sync.
/// </summary>
public sealed class TimeScheduleCompiledDto
{
    public TimeScheduleWriteDto Schedule { get; set; } = new();
    
    public Dictionary<int, int> ProfileIndexMapping { get; set; } = new(); // TimeProfileId -> ControllerProfileIndex
}

