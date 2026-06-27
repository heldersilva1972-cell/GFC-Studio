using System;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class FinancialAnalyticsRequest
    {
        public List<string> IncomeTypes { get; set; } = new();
        public string Period { get; set; } = "year"; // year, month, week, custom
        public string GroupBy { get; set; } = "month"; // month, week, day
        public List<int> Years { get; set; } = new();
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? DayOfWeekFilter { get; set; }
        public int? SelectedMonth { get; set; } // 1-12
        public int? SelectedWeek { get; set; } // 1-53
    }

    public class FinancialDataPoint
    {
        public string Label { get; set; } = "";
        public decimal Amount { get; set; }
        public string IncomeType { get; set; } = "";
        public int Year { get; set; }
        public DateTime Date { get; set; }
    }

    public class FinancialSummary
    {
        public decimal TotalRevenue { get; set; }
        public Dictionary<string, decimal> RevenueByType { get; set; } = new();
        public Dictionary<int, decimal> RevenueByYear { get; set; } = new();
        public decimal ComparisonGrowth { get; set; } // Percentage growth compared to previous period/year
    }

    public class FinancialSnapshotDto
    {
        public int Year { get; set; }
        public int? Month { get; set; }
        public decimal BarSalesDownstairs { get; set; }
        public decimal BarSalesUpstairs { get; set; }
        public decimal LotteryCommissions { get; set; }
        public decimal LotteryBonuses { get; set; }
        public decimal MembershipDues { get; set; }
        public decimal HallRentals { get; set; }
        
        public decimal TotalIncome => BarSalesDownstairs + BarSalesUpstairs + MembershipDues + HallRentals;
        
        // Expenses
        public decimal GrossPayroll { get; set; }
        public decimal EmployerFica { get; set; }
        public decimal EmployerPfml { get; set; }
        public decimal MaUnemployment { get; set; }
        public decimal Reimbursements { get; set; }
        public decimal PaidBills { get; set; }
        public decimal PaidLoans { get; set; }
        
        public decimal TotalExpenses => GrossPayroll + EmployerFica + EmployerPfml + MaUnemployment + Reimbursements + PaidBills + PaidLoans;
        public decimal NetProfit => TotalIncome - TotalExpenses;
    }
}
