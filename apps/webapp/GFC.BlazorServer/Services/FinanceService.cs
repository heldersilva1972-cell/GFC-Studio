using GFC.BlazorServer.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models.Finance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var sw = Stopwatch.StartNew();
            Console.WriteLine($"[FINANCE] GetBillsAsync START: {month}/{year}");
            
            using var db = await _dbFactory.CreateDbContextAsync();
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var upcomingLimit = endOfMonth.AddDays(7);

            try
            {
                var results = await db.FinanceBills
                    .AsNoTracking()
                    .AsSplitQuery() // [OPTIMIZATION] Prevents slow Cartesian product joins
                    .Include(b => b.Vendor)
                    .Include(b => b.Category)
                    .Include(b => b.Payments)
                    .Where(b => (b.DueDate >= startOfMonth && b.DueDate <= endOfMonth) || 
                                (b.Status != "Paid" && b.DueDate < startOfMonth) ||
                                (b.Status != "Paid" && b.DueDate > endOfMonth && b.DueDate <= upcomingLimit))
                    .OrderBy(b => b.DueDate)
                    .ToListAsync();
                
                sw.Stop();
                Console.WriteLine($"[FINANCE] GetBillsAsync FINISHED in {sw.ElapsedMilliseconds}ms. Found {results.Count} bills.");
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FINANCE] GetBillsAsync ERROR after {sw.ElapsedMilliseconds}ms: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<FinanceBill>> GetBillsForYearAsync(int year)
        {
            var sw = Stopwatch.StartNew();
            using var db = await _dbFactory.CreateDbContextAsync();
            var startOfYear = new DateTime(year, 1, 1);
            var endOfYear = new DateTime(year, 12, 31);

            var results = await db.FinanceBills
                .AsNoTracking()
                .AsSplitQuery() // [OPTIMIZATION]
                .Include(b => b.Vendor)
                .Include(b => b.Category)
                .Include(b => b.Payments)
                .Where(b => b.DueDate >= startOfYear && b.DueDate <= endOfYear)
                .OrderBy(b => b.DueDate)
                .ToListAsync();

            sw.Stop();
            Console.WriteLine($"[FINANCE] GetBillsForYearAsync ({year}) FINISHED in {sw.ElapsedMilliseconds}ms.");
            return results;
        }

        public async Task<IEnumerable<FinanceBill>> GetBillsForReportAsync(int month, int year)
        {
            var sw = Stopwatch.StartNew();
            using var db = await _dbFactory.CreateDbContextAsync();
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            // [REPORT LOGIC] Only include bills DUE in this month OR bills PAID in this month
            var results = await db.FinanceBills
                .AsNoTracking()
                .AsSplitQuery()
                .Include(b => b.Vendor)
                .Include(b => b.Category)
                .Include(b => b.Payments)
                .Where(b => (b.DueDate >= startOfMonth && b.DueDate <= endOfMonth) || 
                            b.Payments.Any(p => p.PaymentDate >= startOfMonth && p.PaymentDate <= endOfMonth))
                .OrderBy(b => b.DueDate)
                .ToListAsync();

            sw.Stop();
            Console.WriteLine($"[FINANCE] GetBillsForReportAsync ({month}/{year}) FINISHED in {sw.ElapsedMilliseconds}ms.");
            return results;
        }

        public async Task<FinanceBill?> GetBillByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinanceBills
                .AsNoTracking()
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
                LoanId = bill.LoanId,
                AmountPaid = amount,
                PaymentDate = date,
                PaymentMethod = method,
                Note = note,
                ProcessedBy = userId
            };

            db.FinancePayments.Add(payment);

            if (bill.LoanId != null)
            {
                var loan = await db.FinanceLoans.FindAsync(bill.LoanId.Value);
                if (loan != null)
                {
                    if (amount > loan.CurrentBalance)
                    {
                        throw new Exception($"Payment amount cannot exceed the remaining loan balance of {loan.CurrentBalance:C}.");
                    }
                    loan.CurrentBalance -= amount;
                    if (loan.CurrentBalance <= 0)
                    {
                        loan.CurrentBalance = 0;
                        loan.IsActive = false;
                    }
                }
            }

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
                    LoanId = currentBill.LoanId,
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

        public Task<IEnumerable<FinanceVendor>> GetAllVendorsAsync()
        {
            return GetAllVendorsAsync(false);
        }

        public async Task<IEnumerable<FinanceVendor>> GetAllVendorsAsync(bool includeInactive)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var query = db.FinanceVendors
                .AsNoTracking()
                .Include(v => v.DefaultCategory)
                .AsQueryable();

            if (!includeInactive)
            {
                query = query.Where(v => v.IsActive);
            }

            return await query.OrderBy(v => v.Name).ToListAsync();
        }

        public async Task<FinanceVendor?> GetVendorByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinanceVendors
                .Include(v => v.DefaultCategory)
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

        public async Task<string?> GetLastPaymentMethodForVendorAsync(int vendorId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinancePayments
                .AsNoTracking()
                .Where(p => p.BillId != null && p.Bill != null && p.Bill.VendorId == vendorId)
                .OrderByDescending(p => p.PaymentDate)
                .ThenByDescending(p => p.Id)
                .Select(p => p.PaymentMethod)
                .FirstOrDefaultAsync();
        }

        #endregion

        #region Category Management

        public async Task<IEnumerable<FinanceCategory>> GetAllCategoriesAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinanceCategories
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
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
                .AsNoTracking()
                .Where(p => p.PaymentDate >= startOfMonth && p.PaymentDate <= endOfMonth)
                .SumAsync(p => p.AmountPaid);
        }

        public async Task<int> GetUpcomingAlertCountAsync(int days)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var today = DateTime.Today;
            var threshold = today.AddDays(days);
            
            return await context.FinanceBills
                .AsNoTracking()
                .CountAsync(b => b.Status != "Paid" 
                            && b.DueDate.Date <= threshold);
        }

        #endregion

        #region Loan Management

        public async Task<IEnumerable<FinanceLoan>> GetAllLoansAsync(bool includeInactive = false)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var query = db.FinanceLoans
                .AsNoTracking()
                .Include(l => l.Payments)
                .AsQueryable();

            if (!includeInactive)
            {
                query = query.Where(l => l.IsActive);
            }

            return await query.OrderByDescending(l => l.OriginDate).ToListAsync();
        }

        public async Task<FinanceLoan?> GetLoanByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinanceLoans
                .AsNoTracking()
                .Include(l => l.Payments.OrderByDescending(p => p.PaymentDate))
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<FinanceLoan> CreateLoanAsync(FinanceLoan loan)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            loan.CreatedAt = DateTime.Now;
            loan.CurrentBalance = loan.OriginalBalance;
            db.FinanceLoans.Add(loan);
            await db.SaveChangesAsync();
            return loan;
        }

        public async Task UpdateLoanAsync(FinanceLoan loan)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.FinanceLoans.FindAsync(loan.Id);
            if (existing != null)
            {
                loan.CreatedAt = existing.CreatedAt;
                db.Entry(existing).CurrentValues.SetValues(loan);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteLoanAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var loan = await db.FinanceLoans.Include(l => l.Payments).FirstOrDefaultAsync(l => l.Id == id);
            if (loan != null)
            {
                if (loan.Payments != null && loan.Payments.Any())
                {
                    db.FinancePayments.RemoveRange(loan.Payments);
                }
                db.FinanceLoans.Remove(loan);
                await db.SaveChangesAsync();
            }
        }

        public async Task RecordLoanPaymentAsync(int loanId, decimal amount, DateTime date, string? method = null, string? note = null, int? userId = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var loan = await db.FinanceLoans.FindAsync(loanId);
            if (loan == null) throw new Exception("Loan not found.");

            if (amount > loan.CurrentBalance)
            {
                throw new Exception($"Payment amount cannot exceed the remaining loan balance of {loan.CurrentBalance:C}.");
            }

            var payment = new FinancePayment
            {
                LoanId = loanId,
                BillId = null,
                AmountPaid = amount,
                PaymentDate = date,
                PaymentMethod = method,
                Note = string.IsNullOrWhiteSpace(note) ? $"Loan Payment: {loan.LenderName}" : $"[Loan Payment: {loan.LenderName}] {note}",
                ProcessedBy = userId
            };

            db.FinancePayments.Add(payment);
            
            loan.CurrentBalance -= amount;
            if (loan.CurrentBalance <= 0)
            {
                loan.CurrentBalance = 0;
                loan.IsActive = false;
            }

            await db.SaveChangesAsync();
        }

        public async Task<FinancePayment?> GetPaymentByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinancePayments
                .AsNoTracking()
                .Include(p => p.Bill)
                .Include(p => p.Loan)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdatePaymentAsync(FinancePayment payment)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.FinancePayments.FindAsync(payment.Id);
            if (existing == null) throw new Exception("Payment not found.");

            db.Entry(existing).CurrentValues.SetValues(payment);
            await db.SaveChangesAsync();

            // Recalculate bill if applicable
            if (payment.BillId != null)
            {
                var bill = await db.FinanceBills
                    .Include(b => b.Payments)
                    .FirstOrDefaultAsync(b => b.Id == payment.BillId);
                if (bill != null)
                {
                    var totalPaid = bill.Payments.Sum(p => p.AmountPaid);
                    if (totalPaid >= bill.OriginalAmount)
                    {
                        bill.Status = BillStatus.Paid.ToString();
                    }
                    else if (totalPaid > 0)
                    {
                        bill.Status = BillStatus.Partial.ToString();
                    }
                    else
                    {
                        bill.Status = BillStatus.Pending.ToString();
                    }
                    bill.UpdatedAt = DateTime.Now;
                    await db.SaveChangesAsync();
                }
            }

            // Recalculate loan if applicable
            if (payment.LoanId != null)
            {
                var loan = await db.FinanceLoans
                    .Include(l => l.Payments)
                    .FirstOrDefaultAsync(l => l.Id == payment.LoanId);
                if (loan != null)
                {
                    var totalPaid = loan.Payments.Sum(p => p.AmountPaid);
                    loan.CurrentBalance = loan.OriginalBalance - totalPaid;
                    if (loan.CurrentBalance <= 0)
                    {
                        loan.CurrentBalance = 0;
                        loan.IsActive = false;
                    }
                    else
                    {
                        loan.IsActive = true;
                    }
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task DeletePaymentAsync(int paymentId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var payment = await db.FinancePayments.FindAsync(paymentId);
            if (payment == null) return;

            var billId = payment.BillId;
            var loanId = payment.LoanId;

            db.FinancePayments.Remove(payment);
            await db.SaveChangesAsync();

            // Recalculate bill status
            if (billId != null)
            {
                var bill = await db.FinanceBills
                    .Include(b => b.Payments)
                    .FirstOrDefaultAsync(b => b.Id == billId);
                if (bill != null)
                {
                    var totalPaid = bill.Payments.Sum(p => p.AmountPaid);
                    if (totalPaid >= bill.OriginalAmount)
                    {
                        bill.Status = BillStatus.Paid.ToString();
                    }
                    else if (totalPaid > 0)
                    {
                        bill.Status = BillStatus.Partial.ToString();
                    }
                    else
                    {
                        bill.Status = BillStatus.Pending.ToString();
                    }
                    bill.UpdatedAt = DateTime.Now;
                    await db.SaveChangesAsync();
                }
            }

            // Recalculate loan balance
            if (loanId != null)
            {
                var loan = await db.FinanceLoans
                    .Include(l => l.Payments)
                    .FirstOrDefaultAsync(l => l.Id == loanId);
                if (loan != null)
                {
                    var totalPaid = loan.Payments.Sum(p => p.AmountPaid);
                    loan.CurrentBalance = loan.OriginalBalance - totalPaid;
                    if (loan.CurrentBalance <= 0)
                    {
                        loan.CurrentBalance = 0;
                        loan.IsActive = false;
                    }
                    else
                    {
                        loan.IsActive = true;
                    }
                    await db.SaveChangesAsync();
                }
            }
        }

        #endregion
    }
}
