using GFC.Core.Interfaces;
using GFC.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/mobile-reporting")]
public class MobileReportingController : ControllerBase
{
    private readonly IMobileReportingService _reportingService;
    private readonly IVersionService _versionService;
    private readonly IDeviceTrustService _deviceTrustService;

    public MobileReportingController(
        IMobileReportingService reportingService, 
        IVersionService versionService,
        IDeviceTrustService deviceTrustService)
    {
        _reportingService = reportingService;
        _versionService = versionService;
        _deviceTrustService = deviceTrustService;
    }

    [HttpGet("device")]
    public async Task<IActionResult> GetDevice(string token)
    {
        var device = await _deviceTrustService.GetDeviceByTokenAsync(token);
        return device != null ? Ok(device) : NotFound();
    }

    [HttpGet("device/auto-login")]
    public async Task<IActionResult> AutoLogin(string token, string username)
    {
        var userId = await _deviceTrustService.ValidateStationAutoLoginAsync(token, username);
        return userId != null ? Ok(userId) : Unauthorized();
    }

    [HttpGet("version")]
    public ActionResult<string> GetVersion()
    {
        return _versionService.GetFullVersion();
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
}
