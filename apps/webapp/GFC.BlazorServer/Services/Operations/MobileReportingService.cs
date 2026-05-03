using GFC.Core.Interfaces;
using GFC.Core.DTOs;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using GFC.Data;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Operations;

public class MobileReportingService : IMobileReportingService
{
    private readonly IDbContextFactory<GfcDbContext> _dbFactory;
    private readonly ILotteryShiftService _lottoService;
    private readonly ILotteryRateRepository _rateRepo;
    private readonly IAuditLogger _auditLogger;
    private readonly IVersionService _versionService;

    public MobileReportingService(
        IDbContextFactory<GfcDbContext> dbFactory,
        ILotteryShiftService lottoService,
        ILotteryRateRepository rateRepo,
        IAuditLogger auditLogger,
        IVersionService versionService)
    {
        _dbFactory = dbFactory;
        _lottoService = lottoService;
        _rateRepo = rateRepo;
        _auditLogger = auditLogger;
        _versionService = versionService;
    }

    public int PendingCount => 0;
    public Task<int> GetPendingCountAsync() => Task.FromResult(0);
    public event Action? OutboxChanged;

    public async Task<MobileShiftData> GetShiftReportDataAsync(DateTime date, string shiftType, bool isRental)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        
        var barEntry = await db.BarSaleEntries.AsNoTracking()
            .FirstOrDefaultAsync(e => (e.AdjustedSaleDate ?? e.SaleDate).Date == date.Date && e.Shift == shiftType && e.IsRentalHall == isRental);
            
        var lottoShifts = await Task.Run(() => _lottoService.GetShiftsByDateRange(date, date));
        var lottoEntry = lottoShifts.FirstOrDefault(s => s.ShiftType == shiftType);

        var data = new MobileShiftData
        {
            Date = date,
            ShiftType = shiftType,
            IsRentalHall = isRental,
            ExistingEntryFound = (barEntry != null || lottoEntry != null),
            BarSales = barEntry?.TotalSales,
            TotalHours = barEntry?.TotalHours,
            Notes = barEntry?.Notes ?? lottoEntry?.Notes,
            Status = lottoEntry?.Status ?? (barEntry?.Status ?? "Draft"),
            ModifiedBy = lottoEntry?.ModifiedBy ?? barEntry?.ModifiedBy
        };

        // [POS PARITY] Initialize carryover and baselines
        if (lottoEntry != null)
        {
            data.LottoOpeningCash = lottoEntry.StartingCash;
            data.LottoSales = lottoEntry.TotalSales;
            data.LottoCashes = lottoEntry.TotalPayouts;
            data.LottoInstantTickets = lottoEntry.TotalCancels;
            data.LottoNetDue = lottoEntry.NetDue;
            data.LottoBackupBag = lottoEntry.BackupBagAmount;
            data.LottoCashCounted = lottoEntry.EndingCash;
            data.Status = lottoEntry.Status;
        }
        else
        {
            data.LottoOpeningCash = await GetCarryoverCashAsync(date, shiftType);
        }

        if (shiftType == "Night")
        {
            var dayShift = lottoShifts.FirstOrDefault(s => s.ShiftType == "Day");
            if (dayShift != null)
            {
                data.PrevDaySales = dayShift.TotalSales;
                data.PrevDayCashes = dayShift.TotalPayouts;
                data.PrevDayTickets = dayShift.TotalCancels;
                data.PrevDayNetDue = dayShift.NetDue;
            }
        }
        else if (shiftType == "Day")
        {
            // [CARRYOVER FIX]: Fetch yesterday's Night shift to provide baselines for Today's Day shift
            var yesterday = await db.LotteryShifts.AsNoTracking()
                .Where(s => s.ShiftDate.Date < data.Date.Date)
                .OrderByDescending(s => s.ShiftDate).ThenByDescending(s => s.ShiftType)
                .FirstOrDefaultAsync();
            
            if (yesterday != null)
            {
                data.PrevDaySales = yesterday.TotalSales;
                data.PrevDayCashes = yesterday.TotalPayouts;
                data.PrevDayTickets = yesterday.TotalCancels;
                data.PrevDayNetDue = yesterday.NetDue;
            }
        }

        if (lottoEntry == null && barEntry != null)
        {
            data.Status = barEntry.Status;
        }


