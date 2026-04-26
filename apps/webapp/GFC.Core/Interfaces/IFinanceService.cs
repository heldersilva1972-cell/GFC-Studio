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
        Task<FinanceVendor?> GetVendorByIdAsync(int id);
        Task<FinanceVendor> CreateVendorAsync(FinanceVendor vendor);
        Task UpdateVendorAsync(FinanceVendor vendor);
        Task DeleteVendorAsync(int id);

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
