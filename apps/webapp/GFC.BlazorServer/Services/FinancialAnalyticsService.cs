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
        Task<List<LotteryShift>> GetLotteryAnalyticsAsync(DateTime startDate, DateTime endDate, string? shiftType = null, string? employeeName = null);
        Task<List<EmployeeHoursDto>> GetEmployeeHoursAsync(DateTime startDate, DateTime endDate, string? username = null, string? location = "All");
        Task<FinancialSnapshotDto> GetFinancialSnapshotAsync(int year);
    }

    public class FinancialSnapshotDto
    {
        public int Year { get; set; }
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
        
        public decimal TotalExpenses => GrossPayroll + EmployerFica + EmployerPfml + MaUnemployment + Reimbursements;
        public decimal NetProfit => TotalIncome - TotalExpenses;
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

            if (request.IncomeTypes.Contains("Downstairs Bar") || request.IncomeTypes.Contains("Bar Sales"))
            {
                var points = await db.BarSaleEntries
                    .Where(e => request.Years.Contains((e.AdjustedSaleDate ?? e.SaleDate).Year) && !e.IsRentalHall)
                    .Select(e => new FinancialDataPoint 
                    { 
                        Date = (e.AdjustedSaleDate ?? e.SaleDate), 
                        Amount = e.TotalSales, 
                        IncomeType = "Downstairs Bar", 
                        Year = (e.AdjustedSaleDate ?? e.SaleDate).Year 
                    })
                    .ToListAsync();
                allPoints.AddRange(points);
            }

            if (request.IncomeTypes.Contains("Upstairs Bar"))
            {
                var points = await db.BarSaleEntries
                    .Where(e => request.Years.Contains((e.AdjustedSaleDate ?? e.SaleDate).Year) && e.IsRentalHall)
                    .Select(e => new FinancialDataPoint 
                    { 
                        Date = (e.AdjustedSaleDate ?? e.SaleDate), 
                        Amount = e.TotalSales, 
                        IncomeType = "Upstairs Bar", 
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

            // if (request.IncomeTypes.Contains("Lottery"))
            // {
            //     Data removed per user request. Shows as $0.
            // }

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
                        CreatedBy = !string.IsNullOrWhiteSpace(e.CreatedBy) ? e.CreatedBy : "Unknown",
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
                    .Where(e => e.ShiftDate >= start && e.ShiftDate <= end && e.Status == "Submitted")
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
                        TotalCancels = (decimal?)e.TotalCancels ?? 0m,
                        BagRefillAmount = (decimal?)e.BagRefillAmount ?? 0m,
                        
                        // Persistent metrics directly from DB
                        NetSales = (decimal?)e.NetSales ?? 0m,
                        ExpectedCash = (decimal?)e.ExpectedCash ?? 0m,
                        Variance = (decimal?)e.Variance ?? 0m,
                        LotteryIncome = (decimal?)e.LotteryIncome ?? 0m,
                        NetIncome = (decimal?)e.NetIncome ?? 0m,
                        
                        // Shift Activity directly from DB
                        ShiftSalesActivity = (decimal?)e.ShiftSalesActivity ?? 0m,
                        ShiftPayoutsActivity = (decimal?)e.ShiftPayoutsActivity ?? 0m,
                        ShiftCancelsActivity = (decimal?)e.ShiftCancelsActivity ?? 0m,
                        ShiftNetDueActivity = (decimal?)e.ShiftNetDueActivity ?? 0m,
                        
                        Notes = e.Notes ?? "",
                        EmployeeName = e.EmployeeName ?? "Unknown",
                        CreatedDate = (DateTime?)e.CreatedDate ?? DateTime.MinValue,
                        Status = e.Status ?? "Submitted"
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
                             ShiftId = dayLotto?.ShiftId ?? 0,
                             ShiftType = "Day",
                             IsRentalHall = false,
                             BarSales = dayBar?.TotalSales ?? 0,
                             TotalHours = dayBar?.TotalHours,
                             LottoSales = dayLotto?.TotalSales ?? 0,
                             LottoPayouts = dayLotto?.TotalPayouts ?? 0,
                             LottoNetDue = dayLotto?.NetDue ?? 0,
                             LottoCancels = dayLotto?.TotalCancels ?? 0,
                             StartingCash = dayLotto?.StartingCash ?? 0,
                             EndingCash = dayLotto?.EndingCash ?? 0,
                             BackupBagAmount = dayLotto?.BackupBagAmount ?? 0,
                             EnvelopeAmount = dayLotto?.EnvelopeAmount ?? 0,
                             BagRefillAmount = dayLotto?.BagRefillAmount ?? 0,
                             NetSales = dayLotto?.NetSales ?? 0,
                             ExpectedCash = dayLotto?.ExpectedCash ?? 0,
                             Variance = dayLotto?.Variance ?? 0,
                             LotteryIncome = dayLotto?.LotteryIncome ?? 0,
                             NetIncome = dayLotto?.NetIncome ?? 0,
                             ShiftSalesActivity = dayLotto?.ShiftSalesActivity ?? (dayLotto?.TotalSales ?? 0),
                             ShiftPayoutsActivity = dayLotto?.ShiftPayoutsActivity ?? (dayLotto?.TotalPayouts ?? 0),
                             ShiftCancelsActivity = dayLotto?.ShiftCancelsActivity ?? (dayLotto?.TotalCancels ?? 0),
                             ShiftNetDueActivity = dayLotto?.ShiftNetDueActivity ?? (dayLotto?.NetDue ?? 0),
                             Notes = !string.IsNullOrWhiteSpace(dayBar?.Notes) ? dayBar.Notes : dayLotto?.Notes,
                             Status = dayLotto?.Status,
                             CreatedBy = !string.IsNullOrWhiteSpace(dayBar?.CreatedBy) ? dayBar.CreatedBy : 
                                        (!string.IsNullOrWhiteSpace(dayLotto?.EmployeeName) ? dayLotto.EmployeeName : "Unknown"),
                             CreatedAt = dayBar?.CreatedAt ?? dayLotto?.CreatedDate ?? date
                         });
                    }

                    // Map Night Shift
                    if (nightBar != null || nightLotto != null)
                    {
                        dailyReport.Shifts.Add(new ShiftReportDto {
                             ShiftId = nightLotto?.ShiftId ?? 0,
                             ShiftType = "Night",
                             IsRentalHall = false,
                             BarSales = nightBar?.TotalSales ?? 0,
                             TotalHours = nightBar?.TotalHours,
                             LottoSales = nightLotto?.TotalSales ?? 0,
                             LottoPayouts = nightLotto?.TotalPayouts ?? 0,
                             LottoNetDue = nightLotto?.NetDue ?? 0,
                             LottoCancels = nightLotto?.TotalCancels ?? 0,
                             StartingCash = nightLotto?.StartingCash ?? 0,
                             EndingCash = nightLotto?.EndingCash ?? 0,
                             BackupBagAmount = nightLotto?.BackupBagAmount ?? 0,
                             EnvelopeAmount = nightLotto?.EnvelopeAmount ?? 0,
                             BagRefillAmount = nightLotto?.BagRefillAmount ?? 0,
                             NetSales = nightLotto?.NetSales ?? 0,
                             ExpectedCash = nightLotto?.ExpectedCash ?? 0,
                             Variance = nightLotto?.Variance ?? 0,
                             LotteryIncome = nightLotto?.LotteryIncome ?? 0,
                             NetIncome = nightLotto?.NetIncome ?? 0,
                             ShiftSalesActivity = nightLotto?.ShiftSalesActivity ?? (nightLotto?.TotalSales ?? 0),
                             ShiftPayoutsActivity = nightLotto?.ShiftPayoutsActivity ?? (nightLotto?.TotalPayouts ?? 0),
                             ShiftCancelsActivity = nightLotto?.ShiftCancelsActivity ?? (nightLotto?.TotalCancels ?? 0),
                             ShiftNetDueActivity = nightLotto?.ShiftNetDueActivity ?? (nightLotto?.NetDue ?? 0),
                             Notes = !string.IsNullOrWhiteSpace(nightBar?.Notes) ? nightBar.Notes : nightLotto?.Notes,
                             Status = nightLotto?.Status,
                             CreatedBy = !string.IsNullOrWhiteSpace(nightBar?.CreatedBy) ? nightBar.CreatedBy : 
                                        (!string.IsNullOrWhiteSpace(nightLotto?.EmployeeName) ? nightLotto.EmployeeName : "Unknown"),
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



        public async Task<List<LotteryShift>> GetLotteryAnalyticsAsync(DateTime startDate, DateTime endDate, string? shiftType = null, string? employeeName = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var query = db.LotteryShifts
                .Where(s => s.ShiftDate >= startDate && s.ShiftDate <= endDate && s.Status == "Submitted");

            if (!string.IsNullOrEmpty(shiftType))
                query = query.Where(s => s.ShiftType == shiftType);

            if (!string.IsNullOrEmpty(employeeName))
                query = query.Where(s => s.EmployeeName == employeeName);

            return await query
                .OrderByDescending(s => s.ShiftDate)
                .ThenByDescending(s => s.ShiftId)
                .ToListAsync();
        }

        public async Task<List<EmployeeHoursDto>> GetEmployeeHoursAsync(DateTime startDate, DateTime endDate, string? username = null, string? location = "All")
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var start = startDate.Date;
            var end = endDate.Date;

            var query = db.BarSaleEntries
                .AsNoTracking()
                .Where(e => (e.AdjustedSaleDate ?? e.SaleDate).Date >= start && (e.AdjustedSaleDate ?? e.SaleDate).Date <= end && e.Status == "Submitted");

            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(e => e.CreatedBy == username);

            var sys = await db.SystemSettings.AsNoTracking().FirstOrDefaultAsync(s => s.Id == 1);
            decimal fallbackEmployeeTax = 0, fallbackEmployerTax = 0;
            if (sys != null)
            {
                fallbackEmployeeTax = (sys.MaStateTaxRate + sys.PfmlEmployeeRate + sys.FicaEmployeeRate) / 100m;
                fallbackEmployerTax = (sys.MaUnemploymentRate + sys.PfmlEmployerRate + sys.FicaEmployerRate) / 100m;
            }

            var entries = await query
                .Select(e => new {
                    Date = (e.AdjustedSaleDate ?? e.SaleDate).Date,
                    Hours = e.TotalHours ?? 0m,
                    Shift = e.Shift ?? "Day",
                    IsHall = e.IsRentalHall,
                    User = !string.IsNullOrWhiteSpace(e.CreatedBy) ? e.CreatedBy : "Unknown",
                    HistoricalRate = e.HourlyRate_AtTimeOfShift,
                    HistoricalEmployeeTax = e.TotalEmployeeTaxes_AtTimeOfShift,
                    HistoricalEmployerTax = e.TotalEmployerTaxes_AtTimeOfShift
                })
                .ToListAsync();

            if (!entries.Any()) return new List<EmployeeHoursDto>();

            // Group entries by user and match with employee metadata

            // Fetch users (Remove strict employee tracking filter to ensure all recorded hours are visible)
            var users = await db.AppUsers.AsNoTracking()
                .ToListAsync();
            
            var allMembers = await db.Members.AsNoTracking().ToListAsync();

            // Group entries by user
            var entriesByUser = entries
                .GroupBy(e => e.User)
                .ToDictionary(g => g.Key, g => g.ToList());

            var filteredResults = new List<EmployeeHoursDto>();

            foreach (var user in users)
            {
                // Skip if this employee has no entries in the current period/location
                if (!entriesByUser.ContainsKey(user.Username)) continue;

                var userEntries = entriesByUser[user.Username];
                var totalHours = userEntries.Sum(e => e.Hours);

                // Skip if they worked 0 hours (e.g. they only recorded a shift with 0 time)
                if (totalHours <= 0) continue;

                var defaultRate = user.HourlyRate ?? 0;
                
                decimal totalPay = 0, netPay = 0, totalPayrollCost = 0;
                decimal downstairsPay = 0, upstairsPay = 0;
                decimal totalWithheld = 0, totalEmployerAddOn = 0;

                foreach (var e in userEntries)
                {
                    // [DYNAMIC CALCULATION] AS REQUESTED: math always uses current system settings and current staff rates
                    decimal rate = defaultRate;
                    decimal employeeTax = fallbackEmployeeTax;
                    decimal employerTax = fallbackEmployerTax;

                    decimal shiftGross = e.Hours * rate;
                    decimal shiftWithheld = shiftGross * employeeTax;
                    decimal shiftEmployerAddOn = shiftGross * employerTax;
                    
                    decimal shiftNet = shiftGross - shiftWithheld;
                    decimal shiftCost = shiftGross + shiftEmployerAddOn;

                    totalPay += shiftGross;
                    netPay += shiftNet;
                    totalPayrollCost += shiftCost;
                    totalWithheld += shiftWithheld;
                    totalEmployerAddOn += shiftEmployerAddOn;

                    if (e.IsHall) upstairsPay += shiftGross;
                    else downstairsPay += shiftGross;
                }

                var dto = new EmployeeHoursDto {
                    Username = user.Username,
                    TotalHours = totalHours,
                    DownstairsHours = userEntries.Where(e => !e.IsHall).Sum(e => e.Hours),
                    UpstairsHours = userEntries.Where(e => e.IsHall).Sum(e => e.Hours),
                    EntryCount = userEntries.Count(e => e.Hours > 0),
                    HourlyRate = user.HourlyRate, // Keep current rate for display
                    StartDate = start,
                    EndDate = end,
                    TotalPay = totalPay,
                    NetPay = netPay,
                    TotalWithheld = totalWithheld,
                    TotalEmployerAddOn = totalEmployerAddOn,
                    TotalPayrollCost = totalPayrollCost,
                    DownstairsPay = downstairsPay,
                    UpstairsPay = upstairsPay
                };

                // Fill daily breakdown and shift types
                foreach (var entryGroup in userEntries.GroupBy(e => e.Date))
                {
                    var date = entryGroup.Key;
                    var hours = entryGroup.Sum(e => e.Hours);
                    dto.DailyHours[date] = hours;

                    // Determine shift signature
                    var shifts = entryGroup.Select(e => e.IsHall ? "Hall" : e.Shift).Distinct().ToList();
                    if (shifts.Count > 1) 
                        dto.DailyShiftTypes[date] = "Both";
                    else if (shifts.Any())
                        dto.DailyShiftTypes[date] = shifts.First();
                }

                // Link member name for better display
                if (user.MemberId != null)
                {
                    var member = allMembers.FirstOrDefault(m => m.MemberID == user.MemberId);
                    if (member != null)
                    {
                        dto.MemberName = $"{member.FirstName} {member.LastName}{(string.IsNullOrEmpty(member.Suffix) ? "" : " " + member.Suffix)}";
                    }
                }
                
                if (string.IsNullOrEmpty(dto.MemberName))
                {
                    dto.MemberName = user.Email ?? user.Username;
                }

                filteredResults.Add(dto);
            }

            return filteredResults.OrderByDescending(d => d.TotalHours).ToList();
        }

        public async Task<FinancialSnapshotDto> GetFinancialSnapshotAsync(int year)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var snapshot = new FinancialSnapshotDto { Year = year };
            var start = new DateTime(year, 1, 1);
            var end = new DateTime(year, 12, 31);

            // 1. Income - Bar Sales
            var barSales = await db.BarSaleEntries.AsNoTracking()
                .Where(b => (b.AdjustedSaleDate ?? b.SaleDate).Year == year)
                .ToListAsync();
            
            snapshot.BarSalesDownstairs = barSales.Where(b => !b.IsRentalHall).Sum(b => b.TotalSales);
            snapshot.BarSalesUpstairs = barSales.Where(b => b.IsRentalHall).Sum(b => b.TotalSales);

            // 2. Income - Lottery
            // Set to 0 per user request until data source is finalized
            snapshot.LotteryCommissions = 0m;
            snapshot.LotteryBonuses = 0m;

            // 3. Income - Membership Dues
            snapshot.MembershipDues = await db.DuesPayments.AsNoTracking()
                .Where(d => d.Year == year)
                .SumAsync(d => d.Amount) ?? 0m;

            // 4. Income - Hall Rentals
            snapshot.HallRentals = await db.HallRentals.AsNoTracking()
                .Where(h => h.EventDate.Year == year)
                .SumAsync(h => (decimal?)h.TotalPrice) ?? 0m;

            // 5. Expenses - Reimbursements
            snapshot.Reimbursements = await db.ReimbursementItems.AsNoTracking()
                .Include(i => i.Request)
                .Where(i => i.Request.Status == "Paid" && i.Request.PaidDateUtc != null && i.Request.PaidDateUtc.Value.Year == year)
                .SumAsync(i => (decimal?)i.Amount) ?? 0m;

            // 7. Expenses - Payroll
            var payroll = await GetEmployeeHoursAsync(start, end);
            snapshot.GrossPayroll = payroll.Sum(p => p.TotalPay);
            
            // 8. Tax Calculation based on SystemSettings (Id = 1)
            var config = await db.SystemSettings.AsNoTracking().FirstOrDefaultAsync(s => s.Id == 1);
            if (config != null && snapshot.GrossPayroll > 0)
            {
                snapshot.EmployerFica = snapshot.GrossPayroll * (config.FicaEmployerRate / 100m);
                snapshot.EmployerPfml = snapshot.GrossPayroll * (config.PfmlEmployerRate / 100m);
                snapshot.MaUnemployment = snapshot.GrossPayroll * (config.MaUnemploymentRate / 100m);
            }

            return snapshot;
        }
    }
}

