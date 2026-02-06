using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GFC.BlazorServer.Services
{
    public interface IFinancialAnalyticsService
    {
        Task<List<FinancialDataPoint>> GetAggregatedDataAsync(FinancialAnalyticsRequest request);
        Task<FinancialSummary> GetSummaryAsync(FinancialAnalyticsRequest request);
        Task<List<int>> GetAvailableYearsAsync();
    }

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

    public class FinancialAnalyticsService : IFinancialAnalyticsService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;

        public FinancialAnalyticsService(IDbContextFactory<GfcDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<int>> GetAvailableYearsAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var barYears = await db.BarSaleEntries.Select(e => e.SaleDate.Year).Distinct().ToListAsync();
            var rentalYears = await db.HallRentals.Select(e => e.EventDate.Year).Distinct().ToListAsync();
            var duesYears = await db.DuesPayments.Select(e => e.Year).Distinct().ToListAsync();
            
            // Lottery years from new weekly stats
            var lotteryYears = await db.LotteryWeeklyStats.Select(s => s.WeekEndingDate.Year).Distinct().ToListAsync();

            return barYears.Union(rentalYears).Union(duesYears).Union(lotteryYears)
                .OrderByDescending(y => y)
                .ToList();
        }

        public async Task<List<FinancialDataPoint>> GetAggregatedDataAsync(FinancialAnalyticsRequest request)
        {
            var allPoints = new List<FinancialDataPoint>();
            using var db = await _dbFactory.CreateDbContextAsync();

            if (request.IncomeTypes.Contains("Bar Sales"))
            {
                var points = await db.BarSaleEntries
                    .Where(e => request.Years.Contains((e.AdjustedSaleDate ?? e.SaleDate).Year))
                    .Select(e => new FinancialDataPoint 
                    { 
                        Date = (e.AdjustedSaleDate ?? e.SaleDate), 
                        Amount = e.TotalSales, 
                        IncomeType = "Bar Sales", 
                        Year = (e.AdjustedSaleDate ?? e.SaleDate).Year 
                    })
                    .ToListAsync();
                allPoints.AddRange(points);
            }

            if (request.IncomeTypes.Contains("Hall Rentals"))
            {
                var points = await db.HallRentals
                    .Where(e => request.Years.Contains(e.EventDate.Year) && e.Status == "Completed")
                    .Select(e => new FinancialDataPoint 
                    { 
                        Date = e.EventDate, 
                        Amount = e.TotalPrice, 
                        IncomeType = "Hall Rentals", 
                        Year = e.EventDate.Year 
                    })
                    .ToListAsync();
                allPoints.AddRange(points);
            }

            if (request.IncomeTypes.Contains("Membership Dues"))
            {
                var points = await db.DuesPayments
                    .Where(e => request.Years.Contains(e.Year) && e.Amount.HasValue && e.PaidDate.HasValue)
                    .Select(e => new FinancialDataPoint 
                    { 
                        Date = e.PaidDate.Value, 
                        Amount = e.Amount.Value, 
                        IncomeType = "Membership Dues", 
                        Year = e.Year 
                    })
                    .ToListAsync();
                allPoints.AddRange(points);
            }

            if (request.IncomeTypes.Contains("Lottery"))
            {
                var lotusPoints = await db.LotteryWeeklyStats
                    .Where(s => request.Years.Contains(s.WeekEndingDate.Year))
                    .Select(s => new 
                    { 
                        Date = s.WeekEndingDate, 
                        Amount = Math.Abs(s.OnlineCommission) + Math.Abs(s.InstantCommission) + 
                                 Math.Abs(s.OnlineCashBonus) + Math.Abs(s.InstantCashBonus) + 
                                 Math.Abs(s.OnlineClaimsBonus) + Math.Abs(s.InstantClaimsBonus),
                        Year = s.WeekEndingDate.Year 
                    })
                    .ToListAsync();

                allPoints.AddRange(lotusPoints.Select(p => new FinancialDataPoint
                {
                    Date = p.Date,
                    Amount = p.Amount,
                    IncomeType = "Lottery",
                    Year = p.Year
                }));
            }

            // Apply filters
            var filtered = allPoints.AsQueryable();
            
            if (request.StartDate.HasValue)
                filtered = filtered.Where(p => p.Date >= request.StartDate.Value);
            
            if (request.EndDate.HasValue)
                filtered = filtered.Where(p => p.Date <= request.EndDate.Value);

            if (request.SelectedMonth.HasValue && request.Period == "month")
                filtered = filtered.Where(p => p.Date.Month == request.SelectedMonth.Value);

            if (request.SelectedWeek.HasValue && request.Period == "week")
                filtered = filtered.Where(p => System.Globalization.ISOWeek.GetWeekOfYear(p.Date) == request.SelectedWeek.Value);

            if (!string.IsNullOrEmpty(request.DayOfWeekFilter))
            {
                var day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), request.DayOfWeekFilter);
                filtered = filtered.Where(p => p.Date.DayOfWeek == day);
            }

            return filtered.ToList();
        }

        public async Task<FinancialSummary> GetSummaryAsync(FinancialAnalyticsRequest request)
        {
            var data = await GetAggregatedDataAsync(request);
            var summary = new FinancialSummary
            {
                TotalRevenue = data.Sum(p => p.Amount),
                RevenueByType = data.GroupBy(p => p.IncomeType)
                                   .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount)),
                RevenueByYear = data.GroupBy(p => p.Year)
                                   .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount))
            };
            return summary;
        }
    }
}
