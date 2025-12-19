using GFC.Core.DTOs;
using GFC.Core.Models;

namespace GFC.Core.Interfaces
{
    public interface ILotteryShiftService
    {
        LotteryShift? GetShift(int shiftId);
        List<LotteryShiftDto> GetShiftsByDateRange(DateTime startDate, DateTime endDate);
        List<LotteryShiftDto> GetShiftsByEmployee(string employeeName, DateTime? startDate = null, DateTime? endDate = null);
        List<LotteryShiftDto> GetAllShifts();
        List<LotteryShiftDto> GetUnreconciledShifts();
        int CreateShift(LotteryShift shift, string? createdBy = null);
        void UpdateShift(LotteryShift shift, string? modifiedBy = null);
        void DeleteShift(int shiftId);
        void MarkReconciled(int shiftId, string? reconciledBy = null);
        void MarkUnreconciled(int shiftId);
        
        // Summary methods
        LotteryShiftSummaryDto GetDailySummary(DateTime date);
        LotteryShiftSummaryDto GetWeeklySummary(DateTime weekStart);
        LotteryShiftSummaryDto GetMonthlySummary(int year, int month);
        List<LotteryShiftSummaryDto> GetWeeklySummaries(DateTime startDate, DateTime endDate);
        List<LotteryShiftSummaryDto> GetMonthlySummaries(int year);
        
        // Statistics
        decimal GetTotalSales(DateTime? startDate = null, DateTime? endDate = null);
        decimal GetTotalPayouts(DateTime? startDate = null, DateTime? endDate = null);
        decimal GetTotalNetSales(DateTime? startDate = null, DateTime? endDate = null);
        decimal GetTotalVariance(DateTime? startDate = null, DateTime? endDate = null);
        List<string> GetEmployeeNames();
    }
}

