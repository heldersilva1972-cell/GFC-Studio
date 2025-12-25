// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using GFC.BlazorServer.Services;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/studio")]
    public class StudioApiController : ControllerBase
    {
        private readonly GFC.BlazorServer.Services.IStudioService _studioService;
        private readonly ILogger<StudioApiController> _logger;

        public StudioApiController(GFC.BlazorServer.Services.IStudioService studioService, ILogger<StudioApiController> logger)
        {
            _studioService = studioService;
            _logger = logger;
        }

        [HttpGet("page/{pageId}")]
        public async Task<IActionResult> GetPageSections(int pageId)
        {
            try
            {
                var draft = await _studioService.GetLatestDraftAsync(pageId);
                if (draft == null || string.IsNullOrEmpty(draft.ContentJson))
                {
                    return Ok(new List<StudioSection>());
                }

                var sections = JsonSerializer.Deserialize<List<StudioSection>>(draft.ContentJson);
                return Ok(sections);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error getting page sections for page {PageId}", pageId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
