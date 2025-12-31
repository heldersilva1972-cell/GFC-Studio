using System;
using System.Collections.Generic;

namespace GFC.BlazorServer.Connectors.Mengqi.Models;

public sealed class TimeScheduleDto
{
    public IReadOnlyList<TimeZoneBlock> TimeZones { get; init; } = Array.Empty<TimeZoneBlock>();

    public IReadOnlyList<HolidayBlock> Holidays { get; init; } = Array.Empty<HolidayBlock>();

    public IReadOnlyList<TaskBlock> Tasks { get; init; } = Array.Empty<TaskBlock>();

    public sealed record TimeZoneBlock(int Index, IReadOnlyList<DailySchedule> Days);

    public sealed record DailySchedule(byte StartHour, byte StartMinute, byte EndHour, byte EndMinute);

    public sealed record HolidayBlock(int Index, DateOnly StartDate, DateOnly EndDate);

    public sealed record TaskBlock(int Index, byte Action, byte Door, byte ScheduleId);
}


