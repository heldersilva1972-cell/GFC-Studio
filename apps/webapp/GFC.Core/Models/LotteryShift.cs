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
        public string? ShiftType { get; set; } // Optional: "Day", "Night", "Morning", "Afternoon", etc.
        public string? MachineId { get; set; } // Optional: For tracking multiple machines
        
        // Cash tracking
        public decimal StartingCash { get; set; }
        public decimal EndingCash { get; set; }
        
        // Machine report values
        public decimal TotalSales { get; set; }
        public decimal TotalPayouts { get; set; }
        public decimal TotalCancels { get; set; }
        
        // Calculated values
        public decimal NetSales => TotalSales - TotalPayouts - TotalCancels;
        public decimal ExpectedCash => StartingCash + NetSales;
        public decimal Variance => EndingCash - ExpectedCash;
        
        // Additional tracking
        public string? Notes { get; set; }
        public string? Status { get; set; } // "Draft", "Submitted", "Approved", "Reconciled"
        public bool IsReconciled { get; set; }
        public string? ReconciledBy { get; set; }
        public DateTime? ReconciledDate { get; set; }
        
        // Audit fields
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

