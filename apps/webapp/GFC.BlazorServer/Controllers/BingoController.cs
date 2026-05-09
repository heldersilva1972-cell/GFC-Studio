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
            session.Status = "Submitted";

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

            // [DE-DUPE] Check if a session already exists for this date.
            // If it does, we remove the old one and its entries before saving the fresh data.
            // This prevents the "multiple rows for one day" issue during sync retries.
            var existing = await _context.BingoSessions
                .Include(s => s.GameEntries)
                .Include(s => s.AdmissionEntries)
                .FirstOrDefaultAsync(s => s.SessionDate.Date == session.SessionDate.Date);

            if (existing != null)
            {
                Console.WriteLine($"[BINGO] Updating existing session for {session.SessionDate:yyyy-MM-dd} (Replacing old record)");
                _context.BingoSessions.Remove(existing);
                await _context.SaveChangesAsync();
            }

            _context.BingoSessions.Add(session);
            await _context.SaveChangesAsync();
            Console.WriteLine($"[BINGO] ✓ Session for {session.SessionDate:yyyy-MM-dd} saved successfully. ID: {session.Id}");

            // Clean up existing transactions for this session if it's an update
            var existingTransactions = await _context.BingoLotteryTransactions
                .Where(t => t.SessionId == session.Id)
                .ToListAsync();
            if (existingTransactions.Any())
            {
                _context.BingoLotteryTransactions.RemoveRange(existingTransactions);
                await _context.SaveChangesAsync();
            }

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
                    Description = $"Lottery tax for session on {session.SessionDate:MM/dd/yyyy}",
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
