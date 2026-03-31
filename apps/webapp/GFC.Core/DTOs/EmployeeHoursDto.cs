using System;

namespace GFC.Core.DTOs
{
    public class EmployeeHoursDto
    {
        public string Username { get; set; } = string.Empty;
        public string? MemberName { get; set; }
        public decimal TotalHours { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int WeekNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EntryCount { get; set; }
        public decimal? HourlyRate { get; set; }
        public decimal TotalPay => (HourlyRate ?? 0) * TotalHours;
        
        // Tax & Cost Calculations (Defaults to 0, populated by UI/Settings)
        public decimal EmployeeDeductionRate { get; set; } 
        public decimal EmployerSurchargeRate { get; set; }
        
        public decimal NetPay => TotalPay * (1 - EmployeeDeductionRate);
        public decimal TotalPayrollCost => TotalPay * (1 + EmployerSurchargeRate);

        public System.Collections.Generic.Dictionary<DateTime, decimal> DailyHours { get; set; } = new();
        public System.Collections.Generic.Dictionary<DateTime, string> DailyShiftTypes { get; set; } = new();
    }
}
