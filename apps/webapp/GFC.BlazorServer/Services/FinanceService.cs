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
                        .ThenInclude(v => v!.DefaultPaymentType)
                    .Include(b => b.Category)
                    .Include(b => b.Payments)
                    .Where(b => (b.DueDate >= startOfMonth && b.DueDate <= endOfMonth) || 
                                (b.Status != "Paid" && b.DueDate < startOfMonth) ||
                                (b.Status != "Paid" && b.DueDate > endOfMonth && b.DueDate <= upcomingLimit) ||
                                b.Payments.Any(p => p.PaymentDate >= startOfMonth && p.PaymentDate <= endOfMonth))
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
                    .ThenInclude(v => v!.DefaultPaymentType)
                .Include(b => b.Category)
                .Include(b => b.Payments)
                .Where(b => (b.DueDate >= startOfYear && b.DueDate <= endOfYear) ||
                            b.Payments.Any(p => p.PaymentDate >= startOfYear && p.PaymentDate <= endOfYear))
                .OrderBy(b => b.DueDate)
                .ToListAsync();

            sw.Stop();
            Console.WriteLine($"[FINANCE] GetBillsForYearAsync ({year}) FINISHED in {sw.ElapsedMilliseconds}ms.");
            return results;
        }

        public async Task<IEnumerable<FinanceBill>> GetAllBillsAsync()
        {
            var sw = Stopwatch.StartNew();
            using var db = await _dbFactory.CreateDbContextAsync();

            var results = await db.FinanceBills
                .AsNoTracking()
                .AsSplitQuery() // [OPTIMIZATION]
                .Include(b => b.Vendor)
                    .ThenInclude(v => v!.DefaultPaymentType)
                .Include(b => b.Category)
                .Include(b => b.Payments)
                .OrderBy(b => b.DueDate)
                .ToListAsync();

            sw.Stop();
            Console.WriteLine($"[FINANCE] GetAllBillsAsync FINISHED in {sw.ElapsedMilliseconds}ms.");
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
                    .ThenInclude(v => v!.DefaultPaymentType)
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
                    .ThenInclude(v => v!.DefaultPaymentType)
                .Include(b => b.Category)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<FinanceBill> CreateBillAsync(FinanceBill bill, string? performedBy = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.FinanceBills.Add(bill);
            await db.SaveChangesAsync();

            return bill;
        }

        public async Task UpdateBillAsync(FinanceBill bill, string? performedBy = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.FinanceBills.FindAsync(bill.Id);
            if (existing != null)
            {
                var vendor = await db.FinanceVendors.FindAsync(bill.VendorId);
                var vendorName = vendor?.Name ?? "Unknown";
                
                var changeDesc = $"Updated bill for vendor '{vendorName}'. " +
                                 $"Original Amount: {existing.OriginalAmount:C} -> {bill.OriginalAmount:C}, " +
                                 $"Due Date: {existing.DueDate:MM/dd/yyyy} -> {bill.DueDate:MM/dd/yyyy}.";

                db.Entry(existing).CurrentValues.SetValues(bill);
                existing.UpdatedAt = DateTime.Now;
                await db.SaveChangesAsync();

                await LogActionAsync(db, "Edit Bill", changeDesc, performedBy);
            }
        }

        public async Task DeleteBillAsync(int id, string? performedBy = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var bill = await db.FinanceBills.Include(b => b.Vendor).FirstOrDefaultAsync(b => b.Id == id);
            if (bill != null)
            {
                var vendorName = bill.Vendor?.Name ?? "Unknown";
                var desc = $"Deleted bill for vendor '{vendorName}' due on {bill.DueDate:MM/dd/yyyy} for {bill.OriginalAmount:C}.";

                db.FinanceBills.Remove(bill);
                await db.SaveChangesAsync();

                await LogActionAsync(db, "Delete Bill", desc, performedBy);
            }
        }

        public async Task MarkAsPaidAsync(int billId, decimal amount, DateTime date, string? method = null, string? note = null, int? userId = null, string? performedBy = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var bill = await db.FinanceBills
                .Include(b => b.Payments)
                .Include(b => b.Vendor)
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
                .Include(v => v.DefaultPaymentType)
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

        #region Payment Type Management

        public async Task<IEnumerable<FinancePaymentType>> GetAllPaymentTypesAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinancePaymentTypes
                .AsNoTracking()
                .Where(pt => pt.IsActive)
                .OrderBy(pt => pt.Name)
                .ToListAsync();
        }

        public async Task<FinancePaymentType> CreatePaymentTypeAsync(FinancePaymentType type)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            type.CreatedAt = DateTime.Now;
            db.FinancePaymentTypes.Add(type);
            await db.SaveChangesAsync();
            return type;
        }

        public async Task UpdatePaymentTypeAsync(FinancePaymentType type)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.FinancePaymentTypes.FindAsync(type.Id);
            if (existing != null)
            {
                existing.Name = type.Name.Trim();
                await db.SaveChangesAsync();
            }
        }

        public async Task DeletePaymentTypeAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var pt = await db.FinancePaymentTypes.FindAsync(id);
            if (pt != null)
            {
                pt.IsActive = false;
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

        public async Task SkipLoanMonthAsync(int loanId, string yearMonth)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var loan = await db.FinanceLoans.FindAsync(loanId);
            if (loan != null)
            {
                var skipped = string.IsNullOrWhiteSpace(loan.SkippedMonths) 
                    ? new List<string>() 
                    : loan.SkippedMonths.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!skipped.Contains(yearMonth))
                {
                    skipped.Add(yearMonth);
                    loan.SkippedMonths = string.Join(",", skipped);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task UnskipLoanMonthAsync(int loanId, string yearMonth)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var loan = await db.FinanceLoans.FindAsync(loanId);
            if (loan != null)
            {
                var skipped = string.IsNullOrWhiteSpace(loan.SkippedMonths)
                    ? new List<string>()
                    : loan.SkippedMonths.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

                if (skipped.Contains(yearMonth))
                {
                    skipped.Remove(yearMonth);
                    loan.SkippedMonths = skipped.Count == 0 ? null : string.Join(",", skipped);
                    await db.SaveChangesAsync();
                }
            }
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

        public async Task UpdatePaymentAsync(FinancePayment payment, string? performedBy = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.FinancePayments
                .Include(p => p.Bill).ThenInclude(b => b!.Vendor)
                .Include(p => p.Loan)
                .FirstOrDefaultAsync(p => p.Id == payment.Id);
            if (existing == null) throw new Exception("Payment not found.");

            string payeeName = existing.Bill?.Vendor?.Name ?? existing.Loan?.LenderName ?? "Unknown";
            var changeDesc = $"Updated payment for '{payeeName}'. " +
                             $"Amount: {existing.AmountPaid:C} -> {payment.AmountPaid:C}, " +
                             $"Date: {existing.PaymentDate:MM/dd/yyyy} -> {payment.PaymentDate:MM/dd/yyyy}, " +
                             $"Method: {existing.PaymentMethod} -> {payment.PaymentMethod}.";

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

            await LogActionAsync(db, "Edit Payment", changeDesc, performedBy);
        }

        public async Task DeletePaymentAsync(int paymentId, string? performedBy = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var payment = await db.FinancePayments
                .Include(p => p.Bill).ThenInclude(b => b!.Vendor)
                .Include(p => p.Loan)
                .FirstOrDefaultAsync(p => p.Id == paymentId);
            if (payment == null) return;

            var billId = payment.BillId;
            var loanId = payment.LoanId;
            string payeeName = payment.Bill?.Vendor?.Name ?? payment.Loan?.LenderName ?? "Unknown";
            var desc = $"Deleted payment of {payment.AmountPaid:C} made on {payment.PaymentDate:MM/dd/yyyy} for '{payeeName}'.";

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

            await LogActionAsync(db, "Delete Payment", desc, performedBy);
        }

        public async Task<IEnumerable<FinanceAuditLog>> GetAuditLogsAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.FinanceAuditLogs
                .AsNoTracking()
                .OrderByDescending(al => al.ActionDate)
                .ToListAsync();
        }

        private async Task LogActionAsync(GfcDbContext db, string actionType, string description, string? performedBy)
        {
            try
            {
                var log = new FinanceAuditLog
                {
                    ActionDate = DateTime.Now,
                    ActionType = actionType,
                    Description = description,
                    PerformedBy = performedBy ?? "System"
                };
                db.FinanceAuditLogs.Add(log);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FINANCE] Audit logging failed: {ex.Message}");
            }
        }

        #endregion
    }
}
