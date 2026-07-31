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

            // Prevention of double-submission/duplicates by checking Date + Shift Type
            var existing = _repository.GetDuplicateShift(shift.EmployeeName, shift.ShiftDate, shift.ShiftType);
            if (existing != null)
            {
                throw new InvalidOperationException($"A shift record for {shift.EmployeeName} at {shift.ShiftDate:MMM dd, yyyy h:mm tt} already exists.");
            }

            shift.CreatedDate = DateTime.UtcNow;
            shift.CreatedBy = createdBy;
            shift.Status ??= "Submitted";
            shift.IsReconciled = false;
            
            var createdId = _repository.Create(shift);

            // Propagate to Night shift if Day shift was created (updates baseline)
            if (string.Equals(shift.ShiftType, "Day", StringComparison.OrdinalIgnoreCase))
            {
                PropagateDayShiftToNight(shift, createdBy);
            }

            return createdId;
        }

        public void UpdateShift(LotteryShift shift, string? modifiedBy = null)
        {
            // CORE FIX: Mandatory server-side audit before saving update
            ReconcileShiftMath(shift);

            var oldShift = _repository.GetById(shift.ShiftId);
            var existing = _repository.GetDuplicateShift(shift.EmployeeName, shift.ShiftDate, shift.ShiftType);
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

            // Propagate to Night shift if Day shift was updated (updates baseline)
            if (string.Equals(shift.ShiftType, "Day", StringComparison.OrdinalIgnoreCase))
            {
                PropagateDayShiftToNight(shift, modifiedBy);
            }
        }

        private void PropagateDayShiftToNight(LotteryShift dayShift, string? modifiedBy)
        {
            if (dayShift == null) return;
            var shifts = _repository.GetByDateRange(dayShift.ShiftDate.Date, dayShift.ShiftDate.Date);
            var nightShift = shifts.FirstOrDefault(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase));
            if (nightShift != null)
            {
                nightShift.ShiftSalesActivity = nightShift.TotalSales - dayShift.TotalSales;
                nightShift.ShiftPayoutsActivity = nightShift.TotalPayouts - dayShift.TotalPayouts;
                nightShift.ShiftCancelsActivity = nightShift.TotalCancels - dayShift.TotalCancels;

                ReconcileShiftMath(nightShift);

                nightShift.ModifiedDate = DateTime.UtcNow;
                nightShift.ModifiedBy = modifiedBy;
                _repository.Update(nightShift);
            }
        }

        private void PropagateDayShiftDeletionToNight(DateTime shiftDate, string? modifiedBy)
        {
            var shifts = _repository.GetByDateRange(shiftDate.Date, shiftDate.Date);
            var nightShift = shifts.FirstOrDefault(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase));
            if (nightShift != null)
            {
                nightShift.ShiftSalesActivity = nightShift.TotalSales;
                nightShift.ShiftPayoutsActivity = nightShift.TotalPayouts;
                nightShift.ShiftCancelsActivity = nightShift.TotalCancels;

                ReconcileShiftMath(nightShift);

                nightShift.ModifiedDate = DateTime.UtcNow;
                nightShift.ModifiedBy = modifiedBy;
                _repository.Update(nightShift);
            }
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

            // [BUSINESS RULE]: Drops and refills only happen at NIGHT. 
            // Force to zero for Day shifts to prevent accidental data contamination.
            if (string.Equals(shift.ShiftType, "Day", StringComparison.OrdinalIgnoreCase))
            {
                shift.EnvelopeAmount = 0;
                shift.BagRefillAmount = 0;
            }
            
            // [FIX]: Expected Cash for the NEXT shift should subtract distributions.
            shift.ExpectedCash = shift.StartingCash + shift.NetSales + shift.BackupBagAmount - shift.BagRefillAmount - shift.EnvelopeAmount;
            
            // [CRITICAL FIX]: Variance must be calculated against the PRE-DROP expectation.
            // Otherwise, every dollar dropped into an envelope is flagged as "Missing" (Short).
            decimal expectedBeforeDrops = shift.StartingCash + shift.NetSales + shift.BackupBagAmount;
            shift.Variance = shift.EndingCash - expectedBeforeDrops;

            // [NEW] Persist the income math so it's available for reporting without re-calculation
            var rate = _rateRepository.GetApplicableRate(shift.ShiftDate.Year);
            shift.LotteryIncome = (shift.ShiftSalesActivity * rate.SalesCommissionMultiplier) +
                                 (shift.ShiftPayoutsActivity * rate.CashingBonusMultiplier) +
                                 (shift.ShiftCancelsActivity * rate.TicketBonusMultiplier);
            
            shift.NetIncome = shift.LotteryIncome + shift.Variance;

            // Calculate and persist Net Due Activity (Night Net Due - Day Net Due)
            if (string.Equals(shift.ShiftType, "Night", StringComparison.OrdinalIgnoreCase))
            {
                var dayShifts = _repository.GetByDateRange(shift.ShiftDate.Date, shift.ShiftDate.Date);
                var dayShift = dayShifts.FirstOrDefault(s => string.Equals(s.ShiftType, "Day", StringComparison.OrdinalIgnoreCase));
                if (dayShift != null)
                {
                    shift.ShiftNetDueActivity = shift.NetDue - dayShift.NetDue;
                }
                else
                {
                    shift.ShiftNetDueActivity = shift.NetDue;
                }
            }
            else
            {
                shift.ShiftNetDueActivity = shift.NetDue;
            }
        }

        public void DeleteShift(int shiftId)
        {
            var shift = _repository.GetById(shiftId);
            _repository.Delete(shiftId);

            if (shift != null && string.Equals(shift.ShiftType, "Day", StringComparison.OrdinalIgnoreCase))
            {
                PropagateDayShiftDeletionToNight(shift.ShiftDate, "System");
            }
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

            _auditLogRepository.Insert(new AuditLogEntry
            {
                TimestampUtc = DateTime.UtcNow,
                Action = "Shift Submitted On Behalf",
                Details = $"Shift #{shift.ShiftId} for {shift.EmployeeName} on {shift.ShiftDate:MM/dd/yyyy} ({shift.ShiftType}) submitted on behalf by Manager [{managerUsername}].",
                PageUrl = "/lottery"
            });

            if (string.Equals(shift.ShiftType, "Day", StringComparison.OrdinalIgnoreCase))
            {
                PropagateDayShiftToNight(shift, managerUsername);
            }
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

            // GROUP BY DAY to correctly show cumulative machine totals (Latest reading of the day, skipping closed shifts)
            var shiftsByDay = dtos.GroupBy(s => s.ShiftDate.Date).Select(g => {
                var night = g.FirstOrDefault(s => string.Equals(s.ShiftType, "Night", StringComparison.OrdinalIgnoreCase));
                var day = g.FirstOrDefault(s => string.Equals(s.ShiftType, "Day", StringComparison.OrdinalIgnoreCase));
                
                var representative = night;
                if (night == null || string.Equals(night.EmployeeName, "Club Closed", StringComparison.OrdinalIgnoreCase))
                {
                    if (day != null && !string.Equals(day.EmployeeName, "Club Closed", StringComparison.OrdinalIgnoreCase))
                    {
                        representative = day;
                    }
                }
                
                return new {
                    Date = g.Key,
                    LatestShift = representative ?? night ?? day ?? g.First()
                };
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
                
                // [FIX]: Monthly/Weekly Variance should correctly measure the summed variance of all shifts.
                TotalVariance = dtos.Sum(s => s.Variance),
                
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

            // [FIX]: Expected Cash for display should be the target baseline ($1,200) after distributions.
            decimal reconciledExpected = shift.StartingCash + reconciledNetSales + shift.BackupBagAmount - shift.BagRefillAmount - shift.EnvelopeAmount;
            
            // [CRITICAL FIX]: Variance calculation must ignore drops/refills to avoid "Ghost Discrepancies".
            decimal expectedBeforeDrops = shift.StartingCash + reconciledNetSales + shift.BackupBagAmount;
            decimal reconciledVariance = shift.EndingCash - expectedBeforeDrops;

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
                EnvelopeAmount = shift.EnvelopeAmount,
                BagRefillAmount = shift.BagRefillAmount,
                CreatedBy = shift.CreatedBy,
                CreatedDate = shift.CreatedDate,
                ModifiedBy = shift.ModifiedBy,
                ModifiedDate = shift.ModifiedDate,
                ShiftSalesActivity = shift.ShiftSalesActivity,
                ShiftPayoutsActivity = shift.ShiftPayoutsActivity,
                ShiftCancelsActivity = shift.ShiftCancelsActivity,
                ShiftNetDueActivity = shift.ShiftNetDueActivity
            };
        }
    }
}
