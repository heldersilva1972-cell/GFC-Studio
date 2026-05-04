using GFC.Core.Models;
using GFC.Core.Models.Diagnostics;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/mobile-diagnostics")]
[Authorize] // Access based on user permissions checked in UI, but API is protected
public class MobileDiagnosticsController : ControllerBase
{
    private readonly IDiagnosticsService _diagnosticsService;

    public MobileDiagnosticsController(IDiagnosticsService diagnosticsService)
    {
        _diagnosticsService = diagnosticsService;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<SystemDiagnosticsInfo>> GetStats()
    {
        try
        {
            var stats = await _diagnosticsService.GetDiagnosticsAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error fetching system diagnostics: {ex.Message}");
        }
    }

    [HttpPost("test-db")]
    public async Task<ActionResult<DiagnosticActionResult>> TestDatabase()
    {
        try
        {
            var result = await _diagnosticsService.TestDatabaseConnectionAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error testing database connection: {ex.Message}");
        }
    }
}
