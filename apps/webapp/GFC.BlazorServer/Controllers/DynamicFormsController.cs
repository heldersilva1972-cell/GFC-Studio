// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/forms")]
    public class DynamicFormsController : ControllerBase
    {
        private readonly IFormBuilderService _formBuilderService;

        public DynamicFormsController(IFormBuilderService formBuilderService)
        {
            _formBuilderService = formBuilderService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetForm(string name)
        {
            var form = await _formBuilderService.GetFormByNameAsync(name);
            if (form == null)
            {
                return NotFound();
            }
            return Ok(form);
        }

        [HttpPost]
        public async Task<IActionResult> SaveForm([FromBody] DynamicForm form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _formBuilderService.SaveFormAsync(form);
            return NoContent();
        }
    }
}
