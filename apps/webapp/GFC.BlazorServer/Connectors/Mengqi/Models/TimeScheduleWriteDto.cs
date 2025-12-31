using System;
using System.Collections.Generic;

namespace GFC.BlazorServer.Connectors.Mengqi.Models;

/// <summary>
///     Represents the compiled low-level blocks that must be written back to controller memory.
/// </summary>
public sealed class TimeScheduleWriteDto
{
    public IReadOnlyList<TimeScheduleDto.TimeZoneBlock> TimeZones { get; init; } = Array.Empty<TimeScheduleDto.TimeZoneBlock>();

    public IReadOnlyList<TimeScheduleDto.HolidayBlock> Holidays { get; init; } = Array.Empty<TimeScheduleDto.HolidayBlock>();

    public IReadOnlyList<TimeScheduleDto.TaskBlock> Tasks { get; init; } = Array.Empty<TimeScheduleDto.TaskBlock>();
}


