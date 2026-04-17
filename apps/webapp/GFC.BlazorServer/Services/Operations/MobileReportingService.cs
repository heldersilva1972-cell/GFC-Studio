using GFC.Core.Interfaces;
using GFC.Core.DTOs;
using GFC.Core.Models;
using GFC.Data;
using GFC.BlazorServer.Data;
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
            
        var lottoShifts = _lottoRepo.GetByDateRange(date, date);
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
            return day.EndingCash;
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
            .Where(s => s.ShiftDate.Date <= date.Date && s.Status == "Submitted")
            .SumAsync(s => s.BackupBagAmount - s.BagRefillAmount);
    }

    public async Task<bool> SaveShiftReportAsync(MobileShiftData data, string username)
    {
        // Implementation of SilentSave logic migrated from Razor
        using var db = await _dbFactory.CreateDbContextAsync();
        
        var existingBar = await db.BarSaleEntries.FirstOrDefaultAsync(e => 
            (e.AdjustedSaleDate ?? e.SaleDate).Date == data.Date.Date && e.Shift == data.ShiftType && e.IsRentalHall == data.IsRentalHall);

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
        existingBar.Status = "Draft";

        await db.SaveChangesAsync();

        if (!data.IsRentalHall)
        {
            var lottoShifts = _lottoRepo.GetByDateRange(data.Date, data.Date);
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
            lotto.Notes = data.Notes;
            lotto.Status = "Draft";
            lotto.ModifiedDate = DateTime.UtcNow;
            lotto.ModifiedBy = username;
            lotto.EmployeeName = username;

            if (lotto.ShiftId == 0) _lottoRepo.Create(lotto);
            else _lottoRepo.Update(lotto);
        }

        return true;
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
        var barEntries = await db.BarSaleEntries.AsNoTracking()
            .Where(e => (e.AdjustedSaleDate ?? e.SaleDate).Date == date.Date)
            .ToListAsync();

        // Get all lottery shifts for the day
        var lottoShifts = _lottoRepo.GetByDateRange(date, date);

        var shifts = new[] { "Day", "Night", "Hall" };
        foreach (var shift in shifts)
        {
            bool isRental = shift == "Hall";
            var bar = barEntries.FirstOrDefault(e => e.Shift == (isRental ? "Night" : shift) && e.IsRentalHall == isRental);
            var lotto = lottoShifts.FirstOrDefault(s => s.ShiftType == shift);

            var status = shift switch
            {
                "Day" => summary.Day,
                "Night" => summary.Night,
                "Hall" => summary.Hall,
                _ => null
            };

            if (status != null)
            {
                status.Submitted = (bar?.Status == "Submitted" || lotto?.Status == "Submitted");
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
}
