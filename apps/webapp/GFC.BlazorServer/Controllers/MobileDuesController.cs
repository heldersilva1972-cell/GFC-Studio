using GFC.Core.Interfaces;
using GFC.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/mobile-dues")]
[Authorize]
public class MobileDuesController : ControllerBase
{
    private readonly IDuesInsightService _duesInsightService;

    public MobileDuesController(IDuesInsightService duesInsightService)
    {
        _duesInsightService = duesInsightService;
    }

    [HttpGet]
    [HttpGet("list")]
    public async Task<ActionResult<List<DuesListItemDto>>> GetDuesList(int year, bool paidTab)
    {
        try
        {
            var list = (await _duesInsightService.GetDuesAsync(year, paidTab)).ToList();
            return Ok(list);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                Message = ex.Message, 
                StackTrace = ex.StackTrace,
                InnerMessage = ex.InnerException?.Message 
            });
        }
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DuesSummaryDto>> GetSummary(int year)
    {
        try
        {
            var summary = await _duesInsightService.GetSummaryAsync(year);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { 
                Message = ex.Message, 
                StackTrace = ex.StackTrace,
                InnerMessage = ex.InnerException?.Message 
            });
        }
    }
}
