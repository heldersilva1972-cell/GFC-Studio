namespace GFC.Core.Models
{
    /// <summary>
    /// Represents a lottery sales shift with cash and sales tracking.
    /// </summary>
    public class LotteryShift
    {
        public int ShiftId { get; set; }
        public DateTime ShiftDate { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string? ShiftType { get; set; } // Optional: "Day", "Night", etc.
        public string? MachineId { get; set; } 
        
        // Cash tracking
        public decimal StartingCash { get; set; }
        public decimal EndingCash { get; set; }
        
        // Machine report values (Cumulative for Night Shift)
        public decimal TotalSales { get; set; }
        public decimal TotalPayouts { get; set; }
        public decimal TotalCancels { get; set; }
        
        public decimal Commission { get; set; }
        public decimal CashBonus { get; set; }
        public decimal ClaimsBonus { get; set; }
        public decimal NetDue { get; set; }

        public decimal BackupBagAmount { get; set; }
        public decimal EnvelopeAmount { get; set; }
        public decimal BagRefillAmount { get; set; }

        // Shift-Specific Activity (Calculated at submission, persistent for reporting)
        public decimal ShiftSalesActivity { get; set; }
        public decimal ShiftPayoutsActivity { get; set; }
        public decimal ShiftCancelsActivity { get; set; }
        public decimal ShiftNetDueActivity { get; set; }

        // Financial Metrics - Persistent instead of calculated on the fly
        public decimal NetSales { get; set; }
        public decimal ExpectedCash { get; set; }
        public decimal Variance { get; set; }
        public decimal LotteryIncome { get; set; }
        public decimal NetIncome { get; set; }

        public decimal? OriginalTotalSales { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; } // "Draft", "Submitted"
        
        public bool IsReconciled { get; set; }
        public string? ReconciledBy { get; set; }
        public DateTime? ReconciledDate { get; set; }
        
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? TicketImageUrl { get; set; }
        public string? AcknowledgedBy { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
    }
}
