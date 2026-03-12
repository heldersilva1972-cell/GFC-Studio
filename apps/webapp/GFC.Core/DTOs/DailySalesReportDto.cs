using System;
using System.Collections.Generic;
using System.Linq;

namespace GFC.Core.DTOs
{
    public class DailySalesReportDto
    {
        public DateTime Date { get; set; }
        public List<ShiftReportDto> Shifts { get; set; } = new();
        
        // Daily Totals
        public decimal TotalBarSales => Shifts.Sum(s => s.BarSales);
        public decimal TotalLottoSales => Shifts.Sum(s => s.LottoSales);
        public decimal TotalLottoPayouts => Shifts.Sum(s => s.LottoPayouts);
        public decimal TotalLottoNetSales => Shifts.Sum(s => s.LottoNetSales);
        public decimal TotalLottoNetDue => Shifts.Sum(s => s.LottoNetDue);
        public decimal TotalVariance => Shifts.Sum(s => s.Variance);
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
        
        // Calculations
        public decimal LottoNetSales => LottoSales - LottoPayouts;
        public decimal ExpectedCash => StartingCash + LottoSales - LottoPayouts + BackupBagAmount;
        public decimal Variance => EndingCash - ExpectedCash;
        
        public string? Notes { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