        // Time lock removed at user request to allow editing regardless of shift age
        if (data.ExistingEntryFound)
        {
            var createdBy = barEntry?.CreatedBy ?? lottoEntry?.CreatedBy;
            data.LockOwner = createdBy;
        }

        return data;
    }

    public async Task<bool> IsShiftSubmittedAsync(DateTime date, string shiftType, bool isRental)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var isSubmitted = await db.BarSaleEntries.AsNoTracking()
            .AnyAsync(e => (e.AdjustedSaleDate ?? e.SaleDate).Date == date.Date && e.Shift == shiftType && e.IsRentalHall == isRental && e.Status == "Submitted");
            
        if (!isSubmitted)
        {
            var lottoShifts = await Task.Run(() => _lottoService.GetShiftsByDateRange(date, date));
            isSubmitted = lottoShifts.Any(s => s.ShiftType == shiftType && s.Status == "Submitted");
        }
        
        return isSubmitted;
    }

    public async Task<decimal> GetCarryoverCashAsync(DateTime date, string shiftType)
    {
        var rate = await GetLotteryRateAsync(date.Year);
        var target = rate?.TargetDrawerAmount ?? 1200;

        using var db = await _dbFactory.CreateDbContextAsync();
        if (shiftType == "Night")
        {
            var day = await db.LotteryShifts.AsNoTracking().FirstOrDefaultAsync(s => s.ShiftDate.Date == date.Date && s.ShiftType == "Day");
            if (day == null || day.EndingCash <= 0) return target;
            return day.EndingCash - day.EnvelopeAmount - day.BagRefillAmount;
        }

        else
        {
            var yesterday = await db.LotteryShifts.AsNoTracking()
                .Where(s => s.ShiftDate.Date < date.Date)
                .OrderByDescending(s => s.ShiftDate).ThenByDescending(s => s.ShiftType)
                .FirstOrDefaultAsync();
            
            // [CARRYOVER RESILIENCE]: If yesterday's record is missing, return target (1200) instead of 0
            if (yesterday == null || yesterday.EndingCash <= 0) return target;
            
            var actualCash = yesterday.EndingCash - yesterday.EnvelopeAmount - yesterday.BagRefillAmount;
            return actualCash > 0 ? actualCash : target;
        }
    }

    public async Task<decimal> GetCumulativeBagDebtAsync(DateTime date)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        return await db.LotteryShifts.AsNoTracking()
            .Where(s => s.ShiftDate.Date <= date.Date && (s.Status == "Submitted" || s.Status == "Draft"))
            .SumAsync(s => s.BackupBagAmount - s.BagRefillAmount);

    }

    public async Task<bool> SaveShiftReportAsync(MobileShiftData data, string username)
    {
        // Implementation of SilentSave logic migrated from Razor
        using var db = await _dbFactory.CreateDbContextAsync();
        
        var targetDate = data.Date.Date;
        
        // [PAYROLL PROTECTION]: If the save is coming from the background outbox,
        // we must preserve the original author's identity for payroll mapping.
        string effectiveUsername = username;
        if (username == "System.Outbox" && !string.IsNullOrEmpty(data.ModifiedBy))
        {
            effectiveUsername = data.ModifiedBy;
        }

        var barEntries = await db.BarSaleEntries
            .Where(e => (e.AdjustedSaleDate ?? e.SaleDate).Date == targetDate && e.Shift == data.ShiftType && e.IsRentalHall == data.IsRentalHall)
            .ToListAsync();

        // [DE-DUPE] If multiple records exist for the same shift, use the first one and delete others
        var existingBar = barEntries.FirstOrDefault();
        if (barEntries.Count > 1) {
            var extras = barEntries.Skip(1).ToList();
            db.BarSaleEntries.RemoveRange(extras);
            Console.WriteLine($"[CLEANUP] Removed {extras.Count} duplicate BarSaleEntries for {targetDate:yyyy-MM-dd} {data.ShiftType}");
        }

        if (existingBar == null)
        {
            existingBar = new BarSaleEntry
            {
                SaleDate = data.Date,
                AdjustedSaleDate = data.Date,
                Shift = data.ShiftType,
                IsRentalHall = data.IsRentalHall,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = effectiveUsername
            };
            db.BarSaleEntries.Add(existingBar);
        }

        // [SAFE-SYNC] If the database already has a 'Submitted' record, and the tablet is sending a 'Draft',
        // we PROTECT the existing values to prevent an offline accident from wiping out a final report.
        bool protectExistingValues = (existingBar.Status == "Submitted" && data.Status == "Draft");

        if (!protectExistingValues)
        {
            existingBar.TotalSales = data.BarSales ?? 0;
            existingBar.TotalHours = data.TotalHours;
            
            // [NOTES MERGE] Don't overwrite notes if they already contain data the tablet might not have seen
            if (!string.IsNullOrEmpty(existingBar.Notes) && existingBar.Notes != data.Notes && !string.IsNullOrEmpty(data.Notes))
            {
                if (!data.Notes.Contains(existingBar.Notes))
                {
                    existingBar.Notes = existingBar.Notes + "\n---\n" + data.Notes;
                }
                else
                {
                    existingBar.Notes = data.Notes;
                }
            }
            else
            {
                existingBar.Notes = data.Notes;
            }

            existingBar.ModifiedAt = DateTime.UtcNow;
            existingBar.ModifiedBy = effectiveUsername;
            existingBar.Status = data.Status;
            existingBar.EmployeeUsername = effectiveUsername;
        }


        try
        {
            await db.SaveChangesAsync();

            if (!data.IsRentalHall && data.ShiftType != "Janitor")
            {
                // [FIX] Ensure ShiftType is never empty to prevent DB rejections
                if (string.IsNullOrEmpty(data.ShiftType)) {
                    data.ShiftType = data.IsRentalHall ? "Hall" : "Day";
                }

                var lottoShifts = await Task.Run(() => _lottoService.GetShiftsByDateRange(data.Date, data.Date));
                var lottoDto = lottoShifts.FirstOrDefault(s => s.ShiftType == data.ShiftType);
                
                LotteryShift lotto;
                if (lottoDto == null)
                {
                    lotto = new LotteryShift
                    {
                        ShiftDate = data.Date,
                        ShiftType = data.ShiftType,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = effectiveUsername
                    };
                }
                else
                {
                    lotto = await Task.Run(() => _lottoService.GetShift(lottoDto.ShiftId)) ?? new LotteryShift { ShiftDate = data.Date, ShiftType = data.ShiftType };
                }

                // [FIX] Baseline Extraction for Activity Math
                decimal baselineSales = 0, baselinePayouts = 0, baselineCancels = 0;
                if (data.ShiftType == "Night")
                {
                    var dayShift = lottoShifts.FirstOrDefault(s => s.ShiftType == "Day");
                    if (dayShift != null)
                    {
                        baselineSales = dayShift.TotalSales;
                        baselinePayouts = dayShift.TotalPayouts;
                        baselineCancels = dayShift.TotalCancels;
                    }
                }
                else if (data.ShiftType == "Day")
                {
                    // [DAY-BASELINE FIX]: Compare against yesterday's Night shift to get accurate Day-only activity
                    var yesterday = await db.LotteryShifts.AsNoTracking()
                        .Where(s => s.ShiftDate.Date < data.Date.Date)
                        .OrderByDescending(s => s.ShiftDate).ThenByDescending(s => s.ShiftType)
                        .FirstOrDefaultAsync();
                    
                    if (yesterday != null)
                    {
                        baselineSales = yesterday.TotalSales;
                        baselinePayouts = yesterday.TotalPayouts;
                        baselineCancels = yesterday.TotalCancels;
                    }
                }

                // [SAFE-SYNC] Apply values only if we aren't protecting a previously submitted report
                if (!protectExistingValues)
                {
                    lotto.StartingCash = data.LottoOpeningCash ?? 0;
                    lotto.TotalSales = data.LottoSales ?? 0;
                    lotto.TotalPayouts = data.LottoCashes ?? 0;
                    lotto.TotalCancels = data.LottoInstantTickets ?? 0;
                    lotto.NetDue = data.LottoNetDue ?? 0;
                    lotto.BackupBagAmount = data.LottoBackupBag ?? 0;
                    lotto.EndingCash = data.LottoCashCounted ?? 0;
                    lotto.EnvelopeAmount = data.EnvelopeAmount ?? 0;
                    lotto.BagRefillAmount = data.BagRefillAmount ?? 0;
                    
                    // Note merging for lotto
                    if (!string.IsNullOrEmpty(lotto.Notes) && lotto.Notes != data.Notes && !string.IsNullOrEmpty(data.Notes))
                    {
                        if (!data.Notes.Contains(lotto.Notes))
                            lotto.Notes = lotto.Notes + "\n---\n" + data.Notes;
                        else
                            lotto.Notes = data.Notes;
                    }
                    else
                    {
                        lotto.Notes = data.Notes;
                    }

                    // Preserve 'Submitted' status
                    if (!string.Equals(lotto.Status, "Submitted", StringComparison.OrdinalIgnoreCase))
                    {
                        lotto.Status = data.Status ?? "Draft";
                    }
                }

                lotto.ModifiedDate = DateTime.UtcNow;
                lotto.ModifiedBy = effectiveUsername;
                lotto.EmployeeName = effectiveUsername;

                // [MATH] Populate the activity fields so the service can calculate variance correctly
                lotto.ShiftSalesActivity = lotto.TotalSales - baselineSales;
                lotto.ShiftPayoutsActivity = lotto.TotalPayouts - baselinePayouts;
                lotto.ShiftCancelsActivity = lotto.TotalCancels - baselineCancels;

                if (lotto.ShiftId == 0) await Task.Run(() => _lottoService.CreateShift(lotto, effectiveUsername));
                else await Task.Run(() => _lottoService.UpdateShift(lotto, effectiveUsername));
            }

            return true;
        }
        catch (Exception ex)
        {
            // Log the failure but don't crash the controller
            Console.WriteLine($"[Sync Error] Failed to save shift report: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SubmitFinalReportAsync(MobileShiftData data, string username)
    {
        // Finalize and set status to 'Submitted'
        data.Status = "Submitted";
        return await SaveShiftReportAsync(data, username);
    }

    public async Task<DailyShiftSummary> GetDailySummaryAsync(DateTime date)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var summary = new DailyShiftSummary();

        // Get all bar entries for the day
        var targetDate = date.Date;
        var barEntries = await db.BarSaleEntries.AsNoTracking()
            .Where(e => (e.AdjustedSaleDate ?? e.SaleDate).Date == targetDate)
            .ToListAsync();

        // Get all lottery shifts for the day (Offload sync DB call)
        var lottoShifts = await Task.Run(() => _lottoService.GetShiftsByDateRange(date, date));

        var shifts = new[] { "Day", "Night", "Hall" };
        foreach (var shift in shifts)
        {
            bool isRental = shift == "Hall";
            // [FIX] For Hall shift, the Mobile App saves with ShiftType = "Hall".
            // Legacy system might have used "Night", but we must check "Hall" if that's what's saved.

            // [FIX] Prioritize 'Submitted' records if duplicates exist
            var bar = barEntries
                .Where(e => e.Shift == shift)
                .OrderByDescending(e => e.Status == "Submitted")
                .ThenByDescending(e => e.ModifiedAt)
                .FirstOrDefault();

            var lotto = lottoShifts
                .Where(s => s.ShiftType == shift)
                .OrderByDescending(s => s.Status == "Submitted")
                .ThenByDescending(s => s.ModifiedDate)
                .FirstOrDefault();

            var status = shift switch
            {
                "Day" => summary.Day,
                "Night" => summary.Night,
                "Hall" => summary.Hall,
                _ => null
            };

            if (status != null)
            {
                status.Submitted = (bar != null && string.Equals(bar.Status, "Submitted", StringComparison.OrdinalIgnoreCase)) || 
                                   (lotto != null && string.Equals(lotto.Status, "Submitted", StringComparison.OrdinalIgnoreCase));
                status.HasData = (bar != null || lotto != null);
                status.Closer = lotto?.ModifiedBy ?? bar?.ModifiedBy ?? lotto?.CreatedBy ?? bar?.CreatedBy;
                status.Modified = lotto?.ModifiedDate != null || bar?.ModifiedAt != null;
            }
        }

        return summary;
    }

    public Task<string> GetServerVersionAsync()
    {
        return Task.FromResult(_versionService.GetFullVersion());
    }

    public async Task<LotteryCommissionRate> GetLotteryRateAsync(int year)
    {
        return await Task.Run(() => _rateRepo.GetApplicableRate(year)) ?? new LotteryCommissionRate { Year = year };
    }

    // [INTERFACE SATISFACTION] The server-side service is the destination and does not need a local outbox.
    public Task FlushOutboxAsync() => Task.CompletedTask;
}


