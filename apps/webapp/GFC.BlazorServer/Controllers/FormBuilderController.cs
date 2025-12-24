// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormBuilderController : ControllerBase
    {
        private readonly GfcDbContext _context;

        public FormBuilderController(GfcDbContext context)
        {
            _context = context;
        }

        // GET: api/FormBuilder/forms
        [HttpGet("forms")]
        public async Task<IActionResult> GetForms()
        {
            var forms = await _context.Forms.Include(f => f.Fields).ToListAsync();
            return Ok(forms);
        }

        // GET: api/FormBuilder/forms/{id}
        [HttpGet("forms/{id}")]
        public async Task<IActionResult> GetForm(int id)
        {
            var form = await _context.Forms.Include(f => f.Fields).FirstOrDefaultAsync(f => f.Id == id);
            if (form == null)
            {
                return NotFound();
            }
            return Ok(form);
        }

        // POST: api/FormBuilder/forms
        [HttpPost("forms")]
        public async Task<IActionResult> CreateForm([FromBody] Form form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetForm), new { id = form.Id }, form);
        }

        // POST: api/FormBuilder/submissions
        [HttpPost("submissions")]
        public async Task<IActionResult> CreateSubmission([FromBody] FormSubmission submission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            // Here you would typically trigger an email notification
            // For now, we'll just return a success response.

            return Ok(new { message = "Submission received successfully." });
        }
    }
}
