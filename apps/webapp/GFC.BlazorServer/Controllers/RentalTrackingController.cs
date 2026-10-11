using System;
using System.Threading.Tasks;
using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/rental-tracking")]
    [AllowAnonymous]
    public class RentalTrackingController : ControllerBase
    {
        private readonly IRentalAnalyticsService _analyticsService;

        public RentalTrackingController(IRentalAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpPost("ping")]
        public async Task<IActionResult> Ping([FromBody] RentalTrackingPingRequest request)
        {
            try
            {
                var ip = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
                }
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                await _analyticsService.RecordPingAsync(request, ip, userAgent);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
