using GFC.Core.Interfaces;
using GFC.Core.DTOs;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using GFC.Data;
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Operations;

public class MobileReportingService : IMobileReportingService
{
    private readonly IDbContextFactory<GfcDbContext> _dbFactory;
    private readonly ILotteryShiftRepository _lottoRepo;
    private readonly IAuditLogger _auditLogger;
    private readonly IVersionService _versionService;

    public MobileReportingService(
        IDbContextFactory<GfcDbContext> dbFactory,
        ILotteryShiftRepository lottoRepo,
        IAuditLogger auditLogger,
        IVersionService versionService)
    {
        _dbFactory = dbFactory;
        _lottoRepo = lottoRepo;
        _auditLogger = auditLogger;
        _versionService = versionService;
    }

    public async Task<MobileShiftData> GetShiftReportDataAsync(DateTime date, string shiftType, bool isRental)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        
        var barEntry = await db.BarSaleEntries.AsNoTracking()
            .FirstOrDefaultAsync(e => (e.AdjustedSaleDate ?? e.SaleDate).Date == date.Date && e.Shift == shiftType && e.IsRentalHall == isRental);
            
        var lottoShifts = await Task.Run(() => _lottoRepo.GetByDateRange(date, date));
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

        if (lottoEntry != null)
        {
            data.LottoSales = lottoEntry.TotalSales;
            data.LottoCashes = lottoEntry.TotalPayouts;
            data.LottoInstantTickets = lottoEntry.TotalCancels;
            data.LottoNetDue = lottoEntry.NetDue;
            data.LottoBackupBag = lottoEntry.BackupBagAmount;
            data.LottoCashCounted = lottoEntry.EndingCash;
            data.LottoOpeningCash = await GetCarryoverCashAsync(date, shiftType);

            if (shiftType == "Night")
            {
                var dayShift = lottoShifts.FirstOrDefault(s => s.ShiftType == "Day");
                if (dayShift != null)
                {
                    data.PrevDaySales = dayShift.TotalSales;
                    data.PrevDayCashes = dayShift.TotalPayouts;
                    data.PrevDayTickets = dayShift.TotalCancels;
                }
            }
            data.Status = lottoEntry.Status;
        }
        else if (barEntry != null)
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

    public async Task<decimal> GetCarryoverCashAsync(DateTime date, string shiftType)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        if (shiftType == "Night")
        {
            var day = await db.LotteryShifts.AsNoTracking().FirstOrDefaultAsync(s => s.ShiftDate.Date == date.Date && s.ShiftType == "Day");
            if (day == null || day.EndingCash <= 0) return 1200;
            return day.EndingCash - day.EnvelopeAmount - day.BagRefillAmount;
        }

        else
        {
            var yesterday = await db.LotteryShifts.AsNoTracking()
                .Where(s => s.ShiftDate.Date < date.Date)
                .OrderByDescending(s => s.ShiftDate).ThenByDescending(s => s.ShiftType)
                .FirstOrDefaultAsync();
            
            if (yesterday == null || yesterday.EndingCash <= 0) return 1200;
            return Math.Min(1200, yesterday.EndingCash - yesterday.EnvelopeAmount - yesterday.BagRefillAmount);
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
                CreatedBy = username
            };
            db.BarSaleEntries.Add(existingBar);
        }

        existingBar.TotalSales = data.BarSales ?? 0;
        existingBar.TotalHours = data.TotalHours;
        existingBar.Notes = data.Notes;
        existingBar.ModifiedAt = DateTime.UtcNow;
        existingBar.ModifiedBy = username;
        existingBar.Status = data.Status;


        try
        {
            await db.SaveChangesAsync();

            if (!data.IsRentalHall)
            {
        // [FIX] Ensure ShiftType is never empty to prevent DB rejections
        if (string.IsNullOrEmpty(data.ShiftType)) {
            data.ShiftType = data.IsRentalHall ? "Hall" : "Day";
        }

        var lottoShifts = await Task.Run(() => _lottoRepo.GetByDateRange(data.Date, data.Date));
        var lotto = lottoShifts.FirstOrDefault(s => s.ShiftType == data.ShiftType);
                
                if (lotto == null)
                {
                    lotto = new LotteryShift
                    {
                        ShiftDate = data.Date,
                        ShiftType = data.ShiftType,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = username
                    };
                }

                lotto.TotalSales = data.LottoSales ?? 0;
                lotto.TotalPayouts = data.LottoCashes ?? 0;
                lotto.TotalCancels = data.LottoInstantTickets ?? 0;
                lotto.NetDue = data.LottoNetDue ?? 0;
                lotto.BackupBagAmount = data.LottoBackupBag ?? 0;
                lotto.EndingCash = data.LottoCashCounted ?? 0;
                lotto.EnvelopeAmount = data.EnvelopeAmount ?? 0;
                lotto.BagRefillAmount = data.BagRefillAmount ?? 0;
                lotto.Notes = data.Notes;
                lotto.Status = data.Status;

                lotto.ModifiedDate = DateTime.UtcNow;
                lotto.ModifiedBy = username;
                lotto.EmployeeName = username;

                if (lotto.ShiftId == 0) await Task.Run(() => _lottoRepo.Create(lotto));
                else await Task.Run(() => _lottoRepo.Update(lotto));
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
        var lottoShifts = await Task.Run(() => _lottoRepo.GetByDateRange(date, date));

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

    // [INTERFACE SATISFACTION] The server-side service is the destination and does not need a local outbox.
    public Task FlushOutboxAsync() => Task.CompletedTask;
    public event Action? OutboxChanged;
}


