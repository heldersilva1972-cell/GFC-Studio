using GFC.Core.Models;
using GFC.Core.DTOs;
using GFC.BlazorServer.Data;
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

        public BingoController(GfcDbContext context)
        {
            _context = context;
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

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<BingoSession>>> GetHistory()
        {
            return await _context.BingoSessions
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.SessionDate)
                .Take(20)
                .ToListAsync();
        }
    }
}
