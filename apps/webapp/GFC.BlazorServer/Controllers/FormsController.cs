// [NEW]
using GFC.BlazorServer.Services;
using GFC.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormsController(IFormService formService)
        {
            _formService = formService;
        }

        // GET: api/forms
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetForms()
        {
            var forms = await _formService.GetAllFormsAsync();
            return Ok(forms);
        }

        // GET: api/forms/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForm(int id)
        {
            var form = await _formService.GetFormByIdAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            return Ok(form);
        }

        // POST: api/forms
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateForm([FromBody] Form form)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdForm = await _formService.CreateFormAsync(form);
            return CreatedAtAction(nameof(GetForm), new { id = createdForm.Id }, createdForm);
        }

        // PUT: api/forms/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateForm(int id, [FromBody] Form form)
        {
            if (id != form.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }
            await _formService.UpdateFormAsync(form);
            return NoContent();
        }

        // DELETE: api/forms/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteForm(int id)
        {
            await _formService.DeleteFormAsync(id);
            return NoContent();
        }

        // POST: api/forms/{formId}/submit
        [HttpPost("{formId}/submit")]
        public async Task<IActionResult> Submit(int formId)
        {
            var form = await Request.ReadFormAsync();
            var submissionData = form["submissionData"];
            var submission = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(submissionData);

            foreach (var file in form.Files)
            {
                var sanitizedFileName = System.Text.RegularExpressions.Regex.Replace(file.FileName, @"[^a-zA-Z0-9_.-]", "");
                var uniqueFileName = $"{Guid.NewGuid()}_{sanitizedFileName}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "Uploads", uniqueFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                submission[file.Name] = $"/uploads/{uniqueFileName}";
            }

            var formSubmission = new FormSubmission
            {
                FormId = formId,
                SubmissionData = System.Text.Json.JsonSerializer.Serialize(submission)
            };

            await _formService.SaveSubmissionAsync(formSubmission);
            return Ok(new { message = "Submission received successfully." });
        }

        // GET: api/forms/{formId}/submissions
        [HttpGet("{formId}/submissions")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSubmissions(int formId)
        {
            var submissions = await _formService.GetSubmissionsByFormIdAsync(formId);
            return Ok(submissions);
        }
    }
}
