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
    }
}
