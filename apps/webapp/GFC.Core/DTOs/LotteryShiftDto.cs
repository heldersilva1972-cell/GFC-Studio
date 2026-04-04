namespace GFC.Core.DTOs
{
    /// <summary>
    /// DTO for displaying lottery shift information in lists and reports.
    /// </summary>
    public class LotteryShiftDto
    {
        public int ShiftId { get; set; }
        public DateTime ShiftDate { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string? ShiftType { get; set; }
        public string? MachineId { get; set; }
        public decimal StartingCash { get; set; }
        public decimal EndingCash { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalPayouts { get; set; }
        public decimal TotalCancels { get; set; }
        public decimal Commission { get; set; }
        public decimal CashBonus { get; set; }
        public decimal ClaimsBonus { get; set; }
        public decimal NetDue { get; set; }
        public decimal NetSales { get; set; }
        public decimal ExpectedCash { get; set; }
        public decimal Variance { get; set; }
        public decimal LotteryIncome { get; set; }
        public decimal BagRefillAmount { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
        public bool IsReconciled { get; set; }
        public string? ReconciledBy { get; set; }
        public DateTime? ReconciledDate { get; set; }
        public decimal BackupBagAmount { get; set; }
        public decimal EnvelopeAmount { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // Joined Data from BarSaleEntries
        public decimal? BarLaborHours { get; set; }
        public decimal? BarRegisterSales { get; set; }
    }

    /// <summary>
    /// DTO for lottery shift summaries (daily, weekly, monthly).
    /// </summary>
    public class LotteryShiftSummaryDto
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string PeriodLabel { get; set; } = string.Empty;
        public int ShiftCount { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalPayouts { get; set; }
        public decimal TotalCancels { get; set; }
        public decimal TotalNetDue { get; set; }
        public decimal TotalNetSales { get; set; }
        public decimal TotalEnvelope { get; set; }
        public decimal TotalVariance { get; set; }
        public decimal AverageVariance { get; set; }
        public int VarianceCount { get; set; } // Number of shifts with variance
        public decimal LargestVariance { get; set; }
        public decimal SmallestVariance { get; set; }
    }
}

