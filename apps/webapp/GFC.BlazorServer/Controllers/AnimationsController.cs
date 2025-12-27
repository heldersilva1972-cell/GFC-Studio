// [MODIFIED]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimationsController : ControllerBase
    {
        private readonly IAnimationService _animationService;

        public AnimationsController(IAnimationService animationService)
        {
            _animationService = animationService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnimationDefinition>> GetAnimationDefinition(string id)
        {
            var animation = await _animationService.GetAnimationByIdAsync(id);

            if (animation == null)
            {
                return NotFound();
            }

            return Ok(animation);
        }
    }
}
