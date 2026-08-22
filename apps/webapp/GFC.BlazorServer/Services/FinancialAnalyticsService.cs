using GFC.BlazorServer.Data;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;


namespace GFC.BlazorServer.Services
{
    public interface IFinancialAnalyticsService
    {
        public static int DiagnosticRawCount { get; set; }
        Task<List<FinancialDataPoint>> GetAggregatedDataAsync(FinancialAnalyticsRequest request);
        Task<FinancialSummary> GetSummaryAsync(FinancialAnalyticsRequest request);
        Task<List<int>> GetAvailableYearsAsync();
        Task<(List<DailySalesReportDto> Data, int TotalBar, int TotalLotto, string Server, string Database, string Error)> GetDailySalesReportsAsync(DateTime startDate, DateTime endDate);
        Task<List<LotteryShift>> GetLotteryAnalyticsAsync(DateTime startDate, DateTime endDate, string? shiftType = null, string? employeeName = null);
        Task<List<EmployeeHoursDto>> GetEmployeeHoursAsync(DateTime startDate, DateTime endDate, string? username = null, string? location = "All");
        Task<FinancialSnapshotDto> GetFinancialSnapshotAsync(int year, int? month = null);
        Task<IncomeAuditSummaryDto> GetIncomeSummaryWithAuditAsync(DateTime startDate, DateTime endDate, int? comparisonYear = null);
        Task AcknowledgeNoteAsync(string noteType, int recordId, string username);
    }


    public class FinancialAnalyticsService : IFinancialAnalyticsService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;
        private readonly ILotteryRateRepository _rateRepository; // [NEW]

        public FinancialAnalyticsService(IDbContextFactory<GfcDbContext> dbFactory, ILotteryRateRepository rateRepository)
        {
            _dbFactory = dbFactory;
            _rateRepository = rateRepository;
        }

        public async Task<List<int>> GetAvailableYearsAsync()
        {
            try 
            {
                using var db = await _dbFactory.CreateDbContextAsync();
            
            // Only include years that have ACTUALIZED data (Completed, Submitted, or Paid)
            var barYears = await db.BarSaleEntries
                .Where(e => e.Status == "Submitted")
                .Select(e => (e.AdjustedSaleDate ?? e.SaleDate).Year)
                .Distinct()
                .ToListAsync();

            var rentalYears = await db.HallRentals
                .Where(e => e.Status == "Completed")
                .Select(e => e.EventDate.Year)
                .Distinct()
                .ToListAsync();

            var duesYears = await db.DuesPayments
                .Where(e => e.Amount.HasValue && e.PaidDate.HasValue)
                .Select(e => e.Year)
                .Distinct()
                .ToListAsync();
            
            var lotteryShiftYears = await db.LotteryShifts
                .Where(e => e.Status == "Submitted")
                .Select(e => e.ShiftDate.Year)
                .Distinct()
                .ToListAsync();

            var lotteryWeeklyYears = await db.LotteryWeeklyStats
                .Select(s => s.WeekEndingDate.Year)
                .Distinct()
                .ToListAsync();

            var lotteryYears = lotteryShiftYears.Union(lotteryWeeklyYears);

            return barYears.Union(rentalYears).Union(duesYears).Union(lotteryYears)
                .Where(y => y > 2000 && y <= DateTime.Now.Year) // Sanity check and historical only
                .OrderByDescending(y => y)
                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FinancialService] Error getting available years: {ex.Message}");
                return new List<int> { DateTime.Now.Year };
            }
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

