using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using GFC.Core.DTOs;


namespace GFC.BlazorServer.Services
{
    public interface IFinancialAnalyticsService
    {
        Task<List<FinancialDataPoint>> GetAggregatedDataAsync(FinancialAnalyticsRequest request);
        Task<FinancialSummary> GetSummaryAsync(FinancialAnalyticsRequest request);
        Task<List<int>> GetAvailableYearsAsync();
        Task<(List<DailySalesReportDto> Data, int TotalBar, int TotalLotto, string Server, string Database, string Error)> GetDailySalesReportsAsync(DateTime startDate, DateTime endDate);
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

        public async Task<(List<DailySalesReportDto> Data, int TotalBar, int TotalLotto, string Server, string Database, string Error)> GetDailySalesReportsAsync(DateTime startDate, DateTime endDate)
        {
            string currentStep = "Initializing";
            string dbServer = "Unknown";
            string dbName = "Unknown";
            try {
                currentStep = "Connecting to DB";
                using var db = await _dbFactory.CreateDbContextAsync();
                
                var conn = db.Database.GetDbConnection();
                dbName = conn.Database ?? "Unknown";
                dbServer = conn.DataSource ?? "Unknown";

                currentStep = "Counting BarSaleEntries";
                int totalBar = 0;
                try { totalBar = await db.BarSaleEntries.CountAsync(); } 
                catch (Exception ex) { Console.WriteLine($"[Diag] Bar count fail: {ex.Message}"); }
                
                currentStep = "Counting LotteryShifts";
                int totalLotto = 0;
                try { totalLotto = await db.LotteryShifts.CountAsync(); }
                catch (Exception ex) { Console.WriteLine($"[Diag] Lotto count fail: {ex.Message}"); }

                var start = startDate.Date;
                var end = endDate.Date;

                // 1. Fetch Bar Data - Search by either the raw SaleDate OR the AdjustedSaleDate
                currentStep = "Fetching BarSaleEntries";
                var barEntriesRaw = await db.BarSaleEntries
                    .AsNoTracking()
                    .Where(e => (e.SaleDate >= start && e.SaleDate <= end) || 
                                (e.AdjustedSaleDate != null && e.AdjustedSaleDate >= start && e.AdjustedSaleDate <= end))
                    .Select(e => new {
                        SaleDate = (DateTime?)e.SaleDate ?? DateTime.MinValue,
                        AdjustedSaleDate = e.AdjustedSaleDate,
                        Shift = e.Shift ?? "Day",
                        IsRentalHall = e.IsRentalHall,
                        TotalSales = (decimal?)e.TotalSales ?? 0m,
                        TotalHours = (decimal?)e.TotalHours ?? 0m,
                        Notes = e.Notes ?? "",
                        CreatedBy = e.CreatedBy ?? "Unknown",
                        CreatedAt = (DateTime?)e.CreatedAt ?? DateTime.MinValue
                    })
                    .ToListAsync();
                
                // Final refinement in memory to ensure we group by the CORRECT date (Adjusted if available)
                var barEntries = barEntriesRaw.Select(e => new {
                    Date = (e.AdjustedSaleDate ?? e.SaleDate).Date,
                    e.Shift,
                    e.IsRentalHall,
                    e.TotalSales,
                    e.TotalHours,
                    e.Notes,
                    e.CreatedBy,
                    e.CreatedAt
                }).Where(e => e.Date >= start && e.Date <= end).ToList();

                Console.WriteLine($"[FinancialService] Found {barEntries.Count} barEntries.");

                // 2. Fetch Lottery Data
                currentStep = "Fetching LotteryShifts";
                var lottoShifts = await db.LotteryShifts
                    .AsNoTracking()
                    .Where(e => e.ShiftDate >= start && e.ShiftDate <= end)
                    .Select(e => new {
                        e.ShiftId,
                        ShiftDate = (DateTime?)e.ShiftDate ?? DateTime.MinValue,
                        ShiftType = e.ShiftType ?? "Day",
                        TotalSales = (decimal?)e.TotalSales ?? 0m,
                        TotalPayouts = (decimal?)e.TotalPayouts ?? 0m,
                        NetDue = (decimal?)e.NetDue ?? 0m,
                        StartingCash = (decimal?)e.StartingCash ?? 0m,
                        EndingCash = (decimal?)e.EndingCash ?? 0m,
                        BackupBagAmount = (decimal?)e.BackupBagAmount ?? 0m,
                        EnvelopeAmount = (decimal?)e.EnvelopeAmount ?? 0m,
                        Notes = e.Notes ?? "",
                        EmployeeName = e.EmployeeName ?? "Unknown",
                        CreatedDate = (DateTime?)e.CreatedDate ?? DateTime.MinValue
                    })
                    .ToListAsync();

                Console.WriteLine($"[FinancialService] Found {lottoShifts.Count} lottoShifts.");



            // 3. Group and Aggregate
            var dates = barEntries.Select(e => e.Date)
                .Union(lottoShifts.Select(s => s.ShiftDate.Date))
                .OrderByDescending(d => d)
                .ToList();

            var reports = new List<DailySalesReportDto>();

                foreach (var date in dates)
                {
                    var dailyReport = new DailySalesReportDto { Date = date };
                    
                    // Get shifts for this date
                    var dayBar = barEntries.FirstOrDefault(e => e.Date == date && e.Shift == "Day" && !e.IsRentalHall);
                    var nightBar = barEntries.FirstOrDefault(e => e.Date == date && e.Shift == "Night" && !e.IsRentalHall);
                    var hallBar = barEntries.FirstOrDefault(e => e.Date == date && e.IsRentalHall);


                    var dayLotto = lottoShifts.FirstOrDefault(s => s.ShiftDate.Date == date && s.ShiftType == "Day");
                    var nightLotto = lottoShifts.FirstOrDefault(s => s.ShiftDate.Date == date && s.ShiftType == "Night");

                    // Map Day Shift
                    if (dayBar != null || dayLotto != null)
                    {
                        dailyReport.Shifts.Add(new ShiftReportDto {
                            ShiftType = "Day",
                            IsRentalHall = false,
                            BarSales = dayBar?.TotalSales ?? 0,
                            TotalHours = dayBar?.TotalHours,
                            LottoSales = dayLotto?.TotalSales ?? 0,
                            LottoPayouts = dayLotto?.TotalPayouts ?? 0,
                            LottoNetDue = dayLotto?.NetDue ?? 0,
                            StartingCash = dayLotto?.StartingCash ?? 0,
                            EndingCash = dayLotto?.EndingCash ?? 0,
                            BackupBagAmount = dayLotto?.BackupBagAmount ?? 0,
                            EnvelopeAmount = dayLotto?.EnvelopeAmount ?? 0,
                            Notes = dayBar?.Notes ?? dayLotto?.Notes,
                            CreatedBy = dayBar?.CreatedBy ?? dayLotto?.EmployeeName,
                            CreatedAt = dayBar?.CreatedAt ?? dayLotto?.CreatedDate ?? date

                        });
                    }

                    // Map Night Shift
                    if (nightBar != null || nightLotto != null)
                    {
                        dailyReport.Shifts.Add(new ShiftReportDto {
                            ShiftType = "Night",
                            IsRentalHall = false,
                            BarSales = nightBar?.TotalSales ?? 0,
                            TotalHours = nightBar?.TotalHours,
                            LottoSales = nightLotto?.TotalSales ?? 0,
                            LottoPayouts = nightLotto?.TotalPayouts ?? 0,
                            LottoNetDue = nightLotto?.NetDue ?? 0,
                            StartingCash = nightLotto?.StartingCash ?? 0,
                            EndingCash = nightLotto?.EndingCash ?? 0,
                            BackupBagAmount = nightLotto?.BackupBagAmount ?? 0,
                            EnvelopeAmount = nightLotto?.EnvelopeAmount ?? 0,
                            Notes = nightBar?.Notes ?? nightLotto?.Notes,
                            CreatedBy = nightBar?.CreatedBy ?? nightLotto?.EmployeeName,
                            CreatedAt = nightBar?.CreatedAt ?? nightLotto?.CreatedDate ?? date

                        });
                    }

                    // Map Hall Rental
                    if (hallBar != null)
                    {
                        dailyReport.Shifts.Add(new ShiftReportDto {
                            ShiftType = "Hall",
                            IsRentalHall = true,
                            BarSales = hallBar.TotalSales,
                            TotalHours = hallBar.TotalHours,
                            Notes = hallBar.Notes,
                            CreatedBy = hallBar.CreatedBy,
                            CreatedAt = hallBar.CreatedAt
                        });
                    }

                    reports.Add(dailyReport);
                }

                return (reports, totalBar, totalLotto, dbServer, dbName, "None");
            }
            catch (Exception ex)
            {
                string msg = $"Error at {currentStep}: {ex.Message}";
                Console.WriteLine($"[FinancialService:Critical] {msg}");
                return (new List<DailySalesReportDto>(), 0, 0, dbServer, dbName, msg);
            }
        }



    }
}

