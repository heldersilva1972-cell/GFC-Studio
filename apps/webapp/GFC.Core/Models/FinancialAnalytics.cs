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

        // Period-over-Period (MoM MTD Like-for-Like) Comparison
        public int DaysCompared { get; set; }
        public decimal CurrentMtdTotalIncome { get; set; }
        public decimal CurrentMtdTotalExpenses { get; set; }
        public decimal CurrentMtdNetProfit => CurrentMtdTotalIncome - CurrentMtdTotalExpenses;

        public decimal PrevPeriodTotalIncome { get; set; }
        public decimal PrevPeriodTotalExpenses { get; set; }
        public decimal PrevPeriodNetProfit => PrevPeriodTotalIncome - PrevPeriodTotalExpenses;

        public decimal IncomeMomGrowth => PrevPeriodTotalIncome > 0 ? (((CurrentMtdTotalIncome > 0 ? CurrentMtdTotalIncome : TotalIncome) - PrevPeriodTotalIncome) / PrevPeriodTotalIncome) * 100m : 0m;
        public decimal ExpensesMomGrowth => PrevPeriodTotalExpenses > 0 ? (((CurrentMtdTotalExpenses > 0 ? CurrentMtdTotalExpenses : TotalExpenses) - PrevPeriodTotalExpenses) / PrevPeriodTotalExpenses) * 100m : 0m;
        public decimal NetProfitMomGrowth => PrevPeriodNetProfit != 0 ? (((CurrentMtdNetProfit != 0 ? CurrentMtdNetProfit : NetProfit) - PrevPeriodNetProfit) / Math.Abs(PrevPeriodNetProfit)) * 100m : 0m;
    }

    public class IncomeAuditSummaryDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalBarSales { get; set; }
        public decimal TotalLotteryCommissions { get; set; }
        public decimal TotalMembershipDues { get; set; }
        public decimal CombinedTotalIncome => TotalBarSales + TotalLotteryCommissions + TotalMembershipDues;
        
        public int? ComparisonYear { get; set; }
        public decimal ComparisonBarSales { get; set; }
        public decimal ComparisonLotteryCommissions { get; set; }
        public decimal ComparisonMembershipDues { get; set; }
        public decimal ComparisonTotalIncome => ComparisonBarSales + ComparisonLotteryCommissions + ComparisonMembershipDues;
        public decimal GrowthPercentage => ComparisonTotalIncome > 0 ? ((CombinedTotalIncome - ComparisonTotalIncome) / ComparisonTotalIncome) * 100m : 0m;
        
        // Month-over-Month (MoM MTD Like-for-Like) comparison
        public int PrevMonthDaysCompared { get; set; }
        public decimal CurrentMtdBarSales { get; set; }
        public decimal CurrentMtdLotteryCommissions { get; set; }
        public decimal CurrentMtdMembershipDues { get; set; }
        public decimal CurrentMtdTotalIncome => CurrentMtdBarSales + CurrentMtdLotteryCommissions + CurrentMtdMembershipDues;

        public decimal PrevMonthBarSales { get; set; }
        public decimal PrevMonthLotteryCommissions { get; set; }
        public decimal PrevMonthMembershipDues { get; set; }
        public decimal PrevMonthTotalIncome => PrevMonthBarSales + PrevMonthLotteryCommissions + PrevMonthMembershipDues;

        public decimal BarSalesMomGrowth => PrevMonthBarSales > 0 ? (((CurrentMtdBarSales > 0 ? CurrentMtdBarSales : TotalBarSales) - PrevMonthBarSales) / PrevMonthBarSales) * 100m : 0m;
        public decimal LotteryMomGrowth => PrevMonthLotteryCommissions > 0 ? (((CurrentMtdLotteryCommissions > 0 ? CurrentMtdLotteryCommissions : TotalLotteryCommissions) - PrevMonthLotteryCommissions) / PrevMonthLotteryCommissions) * 100m : 0m;
        public decimal DuesMomGrowth => PrevMonthMembershipDues > 0 ? (((CurrentMtdMembershipDues > 0 ? CurrentMtdMembershipDues : TotalMembershipDues) - PrevMonthMembershipDues) / PrevMonthMembershipDues) * 100m : 0m;
        public decimal CombinedMomGrowth => PrevMonthTotalIncome > 0 ? (((CurrentMtdTotalIncome > 0 ? CurrentMtdTotalIncome : CombinedTotalIncome) - PrevMonthTotalIncome) / PrevMonthTotalIncome) * 100m : 0m;

        public decimal BarSalesMomDiff => (CurrentMtdBarSales > 0 ? CurrentMtdBarSales : TotalBarSales) - PrevMonthBarSales;
        public decimal LotteryMomDiff => (CurrentMtdLotteryCommissions > 0 ? CurrentMtdLotteryCommissions : TotalLotteryCommissions) - PrevMonthLotteryCommissions;
        public decimal DuesMomDiff => (CurrentMtdMembershipDues > 0 ? CurrentMtdMembershipDues : TotalMembershipDues) - PrevMonthMembershipDues;
        public decimal CombinedMomDiff => (CurrentMtdTotalIncome > 0 ? CurrentMtdTotalIncome : CombinedTotalIncome) - PrevMonthTotalIncome;

        public List<MissingEntryWarningDto> Warnings { get; set; } = new();
        public List<IncomeStreamEntryDto> LedgerEntries { get; set; } = new();
        public List<IncomeStreamEntryDto> ComparisonLedgerEntries { get; set; } = new();
        public Dictionary<int, MultiYearStreamSummaryDto> MultiYearSummaries { get; set; } = new();
    }

    public class MultiYearStreamSummaryDto
    {
        public int Year { get; set; }
        public decimal TotalBarSales { get; set; }
        public decimal TotalLotteryCommissions { get; set; }
        public decimal TotalMembershipDues { get; set; }
        public decimal CombinedTotalIncome => TotalBarSales + TotalLotteryCommissions + TotalMembershipDues;

        public decimal[] MonthlyBarSales { get; set; } = new decimal[12];
        public decimal[] MonthlyLotteryCommissions { get; set; } = new decimal[12];
        public decimal[] MonthlyMembershipDues { get; set; } = new decimal[12];
        public decimal[] MonthlyTotalIncome => Enumerable.Range(0, 12).Select(i => MonthlyBarSales[i] + MonthlyLotteryCommissions[i] + MonthlyMembershipDues[i]).ToArray();
    }

    public class MissingEntryWarningDto
    {
        public DateTime Date { get; set; }
        public string Stream { get; set; } = ""; // "Bar Sales" or "Lottery"
        public string Details { get; set; } = "";
        public string ActionUrl { get; set; } = "";
    }

    public class IncomeStreamEntryDto
    {
        public DateTime Date { get; set; }
        public string Stream { get; set; } = "";
        public string Shift { get; set; } = "";
        public bool IsFullDayShift { get; set; }
        public string Description { get; set; } = "";
        public decimal Amount { get; set; }
        public string SourceUrl { get; set; } = "";
    }
}

