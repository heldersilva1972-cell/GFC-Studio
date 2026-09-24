using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GFC.BlazorServer.Auth;
using GFC.BlazorServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/reporting")]
    [ReportingApiKeyAuth]
    public class ReportingApiController : ControllerBase
    {
        private readonly GfcDbContext _dbContext;

        public ReportingApiController(GfcDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Metadata / Schema endpoint for Tableau and BI tools
        /// </summary>
        [HttpGet("metadata")]
        public IActionResult GetMetadata()
        {
            var schemas = new
            {
                version = "1.0",
                description = "GFC-Studio WebApp Reporting Gateway for Tableau",
                availableEndpoints = new[]
                {
                    new { key = "pos-sales", endpoint = "/api/reporting/pos-sales", description = "Point of Sale transactions and category revenue" },
                    new { key = "lottery-summary", endpoint = "/api/reporting/lottery-summary", description = "Pull-Tabs & Lottery machine shifts and collections" },
                    new { key = "bar-sales", endpoint = "/api/reporting/bar-sales", description = "Daily and shift-level bar sales" },
                    new { key = "member-stats", endpoint = "/api/reporting/member-stats", description = "Member aggregates, demographics, and statuses" },
                    new { key = "hall-rentals", endpoint = "/api/reporting/hall-rentals", description = "Hall rental bookings, guest counts, and revenue" },
                    new { key = "bingo-sessions", endpoint = "/api/reporting/bingo-sessions", description = "Bingo game sessions, admissions, gross receipts, and prize payouts" },
                    new { key = "dues-payments", endpoint = "/api/reporting/dues-payments", description = "Member annual dues payment history and tender types" },
                    new { key = "finance-bills", endpoint = "/api/reporting/finance-bills", description = "Club bills, invoices, due dates, and payment balances" },
                    new { key = "liquor-inventory", endpoint = "/api/reporting/liquor-inventory", description = "Liquor item catalog, bottle sizes, and vendor pricing" },
                    new { key = "door-swipes", endpoint = "/api/reporting/door-swipes", description = "Key card swipe events, entry access history, and door logs" }
                }
            };
            return Ok(schemas);
        }

        /// <summary>
        /// Point of Sale (POS) sales data
        /// </summary>
        [HttpGet("pos-sales")]
        public async Task<IActionResult> GetPosSales([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int limit = 1000)
        {
            var query = _dbContext.PosSales.AsNoTracking().AsQueryable();

            if (startDate.HasValue)
                query = query.Where(s => s.Timestamp >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(s => s.Timestamp <= endDate.Value);

            var items = await query
                .OrderByDescending(s => s.Timestamp)
                .Take(Math.Clamp(limit, 1, 5000))
                .Select(s => new
                {
                    s.Id,
                    s.Timestamp,
                    s.TerminalName,
                    s.BartenderName,
                    s.TotalAmount,
                    s.PaymentType,
                    s.AmountReceived,
                    s.ChangeDue,
                    s.IsVoided,
                    s.IsCorrection
                })
                .ToListAsync();

            HttpContext.Items["ReportingRecordCount"] = items.Count;
            return Ok(items);
        }

        /// <summary>
        /// Lottery & Pull-tabs weekly statistics and shift collections
        /// </summary>
        [HttpGet("lottery-summary")]
        public async Task<IActionResult> GetLotterySummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int limit = 500)
        {
            var query = _dbContext.LotteryWeeklyStats.AsNoTracking().AsQueryable();

            if (startDate.HasValue)
                query = query.Where(l => l.WeekEndingDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(l => l.WeekEndingDate <= endDate.Value);

            var items = await query
                .OrderByDescending(l => l.WeekEndingDate)
                .Take(Math.Clamp(limit, 1, 2000))
                .ToListAsync();

            HttpContext.Items["ReportingRecordCount"] = items.Count;
            return Ok(items);
        }

        /// <summary>
        /// Bar Sales entries
        /// </summary>
        [HttpGet("bar-sales")]
        public async Task<IActionResult> GetBarSales([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int limit = 1000)
        {
            var query = _dbContext.BarSaleEntries.AsNoTracking().AsQueryable();

            if (startDate.HasValue)
                query = query.Where(b => b.SaleDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(b => b.SaleDate <= endDate.Value);

            var items = await query
                .OrderByDescending(b => b.SaleDate)
                .Take(Math.Clamp(limit, 1, 5000))
                .ToListAsync();

            HttpContext.Items["ReportingRecordCount"] = items.Count;
            return Ok(items);
        }

        /// <summary>
        /// High-level Member demographics (sanitized, no sensitive PII/secrets)
        /// </summary>
        [HttpGet("member-stats")]
        public async Task<IActionResult> GetMemberStats([FromQuery] int limit = 1000)
        {
            var items = await _dbContext.Members
                .AsNoTracking()
                .OrderBy(m => m.LastName)
                .ThenBy(m => m.FirstName)
                .Take(Math.Clamp(limit, 1, 5000))
                .Select(m => new
                {
                    m.MemberID,
                    m.Status,
                    m.ApplicationDate,
                    m.AcceptedDate,
                    m.City,
                    m.State,
                    m.PostalCode,
                    IsDeceased = m.DateOfDeath != null,
                    IsLifeEligible = m.LifeEligibleDate != null
                })
                .ToListAsync();

            HttpContext.Items["ReportingRecordCount"] = items.Count;
            return Ok(items);
        }

        /// <summary>
        /// Hall Rentals booking & financial summary
        /// </summary>
        [HttpGet("hall-rentals")]
        public async Task<IActionResult> GetHallRentals([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int limit = 500)
        {
            var query = _dbContext.HallRentals.AsNoTracking().AsQueryable();

            if (startDate.HasValue)
                query = query.Where(h => h.EventDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(h => h.EventDate <= endDate.Value);

            var items = await query
                .OrderByDescending(h => h.EventDate)
                .Take(Math.Clamp(limit, 1, 2000))
                .Select(h => new
                {
                    h.Id,
                    h.EventDate,
                    h.Status,
                    h.GuestCount,
                    h.KitchenUsed,
                    h.TotalPrice
                })
                .ToListAsync();

            HttpContext.Items["ReportingRecordCount"] = items.Count;
            return Ok(items);
        }

        /// <summary>
        /// Bingo Sessions and financial gross receipts & prizes
        /// </summary>
        [HttpGet("bingo-sessions")]
        public async Task<IActionResult> GetBingoSessions([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int limit = 500)
        {
            var query = _dbContext.BingoSessions.AsNoTracking().AsQueryable();

            if (startDate.HasValue)
                query = query.Where(b => b.SessionDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(b => b.SessionDate <= endDate.Value);

            var items = await query
                .OrderByDescending(b => b.SessionDate)
                .Take(Math.Clamp(limit, 1, 2000))
                .Select(b => new
                {
                    b.Id,
                    b.SessionDate,
                    b.AdmissionCount,
                    b.TotalGrossReceipts,
                    b.TotalPrizesPaid,
                    b.TotalClubTake,
                    b.TotalLotteryTake,
                    b.Status,
                    b.Category
                })
                .ToListAsync();

            HttpContext.Items["ReportingRecordCount"] = items.Count;
            return Ok(items);
        }

        /// <summary>
        /// Member Annual Dues Payments
        /// </summary>
        [HttpGet("dues-payments")]
        public async Task<IActionResult> GetDuesPayments([FromQuery] int? year, [FromQuery] int limit = 1000)
        {
            var query = _dbContext.DuesPayments.AsNoTracking().AsQueryable();

            if (year.HasValue)
                query = query.Where(d => d.Year == year.Value);

            var items = await query
                .OrderByDescending(d => d.PaidDate)
                .Take(Math.Clamp(limit, 1, 5000))
                .Select(d => new
                {
                    d.Id,
                    d.MemberId,
                    d.Year,
                    d.Amount,
                    d.PaidDate,
                    d.PaymentType
                })
                .ToListAsync();

            HttpContext.Items["ReportingRecordCount"] = items.Count;
            return Ok(items);
        }

        /// <summary>
        /// Bills & Invoices from Finance System
        /// </summary>
        [HttpGet("finance-bills")]
        public async Task<IActionResult> GetFinanceBills([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int limit = 500)
        {
            var query = _dbContext.FinanceBills.AsNoTracking().Include(b => b.Vendor).Include(b => b.Category).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(b => b.DueDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(b => b.DueDate <= endDate.Value);

            var items = await query
                .OrderByDescending(b => b.DueDate)
                .Take(Math.Clamp(limit, 1, 2000))
                .Select(b => new
                {
                    b.Id,
                    VendorName = b.Vendor != null ? b.Vendor.Name : "Unknown",
                    CategoryName = b.Category != null ? b.Category.Name : "General",
                    b.Description,
                    b.OriginalAmount,
                    b.DueDate
                })
                .ToListAsync();

            HttpContext.Items["ReportingRecordCount"] = items.Count;
            return Ok(items);
        }

        /// <summary>
        /// Liquor items and catalog prices
        /// </summary>
        [HttpGet("liquor-inventory")]
        public async Task<IActionResult> GetLiquorInventory([FromQuery] int limit = 1000)
        {
            var items = await _dbContext.LiquorItems
                .AsNoTracking()
                .OrderBy(l => l.Name)
                .Take(Math.Clamp(limit, 1, 5000))
                .Select(l => new
                {
                    l.Id,
                    l.Name,
                    l.Category,
                    l.BottleSize,
                    l.CurrentPrice,
                    l.UpcCode
                })
                .ToListAsync();

            HttpContext.Items["ReportingRecordCount"] = items.Count;
            return Ok(items);
        }

        /// <summary>
        /// Door access / controller swipe events
        /// </summary>
        [HttpGet("door-swipes")]
        public async Task<IActionResult> GetDoorSwipes([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int limit = 1000)
        {
            var query = _dbContext.ControllerEvents.AsNoTracking().Include(c => c.Door).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(e => e.TimestampUtc >= startDate.Value.ToUniversalTime());
            if (endDate.HasValue)
                query = query.Where(e => e.TimestampUtc <= endDate.Value.ToUniversalTime());

            var items = await query
                .OrderByDescending(e => e.TimestampUtc)
                .Take(Math.Clamp(limit, 1, 5000))
                .Select(e => new
                {
                    e.Id,
                    e.TimestampUtc,
                    e.ControllerEventTime,
                    DoorName = e.Door != null ? e.Door.Name : $"Door #{e.DoorOrReader}",
                    e.CardNumber,
                    e.EventType,
                    e.ReasonCode,
                    e.IsByCard,
                    e.IsByButton
                })
                .ToListAsync();

            HttpContext.Items["ReportingRecordCount"] = items.Count;
            return Ok(items);
        }
    }
}
