using System;
using System.Collections.Generic;
using System.Linq;

namespace GFC.Core.DTOs
{
    public class DailySalesReportDto
    {
        public DateTime Date { get; set; }
        public List<ShiftReportDto> Shifts { get; set; } = new();
        
        // Daily Totals - Based on the night shift (Gold Standard) if available
        public decimal TotalBarSales => Shifts.Sum(s => s.BarSales); 
        
        // Cumulative Totals for the Day (derived from the Night shift record)
        public decimal TotalLottoSales => GetLatestCumulativeValue(s => s.LottoSales);
        public decimal TotalLottoPayouts => GetLatestCumulativeValue(s => s.LottoPayouts);
        public decimal TotalLottoNetDue => GetLatestCumulativeValue(s => s.LottoNetDue);
        public decimal TotalLottoCancels => GetLatestCumulativeValue(s => s.LottoCancels);

        // Daily Performance Metrics - Sum of shift activities
        public decimal TotalLottoNetSales => Shifts.Sum(s => s.NetSales);
        public decimal TotalEnvelope => Shifts.Sum(s => s.EnvelopeAmount);
        public decimal TotalLotteryIncome => Shifts.Sum(s => s.LotteryIncome);
        public decimal TotalVariance => Shifts.Sum(s => s.Variance);
        public decimal TotalNetIncome => Shifts.Sum(s => s.NetIncome);

        private decimal GetLatestCumulativeValue(Func<ShiftReportDto, decimal> selector)
        {
            // Night reflects the total for the day, so if it exists, use it. Otherwise use Day.
            var night = Shifts.FirstOrDefault(s => s.ShiftType == "Night");
            if (night != null) return selector(night);
            var day = Shifts.FirstOrDefault(s => s.ShiftType == "Day");
            return day != null ? selector(day) : 0;
        }

        // Methods to get shift-specific data without doing math (just returning the persistent field)
        public decimal GetShiftSales(ShiftReportDto shift) => shift.ShiftSalesActivity;
        public decimal GetShiftPayouts(ShiftReportDto shift) => shift.ShiftPayoutsActivity;
        public decimal GetShiftCancels(ShiftReportDto shift) => shift.ShiftCancelsActivity;
        public decimal GetShiftNetDue(ShiftReportDto shift) => shift.ShiftNetDueActivity;
        public decimal GetShiftLotteryIncome(ShiftReportDto shift) => shift.LotteryIncome;
        public decimal GetShiftNetIncome(ShiftReportDto shift) => shift.NetIncome;
    }

    public class ShiftReportDto
    {
        public int ShiftId { get; set; }
        public string ShiftType { get; set; } = string.Empty; // Day, Night, Hall
        public bool IsRentalHall { get; set; }
        
        // Bar Info
        public decimal BarSales { get; set; }
        public decimal? TotalHours { get; set; }
        public decimal? HourlyRate { get; set; }
        
        // Lottery Info (Raw Machine Readings - Cumulative for Night Shift)
        public decimal LottoSales { get; set; }
        public decimal LottoPayouts { get; set; }
        public decimal LottoNetDue { get; set; }
        public decimal LottoCancels { get; set; }
        
        // Cash Info
        public decimal StartingCash { get; set; }
        public decimal EndingCash { get; set; }
        public decimal BackupBagAmount { get; set; }
        public decimal EnvelopeAmount { get; set; }
        public decimal BagRefillAmount { get; set; }
        
        // Persistent Calculations (Populated from LotteryShift model)
        public decimal NetSales { get; set; }
        public decimal ExpectedCash { get; set; }
        public decimal Variance { get; set; }
        public decimal LotteryIncome { get; set; }
        public decimal NetIncome { get; set; }
        
        // Shift-Specific Activity (Non-cumulative)
        public decimal ShiftSalesActivity { get; set; }
        public decimal ShiftPayoutsActivity { get; set; }
        public decimal ShiftCancelsActivity { get; set; }
        public decimal ShiftNetDueActivity { get; set; }
        
        public string? Notes { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Status { get; set; }
    }
}
