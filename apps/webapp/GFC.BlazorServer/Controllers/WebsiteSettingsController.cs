// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebsiteSettingsController : ControllerBase
    {
        private readonly IWebsiteSettingsService _settingsService;

        public WebsiteSettingsController(IWebsiteSettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<ActionResult<WebsiteSettings>> Get()
        {
            var settings = await _settingsService.GetWebsiteSettingsAsync();
            return Ok(settings);
        }

        [HttpPost("toggle-accessibility")]
        public async Task<IActionResult> ToggleAccessibility()
        {
            var settings = await _settingsService.GetWebsiteSettingsAsync();
            settings.HighAccessibilityMode = !settings.HighAccessibilityMode;
            await _settingsService.UpdateWebsiteSettingsAsync(settings);
            return Ok(settings);
        }

        [HttpPost("toggle-large-text")]
        public async Task<IActionResult> ToggleLargeText()
        {
            var settings = await _settingsService.GetWebsiteSettingsAsync();
            settings.LargeTextMode = !settings.LargeTextMode;
            await _settingsService.UpdateWebsiteSettingsAsync(settings);
            return Ok(settings);
        }
    }
}
