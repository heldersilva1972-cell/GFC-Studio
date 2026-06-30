using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.Models.Finance;
using GFC.BlazorServer.Data;
using GFC.Core.DTOs;

namespace GFC.BlazorServer.Services;

public class LotterySettlementService : ILotterySettlementService
{
    private readonly IDbContextFactory<GfcDbContext> _dbFactory;
    private readonly IFinancialAnalyticsService _analyticsService;

    public LotterySettlementService(
        IDbContextFactory<GfcDbContext> dbFactory,
        IFinancialAnalyticsService analyticsService)
    {
        _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        _analyticsService = analyticsService ?? throw new ArgumentNullException(nameof(analyticsService));
    }

    public async Task<IEnumerable<LotteryWeeklySettlement>> GetWeeklySettlementsForYearAsync(int year)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

        // 1. Recalculate/Sync weekly dues for the year to ensure data is fresh
        await RecalculateWeeklyDuesAsync(year);

        // 2. Load and return all settlements for the year ordered by date descending
        var startDate = new DateTime(year, 1, 1);
        var endDate = new DateTime(year, 12, 31);

        return await db.LotteryWeeklySettlements
            .Include(s => s.LinkedBill)
            .Where(s => s.WeekStartDate >= startDate.AddDays(-7) && s.WeekEndDate <= endDate.AddDays(7))
            .OrderByDescending(s => s.WeekStartDate)
            .ToListAsync();
    }

    public async Task SettleWeekManualAsync(int settlementId, string username, string referenceNumber, DateTime settleDate)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        
        var settlement = await db.LotteryWeeklySettlements
            .Include(s => s.LinkedBill)
            .FirstOrDefaultAsync(s => s.Id == settlementId);

        if (settlement == null)
            throw new Exception("Settlement record not found.");

        // Update settlement status
        settlement.Status = "Settled";
        settlement.SettleDate = settleDate;
        settlement.SettleBy = username;
        settlement.ReferenceNumber = referenceNumber;

        // Load settings to check for bill updates
        var settings = await db.SystemSettings.FirstOrDefaultAsync();
        
        // 1. If Bill Creation is Enabled and we have a linked unpaid bill, mark it as paid
        if (settings != null && settings.LotteryBillCreationEnabled && settlement.LinkedBillId.HasValue)
        {
            var bill = await db.FinanceBills
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.Id == settlement.LinkedBillId.Value);
            if (bill != null && bill.Status != "Paid")
            {
                bill.Status = "Paid";
                
                var payment = new FinancePayment
                {
                    BillId = bill.Id,
                    AmountPaid = bill.OriginalAmount - bill.TotalPaid,
                    PaymentDate = settleDate,
                    PaymentMethod = "ACH",
                    Note = $"Manually settled via Lottery Sales by {username}. Ref: {referenceNumber}"
                };
                db.FinancePayments.Add(payment);
                
                // Add a audit log or payment if the system requires it
                var auditLog = new FinanceAuditLog
                {
                    ActionType = "PAY_BILL",
                    ActionDate = DateTime.Now,
                    Description = $"Lottery bill #{bill.Id} auto-paid via manual settlement. Ref: {referenceNumber}",
                    PerformedBy = username
                };
                db.FinanceAuditLogs.Add(auditLog);
            }
        }
        // 2. If Record Paid History is Enabled (and Bill Creation is OFF), create a pre-paid bill in history
        else if (settings != null && settings.LotteryRecordPaidHistoryEnabled && !settings.LotteryBillCreationEnabled)
        {
            var vendorId = await GetOrCreateLotteryVendorIdAsync(db);
            var categoryId = settings.LotteryDefaultCategoryId ?? await GetOrCreateLotteryCategoryIdAsync(db);

            var paidBill = new FinanceBill
            {
                VendorId = vendorId,
                CategoryId = categoryId,
                OriginalAmount = settlement.NetDueAmount,
                DueDate = settlement.WeekEndDate.AddDays(6), // Following Friday
                Status = "Paid",
                Description = $"Auto-generated paid record from Lottery Sales by {username}. Ref: {referenceNumber}",
                CreatedAt = DateTime.UtcNow
            };
 
            db.FinanceBills.Add(paidBill);
            await db.SaveChangesAsync(); // Save to get the paidBill.Id
 
            var payment = new FinancePayment
            {
                BillId = paidBill.Id,
                AmountPaid = settlement.NetDueAmount,
                PaymentDate = settleDate,
                PaymentMethod = "ACH",
                Note = $"Weekly lottery sweep settlement. Ref: {referenceNumber}"
            };
            db.FinancePayments.Add(payment);

            settlement.LinkedBillId = paidBill.Id;
        }

        await db.SaveChangesAsync();
    }

    public async Task SyncWeeklyBillsAsync()
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var settings = await db.SystemSettings.FirstOrDefaultAsync();
        
        // Only run if Bill Creation is enabled
        if (settings == null || !settings.LotteryBillCreationEnabled)
            return;

        // Get the last enabled timestamp (we only create bills for weeks ending after this date)
        var lastEnabled = settings.LotterySettingsLastEnabledUtc ?? DateTime.UtcNow.AddDays(-7);

        // Find all pending settlements that don't have a linked bill yet, ended after the last enabled date, and are fully completed (ended on or before today)
        var today = DateTime.Today;
        var pendingSettlements = await db.LotteryWeeklySettlements
            .Where(s => s.LinkedBillId == null && s.WeekEndDate >= lastEnabled && s.WeekEndDate <= today)
            .ToListAsync();

        if (!pendingSettlements.Any())
            return;

        var vendorId = await GetOrCreateLotteryVendorIdAsync(db);
        var categoryId = settings.LotteryDefaultCategoryId ?? await GetOrCreateLotteryCategoryIdAsync(db);

        foreach (var settlement in pendingSettlements)
        {
            // Create a new unpaid bill
            var bill = new FinanceBill
            {
                VendorId = vendorId,
                CategoryId = categoryId,
                OriginalAmount = settlement.NetDueAmount,
                DueDate = settlement.WeekEndDate.AddDays(6), // Following Friday
                Status = "Pending",
                Description = $"Weekly lottery sweep. Week ending {settlement.WeekEndDate:MM/dd/yyyy}",
                CreatedAt = DateTime.UtcNow
            };

            db.FinanceBills.Add(bill);
            await db.SaveChangesAsync(); // Save to generate Id

            settlement.LinkedBillId = bill.Id;
        }

        await db.SaveChangesAsync();
    }

    public async Task RecalculateWeeklyDuesAsync(int year)
    {
        using var db = await _dbFactory.CreateDbContextAsync();

        // 1. Generate the standard lottery weeks for the year (Sunday to Saturday)
        var weeks = GetLotteryWeeks(year);
        var correctStartDates = weeks.Select(w => w.Start).ToList();

        // 0. Clean up legacy Saturday-to-Friday pending weekly settlements
        var legacyPending = await db.LotteryWeeklySettlements
            .Where(s => s.WeekStartDate.Year == year && s.Status == "Pending" && !correctStartDates.Contains(s.WeekStartDate))
            .ToListAsync();
        if (legacyPending.Any())
        {
            db.LotteryWeeklySettlements.RemoveRange(legacyPending);
            await db.SaveChangesAsync();
        }

        // 2. Fetch daily shift data for the entire year in a single query
        var yearStart = weeks.Min(w => w.Start);
        var yearEnd = weeks.Max(w => w.End);
        
        var (dailyReports, _, _, _, _, _) = await _analyticsService.GetDailySalesReportsAsync(yearStart, yearEnd);

        // Group daily reports by date for fast lookup
        var dailyReportsLookup = dailyReports.ToDictionary(r => r.Date.Date, r => r);

        // 3. For each week, calculate the net due using only night shifts
        foreach (var week in weeks)
        {
            decimal weeklyNetDue = 0;
            decimal weeklyEnvelopeDrop = 0;
            bool hasShifts = false;
            int recordedShiftsCount = 0;

            // Iterate over each day in the Saturday to Friday week
            for (var date = week.Start; date <= week.End; date = date.AddDays(1))
            {
                if (dailyReportsLookup.TryGetValue(date.Date, out var dayReport))
                {
                    // Find the Night shift for this day
                    var nightShift = dayReport.Shifts.FirstOrDefault(s => s.ShiftType == "Night");
                    if (nightShift != null)
                    {
                        weeklyNetDue += nightShift.LottoNetDue;
                        weeklyEnvelopeDrop += nightShift.EnvelopeAmount;
                        hasShifts = true;
                        recordedShiftsCount++;
                    }
                }
            }

            // Only save/update if we actually have shift data for this week
            if (hasShifts)
            {
                var settlement = await db.LotteryWeeklySettlements
                    .FirstOrDefaultAsync(s => s.WeekStartDate == week.Start && s.WeekEndDate == week.End);

                if (settlement == null)
                {
                    settlement = new LotteryWeeklySettlement
                    {
                        WeekStartDate = week.Start,
                        WeekEndDate = week.End,
                        NetDueAmount = weeklyNetDue,
                        RecordedShiftsCount = recordedShiftsCount,
                        EnvelopeDropAmount = weeklyEnvelopeDrop,
                        Status = "Pending"
                    };
                    db.LotteryWeeklySettlements.Add(settlement);
                }
                else
                {
                    // Update the amount if it is not yet settled
                    if (settlement.Status != "Settled")
                    {
                        settlement.NetDueAmount = weeklyNetDue;
                        settlement.RecordedShiftsCount = recordedShiftsCount;
                        settlement.EnvelopeDropAmount = weeklyEnvelopeDrop;
                    }
                }
            }
        }

        await db.SaveChangesAsync();
        
        // After recalculating, sync any new bills if enabled
        await SyncWeeklyBillsAsync();
    }

    private static List<(DateTime Start, DateTime End)> GetLotteryWeeks(int year)
    {
        var weeks = new List<(DateTime Start, DateTime End)>();
        var startOfWeeks = new DateTime(year, 1, 1);
        
        // Start at the first Sunday of the year, or the last Sunday of the previous year
        while (startOfWeeks.DayOfWeek != DayOfWeek.Sunday)
        {
            startOfWeeks = startOfWeeks.AddDays(-1);
        }

        var current = startOfWeeks;
        // Make sure we capture all weeks touching this year
        while (current.Year == year || current.AddDays(6).Year == year)
        {
            weeks.Add((current, current.AddDays(6))); // Sunday to Saturday
            current = current.AddDays(7);
        }
        return weeks;
    }

    private static async Task<int> GetOrCreateLotteryVendorIdAsync(GfcDbContext db)
    {
        var vendor = await db.FinanceVendors.FirstOrDefaultAsync(v => v.Name == "State Lottery");
        if (vendor != null)
            return vendor.Id;

        vendor = new FinanceVendor
        {
            Name = "State Lottery",
            Email = "lottery@state.gov",
            Phone = "1-800-LOTTERY",
            ContactInfo = "Auto-generated vendor for weekly lottery sweeps."
        };
        db.FinanceVendors.Add(vendor);
        await db.SaveChangesAsync();
        return vendor.Id;
    }

    private static async Task<int> GetOrCreateLotteryCategoryIdAsync(GfcDbContext db)
    {
        var category = await db.FinanceCategories.FirstOrDefaultAsync(c => c.Name == "Lottery Payments");
        if (category != null)
            return category.Id;

        category = new FinanceCategory
        {
            Name = "Lottery Payments"
        };
        db.FinanceCategories.Add(category);
        await db.SaveChangesAsync();
        return category.Id;
    }
}
