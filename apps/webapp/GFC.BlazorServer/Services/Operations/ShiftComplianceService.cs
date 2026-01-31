using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GFC.Core.Models;
using GFC.Core.Interfaces;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services.Operations;

public class ShiftComplianceService : IShiftComplianceService
{
    private readonly IDbContextFactory<GfcDbContext> _dbFactory;
    private readonly ILotteryShiftRepository _lotteryRepo;

    public ShiftComplianceService(IDbContextFactory<GfcDbContext> dbFactory, ILotteryShiftRepository lotteryRepo)
    {
        _dbFactory = dbFactory;
        _lotteryRepo = lotteryRepo;
    }

    public async Task<List<MissingShiftAlert>> GetMissingShiftsAsync(int lookbackDays = 2)
    {
        var missingAlerts = new List<MissingShiftAlert>();
        var now = DateTime.Now;
        
        // Define Business Day Rollover (5 AM)
        var businessToday = now.Hour < 5 ? DateTime.Today.AddDays(-1) : DateTime.Today;
        
        using var db = await _dbFactory.CreateDbContextAsync();

        for (int i = 0; i <= lookbackDays; i++)
        {
            var checkDate = businessToday.AddDays(-i);
            
            // For each day, we check for "Day" and "Night"
            await CheckShift(db, checkDate, "Day", missingAlerts);
            await CheckShift(db, checkDate, "Night", missingAlerts);
        }

        return missingAlerts.OrderByDescending(a => a.Date).ThenBy(a => a.ShiftType == "Night" ? 1 : 0).ToList();
    }

    private async Task CheckShift(GfcDbContext db, DateTime date, string shiftType, List<MissingShiftAlert> alerts)
    {
        // Don't alert for "Future" shifts or the "Current" shift if it hasn't ended yet
        if (!IsShiftCompleted(date, shiftType)) return;

        // Check Bar Sales
        var hasBar = await db.BarSaleEntries.AnyAsync(e => 
            (e.AdjustedSaleDate ?? e.SaleDate).Date == date.Date && e.Shift == shiftType);

        // Check Lottery Sales
        var lottoShifts = _lotteryRepo.GetByDateRange(date, date);
        var hasLotto = lottoShifts.Any(s => s.ShiftType == shiftType);

        if (!hasBar || !hasLotto)
        {
            string missingWhat = (!hasBar && !hasLotto) ? "Report" : (!hasBar ? "Bar Sales" : "Lottery Sales");
            alerts.Add(new MissingShiftAlert(
                date, 
                shiftType, 
                $"{date:MMM dd} ({shiftType}) missing {missingWhat}"));
        }
    }

    private bool IsShiftCompleted(DateTime date, string shiftType)
    {
        var now = DateTime.Now;
        // Use 5 AM as business day rollover
        var businessToday = now.Hour < 5 ? DateTime.Today.AddDays(-1) : DateTime.Today;

        if (date < businessToday) return true;

        if (date == businessToday)
        {
            // If it's Day shift, it ends based on configured time (+ 2 hour buffer for reporting)
            if (shiftType == "Day")
            {
                var dayEndTime = ShiftDefaults.DayEnd.Hours == 0 ? 24 : ShiftDefaults.DayEnd.Hours;
                return now.Hour >= (dayEndTime + 2); 
            }
            
            // If it's Night shift, it's only completed once business day rolls over
            if (shiftType == "Night")
            {
                return false; 
            }
        }

        return false;
    }
}
