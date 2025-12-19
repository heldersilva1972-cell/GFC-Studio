using System;
using System.Collections.Generic;

namespace GFC.BlazorServer.Services.Models;

public class ControllerTimeZoneDto
{
    public int Index { get; set; }                // Firmware index
    public string Name { get; set; } = "";        // Label
    public List<ControllerTimeZoneIntervalDto> Intervals { get; set; } = new();
}

public class ControllerTimeZoneIntervalDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int Order { get; set; }
}

public class ControllerHolidayDto
{
    public string Name { get; set; } = "";
    public DateTime Date { get; set; }
    public bool IsRecurring { get; set; }
}

public class ControllerTaskDto
{
    public int Index { get; set; }
    public string Description { get; set; } = "";
    public string FirmwarePayload { get; set; } = ""; // raw hex/text as needed later
}

/// <summary>
/// What the Agent returns when reading schedules from controller.
/// Matches the firmware structure with TimeZoneBlock, HolidayBlock, TaskBlock.
/// </summary>
public class TimeScheduleDto
{
    public List<TimeZoneBlock> TimeZones { get; set; } = new();
    public List<HolidayBlock> Holidays { get; set; } = new();
    public List<TaskBlock> Tasks { get; set; } = new();

    public sealed record TimeZoneBlock(int Index, IReadOnlyList<DailySchedule> Days);
    public sealed record DailySchedule(byte StartHour, byte StartMinute, byte EndHour, byte EndMinute);
    public sealed record HolidayBlock(int Index, DateOnly StartDate, DateOnly EndDate);
    public sealed record TaskBlock(int Index, byte Action, byte Door, byte ScheduleId);
}

/// <summary>
/// What the WebApp sends to Agent when syncing schedules.
/// </summary>
public class TimeScheduleWriteDto
{
    public IReadOnlyList<TimeScheduleDto.TimeZoneBlock> TimeZones { get; set; } = Array.Empty<TimeScheduleDto.TimeZoneBlock>();
    public IReadOnlyList<TimeScheduleDto.HolidayBlock> Holidays { get; set; } = Array.Empty<TimeScheduleDto.HolidayBlock>();
    public IReadOnlyList<TimeScheduleDto.TaskBlock> Tasks { get; set; } = Array.Empty<TimeScheduleDto.TaskBlock>();
}

