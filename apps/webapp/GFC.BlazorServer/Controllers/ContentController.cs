// [NEW]
using GFC.BlazorServer.Services;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly IStudioService _studioService;

        public ContentController(IStudioService studioService)
        {
            _studioService = studioService;
        }

        [HttpGet("page/{slug}")]
        public async Task<IActionResult> GetPage(string slug)
        {
            var page = await _studioService.GetPublishedPageAsync(slug);
            if (page == null)
            {
                return NotFound();
            }
            return Ok(page);
        }
    }
}