            if (request.IncomeTypes.Contains("Lottery"))
            {
                var lottoShifts = await db.LotteryShifts
                    .Where(e => request.Years.Contains(e.ShiftDate.Year) && e.Status == "Submitted")
                    .ToListAsync();
                
                var rTable = new Dictionary<int, LotteryCommissionRate>(); // Cache rates by year
                
                var points = new List<FinancialDataPoint>();
                foreach(var s in lottoShifts)
                {
                    if (!rTable.TryGetValue(s.ShiftDate.Year, out var r))
                    {
                        r = _rateRepository.GetApplicableRate(s.ShiftDate.Year);
                        rTable[s.ShiftDate.Year] = r;
                    }
                    
                    decimal income = (s.ShiftSalesActivity * r.SalesCommissionMultiplier) +
                                     (s.ShiftPayoutsActivity * r.CashingBonusMultiplier) +
                                     (s.ShiftCancelsActivity * r.TicketBonusMultiplier);
                    
                    points.Add(new FinancialDataPoint
                    {
                        Date = s.ShiftDate,
                        Amount = income,
                        IncomeType = "Lottery",
                        Year = s.ShiftDate.Year
                    });
                }
                allPoints.AddRange(points);
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
                        CreatedBy = !string.IsNullOrWhiteSpace(e.CreatedBy) ? e.CreatedBy : "Unknown",
                        EmployeeUsername = e.EmployeeUsername,
                        CreatedAt = (DateTime?)e.CreatedAt ?? DateTime.MinValue,
                        HourlyRate = e.HourlyRate_AtTimeOfShift,
                        Status = e.Status ?? "Draft"
                    })
                    .ToListAsync();
                
                Console.WriteLine($"[FinancialService] Found {barEntriesRaw.Count} raw bar records.");

                // Fetch current user rates as a fallback
                var allUsers = await db.AppUsers.AsNoTracking().ToListAsync();
                var allMembers = await db.Members.AsNoTracking().ToListAsync();
                
                // Map by Username
                var userRatesByUsername = allUsers
                    .Where(u => !string.IsNullOrEmpty(u.Username))
                    .ToDictionary(u => u.Username!, u => u.HourlyRate ?? 0m, StringComparer.OrdinalIgnoreCase);

                // Map by Full Name
                var userRatesByName = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
                foreach (var user in allUsers)
                {
                    if (user.MemberId != null)
                    {
                        var member = allMembers.FirstOrDefault(m => m.MemberID == user.MemberId);
                        if (member != null)
                        {
                            var fullName = $"{member.FirstName} {member.LastName}{(string.IsNullOrEmpty(member.Suffix) ? "" : " " + member.Suffix)}".Trim();
                            if (!userRatesByName.ContainsKey(fullName))
                                userRatesByName.Add(fullName, user.HourlyRate ?? 0m);
                        }
                    }
                }

                // Final refinement in memory to ensure we group by the CORRECT date (Adjusted if available)
                var barEntries = barEntriesRaw.Select(e => new {
                    Date = (e.AdjustedSaleDate ?? e.SaleDate).Date,
                    e.Shift,
                    e.IsRentalHall,
                    e.TotalSales,
                    e.TotalHours,
                    e.Notes,
                    e.CreatedBy,
                    e.CreatedAt,
                    EmployeeUsername = !string.IsNullOrEmpty(e.EmployeeUsername) ? e.EmployeeUsername.Trim() : e.CreatedBy.Trim(),
                    HourlyRate = e.HourlyRate ?? (userRatesByUsername.TryGetValue(!string.IsNullOrEmpty(e.EmployeeUsername) ? e.EmployeeUsername.Trim() : e.CreatedBy.Trim(), out var r1) ? r1 : 
                                                 (userRatesByName.TryGetValue(!string.IsNullOrEmpty(e.EmployeeUsername) ? e.EmployeeUsername.Trim() : e.CreatedBy.Trim(), out var r2) ? r2 : 0m)),
                    e.Status
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

                // Fetch POS Sales and LiquorItems to calculate portion costs and product breakdowns
                currentStep = "Fetching POS Sales for Cost calculation";
                var posSales = await db.PosSales
                    .AsNoTracking()
                    .Where(s => s.Timestamp >= start && s.Timestamp < end.AddDays(1))
                    .ToListAsync();

                currentStep = "Fetching SystemSettings for Cost calculation";
                var settings = await db.SystemSettings.FirstOrDefaultAsync(x => x.Id == 1);
                decimal globalPour = settings?.GlobalLiquorPourSize ?? 1.5m;

                currentStep = "Fetching LiquorItems for Cost calculation";
                var liquorItems = await db.LiquorItems.AsNoTracking().ToListAsync();
                var itemCostMap = liquorItems.ToDictionary(i => i.Id);

                var salesWithShift = new Dictionary<(DateTime Date, string Shift), (decimal TotalCost, List<ShiftItemBreakdownDto> Items)>();

                foreach (var s in posSales)
                {
                    if (s.TerminalName != null && s.TerminalName.Contains("(TRAINING)")) continue;

                    var localTime = s.Timestamp;
                    var localHour = localTime.Hour;
                    DateTime targetDate = localTime.Date;
                    string targetShift = "Day";

                    if (s.ActiveEventId.HasValue && s.ActiveEventId.Value > 0)
                    {
                        targetShift = "Hall";
                    }
                    else if (localHour >= 0 && localHour < 5)
                    {
                        targetDate = targetDate.AddDays(-1);
                        targetShift = "Night";
                    }
                    else if (localHour >= 5 && localHour < 19)
                    {
                        targetShift = "Day";
                    }
                    else
                    {
                        targetShift = "Night";
                    }

                    var key = (Date: targetDate, Shift: targetShift);
                    if (!salesWithShift.ContainsKey(key))
                    {
                        salesWithShift[key] = (0m, new List<ShiftItemBreakdownDto>());
                    }

                    var currentVal = salesWithShift[key];
                    decimal saleCost = 0m;
                    var saleItems = new List<ShiftItemBreakdownDto>();

                    if (!string.IsNullOrEmpty(s.ItemsJson))
                    {
                        try
                        {
                            var items = System.Text.Json.JsonSerializer.Deserialize<List<PosSaleItemDto>>(s.ItemsJson);
                            if (items != null)
                            {
                                foreach (var item in items)
                                {
                                    decimal itemUnitCost = 0m;
                                    LiquorItem? liquor = null;
                                    if (item.Id > 0 && itemCostMap.TryGetValue(item.Id, out liquor))
                                    {
                                        var costItem = liquor.ParentItemId.HasValue && itemCostMap.TryGetValue(liquor.ParentItemId.Value, out var parent) ? parent : liquor;
                                        if (liquor.IsUnitBased || liquor.IsBeer || (liquor.Category != null && liquor.Category.Trim().ToUpper() == "BEER"))
                                        {
                                            itemUnitCost = costItem.PackSize > 0 ? (costItem.CurrentPrice / costItem.PackSize) : costItem.CurrentPrice;
                                        }
                                        else
                                        {
                                            decimal vol = costItem.BottleVolumeOunces ?? 25.4m;
                                            decimal pour = liquor.PourVolumeOunces ?? liquor.PourSize;
                                            if (pour == 0) pour = costItem.PourVolumeOunces ?? costItem.PourSize;
                                            if (pour == 0) pour = globalPour;
                                            itemUnitCost = vol > 0 ? (costItem.CurrentPrice / vol) * pour : 0m;
                                        }
                                    }

                                    decimal itemTotalCost = item.Quantity * itemUnitCost;
                                    decimal itemTotalRev = item.Quantity * item.Price;
                                    saleCost += itemTotalCost;

                                    saleItems.Add(new ShiftItemBreakdownDto
                                    {
                                        Name = item.Name,
                                        Quantity = item.Quantity,
                                        Revenue = itemTotalRev,
                                        Cost = itemTotalCost,
                                        Category = (liquor != null && !string.IsNullOrEmpty(liquor.Category)) ? liquor.Category.Trim() : "Uncategorized"
                                    });

                                    if (item.Modifiers != null)
                                    {
                                        foreach (var mod in item.Modifiers)
                                        {
                                            decimal modUnitCost = 0m;
                                            LiquorItem? modLiquor = null;
                                            if (mod.Id > 0 && itemCostMap.TryGetValue(mod.Id, out modLiquor))
                                            {
                                                var costItem = modLiquor.ParentItemId.HasValue && itemCostMap.TryGetValue(modLiquor.ParentItemId.Value, out var parent) ? parent : modLiquor;
                                                if (modLiquor.IsUnitBased || modLiquor.IsBeer || (modLiquor.Category != null && modLiquor.Category.Trim().ToUpper() == "BEER"))
                                                {
                                                    modUnitCost = costItem.PackSize > 0 ? (costItem.CurrentPrice / costItem.PackSize) : costItem.CurrentPrice;
                                                }
                                                else
                                                {
                                                    decimal vol = costItem.BottleVolumeOunces ?? 25.4m;
                                                    decimal pour = modLiquor.PourVolumeOunces ?? modLiquor.PourSize;
                                                    if (pour == 0) pour = costItem.PourVolumeOunces ?? costItem.PourSize;
                                                    if (pour == 0) pour = globalPour;
                                                    modUnitCost = vol > 0 ? (costItem.CurrentPrice / vol) * pour : 0m;
                                                }
                                            }

                                            decimal modTotalCost = item.Quantity * mod.Quantity * modUnitCost;
                                            decimal modTotalRev = item.Quantity * mod.Quantity * mod.Price;
                                            saleCost += modTotalCost;

                                            saleItems.Add(new ShiftItemBreakdownDto
                                            {
                                                Name = $"{item.Name} (+ {mod.Name})",
                                                Quantity = item.Quantity * mod.Quantity,
                                                Revenue = modTotalRev,
                                                Cost = modTotalCost,
                                                Category = (modLiquor != null && !string.IsNullOrEmpty(modLiquor.Category)) ? modLiquor.Category.Trim() : 
                                                           ((liquor != null && !string.IsNullOrEmpty(liquor.Category)) ? liquor.Category.Trim() : "Modifiers")
                                            });
                                        }
                                    }
                                }
                            }
                        }
                        catch { /* Ignored */ }
                    }

                    // Group accumulated items by Name to prevent duplicates
                    var mergedItems = currentVal.Items;
                    foreach (var newItem in saleItems)
                    {
                        var existing = mergedItems.FirstOrDefault(i => i.Name.Trim().ToLower() == newItem.Name.Trim().ToLower());
                        if (existing != null)
                        {
                            existing.Quantity += newItem.Quantity;
                            existing.Revenue += newItem.Revenue;
                            existing.Cost += newItem.Cost;
                        }
                        else
                        {
                            mergedItems.Add(newItem);
                        }
                    }

                    salesWithShift[key] = (currentVal.TotalCost + saleCost, mergedItems);
                }

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


                    var dayLotto = lottoShifts.FirstOrDefault(s => s.ShiftDate.Date == date && s.ShiftType.Equals("Day", StringComparison.OrdinalIgnoreCase));
                    var nightLotto = lottoShifts.FirstOrDefault(s => s.ShiftDate.Date == date && s.ShiftType.Equals("Night", StringComparison.OrdinalIgnoreCase));

                    // Map Day Shift
                    if (dayBar != null || dayLotto != null)
                    {
                        var lotto = dayLotto;
                        decimal earnings = 0;
                        decimal fees = 0;
                        if (lotto != null) 
                        {
                            var r = _rateRepository.GetApplicableRate(lotto.ShiftDate.Year);
                            earnings = (lotto.ShiftSalesActivity * r.SalesCommissionMultiplier) +
                                       (lotto.ShiftPayoutsActivity * r.CashingBonusMultiplier) +
                                       (lotto.ShiftCancelsActivity * r.TicketBonusMultiplier);

                            if (nightLotto == null)
                            {
                                fees = r.DailySystemFee + r.DailyBondingFee;
                            }
                        }

                        // [SELF-HEALING] Ignore persisted math and recalculate from raw activity
                        decimal activeSales = lotto?.ShiftSalesActivity ?? 0;
                        decimal activePrizes = lotto?.ShiftPayoutsActivity ?? 0;
                        decimal activeTickets = lotto?.ShiftCancelsActivity ?? 0;
                        decimal activeNetDue = lotto?.ShiftNetDueActivity ?? 0;

                        // [REPAIR]: If activity fields are 0 but cumulative fields are populated (Historic bug),
                        // try to find the previous shift in the current dataset to calculate the true delta.
                        if (lotto != null && activeSales == 0 && lotto.TotalSales > 0)
                        {
                            var prevShift = lottoShifts
                                .Where(s => s.ShiftDate < lotto.ShiftDate || (s.ShiftDate == lotto.ShiftDate && s.ShiftType == "Day" && lotto.ShiftType == "Night"))
                                .OrderByDescending(s => s.ShiftDate).ThenByDescending(s => s.ShiftType)
                                .FirstOrDefault();
                            
                            if (prevShift != null)
                            {
                                activeSales = lotto.TotalSales - prevShift.TotalSales;
                                activePrizes = lotto.TotalPayouts - prevShift.TotalPayouts;
                                activeTickets = lotto.TotalCancels - prevShift.TotalCancels;
                                activeNetDue = lotto.NetDue - prevShift.NetDue;
                            }
                            else
                            {
                                // Fallback: If no previous shift found in range, assume cumulative is the activity (Start of time)
                                activeSales = lotto.TotalSales;
                                activePrizes = lotto.TotalPayouts;
                                activeTickets = lotto.TotalCancels;
                                activeNetDue = lotto.NetDue;
                            }
                        }


                        dailyReport.Shifts.Add(new ShiftReportDto {
                             ShiftId = lotto?.ShiftId ?? 0,
                             ShiftType = "Day",
                             IsRentalHall = false,
                             BarSales = dayBar?.TotalSales ?? 0,
                             TotalHours = dayBar?.TotalHours,
                             LottoSales = lotto?.TotalSales ?? 0,
                             LottoPayouts = lotto?.TotalPayouts ?? 0,
                             LottoNetDue = lotto?.NetDue ?? 0,
                             LottoCancels = lotto?.TotalCancels ?? 0,
                             StartingCash = lotto?.StartingCash ?? 0,
                             EndingCash = lotto?.EndingCash ?? 0,
                             BackupBagAmount = lotto?.BackupBagAmount ?? 0,
                             EnvelopeAmount = 0, // Day shifts always 0
                             BagRefillAmount = lotto?.BagRefillAmount ?? 0,
                             NetSales = lotto?.NetSales ?? 0m,
                             ExpectedCash = lotto?.ExpectedCash ?? 0m,
                             Variance = lotto?.Variance ?? 0m,
                             LotteryIncome = lotto?.LotteryIncome ?? 0,
                             IdentifiedFees = fees,
                             NetIncome = lotto?.NetIncome ?? 0,
                             ShiftSalesActivity = activeSales,
                             ShiftPayoutsActivity = activePrizes,
                             ShiftCancelsActivity = activeTickets,
                             ShiftNetDueActivity = activeNetDue,
                             Notes = !string.IsNullOrWhiteSpace(dayBar?.Notes) ? dayBar.Notes : lotto?.Notes,
                             Status = !string.IsNullOrWhiteSpace(dayBar?.Status) ? dayBar.Status : lotto?.Status,
                             CreatedBy = !string.IsNullOrWhiteSpace(dayBar?.EmployeeUsername) ? dayBar.EmployeeUsername : 
                                         (!string.IsNullOrWhiteSpace(lotto?.EmployeeName) ? lotto.EmployeeName : 
                                         (!string.IsNullOrWhiteSpace(dayBar?.CreatedBy) ? dayBar.CreatedBy : "Unknown")),
                             CreatedAt = dayBar?.CreatedAt ?? lotto?.CreatedDate ?? date,
                             HourlyRate = dayBar?.HourlyRate,
                             ProductCost = salesWithShift.TryGetValue((date, "Day"), out var dayData) ? dayData.TotalCost : 0m,
                             SoldItems = salesWithShift.TryGetValue((date, "Day"), out var dayData2) ? dayData2.Items.OrderByDescending(i => i.Revenue).ToList() : new List<ShiftItemBreakdownDto>()
                        });
                    }

                    // Map Night Shift
                    if (nightBar != null || nightLotto != null)
                    {
                        var lotto = nightLotto;
                        decimal earnings = 0;
                        decimal fees = 0;
                        if (lotto != null) 
                        {
                            var r = _rateRepository.GetApplicableRate(lotto.ShiftDate.Year);
                            earnings = (lotto.ShiftSalesActivity * r.SalesCommissionMultiplier) +
                                       (lotto.ShiftPayoutsActivity * r.CashingBonusMultiplier) +
                                       (lotto.ShiftCancelsActivity * r.TicketBonusMultiplier);

                            // NEW: Combined Daily Fees (System + Bonding) applied to the Final shift
                            fees = r.DailySystemFee + r.DailyBondingFee;
                        }

                        // [SELF-HEALING] Ignore persisted math and recalculate from raw activity
                        decimal activeSales = lotto?.ShiftSalesActivity ?? 0;
                        decimal activePrizes = lotto?.ShiftPayoutsActivity ?? 0;
                        decimal activeTickets = lotto?.ShiftCancelsActivity ?? 0;
                        decimal activeNetDue = lotto?.ShiftNetDueActivity ?? 0;

                        // [REPAIR]: If activity fields are 0 but cumulative fields are populated (Historic bug),
                        // try to find the previous shift in the current dataset to calculate the true delta.
                        if (lotto != null && activeSales == 0 && lotto.TotalSales > 0)
                        {
                            var prevShift = lottoShifts
                                .Where(s => s.ShiftDate < lotto.ShiftDate || (s.ShiftDate == lotto.ShiftDate && s.ShiftType == "Day" && lotto.ShiftType == "Night"))
                                .OrderByDescending(s => s.ShiftDate).ThenByDescending(s => s.ShiftType)
                                .FirstOrDefault();
                            
                            if (prevShift != null)
                            {
                                activeSales = lotto.TotalSales - prevShift.TotalSales;
                                activePrizes = lotto.TotalPayouts - prevShift.TotalPayouts;
                                activeTickets = lotto.TotalCancels - prevShift.TotalCancels;
                                activeNetDue = lotto.NetDue - prevShift.NetDue;
                            }
                        }


                        dailyReport.Shifts.Add(new ShiftReportDto {
                             ShiftId = lotto?.ShiftId ?? 0,
                             ShiftType = "Night",
                             IsRentalHall = false,
                             BarSales = nightBar?.TotalSales ?? 0,
                             TotalHours = nightBar?.TotalHours,
                             LottoSales = lotto?.TotalSales ?? 0,
                             LottoPayouts = lotto?.TotalPayouts ?? 0,
                             LottoNetDue = lotto?.NetDue ?? 0,
                             LottoCancels = lotto?.TotalCancels ?? 0,
                             StartingCash = lotto?.StartingCash ?? 0,
                             EndingCash = lotto?.EndingCash ?? 0,
                             BackupBagAmount = lotto?.BackupBagAmount ?? 0,
                             EnvelopeAmount = lotto?.EnvelopeAmount ?? 0,
                             BagRefillAmount = lotto?.BagRefillAmount ?? 0,
                             NetSales = lotto?.NetSales ?? 0m,
                             ExpectedCash = lotto?.ExpectedCash ?? 0m,
                             Variance = lotto?.Variance ?? 0m,
                             LotteryIncome = lotto?.LotteryIncome ?? 0,
                             IdentifiedFees = fees,
                             NetIncome = lotto?.NetIncome ?? 0,
                             ShiftSalesActivity = activeSales,
                             ShiftPayoutsActivity = activePrizes,
                             ShiftCancelsActivity = activeTickets,
                             ShiftNetDueActivity = activeNetDue,
                             Notes = !string.IsNullOrWhiteSpace(nightBar?.Notes) ? nightBar.Notes : lotto?.Notes,
                             Status = !string.IsNullOrWhiteSpace(nightBar?.Status) ? nightBar.Status : lotto?.Status,
                             CreatedBy = !string.IsNullOrWhiteSpace(nightBar?.EmployeeUsername) ? nightBar.EmployeeUsername : 
                                         (!string.IsNullOrWhiteSpace(lotto?.EmployeeName) ? lotto.EmployeeName : 
                                         (!string.IsNullOrWhiteSpace(nightBar?.CreatedBy) ? nightBar.CreatedBy : "Unknown")),
                             CreatedAt = nightBar?.CreatedAt ?? lotto?.CreatedDate ?? date,
                             HourlyRate = nightBar?.HourlyRate,
                             ProductCost = salesWithShift.TryGetValue((date, "Night"), out var nightData) ? nightData.TotalCost : 0m,
                             SoldItems = salesWithShift.TryGetValue((date, "Night"), out var nightData2) ? nightData2.Items.OrderByDescending(i => i.Revenue).ToList() : new List<ShiftItemBreakdownDto>()
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
                            CreatedAt = hallBar.CreatedAt,
                            HourlyRate = hallBar.HourlyRate,
                            ProductCost = salesWithShift.TryGetValue((date, "Hall"), out var hallData) ? hallData.TotalCost : 0m,
                            SoldItems = salesWithShift.TryGetValue((date, "Hall"), out var hallData2) ? hallData2.Items.OrderByDescending(i => i.Revenue).ToList() : new List<ShiftItemBreakdownDto>(),
                            // Explicitly zero out lottery fields to prevent variance leakage
                            Variance = 0,
                            ExpectedCash = 0,
                            ShiftSalesActivity = 0,
                            ShiftPayoutsActivity = 0,
                            ShiftCancelsActivity = 0,
                            ShiftNetDueActivity = 0,
                            Status = hallBar.Status,
                            LotteryIncome = 0,
                            IdentifiedFees = 0,
                            NetIncome = 0
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

            var result = await query
                .OrderByDescending(s => s.ShiftDate)
                .ThenByDescending(s => s.ShiftId)
                .ToListAsync();

            // [NEW] Overwrite income with on-the-fly calculation per user instructions
            foreach (var s in result)
            {
                var r = _rateRepository.GetApplicableRate(s.ShiftDate.Year);
                s.LotteryIncome = (s.ShiftSalesActivity * r.SalesCommissionMultiplier) +
                                  (s.ShiftPayoutsActivity * r.CashingBonusMultiplier) +
                                  (s.ShiftCancelsActivity * r.TicketBonusMultiplier);
            }
            
            return result;
        }

        public async Task<List<EmployeeHoursDto>> GetEmployeeHoursAsync(DateTime startDate, DateTime endDate, string? username = null, string? location = "All")
        {
            try 
            {
                using var db = await _dbFactory.CreateDbContextAsync();
            var start = startDate.Date;
            var end = endDate.Date.AddDays(1); // Include entire end date

            IQueryable<BarSaleEntry> query = db.BarSaleEntries.IgnoreQueryFilters().AsNoTracking();

            // DIAGNOSTIC: Get raw count before any filters
            int rawCount = await query.CountAsync();
            IFinancialAnalyticsService.DiagnosticRawCount = rawCount;
            Console.WriteLine($"[DIAGNOSTIC] Total BarSaleEntries in DB (Ignoring Filters): {rawCount}");

            // Filter out truly deleted records unless we want them for diagnostics
            query = query.Where(e => e.IsDeleted == false);

            // Date filter
            query = query.Where(e => (e.AdjustedSaleDate ?? e.SaleDate) >= start && (e.AdjustedSaleDate ?? e.SaleDate) < end);
            
            // Location Filter
            if (location != "All" && !string.IsNullOrWhiteSpace(location))
            {
                if (location == "Main") query = query.Where(e => !e.IsRentalHall && e.Shift != "Hall");
                else if (location == "Hall") query = query.Where(e => e.IsRentalHall || e.Shift == "Hall");
            }

            if (!string.IsNullOrWhiteSpace(username) && username != "All")
            {
                query = query.Where(e => 
                    (e.EmployeeUsername != null && e.EmployeeUsername.Trim() == username.Trim()) || 
                    (e.ModifiedBy != null && e.ModifiedBy.Trim() == username.Trim()) ||
                    (e.CreatedBy != null && e.CreatedBy.Trim() == username.Trim())
                );
            }

            // Diagnostic: Check raw count ignoring filters to see if IsDeleted is hiding data
            var rawCountNoFilters = await db.BarSaleEntries.IgnoreQueryFilters().CountAsync();
            if (rawCount == 0 && rawCountNoFilters > 0)
            {
                Console.WriteLine($"[DIAGNOSTIC] data exists ({rawCountNoFilters} records) but is HIDDEN by query filter (IsDeleted=1).");
            }

            var sys = await db.SystemSettings.AsNoTracking().OrderBy(s => s.Id).FirstOrDefaultAsync();
            decimal fallbackEmployeeTax = 0, fallbackEmployerTax = 0;
            if (sys != null)
            {
                fallbackEmployeeTax = (sys.MaStateTaxRate + sys.PfmlEmployeeRate + sys.FicaEmployeeRate) / 100m;
                fallbackEmployerTax = (sys.MaUnemploymentRate + sys.PfmlEmployerRate + sys.FicaEmployerRate) / 100m;
            }
            
            Console.WriteLine($"[FinancialService] Fetched settings. Rates: Emp {fallbackEmployeeTax:P2}, Employer {fallbackEmployerTax:P2}");

            var entries = await query
                .Select(e => new {
                    Date = (e.AdjustedSaleDate ?? e.SaleDate).Date,
                    Hours = e.TotalHours ?? 0m,
                    Shift = e.Shift ?? "Day",
                    IsHall = e.IsRentalHall,
                    User = !string.IsNullOrWhiteSpace(e.EmployeeUsername) ? e.EmployeeUsername : (!string.IsNullOrWhiteSpace(e.ModifiedBy) ? e.ModifiedBy : (!string.IsNullOrWhiteSpace(e.CreatedBy) ? e.CreatedBy : "Unknown")),
                    CreatedBy = e.CreatedBy,
                    ModifiedBy = e.ModifiedBy,
                    HistoricalRate = e.HourlyRate_AtTimeOfShift,
                    HistoricalEmployeeTax = e.TotalEmployeeTaxes_AtTimeOfShift,
                    HistoricalEmployerTax = e.TotalEmployerTaxes_AtTimeOfShift
                })
                .ToListAsync();

            if (!entries.Any()) 
            {
                Console.WriteLine($"[FinancialService] No BarSaleEntries found for {start:yyyy-MM-dd} to {end:yyyy-MM-dd}");
                return new List<EmployeeHoursDto>();
            }

            Console.WriteLine($"[FinancialService] Found {entries.Count} bar sale entries. Starting mapping...");
            foreach(var e in entries.Take(5)) Console.WriteLine($" - Entry: {e.User} ({e.Date:MM/dd}), Hours: {e.Hours}");

            // Fetch dynamic tax tables for the current year (2024 by default)
            var currentYear = DateTime.Now.Year;
            // [SMART LOOKUP]: Find latest available tax year if current year is missing
            var taxYear = currentYear;
            if (!await db.TaxBrackets.AnyAsync(b => b.TaxYear == taxYear))
            {
                var latestYear = await db.TaxBrackets.OrderByDescending(b => b.TaxYear).Select(b => b.TaxYear).FirstOrDefaultAsync();
                if (latestYear > 0) taxYear = latestYear;
            }

            var allBrackets = await db.TaxBrackets.AsNoTracking().Where(b => b.TaxYear == taxYear).ToListAsync();
            var allDeductions = await db.TaxStandardDeductions.AsNoTracking().Where(d => d.TaxYear == taxYear).ToListAsync();

            // Fetch users (Remove strict employee tracking filter to ensure all recorded hours are visible)
            var users = await db.AppUsers.AsNoTracking()
                .ToListAsync();
            
            var allMembers = await db.Members.AsNoTracking().ToListAsync();

            // [SMART MATCHING] Build a map of Full Names to Usernames to handle records with names like "Darren Marques"
            var nameToUsername = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var u in users)
            {
                if (!nameToUsername.ContainsKey(u.Username)) nameToUsername[u.Username] = u.Username;
                if (!string.IsNullOrEmpty(u.Email) && !nameToUsername.ContainsKey(u.Email)) nameToUsername[u.Email] = u.Username;
                
                if (u.MemberId.HasValue)
                {
                    var m = allMembers.FirstOrDefault(x => x.MemberID == u.MemberId);
                    if (m != null)
                    {
                        var fullName = $"{m.FirstName} {m.LastName}".Trim();
                        var fullNameWithSuffix = $"{m.FirstName} {m.LastName} {m.Suffix}".Trim();
                        
                        if (!nameToUsername.ContainsKey(fullName)) nameToUsername[fullName] = u.Username;
                        if (!string.IsNullOrEmpty(m.Suffix) && !nameToUsername.ContainsKey(fullNameWithSuffix)) nameToUsername[fullNameWithSuffix] = u.Username;
                    }
                }
            }
            
            // Year-specific overrides for older records that missed the snapshot
            var yearsInRange = Enumerable.Range(start.Year, (end.Year - start.Year) + 1).ToList();
            var yearlyOverrides = await db.YearlyWages.AsNoTracking()
                .Where(w => yearsInRange.Contains(w.Year))
                .ToListAsync();

            var filteredResults = new List<EmployeeHoursDto>();
            var usedEntryIndices = new HashSet<int>();

            foreach (var user in users)
            {
                // Find all names that map to this user (Username OR Full Name)
                var userFullNames = nameToUsername.Where(kvp => kvp.Value == user.Username).Select(kvp => kvp.Key).ToList();
                
                // Get all shifts belonging to this user (Match by Username or any known Full Name)
                var userEntries = entries.Where((e, idx) => 
                    (string.Equals(e.User?.Trim(), user.Username?.Trim(), StringComparison.OrdinalIgnoreCase) || 
                     userFullNames.Any(fn => string.Equals(fn.Trim(), e.User?.Trim(), StringComparison.OrdinalIgnoreCase)))
                ).ToList();

                if (!userEntries.Any()) continue;

                // Track used entries
                for(int i=0; i<entries.Count; i++) {
                    var e = entries[i];
                    if (string.Equals(e.User?.Trim(), user.Username?.Trim(), StringComparison.OrdinalIgnoreCase) || 
                        userFullNames.Any(fn => string.Equals(fn.Trim(), e.User?.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        usedEntryIndices.Add(i);
                    }
                }

                try 
                {
                    var totalHours = userEntries.Sum(e => e.Hours);

                    // Skip if they worked 0 hours
                    if (totalHours <= 0) continue;

                    var defaultRate = user.HourlyRate ?? 0;
                    
                    decimal totalPay = 0, netPay = 0, totalPayrollCost = 0;
                    decimal downstairsPay = 0, upstairsPay = 0;
                    decimal totalWithheld = 0, totalEmployerAddOn = 0;
                    decimal fedWh = 0, ficaSS = 0, ficaMed = 0, maIncomeTax = 0, maSui = 0, maPfml = 0, empSS = 0, empMed = 0;

                    foreach (var e in userEntries)
                    {
                        try 
                        {
                            // [PRECEDENCE]: 1. Live Snapshot from Shift | 2. Yearly Database Override | 3. Current Live User Profile (Fallback)
                            var shiftYear = e.Date.Year;
                            var yearlyOverride = yearlyOverrides.FirstOrDefault(w => w.Username == user.Username && w.Year == shiftYear);
                        
                            decimal rate = e.HistoricalRate ?? yearlyOverride?.HourlyRate ?? defaultRate;
                            decimal shiftGross = e.Hours * rate;
        
                            // [DYNAMIC] IRS Percentage Method (Pulling from your Database)
                            var yearDeduction = allDeductions.FirstOrDefault(d => d.FilingStatus == user.FilingStatus);
                            
                            decimal shiftWithheld = 0;
                            decimal shiftEmployerAddOn = 0;

                            try 
                            {
                                var taxBreakdown = GFC.BlazorServer.Utilities.PayrollTaxCalculator.CalculateFederalTaxes(
                                    shiftGross, user, allBrackets, yearDeduction, "Monthly");
                                
                                // State & PFML Taxes (Using standard MA Rates)
                                decimal stateRate = (sys?.MaStateTaxRate ?? 5.0m) / 100m;
                                decimal shiftMaIncomeTax = Math.Floor(shiftGross * stateRate * 100m) / 100m;
                                
                                decimal suiRate = (sys?.MaUnemploymentRate ?? 2.42m) / 100m;
                                decimal shiftMaSui = Math.Floor(shiftGross * suiRate * 100m) / 100m;
                                
                                decimal pfmlRate = (sys?.PfmlEmployeeRate ?? 0.35m) / 100m;
                                decimal shiftMaPfml = Math.Floor(shiftGross * pfmlRate * 100m) / 100m;

                                shiftWithheld = taxBreakdown.TotalEmployeeWithholding + shiftMaIncomeTax + shiftMaPfml;
                                shiftEmployerAddOn = taxBreakdown.TotalEmployerLiability + (shiftGross * fallbackEmployerTax) + shiftMaSui;

                                // Accumulate Detail Fields
                                fedWh += taxBreakdown.FederalTax;
                                ficaSS += taxBreakdown.SocialSecurity;
                                ficaMed += taxBreakdown.Medicare;
                                maIncomeTax += shiftMaIncomeTax;
                                maSui += shiftMaSui;
                                maPfml += shiftMaPfml;
                                empSS += taxBreakdown.EmployerSocialSecurity;
                                empMed += taxBreakdown.EmployerMedicare;
                            }
                            catch 
                            {
                                // Fallback for missing tax data
                                shiftWithheld = shiftGross * fallbackEmployeeTax;
                                shiftEmployerAddOn = shiftGross * fallbackEmployerTax;
                            }

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
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[FinancialService] Error processing entry for {user.Username}: {ex.Message}");
                        }
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
                        UpstairsPay = upstairsPay,
                        
                        FederalWithholding = fedWh,
                        FicaSocialSecurity = ficaSS,
                        FicaMedicare = ficaMed,
                        MaIncomeTax = maIncomeTax,
                        MaSui = maSui,
                        MaPfml = maPfml,
                        EmployerFicaSocialSecurity = empSS,
                        EmployerFicaMedicare = empMed
                    };

                    // Fill daily breakdown and shift types
                    foreach (var entryGroup in userEntries.GroupBy(e => e.Date))
                    {
                        var date = entryGroup.Key;
                        var hours = entryGroup.Sum(e => e.Hours);
                        dto.DailyHours[date] = hours;

                        // Determine shift signature
                        var shiftsWithHours = entryGroup.Where(e => e.Hours > 0).Select(e => new { Shift = e.IsHall ? "Hall" : e.Shift, e.Hours }).ToList();
                        if (shiftsWithHours.Select(s => s.Shift).Distinct().Count() > 1)
                        {
                            // Pack multiple shifts on the same day (e.g. Day:4.0|Janitor:2.0)
                            dto.DailyShiftTypes[date] = string.Join("|", shiftsWithHours.Select(s => $"{s.Shift.Trim()}:{s.Hours:N1}"));
                        }
                        else if (shiftsWithHours.Any())
                        {
                            dto.DailyShiftTypes[date] = shiftsWithHours.First().Shift;
                        }
                    }

                    // Link member name for better display
                    var member = allMembers.FirstOrDefault(m => m.MemberID == user.MemberId);
                    dto.MemberName = member != null ? $"{member.FirstName} {member.LastName}" : (user.Email ?? user.Username);

                    filteredResults.Add(dto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FinancialService] Skip user {user.Username} due to calculation error: {ex.Message}");
                }
        }

            // [ORPHANS] Handle any entries that didn't match a user
            var orphanedEntries = entries.Where((e, idx) => !usedEntryIndices.Contains(idx)).ToList();
            if (orphanedEntries.Any())
            {
                Console.WriteLine($"[FinancialService] Found {orphanedEntries.Count} orphaned entries. Grouping into Miscellaneous.");
                var miscDto = new EmployeeHoursDto {
                    Username = "miscellaneous",
                    MemberName = "Miscellaneous / Unmapped",
                    TotalHours = orphanedEntries.Sum(e => e.Hours),
                    EntryCount = orphanedEntries.Count,
                    TotalPay = orphanedEntries.Sum(e => e.Hours * 15.0m), // Estimate for unmapped
                    StartDate = start,
                    EndDate = end,
                    Notes = "Check records with empty EmployeeUsername"
                };
                foreach (var entryGroup in orphanedEntries.GroupBy(e => e.Date))
                    miscDto.DailyHours[entryGroup.Key] = entryGroup.Sum(e => e.Hours);
                
                filteredResults.Add(miscDto);
            }

            return filteredResults
                .OrderBy(d => {
                    var u = users.FirstOrDefault(x => x.Username == d.Username);
                    if (u != null && u.MemberId.HasValue)
                    {
                        var m = allMembers.FirstOrDefault(x => x.MemberID == u.MemberId);
                        if (m != null) return m.LastName ?? m.FirstName ?? d.MemberName;
                    }
                    return d.MemberName;
                })
                .ThenBy(d => d.MemberName)
                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FinancialService] ERROR in GetEmployeeHoursAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return new List<EmployeeHoursDto>();
            }
        }

        public async Task<FinancialSnapshotDto> GetFinancialSnapshotAsync(int year, int? month = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var snapshot = new FinancialSnapshotDto { Year = year, Month = month };
            
            DateTime start, end;
            if (month.HasValue)
            {
                start = new DateTime(year, month.Value, 1);
                end = start.AddMonths(1).AddDays(-1);
            }
            else
            {
                start = new DateTime(year, 1, 1);
                end = new DateTime(year, 12, 31);
            }

            // 1. Income - Bar Sales
            var barSales = await db.BarSaleEntries.AsNoTracking()
                .Where(b => (b.AdjustedSaleDate ?? b.SaleDate) >= start && (b.AdjustedSaleDate ?? b.SaleDate) <= end)
                .ToListAsync();
            
            snapshot.BarSalesDownstairs = barSales.Where(b => !b.IsRentalHall).Sum(b => b.TotalSales);
            snapshot.BarSalesUpstairs = barSales.Where(b => b.IsRentalHall).Sum(b => b.TotalSales);

            // 2. Income - Lottery
            var lottoAnalyticShifts = await GetLotteryAnalyticsAsync(start, end);
            decimal totalLottoIncome = 0;
            foreach (var s in lottoAnalyticShifts)
            {
                var r = _rateRepository.GetApplicableRate(s.ShiftDate.Year);
                totalLottoIncome += (s.ShiftSalesActivity * r.SalesCommissionMultiplier) +
                                    (s.ShiftPayoutsActivity * r.CashingBonusMultiplier) +
                                    (s.ShiftCancelsActivity * r.TicketBonusMultiplier);
            }
            snapshot.LotteryCommissions = totalLottoIncome;
            snapshot.LotteryBonuses = 0m; // Included in commissions for simplicity

            // 3. Income - Membership Dues
            if (month.HasValue)
            {
                snapshot.MembershipDues = await db.DuesPayments.AsNoTracking()
                    .Where(d => d.PaidDate.HasValue && d.PaidDate.Value.Year == year && d.PaidDate.Value.Month == month.Value)
                    .SumAsync(d => d.Amount) ?? 0m;
            }
            else
            {
                snapshot.MembershipDues = await db.DuesPayments.AsNoTracking()
                    .Where(d => d.Year == year)
                    .SumAsync(d => d.Amount) ?? 0m;
            }

            // 4. Income - Hall Rentals
            snapshot.HallRentals = await db.HallRentals.AsNoTracking()
                .Where(h => h.EventDate >= start && h.EventDate <= end && h.Status == "Completed")
                .SumAsync(h => (decimal?)h.TotalPrice) ?? 0m;

            // 5. Expenses - Reimbursements
            snapshot.Reimbursements = await db.ReimbursementItems.AsNoTracking()
                .Include(i => i.Request)
                .Where(i => i.Request.Status == "Paid" && i.Request.PaidDateUtc != null && i.Request.PaidDateUtc.Value.Date >= start.Date && i.Request.PaidDateUtc.Value.Date <= end.Date)
                .SumAsync(i => (decimal?)i.Amount) ?? 0m;

            // 6. Expenses - Paid Bills and Loans
            snapshot.PaidBills = await db.FinancePayments.AsNoTracking()
                .Where(p => p.BillId != null && p.PaymentDate >= start && p.PaymentDate <= end)
                .SumAsync(p => p.AmountPaid);

            snapshot.PaidLoans = await db.FinancePayments.AsNoTracking()
                .Where(p => p.LoanId != null && p.PaymentDate >= start && p.PaymentDate <= end)
                .SumAsync(p => p.AmountPaid);

            // 7. Expenses - Payroll
            var payroll = await GetEmployeeHoursAsync(start, end);
            snapshot.GrossPayroll = payroll.Sum(p => p.TotalPay);
            
            // 8. Tax Calculation based on SystemSettings (Id = 1)
            var config = await db.SystemSettings.AsNoTracking().OrderBy(s => s.Id).FirstOrDefaultAsync(s => s.Id == 1);
            if (config != null && snapshot.GrossPayroll > 0)
            {
                snapshot.EmployerFica = snapshot.GrossPayroll * (config.FicaEmployerRate / 100m);
                snapshot.EmployerPfml = snapshot.GrossPayroll * (config.PfmlEmployerRate / 100m);
                snapshot.MaUnemployment = snapshot.GrossPayroll * (config.MaUnemploymentRate / 100m);
            }

            // 9. Period-over-Period (MoM MTD Like-for-Like) Comparison Calculation
            try
            {
                if (month.HasValue)
                {
                    int maxDay = end.Day;
                    bool isInProgressMonth = year == DateTime.Now.Year && month.Value == DateTime.Now.Month;
                    if (isInProgressMonth)
                    {
                        maxDay = Math.Min(DateTime.Now.Day, end.Day);
                    }

                    snapshot.DaysCompared = maxDay;

                    DateTime currentMtdEnd = new DateTime(year, month.Value, maxDay).Date.AddDays(1).AddTicks(-1);
                    var currentMtdSnapshot = isInProgressMonth ? await GetSnapshotByDateRangeAsync(db, start, currentMtdEnd) : null;

                    if (currentMtdSnapshot != null)
                    {
                        snapshot.CurrentMtdTotalIncome = currentMtdSnapshot.TotalIncome;
                        snapshot.CurrentMtdTotalExpenses = currentMtdSnapshot.TotalExpenses;
                    }
                    else
                    {
                        snapshot.CurrentMtdTotalIncome = snapshot.TotalIncome;
                        snapshot.CurrentMtdTotalExpenses = snapshot.TotalExpenses;
                    }

                    DateTime prevStart = start.AddMonths(-1);
                    int daysInPrev = DateTime.DaysInMonth(prevStart.Year, prevStart.Month);
                    int targetPrevDay = Math.Min(maxDay, daysInPrev);
                    DateTime prevEnd = new DateTime(prevStart.Year, prevStart.Month, targetPrevDay).Date.AddDays(1).AddTicks(-1);

                    var prevSnapshot = await GetSnapshotByDateRangeAsync(db, prevStart, prevEnd);
                    snapshot.PrevPeriodTotalIncome = prevSnapshot.TotalIncome;
                    snapshot.PrevPeriodTotalExpenses = prevSnapshot.TotalExpenses;
                }
                else
                {
                    // Full Year comparison against previous year YTD / Full Year
                    int targetDay = 365;
                    bool isCurrentYear = year == DateTime.Now.Year;
                    if (isCurrentYear)
                    {
                        targetDay = DateTime.Now.DayOfYear;
                    }
                    snapshot.DaysCompared = targetDay;

                    DateTime prevStart = new DateTime(year - 1, 1, 1);
                    DateTime prevEnd = isCurrentYear 
                        ? prevStart.AddDays(targetDay - 1).Date.AddDays(1).AddTicks(-1)
                        : new DateTime(year - 1, 12, 31);

                    var prevSnapshot = await GetSnapshotByDateRangeAsync(db, prevStart, prevEnd);
                    snapshot.PrevPeriodTotalIncome = prevSnapshot.TotalIncome;
                    snapshot.PrevPeriodTotalExpenses = prevSnapshot.TotalExpenses;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FinancialService] Error calculating Snapshot MoM: {ex.Message}");
            }

            return snapshot;
        }

        private async Task<FinancialSnapshotDto> GetSnapshotByDateRangeAsync(GfcDbContext db, DateTime start, DateTime end)
        {
            var snapshot = new FinancialSnapshotDto();

            var barSales = await db.BarSaleEntries.AsNoTracking()
                .Where(b => (b.AdjustedSaleDate ?? b.SaleDate) >= start && (b.AdjustedSaleDate ?? b.SaleDate) <= end)
                .ToListAsync();
            
            snapshot.BarSalesDownstairs = barSales.Where(b => !b.IsRentalHall).Sum(b => b.TotalSales);
            snapshot.BarSalesUpstairs = barSales.Where(b => b.IsRentalHall).Sum(b => b.TotalSales);

            var lottoAnalyticShifts = await GetLotteryAnalyticsAsync(start, end);
            decimal totalLottoIncome = 0;
            foreach (var s in lottoAnalyticShifts)
            {
                var r = _rateRepository.GetApplicableRate(s.ShiftDate.Year);
                totalLottoIncome += (s.ShiftSalesActivity * r.SalesCommissionMultiplier) +
                                    (s.ShiftPayoutsActivity * r.CashingBonusMultiplier) +
                                    (s.ShiftCancelsActivity * r.TicketBonusMultiplier);
            }
            snapshot.LotteryCommissions = totalLottoIncome;

            snapshot.MembershipDues = await db.DuesPayments.AsNoTracking()
                .Where(d => d.PaidDate.HasValue && d.PaidDate.Value >= start && d.PaidDate.Value <= end)
                .SumAsync(d => d.Amount) ?? 0m;

            snapshot.HallRentals = await db.HallRentals.AsNoTracking()
                .Where(h => h.EventDate >= start && h.EventDate <= end && h.Status == "Completed")
                .SumAsync(h => (decimal?)h.TotalPrice) ?? 0m;

            snapshot.Reimbursements = await db.ReimbursementItems.AsNoTracking()
                .Include(i => i.Request)
                .Where(i => i.Request.Status == "Paid" && i.Request.PaidDateUtc != null && i.Request.PaidDateUtc.Value.Date >= start.Date && i.Request.PaidDateUtc.Value.Date <= end.Date)
                .SumAsync(i => (decimal?)i.Amount) ?? 0m;

            snapshot.PaidBills = await db.FinancePayments.AsNoTracking()
                .Where(p => p.BillId != null && p.PaymentDate >= start && p.PaymentDate <= end)
                .SumAsync(p => p.AmountPaid);

            snapshot.PaidLoans = await db.FinancePayments.AsNoTracking()
                .Where(p => p.LoanId != null && p.PaymentDate >= start && p.PaymentDate <= end)
                .SumAsync(p => p.AmountPaid);

            var payroll = await GetEmployeeHoursAsync(start, end);
            snapshot.GrossPayroll = payroll.Sum(p => p.TotalPay);

            var config = await db.SystemSettings.AsNoTracking().OrderBy(s => s.Id).FirstOrDefaultAsync(s => s.Id == 1);
            if (config != null && snapshot.GrossPayroll > 0)
            {
                snapshot.EmployerFica = snapshot.GrossPayroll * (config.FicaEmployerRate / 100m);
                snapshot.EmployerPfml = snapshot.GrossPayroll * (config.PfmlEmployerRate / 100m);
                snapshot.MaUnemployment = snapshot.GrossPayroll * (config.MaUnemploymentRate / 100m);
            }

            return snapshot;
        }

        public async Task<IncomeAuditSummaryDto> GetIncomeSummaryWithAuditAsync(DateTime startDate, DateTime endDate, int? comparisonYear = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var start = startDate.Date;
            var end = endDate.Date.AddDays(1).AddTicks(-1);

            var summary = new IncomeAuditSummaryDto
            {
                StartDate = start,
                EndDate = end
            };

            // 1. Bar Sales - include submitted entries as well as active POS entries with TotalSales > 0
            var barEntries = await db.BarSaleEntries.AsNoTracking()
                .Where(b => (b.Status == "Submitted" || b.TotalSales > 0) && (b.AdjustedSaleDate ?? b.SaleDate) >= start && (b.AdjustedSaleDate ?? b.SaleDate) <= end)
                .ToListAsync();

            summary.TotalBarSales = barEntries.Sum(b => b.TotalSales);

            foreach (var b in barEntries)
            {
                var entryDate = (b.AdjustedSaleDate ?? b.SaleDate).Date;
                var shiftName = string.IsNullOrWhiteSpace(b.Shift) ? "Day" : b.Shift;
                summary.LedgerEntries.Add(new IncomeStreamEntryDto
                {
                    Date = entryDate,
                    Stream = "Bar Sales",
                    Shift = shiftName,
                    Description = $"Bar Sales - Shift {shiftName} ({b.EmployeeUsername ?? "Staff"})",
                    Amount = b.TotalSales,
                    SourceUrl = "/finance/bar-sales"
                });
            }

            // Audit Bar Sales missing entries (check each day in date range)
            for (var d = start.Date; d <= endDate.Date; d = d.AddDays(1))
            {
                // Skip if date is in the future
                if (d > DateTime.Now.Date) continue;

                var dayEntries = barEntries.Where(b => (b.AdjustedSaleDate ?? b.SaleDate).Date == d).ToList();
                if (!dayEntries.Any())
                {
                    summary.Warnings.Add(new MissingEntryWarningDto
                    {
                        Date = d,
                        Stream = "Bar Sales",
                        Details = $"No Bar Sales entry recorded for {d:ddd, MMM dd, yyyy}",
                        ActionUrl = "/finance/bar-sales"
                    });
                }
            }

            // 2. Lottery Commissions (From Lottery Sales Management weekly downloaded reports: combination of commissions, cash bonus, and claims bonus)
            var weeklyStats = await db.LotteryWeeklyStats.AsNoTracking()
                .Where(w => w.WeekEndingDate >= start && w.WeekEndingDate <= end)
                .OrderBy(w => w.WeekEndingDate)
                .ToListAsync();

            summary.TotalLotteryCommissions = weeklyStats.Sum(w =>
                (Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission) +
                 Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus) +
                 Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus)) -
                (Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee)));

            foreach (var w in weeklyStats)
            {
                var totalEarnings = (Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission) +
                                    Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus) +
                                    Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus)) -
                                    (Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee));

                summary.LedgerEntries.Add(new IncomeStreamEntryDto
                {
                    Date = w.WeekEndingDate.Date,
                    Stream = "Lottery",
                    Shift = "Weekly",
                    Description = $"Lottery Net Earnings - Week Ending {w.WeekEndingDate:MMM dd, yyyy} (Commissions + Bonuses - Weekly Fees)",
                    Amount = totalEarnings,
                    SourceUrl = "/lottery"
                });
            }

            // 3. Membership Dues
            var duesPayments = await db.DuesPayments.AsNoTracking()
                .Where(dp => dp.PaidDate.HasValue && dp.PaidDate.Value >= start && dp.PaidDate.Value <= end && dp.Amount.HasValue)
                .ToListAsync();

            summary.TotalMembershipDues = duesPayments.Sum(dp => dp.Amount ?? 0m);

            foreach (var dp in duesPayments)
            {
                summary.LedgerEntries.Add(new IncomeStreamEntryDto
                {
                    Date = dp.PaidDate!.Value.Date,
                    Stream = "Membership Dues",
                    Shift = "Dues Payment",
                    Description = $"Dues Payment ({dp.PaymentType ?? "CASH"})",
                    Amount = dp.Amount ?? 0m,
                    SourceUrl = "/membership"
                });
            }

            // Order ledger entries newest first
            summary.LedgerEntries = summary.LedgerEntries.OrderByDescending(e => e.Date).ThenBy(e => e.Stream).ToList();
            summary.Warnings = summary.Warnings.OrderByDescending(w => w.Date).ToList();

            // 4. Period-over-Period (MoM MTD / YoY YTD Like-for-Like) Comparison Calculation
            try
            {
                bool isFullYear = startDate.Month == 1 && endDate.Month == 12 && endDate.Day == 31;
                if (isFullYear)
                {
                    DateTime prevStart = startDate.AddYears(-1);
                    DateTime prevEnd = endDate.AddYears(-1);
                    if (startDate.Year == DateTime.Now.Year)
                    {
                        var todayYtdDay = DateTime.Now.DayOfYear;
                        prevEnd = prevStart.AddDays(todayYtdDay - 1).Date.AddDays(1).AddTicks(-1);
                        summary.PrevMonthDaysCompared = todayYtdDay;
                    }
                    else
                    {
                        summary.PrevMonthDaysCompared = 365;
                    }

                    summary.PrevMonthBarSales = await db.BarSaleEntries.AsNoTracking()
                        .Where(b => (b.Status == "Submitted" || b.TotalSales > 0) && (b.AdjustedSaleDate ?? b.SaleDate) >= prevStart && (b.AdjustedSaleDate ?? b.SaleDate) <= prevEnd)
                        .SumAsync(b => (decimal?)b.TotalSales) ?? 0m;

                    var prevWeeklyStats = await db.LotteryWeeklyStats.AsNoTracking()
                        .Where(w => w.WeekEndingDate >= prevStart && w.WeekEndingDate <= prevEnd)
                        .ToListAsync();

                    summary.PrevMonthLotteryCommissions = prevWeeklyStats.Sum(w =>
                        (Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission) +
                         Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus) +
                         Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus)) -
                        (Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee)));

                    summary.PrevMonthMembershipDues = await db.DuesPayments.AsNoTracking()
                        .Where(dp => dp.PaidDate.HasValue && dp.PaidDate.Value >= prevStart && dp.PaidDate.Value <= prevEnd && dp.Amount.HasValue)
                        .SumAsync(dp => dp.Amount) ?? 0m;
                }
                else
                {
                    int maxDay = endDate.Day;
                    bool isInProgressMonth = startDate.Month == DateTime.Now.Month && startDate.Year == DateTime.Now.Year;
                    if (isInProgressMonth)
                    {
                        var maxEntryDay = summary.LedgerEntries.Any() ? summary.LedgerEntries.Max(e => e.Date.Day) : 1;
                        maxDay = Math.Min(DateTime.Now.Day, Math.Max(maxEntryDay, 1));
                    }

                    summary.PrevMonthDaysCompared = maxDay;

                    DateTime currentMtdEnd = new DateTime(startDate.Year, startDate.Month, maxDay).Date.AddDays(1).AddTicks(-1);

                    if (isInProgressMonth)
                    {
                        summary.CurrentMtdBarSales = summary.LedgerEntries
                            .Where(e => e.Stream == "Bar Sales" && e.Date <= currentMtdEnd)
                            .Sum(e => e.Amount);

                        summary.CurrentMtdLotteryCommissions = summary.LedgerEntries
                            .Where(e => e.Stream == "Lottery" && e.Date <= currentMtdEnd)
                            .Sum(e => e.Amount);

                        summary.CurrentMtdMembershipDues = summary.LedgerEntries
                            .Where(e => e.Stream == "Membership Dues" && e.Date <= currentMtdEnd)
                            .Sum(e => e.Amount);
                    }
                    else
                    {
                        summary.CurrentMtdBarSales = summary.TotalBarSales;
                        summary.CurrentMtdLotteryCommissions = summary.TotalLotteryCommissions;
                        summary.CurrentMtdMembershipDues = summary.TotalMembershipDues;
                    }

                    DateTime prevStart = startDate.AddMonths(-1);
                    int daysInPrev = DateTime.DaysInMonth(prevStart.Year, prevStart.Month);
                    int targetPrevDay = Math.Min(maxDay, daysInPrev);
                    DateTime prevEnd = new DateTime(prevStart.Year, prevStart.Month, targetPrevDay).Date.AddDays(1).AddTicks(-1);

                    summary.PrevMonthBarSales = await db.BarSaleEntries.AsNoTracking()
                        .Where(b => (b.Status == "Submitted" || b.TotalSales > 0) && (b.AdjustedSaleDate ?? b.SaleDate) >= prevStart && (b.AdjustedSaleDate ?? b.SaleDate) <= prevEnd)
                        .SumAsync(b => (decimal?)b.TotalSales) ?? 0m;

                    var prevWeeklyStats = await db.LotteryWeeklyStats.AsNoTracking()
                        .Where(w => w.WeekEndingDate >= prevStart && w.WeekEndingDate <= prevEnd)
                        .ToListAsync();

                    summary.PrevMonthLotteryCommissions = prevWeeklyStats.Sum(w =>
                        (Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission) +
                         Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus) +
                         Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus)) -
                        (Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee)));

                    summary.PrevMonthMembershipDues = await db.DuesPayments.AsNoTracking()
                        .Where(dp => dp.PaidDate.HasValue && dp.PaidDate.Value >= prevStart && dp.PaidDate.Value <= prevEnd && dp.Amount.HasValue)
                        .SumAsync(dp => dp.Amount) ?? 0m;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FinancialService] Error calculating MoM MTD: {ex.Message}");
            }

            // 5. Multi-Year Comparison Calculation (if requested)
            if (comparisonYear.HasValue && comparisonYear.Value > 0)
            {
                summary.ComparisonYear = comparisonYear.Value;
                try
                {
                    int minYr = Math.Min(comparisonYear.Value, start.Year);
                    int maxYr = Math.Max(comparisonYear.Value, start.Year);

                    for (int y = minYr; y <= maxYr; y++)
                    {
                        DateTime compStart;
                        DateTime compEnd;

                        if (start.Month == 1 && end.Month == 12 && end.Day == 31)
                        {
                            compStart = new DateTime(y, 1, 1).Date;
                            compEnd = new DateTime(y, 12, 31).Date.AddDays(1).AddTicks(-1);
                        }
                        else
                        {
                            compStart = new DateTime(y, start.Month, 1).Date;
                            compEnd = compStart.AddMonths(1).AddDays(-1).Date.AddDays(1).AddTicks(-1);
                        }

                        var yrSummary = new MultiYearStreamSummaryDto { Year = y };

                        var yrBarEntries = await db.BarSaleEntries.AsNoTracking()
                            .Where(b => (b.Status == "Submitted" || b.TotalSales > 0) && (b.AdjustedSaleDate ?? b.SaleDate) >= compStart && (b.AdjustedSaleDate ?? b.SaleDate) <= compEnd)
                            .ToListAsync();

                        yrSummary.TotalBarSales = yrBarEntries.Sum(b => b.TotalSales);
                        foreach (var b in yrBarEntries)
                        {
                            int mIdx = (b.AdjustedSaleDate ?? b.SaleDate).Month - 1;
                            if (mIdx >= 0 && mIdx < 12) yrSummary.MonthlyBarSales[mIdx] += b.TotalSales;
                        }

                        var yrWeeklyStats = await db.LotteryWeeklyStats.AsNoTracking()
                            .Where(w => w.WeekEndingDate >= compStart && w.WeekEndingDate <= compEnd)
                            .ToListAsync();

                        yrSummary.TotalLotteryCommissions = yrWeeklyStats.Sum(w =>
                            (Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission) +
                             Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus) +
                             Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus)) -
                            (Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee)));

                        foreach (var w in yrWeeklyStats)
                        {
                            int mIdx = w.WeekEndingDate.Month - 1;
                            if (mIdx >= 0 && mIdx < 12)
                            {
                                decimal netLottery = (Math.Abs(w.OnlineCommission) + Math.Abs(w.InstantCommission) +
                                                     Math.Abs(w.OnlineCashBonus) + Math.Abs(w.InstantCashBonus) +
                                                     Math.Abs(w.OnlineClaimsBonus) + Math.Abs(w.InstantClaimsBonus)) -
                                                    (Math.Abs(w.OnlineServiceFee) + Math.Abs(w.OnlineBondingFee));
                                yrSummary.MonthlyLotteryCommissions[mIdx] += netLottery;
                            }
                        }

                        var yrDuesPayments = await db.DuesPayments.AsNoTracking()
                            .Where(dp => dp.PaidDate.HasValue && dp.PaidDate.Value >= compStart && dp.PaidDate.Value <= compEnd && dp.Amount.HasValue)
                            .ToListAsync();

                        yrSummary.TotalMembershipDues = yrDuesPayments.Sum(dp => dp.Amount ?? 0m);
                        foreach (var dp in yrDuesPayments)
                        {
                            int mIdx = dp.PaidDate!.Value.Month - 1;
                            if (mIdx >= 0 && mIdx < 12) yrSummary.MonthlyMembershipDues[mIdx] += dp.Amount ?? 0m;
                        }

                        summary.MultiYearSummaries[y] = yrSummary;

                        if (y == comparisonYear.Value)
                        {
                            foreach (var b in yrBarEntries)
                            {
                                summary.ComparisonLedgerEntries.Add(new IncomeStreamEntryDto
                                {
                                    Date = (b.AdjustedSaleDate ?? b.SaleDate).Date,
                                    Stream = "Bar Sales",
                                    Shift = string.IsNullOrWhiteSpace(b.Shift) ? "Day" : b.Shift,
                                    Amount = b.TotalSales
                                });
                            }
                        }
                    }

                    if (summary.MultiYearSummaries.TryGetValue(comparisonYear.Value, out var compDto))
                    {
                        summary.ComparisonBarSales = compDto.TotalBarSales;
                        summary.ComparisonLotteryCommissions = compDto.TotalLotteryCommissions;
                        summary.ComparisonMembershipDues = compDto.TotalMembershipDues;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FinancialService] Error calculating multi-year comparison: {ex.Message}");
                }
            }

            return summary;
        }

        public async Task AcknowledgeNoteAsync(string noteType, int recordId, string username)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            if (noteType == "Bar")
            {
                var entry = await db.BarSaleEntries.FindAsync(recordId);
                if (entry != null)
                {
                    entry.AcknowledgedBy = username;
                    entry.AcknowledgedAt = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
            }
            else if (noteType == "Lottery")
            {
                var shift = await db.LotteryShifts.FindAsync(recordId);
                if (shift != null)
                {
                    shift.AcknowledgedBy = username;
                    shift.AcknowledgedAt = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}



