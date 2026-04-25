using GFC.BlazorServer.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models.Finance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;

        public FinanceService(IDbContextFactory<GfcDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        #region Bill Management

        public async Task<IEnumerable<FinanceBill>> GetBillsAsync(int month, int year)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            // Logic:
            // 1. Show everything due in the VIEWED month (Paid or Unpaid)
            // 2. Show everything UNPAID from the PAST (Arrears)
            // 3. Show everything UNPAID in the FUTURE but only if within 14 days of TODAY
            var today = DateTime.Today;
            var upcomingLimit = today.AddDays(14);

            return await db.FinanceBills
                .Include(b => b.Vendor)
                .Include(b => b.Category)
                .Include(b => b.Payments)
                .Where(b => (b.DueDate >= startOfMonth && b.DueDate <= endOfMonth) || 
                            ((b.Status == null || b.Status != "Paid") && b.DueDate < startOfMonth) ||
                            ((b.Status == null || b.Status != "Paid") && b.DueDate > endOfMonth && b.DueDate <= upcomingLimit))
                .OrderBy(b => b.DueDate)
                .ToListAsync();
        }

        public async Task<FinanceBill?> GetBillByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinanceBills
                .Include(b => b.Vendor)
                .Include(b => b.Category)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<FinanceBill> CreateBillAsync(FinanceBill bill)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.FinanceBills.Add(bill);
            await db.SaveChangesAsync();
            return bill;
        }

        public async Task UpdateBillAsync(FinanceBill bill)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.FinanceBills.FindAsync(bill.Id);
            if (existing != null)
            {
                db.Entry(existing).CurrentValues.SetValues(bill);
                existing.UpdatedAt = DateTime.Now;
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteBillAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var bill = await db.FinanceBills.FindAsync(id);
            if (bill != null)
            {
                db.FinanceBills.Remove(bill);
                await db.SaveChangesAsync();
            }
        }

        public async Task MarkAsPaidAsync(int billId, decimal amount, DateTime date, string? method = null, string? note = null, int? userId = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var bill = await db.FinanceBills
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.Id == billId);

            if (bill == null) throw new Exception("Bill not found.");

            var payment = new FinancePayment
            {
                BillId = billId,
                AmountPaid = amount,
                PaymentDate = date,
                PaymentMethod = method,
                Note = note,
                ProcessedBy = userId
            };

            db.FinancePayments.Add(payment);
            await db.SaveChangesAsync();

            // Refresh total paid to decide status
            var totalPaid = bill.Payments.Sum(p => p.AmountPaid);
            if (totalPaid >= bill.OriginalAmount)
            {
                bill.Status = BillStatus.Paid.ToString();
                
                // Handle Recurring Logic (Option A: Auto-generate next)
                if (bill.IsRecurring && !string.IsNullOrEmpty(bill.RecurringFrequency))
                {
                    await GenerateNextRecurringInstance(db, bill);
                }
            }
            else
            {
                bill.Status = BillStatus.Partial.ToString();
            }

            bill.UpdatedAt = DateTime.Now;
            await db.SaveChangesAsync();
        }

        private async Task GenerateNextRecurringInstance(GfcDbContext db, FinanceBill currentBill)
        {
            // Check if a future instance already exists for this vendor/frequency to avoid duplicates
            DateTime nextDueDate = CalculateNextDate(currentBill.DueDate, currentBill.RecurringFrequency!);
            
            bool alreadyExists = await db.FinanceBills.AnyAsync(b => 
                b.VendorId == currentBill.VendorId && 
                b.DueDate == nextDueDate && 
                b.IsRecurring);

            if (!alreadyExists)
            {
                var nextBill = new FinanceBill
                {
                    VendorId = currentBill.VendorId,
                    CategoryId = currentBill.CategoryId,
                    Description = currentBill.Description,
                    OriginalAmount = currentBill.OriginalAmount,
                    DueDate = nextDueDate,
                    WarningDays = currentBill.WarningDays,
                    IsRecurring = true,
                    RecurringFrequency = currentBill.RecurringFrequency,
                    Status = BillStatus.Pending.ToString()
                };
                db.FinanceBills.Add(nextBill);
            }
        }

        private DateTime CalculateNextDate(DateTime current, string frequency)
        {
            return frequency.ToLower() switch
            {
                "weekly" => current.AddDays(7),
                "biweekly" => current.AddDays(14),
                "monthly" => current.AddMonths(1),
                "quarterly" => current.AddMonths(3),
                "yearly" => current.AddYears(1),
                _ => current.AddMonths(1)
            };
        }

        public async Task ToggleDisputeAsync(int billId, string? note = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var bill = await db.FinanceBills.FindAsync(billId);
            if (bill != null)
            {
                if (bill.Status == BillStatus.Disputed.ToString())
                {
                    // Revert to Pending or Partial
                    var totalPaid = await db.FinancePayments.Where(p => p.BillId == billId).SumAsync(p => p.AmountPaid);
                    bill.Status = totalPaid > 0 ? BillStatus.Partial.ToString() : BillStatus.Pending.ToString();
                }
                else
                {
                    bill.Status = BillStatus.Disputed.ToString();
                    if (!string.IsNullOrEmpty(note))
                    {
                        // Append note to description or handle as a separate log later
                        bill.Description = $"[DISPUTED: {note}] " + bill.Description;
                    }
                }
                await db.SaveChangesAsync();
            }
        }

        #endregion

        #region Vendor Management

        public async Task<IEnumerable<FinanceVendor>> GetAllVendorsAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinanceVendors
                .Where(v => v.IsActive)
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        public async Task<FinanceVendor?> GetVendorByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinanceVendors
                .Include(v => v.Bills.OrderByDescending(b => b.DueDate).Take(10))
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<FinanceVendor> CreateVendorAsync(FinanceVendor vendor)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.FinanceVendors.Add(vendor);
            await db.SaveChangesAsync();
            return vendor;
        }

        public async Task UpdateVendorAsync(FinanceVendor vendor)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.FinanceVendors.FindAsync(vendor.Id);
            if (existing != null)
            {
                db.Entry(existing).CurrentValues.SetValues(vendor);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteVendorAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var vendor = await db.FinanceVendors.FindAsync(id);
            if (vendor != null)
            {
                vendor.IsActive = false;
                await db.SaveChangesAsync();
            }
        }

        #endregion

        #region Category Management

        public async Task<IEnumerable<FinanceCategory>> GetAllCategoriesAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinanceCategories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<FinanceCategory> CreateCategoryAsync(FinanceCategory category)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.FinanceCategories.Add(category);
            await db.SaveChangesAsync();
            return category;
        }

        public async Task UpdateCategoryAsync(FinanceCategory category)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.FinanceCategories.FindAsync(category.Id);
            if (existing != null)
            {
                db.Entry(existing).CurrentValues.SetValues(category);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var category = await db.FinanceCategories.FindAsync(id);
            if (category != null)
            {
                category.IsActive = false;
                await db.SaveChangesAsync();
            }
        }

        #endregion

        #region Analytics

        public async Task<decimal> GetTotalDueForMonthAsync(int month, int year)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
            var today = DateTime.Today;
            var upcomingLimit = today.AddDays(14);

            // Calculate total due by summing the REMAINING BALANCE of all unpaid/partial bills in the window
            var query = db.FinanceBills
                .Include(b => b.Payments)
                .Where(b => (b.Status == null || b.Status != "Paid") && 
                            ((b.DueDate >= startOfMonth && b.DueDate <= endOfMonth) || 
                             (b.DueDate < startOfMonth) ||
                             (b.DueDate > endOfMonth && b.DueDate <= upcomingLimit)));

            var bills = await query.ToListAsync();
            return bills.Sum(b => b.BalanceRemaining);
        }

        public async Task<decimal> GetTotalPaidForMonthAsync(int month, int year)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            return await db.FinancePayments
                .Where(p => p.PaymentDate >= startOfMonth && p.PaymentDate <= endOfMonth)
                .SumAsync(p => p.AmountPaid);
        }

        public async Task<int> GetUpcomingAlertCountAsync(int days)
        {
            using var context = _dbFactory.CreateDbContext();
            var today = DateTime.Today;
            var threshold = today.AddDays(days);
            
            // Count everything that is NOT paid and is either OVERDUE or DUE SOON
            return await context.FinanceBills
                .CountAsync(b => b.Status != BillStatus.Paid.ToString() 
                            && b.DueDate.Date <= threshold);
        }

        #endregion
    }
}
