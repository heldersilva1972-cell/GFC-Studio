using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/mobile-analytics")]
[Authorize] // Require authentication for analytics
public class MobileAnalyticsController : ControllerBase
{
    private readonly IFinancialAnalyticsService _financialService;

    public MobileAnalyticsController(IFinancialAnalyticsService financialService)
    {
        _financialService = financialService;
    }

    [HttpGet("years")]
    public async Task<ActionResult<List<int>>> GetAvailableYears()
    {
        try
        {
            var years = await _financialService.GetAvailableYearsAsync();
            return Ok(years);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching available years: {ex.Message}");
        }
    }

    [HttpPost("data")]
    public async Task<ActionResult<List<FinancialDataPoint>>> GetAggregatedData([FromBody] FinancialAnalyticsRequest request)
    {
        try
        {
            var data = await _financialService.GetAggregatedDataAsync(request);
            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching aggregated data: {ex.Message}");
        }
    }

    [HttpPost("summary")]
    public async Task<ActionResult<FinancialSummary>> GetSummary([FromBody] FinancialAnalyticsRequest request)
    {
        try
        {
            var summary = await _financialService.GetSummaryAsync(request);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching financial summary: {ex.Message}");
        }
    }
}
