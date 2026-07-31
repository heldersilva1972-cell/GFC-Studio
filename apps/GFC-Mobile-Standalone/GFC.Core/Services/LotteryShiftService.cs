using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GFC.Core.Services
{
    public class LotteryShiftService : ILotteryShiftService
    {
        private readonly ILotteryShiftRepository _repository;
        private readonly ILotteryRateRepository _rateRepository; // [NEW]
        private readonly IAuditLogRepository _auditLogRepository;

        public LotteryShiftService(ILotteryShiftRepository repository, 
                                   ILotteryRateRepository rateRepository, 
                                   IAuditLogRepository auditLogLogRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _rateRepository = rateRepository ?? throw new ArgumentNullException(nameof(rateRepository));
            _auditLogRepository = auditLogLogRepository ?? throw new ArgumentNullException(nameof(auditLogLogRepository));
        }

        public LotteryShift? GetShift(int shiftId)
        {
            return _repository.GetById(shiftId);
        }

        public List<LotteryShiftDto> GetShiftsByDateRange(DateTime startDate, DateTime endDate)
        {
            var shifts = _repository.GetByDateRange(startDate, endDate);
            
            // Post-processing to fix stale "Beginning Cash" for Drafts
            foreach (var shift in shifts.Where(s => (s.Status == "Draft" || s.Status == "Submitted") && string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase)))
            {
                var dayShift = shifts.FirstOrDefault(s => s.ShiftDate.Date == shift.ShiftDate.Date && string.Equals(s.ShiftType, "Day", StringComparison.OrdinalIgnoreCase));
                if (dayShift != null && dayShift.EndingCash > 0)
                {
                    shift.StartingCash = dayShift.EndingCash;
                }
            }

            var dtos = shifts.Select(MapToDto).ToList();
            ApplyConsolidation(dtos);

            return dtos;
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
            // CORE FIX: Mandatory server-side audit before entry
            ReconcileShiftMath(shift);

            // Prevention of double-submission/duplicates by checking Employee + Exact Time
            var existing = _repository.GetDuplicateShift(shift.EmployeeName, shift.ShiftDate);
            if (existing != null)
            {
                throw new InvalidOperationException($"A shift record for {shift.EmployeeName} at {shift.ShiftDate:MMM dd, yyyy h:mm tt} already exists.");
            }

            shift.CreatedDate = DateTime.UtcNow;
            shift.CreatedBy = createdBy;
            shift.Status ??= "Submitted";
            shift.IsReconciled = false;
            return _repository.Create(shift);
        }

        public void UpdateShift(LotteryShift shift, string? modifiedBy = null)
        {
            // CORE FIX: Mandatory server-side audit before saving update
            ReconcileShiftMath(shift);

            var oldShift = _repository.GetById(shift.ShiftId);
            var existing = _repository.GetDuplicateShift(shift.EmployeeName, shift.ShiftDate);
            if (existing != null && existing.ShiftId != shift.ShiftId)
            {
                throw new InvalidOperationException($"Cannot save because another shift for {shift.EmployeeName} at {shift.ShiftDate:MMM dd, yyyy h:mm tt} already exists.");
            }

            if (oldShift != null && !string.IsNullOrEmpty(shift.CreatedBy) && oldShift.CreatedBy != shift.CreatedBy)
            {
                _repository.UpdateBarSaleOwner(shift.ShiftDate, shift.ShiftType ?? "Day", oldShift.CreatedBy ?? "Unknown", shift.CreatedBy);
                _auditLogRepository.Insert(new AuditLogEntry
                {
                    TimestampUtc = DateTime.UtcNow,
                    Action = "Shift Reassignment",
                    Details = $"REASSIGNED shift on {shift.ShiftDate:MM/dd/yyyy} ({shift.ShiftType}). FROM: [{oldShift.CreatedBy}] -> TO: [{shift.CreatedBy}].",
                    PageUrl = "/lottery"
                });
            }

            shift.ModifiedDate = DateTime.UtcNow;
            shift.ModifiedBy = modifiedBy;
            _repository.Update(shift);
        }

        private void ReconcileShiftMath(LotteryShift shift)
        {
            // This method ignores whatever math the client (phone/browser) sent
            // and recalculates the audit based on the physical data points.
            decimal sales = shift.ShiftSalesActivity;
            decimal payouts = shift.ShiftPayoutsActivity;
            decimal cancels = shift.ShiftCancelsActivity;

            // FALLBACK for legacy records where activity wasn't tracked separately
            if (shift.ShiftId > 0 && sales == 0 && shift.TotalSales > 0)
            {
                shift.NetSales = shift.NetSales; // Preserve existing if no activity fields found
            }
            else
            {
                shift.NetSales = sales - payouts - cancels;
            }

            shift.ExpectedCash = shift.StartingCash + shift.NetSales + shift.BackupBagAmount;
            shift.Variance = shift.EndingCash - shift.ExpectedCash;
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
            _repository.Update(shift);
        }

        public void SubmitOnBehalfOfEmployee(int shiftId, string managerUsername)
        {
            var shift = _repository.GetById(shiftId);
            if (shift == null) return;

            shift.Status = "Submitted";
            shift.ModifiedBy = managerUsername;
            shift.ModifiedDate = DateTime.UtcNow;
            _repository.Update(shift);
        }

        public LotteryShiftSummaryDto GetDailySummary(DateTime date)
        {
            var shifts = _repository.GetByDateRange(date.Date, date.Date);
            return CalculateSummary(shifts, date.Date, date.Date.AddDays(1), date.ToString("ddd, MMM d, yyyy"));
        }

        public LotteryShiftSummaryDto GetWeeklySummary(DateTime weekStart)
        {
            var startDate = weekStart.Date;
            var endDate = startDate.AddDays(7); // Next Sunday
            var shifts = _repository.GetByDateRange(startDate, startDate.AddDays(6)); // Fetches Sun-Sat
            return CalculateSummary(shifts, startDate, endDate, $"{startDate:MMM d} - {endDate.AddDays(-1):MMM d, yyyy}");
        }

        public LotteryShiftSummaryDto GetMonthlySummary(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1); // Start of next month
            var shifts = _repository.GetByDateRange(startDate, endDate.AddDays(-1)); // Feteches up to last day of month
            return CalculateSummary(shifts, startDate, endDate, startDate.ToString("MMMM yyyy"));
        }

        public List<LotteryShiftSummaryDto> GetWeeklySummaries(DateTime startDate, DateTime endDate)
        {
            var summaries = new List<LotteryShiftSummaryDto>();
            var currentWeekStart = GetWeekStart(startDate);
            while (currentWeekStart < endDate)
            {
                summaries.Add(GetWeeklySummary(currentWeekStart));
                currentWeekStart = currentWeekStart.AddDays(7);
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

        public List<LotteryCommissionRate> GetAllRates() => _rateRepository.GetAll();
        public void SaveRate(LotteryCommissionRate rate) => _rateRepository.Save(rate);

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
            return _repository.GetEmployeeMetadata().Select(m => m.FullName).ToList();
        }

        public List<(string Username, string FullName)> GetEmployeeMetadata()
        {
            return _repository.GetEmployeeMetadata();
        }

        private List<LotteryShift> GetShiftsForPeriod(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue) return _repository.GetByDateRange(startDate.Value, endDate.Value);
            return _repository.GetAll();
        }

        private LotteryShiftSummaryDto CalculateSummary(List<LotteryShift> shifts, DateTime periodStart, DateTime periodEnd, string label)
        {
            if (shifts.Count == 0) return new LotteryShiftSummaryDto { PeriodStart = periodStart, PeriodEnd = periodEnd, PeriodLabel = label, ShiftCount = 0 };

            // MAP TO DTOs and APPLY CONSOLIDATION to get the true Audit/Fee state for every shift
            var dtos = shifts.Select(MapToDto).ToList();
            ApplyConsolidation(dtos);

            // GROUP BY DAY to correctly show cumulative machine totals (Latest reading of the day)
            var shiftsByDay = dtos.GroupBy(s => s.ShiftDate.Date).Select(g => new {
                Date = g.Key,
                LatestShift = g.OrderByDescending(s => s.ShiftId).First()
            }).ToList();

            var variances = dtos.Select(s => s.Variance).ToList();
            var varianceCount = variances.Count(v => Math.Abs(v) > 0.01m);
            
            return new LotteryShiftSummaryDto
            {
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                PeriodLabel = label,
                ShiftCount = shifts.Count,
                
                // MACHINE TOTALS: We sum the Night shifts only (Cumulative for the day)
                TotalSales = shiftsByDay.Sum(d => d.LatestShift.TotalSales),
                TotalPayouts = shiftsByDay.Sum(d => d.LatestShift.TotalPayouts),
                TotalCancels = shiftsByDay.Sum(d => d.LatestShift.TotalCancels),
                TotalNetDue = shiftsByDay.Sum(d => d.LatestShift.NetDue),
                
                // ACTIVITY: We sum EVERY shift's results to get the total for the week
                TotalNetSales = dtos.Sum(s => s.NetSales),
                TotalEnvelope = dtos.Sum(s => s.EnvelopeAmount),
                
                // [FIX]: Monthly/Weekly Variance should only measure CASH errors (Counted vs Expected).
                // We exclude the Bag transactions from the "Variance" so it doesn't inflate.
                TotalVariance = dtos.Sum(s => s.EndingCash - (s.StartingCash + s.NetSales)),
                
                // FINANCIALS: The missing fields for the dashboard
                // FINANCIALS: Income is calculated from the cumulative machine readings of the Latest Shift (Night shift)
                // to match the official daily report from the terminal.
                TotalIncome = shiftsByDay.Sum(d => {
                    var rate = _rateRepository.GetApplicableRate(d.Date.Year);
                    return (d.LatestShift.TotalSales * rate.SalesCommissionMultiplier) + 
                           (d.LatestShift.TotalPayouts * rate.CashingBonusMultiplier) + 
                           (d.LatestShift.TotalCancels * rate.TicketBonusMultiplier);
                }),
                TotalFees = dtos.Sum(s => s.IdentifiedFees),
 
                // BACKUP BAG TRACKING
                TotalBagOut = dtos.Sum(s => s.BackupBagAmount),
                TotalBagIn = dtos.Sum(s => s.BagRefillAmount),
                
                AverageVariance = varianceCount > 0 ? variances.Where(v => Math.Abs(v) > 0.01m).Average() : 0,
                VarianceCount = varianceCount,
                LargestVariance = variances.Max(),
                SmallestVariance = variances.Min()
            };
        }

        private void ApplyConsolidation(List<LotteryShiftDto> dtos)
        {
            // CUMULATIVE CONSOLIDATION: Apply Fixed Fees to the Night shift (Daily Audit Point).
            foreach (var group in dtos.GroupBy(s => s.ShiftDate.Date))
            {
                var day = group.FirstOrDefault(s => s.ShiftType == "Day");
                var night = group.FirstOrDefault(s => s.ShiftType == "Night");
                var rate = _rateRepository.GetApplicableRate(group.Key.Year);

                if (day != null && night != null)
                {
                    day.IdentifiedFees = 0;
                    
                    // Sum of both daily fees (Service + Bonding) applied to the Night shift
                    night.IdentifiedFees = rate.DailySystemFee + rate.DailyBondingFee;
                }
                else if (night != null)
                {
                    night.IdentifiedFees = rate.DailySystemFee + rate.DailyBondingFee;
                }
                else if (day != null)
                {
                    day.IdentifiedFees = rate.DailySystemFee + rate.DailyBondingFee;
                }
            }
        }

        private static DateTime GetWeekStart(DateTime date)
        {
            var diff = (7 + (date.DayOfWeek - DayOfWeek.Sunday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        private LotteryShiftDto MapToDto(LotteryShift shift)
        {
            // [SMART LOOKUP]: Percentage based earnings
            var rate = _rateRepository.GetApplicableRate(shift.ShiftDate.Year);

            // 1. Calculate EARNINGS (Sales Comm + Cashing Bonus)
            decimal salesComm = shift.ShiftSalesActivity * rate.SalesCommissionMultiplier;
            decimal cashingBonus = shift.ShiftPayoutsActivity * rate.CashingBonusMultiplier;
            decimal ticketBonus = shift.ShiftCancelsActivity * rate.TicketBonusMultiplier;
            decimal earnings = salesComm + cashingBonus + ticketBonus;

            // 2. APPLY FIXED FEES (Explicit calculation based on Rates)
            // Fees are applied to the Final/Night shift primarily, handled in ApplyConsolidation.
            // For single shift mapping, we default to the combined daily rate.
            decimal fees = rate.DailySystemFee + rate.DailyBondingFee;

            // 3. SELF-HEALING AUDIT: Recalculate on the fly based on RAW activity 
            // to repair any potentially corrupted historic "Saved" math.
            decimal reconciledNetSales = shift.ShiftSalesActivity - shift.ShiftPayoutsActivity - shift.ShiftCancelsActivity;
            
            // If the activity fields are 0 (historic or Day shift cumulative), fallback to persisted field
            if (shift.ShiftId > 0 && shift.ShiftSalesActivity == 0 && shift.TotalSales > 0)
            {
                reconciledNetSales = shift.NetSales;
            }

            decimal reconciledExpected = shift.StartingCash + reconciledNetSales + shift.BackupBagAmount;
            decimal reconciledVariance = shift.EndingCash - reconciledExpected;

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
                Commission = earnings,            // Display as Commission
                LotteryIncome = earnings,         // Display as Income
                IdentifiedFees = fees,            // NEW Field
                NetDue = shift.NetDue,
                NetSales = reconciledNetSales,
                ExpectedCash = reconciledExpected,
                Variance = reconciledVariance,
                Notes = shift.Notes,
                Status = shift.Status,
                IsReconciled = shift.IsReconciled,
                ReconciledBy = shift.ReconciledBy,
                ReconciledDate = shift.ReconciledDate,
                BackupBagAmount = shift.BackupBagAmount,
                EnvelopeAmount = (shift.ShiftType == "Day") ? 0 : shift.EnvelopeAmount,
                BagRefillAmount = shift.BagRefillAmount,
                CreatedBy = shift.CreatedBy,
                CreatedDate = shift.CreatedDate,
                ShiftSalesActivity = shift.ShiftSalesActivity,
                ShiftPayoutsActivity = shift.ShiftPayoutsActivity,
                ShiftCancelsActivity = shift.ShiftCancelsActivity,
                ShiftNetDueActivity = shift.ShiftNetDueActivity
            };
        }
    }
}
