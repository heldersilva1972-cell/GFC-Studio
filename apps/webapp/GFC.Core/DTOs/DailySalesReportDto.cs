using System;
using System.Collections.Generic;
using System.Linq;

namespace GFC.Core.DTOs
{
    public class DailySalesReportDto
    {
        public DateTime Date { get; set; }
        public List<ShiftReportDto> Shifts { get; set; } = new();
        
        // Daily Totals - Based on latest cumulative shift report
        public decimal TotalBarSales => Shifts.Sum(s => s.BarSales); 
        
        public decimal TotalLottoSales => GetLatestCumulativeValue(s => s.LottoSales);
        public decimal TotalLottoPayouts => GetLatestCumulativeValue(s => s.LottoPayouts);
        public decimal TotalLottoNetDue => GetLatestCumulativeValue(s => s.LottoNetDue);
        public decimal TotalLottoCancels => GetLatestCumulativeValue(s => s.LottoCancels);

        public decimal TotalLottoNetSales => TotalLottoSales - TotalLottoPayouts - TotalLottoCancels;
        public decimal TotalEnvelope => Shifts.Sum(s => s.EnvelopeAmount);
        public decimal TotalLotteryIncome => TotalLottoNetSales - TotalLottoNetDue;

        public decimal TotalVariance => Shifts.Sum(s => {
            var sSales = GetShiftSales(s);
            var sPayouts = GetShiftPayouts(s);
            var sCancels = GetShiftCancels(s);
            var expectedInDrawer = s.StartingCash + sSales - sPayouts - sCancels + s.BackupBagAmount;
            return s.EndingCash - expectedInDrawer;
        });

        public decimal TotalNetIncome => TotalLotteryIncome + TotalVariance;

        private decimal GetLatestCumulativeValue(Func<ShiftReportDto, decimal> selector)
        {
            // Night reflects the total for the day, so if it exists, use it. Otherwise use Day.
            var night = Shifts.FirstOrDefault(s => s.ShiftType == "Night");
            if (night != null) return selector(night);
            return Shifts.FirstOrDefault(s => s.ShiftType == "Day") != null 
                   ? selector(Shifts.First(s => s.ShiftType == "Day")) 
                   : 0;
        }

        public decimal GetShiftLotteryIncome(ShiftReportDto shift)
        {
            if (shift.ShiftType == "Day" || shift.IsRentalHall) 
                return shift.LotteryIncome;

            if (shift.ShiftType == "Night")
            {
                var day = Shifts.FirstOrDefault(s => s.ShiftType == "Day");
                if (day == null) return shift.LotteryIncome;

                // (Night Cumulative NetSales - Day Cumulative NetSales) - (Night Cumulative NetDue - Day Cumulative NetDue)
                decimal nightOnlySales = (shift.LottoSales - shift.LottoPayouts - shift.LottoCancels) - (day.LottoSales - day.LottoPayouts - day.LottoCancels);
                decimal nightOnlyDue = shift.LottoNetDue - day.LottoNetDue;
                return nightOnlySales - nightOnlyDue;
            }
            return shift.LotteryIncome;
        }

        public decimal GetShiftSales(ShiftReportDto shift)
        {
            if (shift.ShiftType == "Day" || shift.IsRentalHall) return shift.LottoSales;
            var day = Shifts.FirstOrDefault(s => s.ShiftType == "Day");
            return day != null ? shift.LottoSales - day.LottoSales : shift.LottoSales;
        }

        public decimal GetShiftPayouts(ShiftReportDto shift)
        {
            if (shift.ShiftType == "Day" || shift.IsRentalHall) return shift.LottoPayouts;
            var day = Shifts.FirstOrDefault(s => s.ShiftType == "Day");
            return day != null ? shift.LottoPayouts - day.LottoPayouts : shift.LottoPayouts;
        }

        public decimal GetShiftCancels(ShiftReportDto shift)
        {
            if (shift.ShiftType == "Day" || shift.IsRentalHall) return shift.LottoCancels;
            var day = Shifts.FirstOrDefault(s => s.ShiftType == "Day");
            return day != null ? shift.LottoCancels - day.LottoCancels : shift.LottoCancels;
        }

        public decimal GetShiftNetDue(ShiftReportDto shift)
        {
            return shift.LottoNetDue;
        }

        public decimal GetShiftNetIncome(ShiftReportDto shift)
        {
            return GetShiftLotteryIncome(shift) + shift.Variance;
        }
    }

    public class ShiftReportDto
    {
        public string ShiftType { get; set; } = string.Empty; // Day, Night, Hall
        public bool IsRentalHall { get; set; }
        
        // Bar Info
        public decimal BarSales { get; set; }
        public decimal? TotalHours { get; set; }
        
        // Lottery Info
        public decimal LottoSales { get; set; }
        public decimal LottoPayouts { get; set; }
        public decimal LottoNetDue { get; set; }
        public decimal StartingCash { get; set; }
        public decimal EndingCash { get; set; }
        public decimal BackupBagAmount { get; set; }
        public decimal EnvelopeAmount { get; set; }
        public decimal LottoCancels { get; set; }
        
        // Calculations
        public decimal LottoNetSales => LottoSales - LottoPayouts - LottoCancels;
        public decimal ExpectedCash => StartingCash + LottoSales - LottoPayouts - LottoCancels + BackupBagAmount;
        public decimal Variance => EndingCash - ExpectedCash;
        public decimal LotteryIncome => LottoNetSales - LottoNetDue;
        public decimal NetIncome => LotteryIncome + Variance;
        
        public string? Notes { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
