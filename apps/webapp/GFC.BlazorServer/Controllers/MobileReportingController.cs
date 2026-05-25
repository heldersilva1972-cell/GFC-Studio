using GFC.Core.Interfaces;
using GFC.Core.DTOs;
using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/mobile-reporting")]
[Microsoft.AspNetCore.Cors.EnableCors("GfcEcosystemPolicy")]
public class MobileReportingController : ControllerBase
{
    private readonly IMobileReportingService _reportingService;
    private readonly IVersionService _versionService;
    private readonly IDeviceTrustService _deviceTrustService;
    private readonly IBlazorSystemSettingsService _settingsService;
    private readonly IConfiguration _configuration;

    public MobileReportingController(
        IMobileReportingService reportingService, 
        IVersionService versionService,
        IDeviceTrustService deviceTrustService,
        IBlazorSystemSettingsService settingsService,
        IConfiguration configuration)
    {
        _reportingService = reportingService;
        _versionService = versionService;
        _deviceTrustService = deviceTrustService;
        _settingsService = settingsService;
        _configuration = configuration;
    }

    [HttpGet("version")]
    public ActionResult<string> GetVersion()
    {
        return _versionService.GetMobileVersion();
    }

    [HttpGet("pos-version")]
    public ActionResult<string> GetPosVersion()
    {
        return _versionService.GetPosVersion();
    }

    [HttpPost("sync-version")]
    public async Task<IActionResult> SyncVersion([FromQuery] string project, [FromQuery] string version, [FromQuery] string apiKey)
    {
        var secret = _configuration["MobileSync:SyncApiKey"];
        if (string.IsNullOrEmpty(secret) || apiKey != secret)
        {
            return Unauthorized("Invalid Sync API Key");
        }

        var settings = await _settingsService.GetAsync();
        if (settings == null) return NotFound("System Settings not found");

        if (project.ToUpper() == "POS")
        {
            settings.PosRevision = version;
        }
        else if (project.ToUpper() == "MOBILE")
        {
            settings.MobileRevision = version;
        }
        else if (project.ToUpper() == "WEBAPP")
        {
            settings.WebappRevision = version;
        }

        await _settingsService.UpdateAsync(settings);

        Console.WriteLine($"[SYNC] {project} version updated to {version} in database via remote sync.");
        return Ok(new { Message = "Version updated successfully" });
    }

    [HttpGet("data")]
    public async Task<ActionResult<MobileShiftData>> GetData(DateTime date, string shiftType, bool isRental)
    {
        return await _reportingService.GetShiftReportDataAsync(date, shiftType, isRental);
    }

    [HttpGet("carryover")]
    public async Task<ActionResult<decimal>> GetCarryover(DateTime date, string shiftType)
    {
        return await _reportingService.GetCarryoverCashAsync(date, shiftType);
    }

    [HttpGet("bag-debt")]
    public async Task<ActionResult<decimal>> GetBagDebt(DateTime date)
    {
        return await _reportingService.GetCumulativeBagDebtAsync(date);
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save([FromQuery] string username, [FromBody] MobileShiftData data)
    {
        var result = await _reportingService.SaveShiftReportAsync(data, username);
        return result ? Ok() : BadRequest();
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromQuery] string username, [FromBody] MobileShiftData data)
    {
        var result = await _reportingService.SubmitFinalReportAsync(data, username);
        return result ? Ok() : BadRequest();
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DailyShiftSummary>> GetSummary(DateTime date)
    {
        return await _reportingService.GetDailySummaryAsync(date);
    }

    [HttpGet("lottery-rate")]
    public async Task<ActionResult<LotteryCommissionRate>> GetLotteryRate(int year)
    {
        return await _reportingService.GetLotteryRateAsync(year);
    }

    [HttpGet("settings")]
    public async Task<ActionResult<SystemSettings>> GetSettings()
    {
        var settings = await _settingsService.GetAsync();
        if (settings == null) return NotFound("System Settings not found");
        return Ok(settings);
    }

    [HttpPost("settings")]
    public async Task<IActionResult> UpdateSettings([FromBody] SystemSettings settings)
    {
        if (settings == null) return BadRequest("Settings data is required");
        await _settingsService.UpdateAsync(settings);
        return Ok();
    }
}
