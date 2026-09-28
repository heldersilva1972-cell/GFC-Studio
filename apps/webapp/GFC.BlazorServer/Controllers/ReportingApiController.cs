using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GFC.BlazorServer.Auth;
using GFC.BlazorServer.Data;
using GFC.Core.DTOs;
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
                version = "2.0",
                description = "GFC-Studio WebApp Reporting Gateway for Tableau & BI Tools",
                availableEndpoints = new[]
                {
                    new { key = "pos-sales-items", endpoint = "/api/reporting/pos-sales-items", description = "Line-item POS transaction details, items sold, quantities, unit prices, bartender, and shift" },
                    new { key = "pos-sales", endpoint = "/api/reporting/pos-sales", description = "Point of Sale transactions, totals, and tender breakdowns" },
                    new { key = "staff-shifts", endpoint = "/api/reporting/staff-shifts", description = "Staff shift records, employee names, roles, clock-in/out times, hours worked, hourly rates, and payroll totals" },
                    new { key = "door-swipes", endpoint = "/api/reporting/door-swipes", description = "Debounced door access & entry events mapped to member IDs, names, status, and door locations" },
                    new { key = "member-stats", endpoint = "/api/reporting/member-stats", description = "Master member directory, member IDs, full names, statuses, dues standing, and active keycards" },
                    new { key = "lottery-summary", endpoint = "/api/reporting/lottery-summary", description = "Pull-Tabs & Lottery machine shifts and collections" },
                    new { key = "bar-sales", endpoint = "/api/reporting/bar-sales", description = "Daily and shift-level bar sales" },
                    new { key = "hall-rentals", endpoint = "/api/reporting/hall-rentals", description = "Hall rental bookings, guest counts, and revenue" },
                    new { key = "bingo-sessions", endpoint = "/api/reporting/bingo-sessions", description = "Bingo game sessions, admissions, gross receipts, and prize payouts" },
                    new { key = "dues-payments", endpoint = "/api/reporting/dues-payments", description = "Member annual dues payment history and tender types" },
                    new { key = "finance-bills", endpoint = "/api/reporting/finance-bills", description = "Club bills, invoices, due dates, and payment balances" },
                    new { key = "liquor-inventory", endpoint = "/api/reporting/liquor-inventory", description = "Liquor item catalog, bottle sizes, and vendor pricing" }
                }
            };
            return Ok(schemas);
        }

        /// <summary>
        /// Detailed Point of Sale Line Items & Quantities Sold
        /// </summary>
        [HttpGet("pos-sales-items")]
        public async Task<IActionResult> GetPosSaleItems([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int limit = 2000)
        {
            var query = _dbContext.PosSales.AsNoTracking().Where(s => !s.IsVoided).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(s => s.Timestamp >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(s => s.Timestamp <= endDate.Value);

            var sales = await query
                .OrderByDescending(s => s.Timestamp)
                .Take(Math.Clamp(limit, 1, 10000))
                .ToListAsync();

            var flatItems = new List<object>();

            foreach (var sale in sales)
            {
                var hour = sale.Timestamp.Hour;
                var shiftType = (hour >= 6 && hour < 17) ? "Day" : "Night";

                if (!string.IsNullOrWhiteSpace(sale.ItemsJson) && sale.ItemsJson != "[]")
                {
                    try
                    {
                        var parsed = JsonSerializer.Deserialize<List<PosSaleItemDto>>(sale.ItemsJson);
                        if (parsed != null && parsed.Count > 0)
                        {
                            foreach (var item in parsed)
                            {
                                flatItems.Add(new
                                {
                                    TransactionId = sale.Id,
                                    Timestamp = sale.Timestamp,
                                    Date = sale.Timestamp.ToString("yyyy-MM-dd"),
                                    HourOfDay = sale.Timestamp.Hour,
                                    DayOfWeek = sale.Timestamp.DayOfWeek.ToString(),
                                    TerminalName = sale.TerminalName ?? "TERMINAL 1",
                                    BartenderName = string.IsNullOrWhiteSpace(sale.BartenderName) ? "Staff" : sale.BartenderName,
                                    Shift = shiftType,
                                    PaymentType = sale.PaymentType ?? "CASH",
                                    ItemId = item.Id,
                                    ItemName = item.Name,
                                    UnitPrice = item.Price,
                                    Quantity = item.Quantity,
                                    LineTotal = item.Price * item.Quantity,
                                    ReceiptTotal = sale.TotalAmount
                                });
                            }
                            continue;
                        }
                    }
                    catch { }
                }

                // Fallback for sales without parsed line item JSON
                flatItems.Add(new
                {
                    TransactionId = sale.Id,
                    Timestamp = sale.Timestamp,
                    Date = sale.Timestamp.ToString("yyyy-MM-dd"),
                    HourOfDay = sale.Timestamp.Hour,
                    DayOfWeek = sale.Timestamp.DayOfWeek.ToString(),
                    TerminalName = sale.TerminalName ?? "TERMINAL 1",
                    BartenderName = string.IsNullOrWhiteSpace(sale.BartenderName) ? "Staff" : sale.BartenderName,
                    Shift = shiftType,
                    PaymentType = sale.PaymentType ?? "CASH",
                    ItemId = 0,
                    ItemName = "General Sale",
                    UnitPrice = sale.TotalAmount,
                    Quantity = 1,
                    LineTotal = sale.TotalAmount,
                    ReceiptTotal = sale.TotalAmount
                });
            }

            HttpContext.Items["ReportingRecordCount"] = flatItems.Count;
            return Ok(flatItems);
        }

        /// <summary>
        /// Staff Shifts, Hours Worked, Hourly Rate, and Payroll Compensation
        /// </summary>
        [HttpGet("staff-shifts")]
        public async Task<IActionResult> GetStaffShifts([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int limit = 1000)
        {
            var query = _dbContext.StaffShifts.AsNoTracking().Include(s => s.StaffMember).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(s => s.Date >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(s => s.Date <= endDate.Value);

            var shifts = await query
                .OrderByDescending(s => s.Date)
                .Take(Math.Clamp(limit, 1, 5000))
                .ToListAsync();

            var results = shifts.Select(s =>
            {
                var staff = s.StaffMember;
                var hourlyRate = staff?.HourlyRate ?? 0m;

                var clockIn = s.ClockInTime ?? s.CustomStartTime;
                var clockOut = s.ClockOutTime ?? s.CustomEndTime;

                double hoursWorked = 0;
                if (clockIn.HasValue && clockOut.HasValue)
                {
                    hoursWorked = Math.Max(0, (clockOut.Value - clockIn.Value).TotalHours);
                }
                else if (s.StartTime != default && s.EndTime != default)
                {
                    hoursWorked = Math.Max(0, (s.EndTime - s.StartTime).TotalHours);
                }

                decimal totalShiftPay = (decimal)hoursWorked * hourlyRate;

                return new
                {
                    ShiftId = s.Id,
                    ShiftDate = s.Date.ToString("yyyy-MM-dd"),
                    StaffMemberId = s.StaffMemberId,
                    StaffName = staff?.Name ?? s.StaffName ?? "Unknown Staff",
                    Role = staff?.Role ?? "Staff",
                    ShiftType = s.ShiftType == 1 ? "Day" : "Night",
                    Status = s.Status ?? "Completed",
                    ClockInTime = s.ClockInTime,
                    ClockOutTime = s.ClockOutTime,
                    TotalHoursWorked = Math.Round(hoursWorked, 2),
                    HourlyRate = hourlyRate,
                    TotalShiftPay = Math.Round(totalShiftPay, 2)
                };
            }).ToList();

            HttpContext.Items["ReportingRecordCount"] = results.Count;
            return Ok(results);
        }

        /// <summary>
        /// Point of Sale (POS) sales summary
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
        /// Door access / controller swipe events with Member info & 6-second de-bounce
        /// </summary>
        [HttpGet("door-swipes")]
        public async Task<IActionResult> GetDoorSwipes([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] bool deduplicate = true, [FromQuery] int limit = 1000)
        {
            var query = _dbContext.ControllerEvents.AsNoTracking().Include(c => c.Door).AsQueryable();

            if (startDate.HasValue)
                query = query.Where(e => e.TimestampUtc >= startDate.Value.ToUniversalTime());
            if (endDate.HasValue)
                query = query.Where(e => e.TimestampUtc <= endDate.Value.ToUniversalTime());

            var events = await query
                .OrderBy(e => e.TimestampUtc)
                .Take(Math.Clamp(limit, 1, 10000))
                .ToListAsync();

            // Load Card to Member mappings
            var activeKeyCards = await _dbContext.KeyCards.AsNoTracking().Where(k => k.IsActive).ToListAsync();
            var memberIds = activeKeyCards.Select(k => k.MemberId).Distinct().ToList();
            var members = await _dbContext.Members.AsNoTracking().Where(m => memberIds.Contains(m.MemberID)).ToDictionaryAsync(m => m.MemberID);

            var cardMap = new Dictionary<string, (int MemberId, string FullName, string Status)>();
            foreach (var card in activeKeyCards)
            {
                if (!string.IsNullOrWhiteSpace(card.CardNumber))
                {
                    if (members.TryGetValue(card.MemberId, out var m))
                    {
                        var name = $"{m.FirstName} {m.LastName}".Trim();
                        cardMap[card.CardNumber.Trim()] = (m.MemberID, name, m.Status ?? "ACTIVE");
                    }
                }
            }

            var results = new List<object>();
            var lastEventPerKey = new Dictionary<string, DateTime>();

            foreach (var e in events)
            {
                var cardNumberStr = e.CardNumber.ToString();
                var doorKey = $"{e.CardNumber}_{e.DoorId}_{e.EventType}";
                bool isDoubleSwipe = false;

                if (lastEventPerKey.TryGetValue(doorKey, out var lastTime))
                {
                    if ((e.TimestampUtc - lastTime).TotalSeconds < 6)
                    {
                        isDoubleSwipe = true;
                    }
                }

                if (!isDoubleSwipe)
                {
                    lastEventPerKey[doorKey] = e.TimestampUtc;
                }

                if (deduplicate && isDoubleSwipe)
                {
                    continue; // Skip double burst
                }

                int? memberId = null;
                string memberName = "Unknown / Guest";
                string memberStatus = "UNKNOWN";

                if (e.CardNumber == 1)
                {
                    memberName = "Buzzed In / Guest Entry";
                    memberStatus = "GUEST";
                }
                else if (cardMap.TryGetValue(cardNumberStr, out var info))
                {
                    memberId = info.MemberId;
                    memberName = info.FullName;
                    memberStatus = info.Status;
                }

                results.Add(new
                {
                    e.Id,
                    TimestampUtc = e.TimestampUtc,
                    LocalTime = e.TimestampUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                    MemberId = memberId,
                    MemberName = memberName,
                    MemberStatus = memberStatus,
                    DoorName = e.Door != null ? e.Door.Name : $"Door #{e.DoorOrReader}",
                    CardNumber = e.CardNumber,
                    EventType = e.EventType,
                    AccessGranted = e.EventType == 1,
                    IsDoubleSwipe = isDoubleSwipe,
                    IsByCard = e.IsByCard,
                    IsByButton = e.IsByButton
                });
            }

            // Return latest first
            results.Reverse();

            HttpContext.Items["ReportingRecordCount"] = results.Count;
            return Ok(results);
        }

        /// <summary>
        /// Master Member Directory (Includes MemberID, Name, Status, City, PostalCode, Active Cards)
        /// </summary>
        [HttpGet("member-stats")]
        public async Task<IActionResult> GetMemberStats([FromQuery] int limit = 2000)
        {
            var keyCards = await _dbContext.KeyCards.AsNoTracking().Where(k => k.IsActive).ToListAsync();
            var cardLookup = keyCards.GroupBy(k => k.MemberId).ToDictionary(g => g.Key, g => g.Select(c => c.CardNumber).ToList());

            var items = await _dbContext.Members
                .AsNoTracking()
                .OrderBy(m => m.LastName)
                .ThenBy(m => m.FirstName)
                .Take(Math.Clamp(limit, 1, 10000))
                .ToListAsync();

            var results = items.Select(m => new
            {
                m.MemberID,
                FullName = $"{m.FirstName} {m.LastName}".Trim(),
                FirstName = m.FirstName,
                LastName = m.LastName,
                Status = m.Status ?? "REGULAR",
                m.City,
                m.State,
                m.PostalCode,
                m.ApplicationDate,
                m.AcceptedDate,
                IsDeceased = m.DateOfDeath != null,
                IsLifeEligible = m.LifeEligibleDate != null,
                ActiveCardCount = cardLookup.TryGetValue(m.MemberID, out var cards) ? cards.Count : 0,
                ActiveCardNumbers = cardLookup.TryGetValue(m.MemberID, out var list) ? string.Join(", ", list) : ""
            }).ToList();

            HttpContext.Items["ReportingRecordCount"] = results.Count;
            return Ok(results);
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
    }
}
