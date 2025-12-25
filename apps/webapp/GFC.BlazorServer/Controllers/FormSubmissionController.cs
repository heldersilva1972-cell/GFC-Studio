// [NEW]
using GFC.Core.Models;
using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using System.Web;
using System.Linq;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormSubmissionController : ControllerBase
    {
        private readonly IFormService _formService;

        public FormSubmissionController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] FormSubmission submission)
        {
            if (submission == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Sanitize all string inputs in the submission data to prevent XSS.
            try
            {
                var submissionData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(submission.SubmissionData);
                var sanitizedData = new Dictionary<string, object>();

                foreach (var key in submissionData.Keys)
                {
                    var value = submissionData[key];
                    if (value.ValueKind == JsonValueKind.String)
                    {
                        sanitizedData[key] = HttpUtility.HtmlEncode(value.GetString());
                    }
                    else
                    {
                        sanitizedData[key] = value;
                    }
                }
                submission.SubmissionData = JsonSerializer.Serialize(sanitizedData);
            }
            catch (JsonException)
            {
                return BadRequest("Invalid submission data format.");
            }

            var createdSubmission = await _formService.CreateSubmissionAsync(submission);
            return Ok(createdSubmission);
        }

        [HttpPost("save-for-later")]
        public async Task<IActionResult> SaveForLater([FromBody] SaveForLaterRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.FormData) || string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("Invalid request data.");
            }

            var inquiry = await _formService.SaveRentalInquiryForLaterAsync(request.FormData, request.Email);
            return Ok(new { message = "Save link sent successfully." });
        }
    }

    public class SaveForLaterRequest
    {
        public string FormData { get; set; }
        public string Email { get; set; }
    }
}
