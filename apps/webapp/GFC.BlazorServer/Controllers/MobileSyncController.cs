using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GFC.BlazorServer.Controllers;

/// <summary>
/// Dedicated controller for mobile synchronization tasks, including usage tracking.
/// Satisfies client-side requests from GFC.Mobile and GFC.Pos.Mobile to prevent 404/500 errors.
/// </summary>
[ApiController]
[Route("api/sync")]
[Authorize]
public class MobileSyncController : ControllerBase
{
    private readonly ILogger<MobileSyncController> _logger;

    public MobileSyncController(ILogger<MobileSyncController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Returns the top used pages for a user to populate the 'Most Used' section of the mobile hub.
    /// </summary>
    [HttpGet("usage/top")]
    public IActionResult GetTopUsage(int userId, int count = 3)
    {
        // For now, return a default set of pages to keep the UI clean and responsive.
        // This could be enhanced to query the actual AuditLogRepository in the future.
        var defaultPages = new List<string> { "End of shift sales", "Key Cards", "Liquor HUB", "Dues & Payments" };
        
        _logger.LogDebug("Sync: Returning top {Count} usage pages for user {UserId}", count, userId);
        
        return Ok(defaultPages.Take(count).ToList());
    }

    /// <summary>
    /// Tracks page usage events from mobile clients.
    /// </summary>
    [HttpPost("usage/track")]
    public IActionResult TrackUsage([FromBody] UsageTrackDto trackData)
    {
        // Simple fire-and-forget tracking. 
        // We log it to the server console for now; in production this would hit the Audit database.
        _logger.LogInformation("Sync: Tracked usage for user {UserId} on page {Page}", trackData.UserId, trackData.PageIdentifier);
        
        return Ok();
    }
}

public class UsageTrackDto
{
    public int UserId { get; set; }
    public string? PageIdentifier { get; set; }
}
