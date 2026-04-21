using System;

namespace GFC.Core.Models;

public class EventPerformanceDto
{
    public int Year { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal NetProfit => TotalIncome + TotalExpense; // Expenses are stored as negative numbers
}
