using GFC.Core.Models;
using GFC.Core.DTOs;
using GFC.BlazorServer.Data;
using GFC.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BingoController : ControllerBase
    {
        private readonly GfcDbContext _context;
        private readonly IAuditLogRepository _auditLogRepository;

        public BingoController(GfcDbContext context, IAuditLogRepository auditLogRepository)
        {
            _context = context;
            _auditLogRepository = auditLogRepository;
        }

        [HttpGet("program")]
        public async Task<ActionResult<IEnumerable<BingoSheetDefinition>>> GetProgram()
        {
            return await _context.BingoSheetDefinitions
                .Include(s => s.Games)
                .Where(s => s.IsActive && !s.IsDeleted)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();
        }

        [HttpGet("admissions")]
        public async Task<ActionResult<IEnumerable<BingoAdmissionDefinition>>> GetAdmissions()
        {
            return await _context.BingoAdmissionDefinitions
                .Where(a => a.IsActive && !a.IsDeleted)
                .OrderBy(a => a.DisplayOrder)
                .ToListAsync();
        }

        [HttpGet("settings")]
        public async Task<ActionResult<BingoSettingsDto>> GetSettings()
        {
            var settings = await _context.SystemSettings.FirstOrDefaultAsync();
            var tiers = await _context.BingoPayoutTiers
                .Where(t => !t.IsDeleted)
                .OrderBy(t => t.MinAdmissions)
                .Select(t => new BingoPayoutTierDto 
                { 
                    MinAdmissions = t.MinAdmissions, 
                    PayoutPercentage = t.PayoutPercentage 
                })
                .ToListAsync();

            return Ok(new BingoSettingsDto 
            { 
                BasePrice = settings?.BingoBaseAdmissionPrice ?? 15.00m,
                CardPrice = settings?.BingoAdditionalCardPrice ?? 3.00m,
                PayoutRoundingMode = settings?.BingoPayoutRoundingMode ?? 0,
                PayoutTiers = tiers
            });
        }

        [HttpPost("session")]
        public async Task<IActionResult> SubmitSession([FromBody] BingoSession session)
        {
            Console.WriteLine($"[BINGO] Receiving session submission for {session?.SessionDate:yyyy-MM-dd}...");
            if (session == null) 
            {
                Console.WriteLine("[BINGO] ! Error: Session data is null.");
                return BadRequest("Invalid session data.");
            }

            // Set audit fields if not provided
            session.CreatedAt = DateTime.UtcNow;
            session.CreatedBy = User.Identity?.Name;
            if (string.IsNullOrEmpty(session.Status) || session.Status == "Draft")
            {
                session.Status = "Submitted";
            }

            foreach (var entry in session.GameEntries)
            {
                entry.CreatedAt = DateTime.UtcNow;
                entry.CreatedBy = User.Identity?.Name;
            }

            if (session.AdmissionEntries != null)
            {
                foreach (var entry in session.AdmissionEntries)
                {
                    entry.CreatedAt = DateTime.UtcNow;
                    entry.CreatedBy = User.Identity?.Name;
                }
            }

            // [ATOMIC TRANSACTION] Wrap the entire replacement in a transaction to prevent partial/failed states
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // [DE-DUPE] Check if a session already exists for this date.
                var existing = await _context.BingoSessions
                    .AsNoTracking() // Prevent EF from tracking the old record
                    .Include(s => s.GameEntries)
                    .Include(s => s.AdmissionEntries)
                    .FirstOrDefaultAsync(s => s.SessionDate.Date == session.SessionDate.Date);

                if (existing != null)
                {
                    Console.WriteLine($"[BINGO] Updating existing session for {session.SessionDate:yyyy-MM-dd} (Atomic Replacement)");
                    
                    // 1. Remove linked transactions first
                    var relatedTransactions = await _context.BingoLotteryTransactions
                        .Where(t => t.SessionId == existing.Id)
                        .ToListAsync();
                    if (relatedTransactions.Any()) {
                        _context.BingoLotteryTransactions.RemoveRange(relatedTransactions);
                    }

                    // 2. Remove children manually to be safe (CASCADE should handle this but manual is safer for sync)
                    var relatedGames = await _context.BingoGameEntries.Where(g => g.SessionId == existing.Id).ToListAsync();
                    if (relatedGames.Any()) _context.BingoGameEntries.RemoveRange(relatedGames);

                    var relatedAdmissions = await _context.BingoAdmissionEntries.Where(a => a.BingoSessionId == existing.Id).ToListAsync();
                    if (relatedAdmissions.Any()) _context.BingoAdmissionEntries.RemoveRange(relatedAdmissions);

                    // 3. Remove the parent session
                    var sessionToRemove = await _context.BingoSessions.FindAsync(existing.Id);
                    if (sessionToRemove != null) _context.BingoSessions.Remove(sessionToRemove);
                    
                    await _context.SaveChangesAsync();
                }

                // [IDENTITY PROTECTION] Strip local IDs from mobile to prevent DB conflicts
                session.Id = 0;
                if (session.GameEntries != null) {
                    foreach (var g in session.GameEntries) g.Id = 0;
                }
                if (session.AdmissionEntries != null) {
                    foreach (var a in session.AdmissionEntries) a.Id = 0;
                }

                _context.BingoSessions.Add(session);
                await _context.SaveChangesAsync();
                
                // Create Financial Transactions for this session
                var transactions = new List<BingoLotteryTransaction>
                {
                    new BingoLotteryTransaction
                    {
                        Date = session.SessionDate,
                        Type = "Income",
                        Amount = session.TotalGrossReceipts,
                        Description = $"Gross receipts from session on {session.SessionDate:MM/dd/yyyy}",
                        SessionId = session.Id
                    },
                    new BingoLotteryTransaction
                    {
                        Date = session.SessionDate,
                        Type = "Prize",
                        Amount = -session.TotalPrizesPaid,
                        Description = $"Prizes paid for session on {session.SessionDate:MM/dd/yyyy}",
                        SessionId = session.Id
                    },
                    new BingoLotteryTransaction
                    {
                        Date = session.SessionDate,
                        Type = "LotteryFee",
                        Amount = -session.TotalLotteryTake,
                        Description = $"Lottery percentage for session on {session.SessionDate:MM/dd/yyyy}",
                        SessionId = session.Id
                    }
                };

                if (session.RoundingAdjustment != 0)
                {
                    transactions.Add(new BingoLotteryTransaction
                    {
                        Date = session.SessionDate,
                        Type = "Rounding",
                        Amount = -session.RoundingAdjustment,
                        Description = $"Rounding adjustment for session on {session.SessionDate:MM/dd/yyyy}",
                        SessionId = session.Id
                    });
                }

                _context.BingoLotteryTransactions.AddRange(transactions);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                Console.WriteLine($"[BINGO] ✓ Atomic Sync for {session.SessionDate:yyyy-MM-dd} complete. ID: {session.Id}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"[BINGO] !!! Atomic Sync CRASH: {ex.Message}");
                if (ex.InnerException != null) Console.WriteLine($"[BINGO] !!! Inner Exception: {ex.InnerException.Message}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

            // Progressive Game Progression handling follows

            // Handle Progressive Game Progression
            var sessionDate = session.SessionDate.Date;
            var gameDefs = await _context.BingoGameDefinitions.Include(g => g.Sheet).ToListAsync();
            
            bool anyProgressions = false;
            foreach (var entry in session.GameEntries)
            {
                // Find matching definition
                var def = gameDefs.FirstOrDefault(d => 
                    d.Sheet?.ColorName == entry.SheetColor && 
                    d.GameName == entry.GameName && 
                    d.IsActive && !d.IsDeleted);

                if (def != null && def.IsProgressive)
                {
                    // Only progress if we haven't already for this session date
                    if (def.DateLastProgressed == null || def.DateLastProgressed.Value.Date < sessionDate)
                    {
                        if (entry.BallsCalled > 0) // Only progress if balls were actually recorded
                        {
                            if (entry.BallsCalled <= def.CurrentProgressiveBallGoal)
                            {
                                // JACKPOT WON! Reset to starting goal.
                                def.CurrentProgressiveBallGoal = def.ProgressiveBallGoal;
                            }
                            else
                            {
                                // JACKPOT NOT WON. Increment for next session.
                                def.CurrentProgressiveBallGoal++;
                            }
                            
                            def.DateLastProgressed = sessionDate;
                            anyProgressions = true;
                        }
                    }
                }
            }

            if (anyProgressions)
            {
                await _context.SaveChangesAsync();
            }

            return Ok(new { success = true, sessionId = session.Id });
        }

        [HttpGet("session-by-date")]
        public async Task<ActionResult<BingoSession>> GetSessionByDate([FromQuery] DateTime date)
        {
            var session = await _context.BingoSessions
                .Include(s => s.GameEntries)
                .Include(s => s.AdmissionEntries)
                .FirstOrDefaultAsync(s => s.SessionDate.Date == date.Date && !s.IsDeleted);

            if (session == null)
            {
                return Ok((BingoSession)null);
            }

            return Ok(session);
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<BingoSession>>> GetHistory()
        {
            return await _context.BingoSessions
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.SessionDate)
                .Take(20)
                .ToListAsync();
        }

        [HttpDelete("session")]
        public async Task<IActionResult> CancelSession([FromQuery] DateTime date)
        {
            Console.WriteLine($"[BINGO] Cancelling session for {date:yyyy-MM-dd}...");
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var session = await _context.BingoSessions
                    .Include(s => s.GameEntries)
                    .Include(s => s.AdmissionEntries)
                    .FirstOrDefaultAsync(s => s.SessionDate.Date == date.Date && !s.IsDeleted);

                if (session == null)
                {
                    return NotFound("Session not found.");
                }

                session.IsDeleted = true;
                session.ModifiedAt = DateTime.UtcNow;
                session.ModifiedBy = User.Identity?.Name ?? "Staff";

                if (session.GameEntries != null)
                {
                    foreach (var g in session.GameEntries)
                    {
                        g.IsDeleted = true;
                        g.ModifiedAt = DateTime.UtcNow;
                        g.ModifiedBy = User.Identity?.Name ?? "Staff";
                    }
                }

                if (session.AdmissionEntries != null)
                {
                    foreach (var a in session.AdmissionEntries)
                    {
                        a.IsDeleted = true;
                        a.ModifiedAt = DateTime.UtcNow;
                        a.ModifiedBy = User.Identity?.Name ?? "Staff";
                    }
                }

                // Also delete related lottery transactions
                var relatedTransactions = await _context.BingoLotteryTransactions
                    .Where(t => t.SessionId == session.Id && !t.IsDeleted)
                    .ToListAsync();
                foreach (var t in relatedTransactions)
                {
                    t.IsDeleted = true;
                    t.ModifiedAt = DateTime.UtcNow;
                    t.ModifiedBy = User.Identity?.Name ?? "Staff";
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Get current user id
                var username = User.Identity?.Name ?? "Staff";
                var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username == username);
                
                // Write audit log
                var audit = new AuditLogEntry
                {
                    TimestampUtc = DateTime.UtcNow,
                    PerformedByUserId = user?.UserId,
                    Action = "Cancel Bingo Session",
                    Details = $"Cancelled bingo session for {date:yyyy-MM-dd}. All entries soft-deleted.",
                    PageUrl = "/bingo"
                };
                await _auditLogRepository.InsertAsync(audit);

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"[BINGO] Cancel session crash: {ex.Message}");
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("progressive/override")]
        public async Task<IActionResult> OverrideProgressiveGoal([FromBody] ProgressiveOverrideDto dto)
        {
            if (dto == null || dto.GameDefinitionId <= 0)
            {
                return BadRequest("Invalid override data.");
            }

            var gameDef = await _context.BingoGameDefinitions
                .Include(g => g.Sheet)
                .FirstOrDefaultAsync(g => g.Id == dto.GameDefinitionId && g.IsActive && !g.IsDeleted);

            if (gameDef == null)
            {
                return NotFound("Game definition not found.");
            }

            int oldGoal = gameDef.CurrentProgressiveBallGoal;
            gameDef.CurrentProgressiveBallGoal = dto.NewBallGoal;
            gameDef.DateLastProgressed = DateTime.UtcNow.Date;
            gameDef.ModifiedAt = DateTime.UtcNow;
            gameDef.ModifiedBy = User.Identity?.Name ?? "Staff";

            await _context.SaveChangesAsync();

            // Log audit
            var username = User.Identity?.Name ?? "Staff";
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Username == username);
            
            var audit = new AuditLogEntry
            {
                TimestampUtc = DateTime.UtcNow,
                PerformedByUserId = user?.UserId,
                Action = "Override Progressive Ball Goal",
                Details = $"Manual override for '{gameDef.Sheet?.ColorName} - {gameDef.GameName}'. Changed current goal from {oldGoal} to {dto.NewBallGoal}.",
                PageUrl = "/admin/bingo/setup"
            };
            await _auditLogRepository.InsertAsync(audit);

            return Ok(new { success = true, newGoal = dto.NewBallGoal });
        }

        [HttpGet("progressive/history/{gameName}/{sheetColor}")]
        public async Task<ActionResult<IEnumerable<ProgressiveHistoryDto>>> GetProgressiveHistory(string gameName, string sheetColor)
        {
            var history = new List<ProgressiveHistoryDto>();

            // 1. Get Game Entries where balls were recorded
            var entries = await _context.BingoGameEntries
                .Include(e => e.Session)
                .Where(e => e.GameName == gameName 
                            && e.SheetColor == sheetColor 
                            && e.BallsCalled > 0 
                            && e.Session != null 
                            && !e.Session.IsDeleted 
                            && !e.IsDeleted)
                .OrderByDescending(e => e.Session!.SessionDate)
                .Take(20)
                .ToListAsync();

            foreach (var e in entries)
            {
                history.Add(new ProgressiveHistoryDto
                {
                    Timestamp = e.Session!.SessionDate,
                    Type = "Session Play",
                    Value = e.BallsCalled,
                    Details = $"Balls pulled: {e.BallsCalled}. Prize paid: {e.PrizePaid:C2}.",
                    PerformedBy = e.CreatedBy ?? "System"
                });
            }

            // 2. Get Audit logs for this game (max 100 recent)
            try
            {
                var auditResult = await _auditLogRepository.GetAuditLogsAsync(
                    "Override Progressive Ball Goal",
                    $"'{sheetColor} - {gameName}'",
                    null, null, null, 1, 100
                );

                if (auditResult?.Items != null)
                {
                    foreach (var a in auditResult.Items)
                    {
                        // Parse new goal from details: "...Changed current goal from X to Y."
                        int goalVal = 0;
                        var parts = a.Details?.Split("to ");
                        if (parts != null && parts.Length > 1 && int.TryParse(parts[1].TrimEnd('.'), out var g))
                        {
                            goalVal = g;
                        }

                        history.Add(new ProgressiveHistoryDto
                        {
                            Timestamp = a.TimestampUtc.ToLocalTime(),
                            Type = "Manual Override",
                            Value = goalVal,
                            Details = a.Details ?? "Manual override applied.",
                            PerformedBy = a.PerformedByDisplayName
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BINGO] Error fetching audit logs: {ex.Message}");
            }

            // Return sorted by date descending
            return Ok(history.OrderByDescending(h => h.Timestamp).Take(25));
        }
    }

    public class ProgressiveOverrideDto
    {
        public int GameDefinitionId { get; set; }
        public int NewBallGoal { get; set; }
    }

    public class ProgressiveHistoryDto
    {
        public DateTime Timestamp { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Details { get; set; } = string.Empty;
        public string PerformedBy { get; set; } = string.Empty;
    }
}
