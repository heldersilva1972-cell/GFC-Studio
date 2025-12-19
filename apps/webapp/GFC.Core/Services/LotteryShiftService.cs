using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services
{
    public class LotteryShiftService : ILotteryShiftService
    {
        private readonly ILotteryShiftRepository _repository;

        public LotteryShiftService(ILotteryShiftRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public LotteryShift? GetShift(int shiftId)
        {
            return _repository.GetById(shiftId);
        }

        public List<LotteryShiftDto> GetShiftsByDateRange(DateTime startDate, DateTime endDate)
        {
            var shifts = _repository.GetByDateRange(startDate, endDate);
            return shifts.Select(MapToDto).ToList();
        }

        public List<LotteryShiftDto> GetShiftsByEmployee(string employeeName, DateTime? startDate = null, DateTime? endDate = null)
        {
            var shifts = _repository.GetByEmployee(employeeName, startDate, endDate);
            return shifts.Select(MapToDto).ToList();
        }

        public List<LotteryShiftDto> GetAllShifts()
        {
            var shifts = _repository.GetAll();
            return shifts.Select(MapToDto).ToList();
        }

        public List<LotteryShiftDto> GetUnreconciledShifts()
        {
            var shifts = _repository.GetUnreconciled();
            return shifts.Select(MapToDto).ToList();
        }

        public int CreateShift(LotteryShift shift, string? createdBy = null)
        {
            shift.CreatedDate = DateTime.UtcNow;
            shift.CreatedBy = createdBy;
            shift.Status ??= "Submitted";
            shift.IsReconciled = false;
            return _repository.Create(shift);
        }

        public void UpdateShift(LotteryShift shift, string? modifiedBy = null)
        {
            shift.ModifiedDate = DateTime.UtcNow;
            shift.ModifiedBy = modifiedBy;
            _repository.Update(shift);
        }

        public void DeleteShift(int shiftId)
        {
            _repository.Delete(shiftId);
        }

        public void MarkReconciled(int shiftId, string? reconciledBy = null)
        {
            var shift = _repository.GetById(shiftId);
            if (shift == null) return;
            
            shift.IsReconciled = true;
            shift.ReconciledBy = reconciledBy;
            shift.ReconciledDate = DateTime.UtcNow;
            shift.Status = "Reconciled";
            shift.ModifiedDate = DateTime.UtcNow;
            shift.ModifiedBy = reconciledBy;
            _repository.Update(shift);
        }

        public void MarkUnreconciled(int shiftId)
        {
            var shift = _repository.GetById(shiftId);
            if (shift == null) return;
            
            shift.IsReconciled = false;
            shift.ReconciledBy = null;
            shift.ReconciledDate = null;
            shift.Status = "Submitted";
            shift.ModifiedDate = DateTime.UtcNow;
            _repository.Update(shift);
        }

        public LotteryShiftSummaryDto GetDailySummary(DateTime date)
        {
            var startDate = date.Date;
            // Pass the same date for both start and end - the repository query uses DATEADD(day, 1, @EndDate)
            // So passing the same date ensures: ShiftDate >= date AND ShiftDate < date+1
            var shifts = _repository.GetByDateRange(startDate, startDate);
            
            return CalculateSummary(shifts, startDate, startDate.AddDays(1), date.ToString("ddd, MMM d, yyyy"));
        }

        public LotteryShiftSummaryDto GetWeeklySummary(DateTime weekStart)
        {
            var startDate = weekStart.Date;
            var endDate = startDate.AddDays(7);
            var shifts = _repository.GetByDateRange(startDate, endDate);
            
            var weekEnd = endDate.AddDays(-1);
            var label = $"{startDate:MMM d} - {weekEnd:MMM d, yyyy}";
            return CalculateSummary(shifts, startDate, endDate, label);
        }

        public LotteryShiftSummaryDto GetMonthlySummary(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);
            var shifts = _repository.GetByDateRange(startDate, endDate);
            
            var label = startDate.ToString("MMMM yyyy");
            return CalculateSummary(shifts, startDate, endDate, label);
        }

        public List<LotteryShiftSummaryDto> GetWeeklySummaries(DateTime startDate, DateTime endDate)
        {
            var summaries = new List<LotteryShiftSummaryDto>();
            var currentWeekStart = GetWeekStart(startDate);
            
            while (currentWeekStart < endDate)
            {
                var weekEnd = currentWeekStart.AddDays(7);
                summaries.Add(GetWeeklySummary(currentWeekStart));
                currentWeekStart = weekEnd;
            }
            
            return summaries;
        }

        public List<LotteryShiftSummaryDto> GetMonthlySummaries(int year)
        {
            var summaries = new List<LotteryShiftSummaryDto>();
            for (int month = 1; month <= 12; month++)
            {
                summaries.Add(GetMonthlySummary(year, month));
            }
            return summaries;
        }

        public decimal GetTotalSales(DateTime? startDate = null, DateTime? endDate = null)
        {
            var shifts = GetShiftsForPeriod(startDate, endDate);
            return shifts.Sum(s => s.TotalSales);
        }

        public decimal GetTotalPayouts(DateTime? startDate = null, DateTime? endDate = null)
        {
            var shifts = GetShiftsForPeriod(startDate, endDate);
            return shifts.Sum(s => s.TotalPayouts);
        }

        public decimal GetTotalNetSales(DateTime? startDate = null, DateTime? endDate = null)
        {
            var shifts = GetShiftsForPeriod(startDate, endDate);
            return shifts.Sum(s => s.NetSales);
        }

        public decimal GetTotalVariance(DateTime? startDate = null, DateTime? endDate = null)
        {
            var shifts = GetShiftsForPeriod(startDate, endDate);
            return shifts.Sum(s => s.Variance);
        }

        public List<string> GetEmployeeNames()
        {
            var shifts = _repository.GetAll();
            return shifts.Select(s => s.EmployeeName)
                .Distinct()
                .OrderBy(n => n)
                .ToList();
        }

        private List<LotteryShift> GetShiftsForPeriod(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                return _repository.GetByDateRange(startDate.Value, endDate.Value);
            }
            return _repository.GetAll();
        }

        private LotteryShiftSummaryDto CalculateSummary(List<LotteryShift> shifts, DateTime periodStart, DateTime periodEnd, string label)
        {
            if (shifts.Count == 0)
            {
                return new LotteryShiftSummaryDto
                {
                    PeriodStart = periodStart,
                    PeriodEnd = periodEnd,
                    PeriodLabel = label,
                    ShiftCount = 0
                };
            }

            var variances = shifts.Select(s => s.Variance).ToList();
            var varianceCount = variances.Count(v => Math.Abs(v) > 0.01m); // Count non-zero variances
            
            return new LotteryShiftSummaryDto
            {
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                PeriodLabel = label,
                ShiftCount = shifts.Count,
                TotalSales = shifts.Sum(s => s.TotalSales),
                TotalPayouts = shifts.Sum(s => s.TotalPayouts),
                TotalCancels = shifts.Sum(s => s.TotalCancels),
                TotalNetSales = shifts.Sum(s => s.NetSales),
                TotalVariance = shifts.Sum(s => s.Variance),
                AverageVariance = varianceCount > 0 ? variances.Where(v => Math.Abs(v) > 0.01m).Average() : 0,
                VarianceCount = varianceCount,
                LargestVariance = variances.Max(),
                SmallestVariance = variances.Min()
            };
        }

        private static DateTime GetWeekStart(DateTime date)
        {
            var diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private static LotteryShiftDto MapToDto(LotteryShift shift)
        {
            return new LotteryShiftDto
            {
                ShiftId = shift.ShiftId,
                ShiftDate = shift.ShiftDate,
                EmployeeName = shift.EmployeeName,
                ShiftType = shift.ShiftType,
                MachineId = shift.MachineId,
                StartingCash = shift.StartingCash,
                EndingCash = shift.EndingCash,
                TotalSales = shift.TotalSales,
                TotalPayouts = shift.TotalPayouts,
                TotalCancels = shift.TotalCancels,
                NetSales = shift.NetSales,
                ExpectedCash = shift.ExpectedCash,
                Variance = shift.Variance,
                Notes = shift.Notes,
                Status = shift.Status,
                IsReconciled = shift.IsReconciled,
                ReconciledBy = shift.ReconciledBy,
                ReconciledDate = shift.ReconciledDate,
                CreatedBy = shift.CreatedBy,
                CreatedDate = shift.CreatedDate
            };
        }
    }
}
