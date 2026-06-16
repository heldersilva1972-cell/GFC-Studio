using GFC.Core.Models.Finance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IFinanceService
    {
        // Bill Management
        Task<IEnumerable<FinanceBill>> GetBillsAsync(int month, int year);
        Task<IEnumerable<FinanceBill>> GetBillsForReportAsync(int month, int year);
        Task<IEnumerable<FinanceBill>> GetBillsForYearAsync(int year);
        Task<FinanceBill?> GetBillByIdAsync(int id);
        Task<FinanceBill> CreateBillAsync(FinanceBill bill);
        Task UpdateBillAsync(FinanceBill bill);
        Task DeleteBillAsync(int id);
        Task MarkAsPaidAsync(int billId, decimal amount, DateTime date, string? method = null, string? note = null, int? userId = null);
        Task ToggleDisputeAsync(int billId, string? note = null);

        // Vendor Management
        Task<IEnumerable<FinanceVendor>> GetAllVendorsAsync();
        Task<IEnumerable<FinanceVendor>> GetAllVendorsAsync(bool includeInactive);
        Task<FinanceVendor?> GetVendorByIdAsync(int id);
        Task<FinanceVendor> CreateVendorAsync(FinanceVendor vendor);
        Task UpdateVendorAsync(FinanceVendor vendor);
        Task DeleteVendorAsync(int id);
        Task<string?> GetLastPaymentMethodForVendorAsync(int vendorId);

        // Loan Management
        Task<IEnumerable<FinanceLoan>> GetAllLoansAsync(bool includeInactive = false);
        Task<FinanceLoan?> GetLoanByIdAsync(int id);
        Task<FinanceLoan> CreateLoanAsync(FinanceLoan loan);
        Task UpdateLoanAsync(FinanceLoan loan);
        Task DeleteLoanAsync(int id);
        Task RecordLoanPaymentAsync(int loanId, decimal amount, DateTime date, string? method = null, string? note = null, int? userId = null);

        // Payment Management
        Task<FinancePayment?> GetPaymentByIdAsync(int id);
        Task UpdatePaymentAsync(FinancePayment payment);
        Task DeletePaymentAsync(int paymentId);

        // Category Management
        Task<IEnumerable<FinanceCategory>> GetAllCategoriesAsync();
        Task<FinanceCategory> CreateCategoryAsync(FinanceCategory category);
        Task UpdateCategoryAsync(FinanceCategory category);
        Task DeleteCategoryAsync(int id);

        // Analytics
        Task<decimal> GetTotalDueForMonthAsync(int month, int year);
        Task<decimal> GetTotalPaidForMonthAsync(int month, int year);
        Task<int> GetUpcomingAlertCountAsync(int daysThreshold);
    }
}
