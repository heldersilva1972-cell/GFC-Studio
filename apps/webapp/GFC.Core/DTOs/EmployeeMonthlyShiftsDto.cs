using System;
using System.Collections.Generic;

namespace GFC.Core.DTOs;

public class EmployeeMonthlyShiftsDto
{
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int TotalDaysWorked { get; set; }
    public int TotalShifts { get; set; }
    public decimal TotalHours { get; set; }
    public List<EmployeeShiftItemDto> Shifts { get; set; } = new();
}

public class EmployeeShiftItemDto
{
    public DateTime Date { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public string FormattedDate { get; set; } = string.Empty;
    public string ShiftType { get; set; } = "Day";
    public string Location { get; set; } = "Main";
    public decimal Hours { get; set; }
    public string? Notes { get; set; }
}
