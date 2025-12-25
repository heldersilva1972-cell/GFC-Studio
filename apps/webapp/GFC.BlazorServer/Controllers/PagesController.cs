// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagesController : ControllerBase
    {
        private readonly IStudioPageRepository _studioPageRepository;

        public PagesController(IStudioPageRepository studioPageRepository)
        {
            _studioPageRepository = studioPageRepository;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudioPage>> GetPage(int id)
        {
            var page = await _studioPageRepository.GetByIdAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return Ok(page);
        }
    }
}
