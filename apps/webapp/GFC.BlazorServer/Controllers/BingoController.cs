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
                PayoutTiers = tiers
            });
        }

        [HttpPost("session")]
        public async Task<IActionResult> SubmitSession([FromBody] BingoSession session)
        {
            if (session == null) return BadRequest();

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

            _context.BingoSessions.Add(session);
            await _context.SaveChangesAsync();

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
