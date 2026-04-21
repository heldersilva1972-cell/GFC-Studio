using GFC.Core.Interfaces;
using GFC.Core.DTOs;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Controllers;

[ApiKey]
[ApiController]
[Route("api/sync")]
public class MobileSyncController : ControllerBase
{
    private readonly IMobileReportingService _reportingService;
    private readonly IDbContextFactory<GfcDbContext> _dbFactory;
    private readonly ILogger<MobileSyncController> _logger;

    public MobileSyncController(
        IMobileReportingService reportingService,
        IDbContextFactory<GfcDbContext> dbFactory,
        ILogger<MobileSyncController> logger)
    {
        _reportingService = reportingService;
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Synchronizes a batch of shift data from the mobile client to the server.
    /// </summary>
    [HttpPost("shifts")]
    public async Task<IActionResult> SyncShifts([FromBody] List<MobileShiftData> shifts)
    {
        if (shifts == null || !shifts.Any())
        {
            return BadRequest("No shift data provided.");
        }

        var username = User.Identity?.Name ?? "MobileSyncUser";
        int successCount = 0;

        foreach (var shift in shifts)
        {
            try
            {
                var result = await _reportingService.SaveShiftReportAsync(shift, username);
                if (result) successCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to sync shift for date {Date} and type {Type}", shift.Date, shift.ShiftType);
            }
        }

        return Ok(new { Total = shifts.Count, Synced = successCount });
    }

    [HttpGet("shifts")]
    public async Task<ActionResult<MobileShiftData>> GetShiftData([FromQuery] DateTime date, [FromQuery] string shiftType, [FromQuery] bool isRental)
    {
        try
        {
            var data = await _reportingService.GetShiftReportDataAsync(date, shiftType, isRental);
            if (data == null) return NotFound();
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching shift data for {Date} {Type}", date, shiftType);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DailyShiftSummary>> GetDailySummary([FromQuery] DateTime date)
    {
        try
        {
            var summary = await _reportingService.GetDailySummaryAsync(date);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching daily summary for {Date}", date);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("bag-debt")]
    public async Task<ActionResult<decimal>> GetBagDebt([FromQuery] DateTime date)
    {
        try
        {
            var debt = await _reportingService.GetCumulativeBagDebtAsync(date);
            return Ok(debt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching bag debt for {Date}", date);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("version")]
    public IActionResult GetVersion()
    {
        return Ok("1.5.2-Production");
    }

    /// <summary>
    /// Returns the latest ShiftReport data to allow the client to verify state parity.
    /// </summary>
    [HttpGet("latest")]
    public async Task<ActionResult<ShiftReport>> GetLatest()
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        
        var latestReport = await db.ShiftReports.AsNoTracking()
            .OrderByDescending(r => r.SubmittedAt)
            .FirstOrDefaultAsync();

        if (latestReport == null)
        {
            return NotFound("No shift reports found in the database.");
        }

        return Ok(latestReport);
    }
}


