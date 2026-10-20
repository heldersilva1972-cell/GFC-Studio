using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GFC.BlazorServer.Services;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/lottery/variances")]
    public class LotteryVariancesApiController : ControllerBase
    {
        private readonly ILotteryShiftRepository _shiftRepository;
        private readonly ILotteryShiftService _shiftService;
        private readonly IFirebaseNotificationService _notificationService;

        public LotteryVariancesApiController(
            ILotteryShiftRepository shiftRepository,
            ILotteryShiftService shiftService,
            IFirebaseNotificationService notificationService)
        {
            _shiftRepository = shiftRepository;
            _shiftService = shiftService;
            _notificationService = notificationService;
        }

        public class LotteryVarianceResponseDto
        {
            public int ShiftId { get; set; }
            public string ShiftDate { get; set; } = string.Empty;
            public string? ShiftType { get; set; }
            public string EmployeeName { get; set; } = string.Empty;
            public string? Status { get; set; }
            public string? Notes { get; set; }
            public string? CreatedBy { get; set; }
            public string? MachineId { get; set; }

            // Cash & Envelope Tracking
            public decimal StartingCash { get; set; }
            public decimal EndingCash { get; set; }
            public decimal BackupBagAmount { get; set; }
            public decimal BagRefillAmount { get; set; }
            public decimal EnvelopeAmount { get; set; }

            // Machine Readings / Activity
            public decimal TotalSales { get; set; }
            public decimal TotalPayouts { get; set; }
            public decimal TotalCancels { get; set; }
            public decimal NetDue { get; set; }

            // Shift Activity
            public decimal ShiftSalesActivity { get; set; }
            public decimal ShiftPayoutsActivity { get; set; }
            public decimal ShiftCancelsActivity { get; set; }

            // Financial Metrics
            public decimal NetSales { get; set; }
            public decimal ExpectedCash { get; set; }
            public decimal Variance { get; set; }

            // Acknowledgment
            public bool IsAcknowledged { get; set; }
            public string? AcknowledgedBy { get; set; }
            public string? AcknowledgedAt { get; set; }

            // Bartender History
            public int BartenderTotalShifts { get; set; }
            public decimal BartenderLifetimeVariance { get; set; }
            public decimal BartenderPositiveVariance { get; set; }
            public decimal BartenderNegativeVariance { get; set; }
        }

        public class AcknowledgeRequest
        {
            public int ShiftId { get; set; }
            public string? AcknowledgedBy { get; set; }
        }

        /// <summary>
        /// Returns active (unacknowledged) shifts where |Variance| >= threshold
        /// </summary>
        [HttpGet("active")]
        public IActionResult GetActiveVariances([FromQuery] decimal threshold = 5.0m)
        {
            try
            {
                // Fetch shifts from the last 30 days via service so reconciled ExpectedCash and Variance are used
                var startDate = DateTime.UtcNow.Date.AddDays(-30);
                var endDate = DateTime.UtcNow.Date.AddDays(1);
                var shifts = _shiftService.GetShiftsByDateRange(startDate, endDate);

                // Filter for shifts where |Variance| >= threshold
                var activeShifts = shifts
                    .Where(s => Math.Abs(s.Variance) >= threshold)
                    .OrderByDescending(s => s.ShiftDate)
                    .ToList();

                var results = activeShifts.Select(s => MapToDto(s, shifts)).ToList();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Returns variance history (all shifts exceeding threshold) with optional employee filter
        /// </summary>
        [HttpGet("history")]
        public IActionResult GetVarianceHistory([FromQuery] decimal threshold = 5.0m, [FromQuery] string? employee = null)
        {
            try
            {
                var startDate = DateTime.UtcNow.Date.AddDays(-180); // Last 6 months
                var endDate = DateTime.UtcNow.Date.AddDays(1);
                var shifts = _shiftService.GetShiftsByDateRange(startDate, endDate);

                // Returns all shifts exceeding threshold for historical audit
                var query = shifts.Where(s => Math.Abs(s.Variance) >= threshold);
                if (!string.IsNullOrWhiteSpace(employee))
                {
                    query = query.Where(s => string.Equals(s.EmployeeName, employee, StringComparison.OrdinalIgnoreCase));
                }

                var filtered = query.OrderByDescending(s => s.ShiftDate).ToList();
                var results = filtered.Select(s => MapToDto(s, shifts)).ToList();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Acknowledges / dismisses an active shift variance so it clears from the Connect app dashboard
        /// </summary>
        [HttpPost("acknowledge/{id}")]
        public IActionResult AcknowledgeVariance(int id, [FromBody] AcknowledgeRequest? req)
        {
            try
            {
                var shift = _shiftRepository.GetById(id);
                if (shift == null) return NotFound(new { error = $"Shift {id} not found." });

                var ackUser = !string.IsNullOrWhiteSpace(req?.AcknowledgedBy) ? req.AcknowledgedBy : (User.Identity?.Name ?? "Manager");
                shift.AcknowledgedBy = ackUser;
                shift.AcknowledgedAt = DateTime.UtcNow;

                _shiftRepository.Update(shift);
                return Ok(new { success = true, message = $"Shift {id} variance acknowledged by {ackUser}." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private static LotteryVarianceResponseDto MapToDto(LotteryShiftDto s, List<LotteryShiftDto> allShifts)
        {
            var empShifts = allShifts.Where(x => string.Equals(x.EmployeeName, s.EmployeeName, StringComparison.OrdinalIgnoreCase)).ToList();
            var totalShifts = empShifts.Count;
            var lifetimeVariance = empShifts.Sum(x => x.Variance);
            var positiveVariance = empShifts.Where(x => x.Variance > 0).Sum(x => x.Variance);
            var negativeVariance = empShifts.Where(x => x.Variance < 0).Sum(x => x.Variance);

            return new LotteryVarianceResponseDto
            {
                ShiftId = s.ShiftId,
                ShiftDate = s.ShiftDate.ToString("yyyy-MM-dd"),
                ShiftType = s.ShiftType,
                EmployeeName = s.EmployeeName,
                Status = s.Status,
                Notes = s.Notes,
                CreatedBy = s.CreatedBy,
                MachineId = s.MachineId,
                StartingCash = s.StartingCash,
                EndingCash = s.EndingCash,
                BackupBagAmount = s.BackupBagAmount,
                BagRefillAmount = s.BagRefillAmount,
                EnvelopeAmount = s.EnvelopeAmount,
                TotalSales = s.TotalSales,
                TotalPayouts = s.TotalPayouts,
                TotalCancels = s.TotalCancels,
                NetDue = s.NetDue,
                ShiftSalesActivity = s.ShiftSalesActivity,
                ShiftPayoutsActivity = s.ShiftPayoutsActivity,
                ShiftCancelsActivity = s.ShiftCancelsActivity,
                NetSales = s.NetSales,
                ExpectedCash = s.ExpectedCash,
                Variance = s.Variance,
                IsAcknowledged = !string.IsNullOrEmpty(s.AcknowledgedBy),
                AcknowledgedBy = s.AcknowledgedBy,
                AcknowledgedAt = s.AcknowledgedAt?.ToString("o"),
                BartenderTotalShifts = totalShifts,
                BartenderLifetimeVariance = lifetimeVariance,
                BartenderPositiveVariance = positiveVariance,
                BartenderNegativeVariance = negativeVariance
            };
        }
    }
}
