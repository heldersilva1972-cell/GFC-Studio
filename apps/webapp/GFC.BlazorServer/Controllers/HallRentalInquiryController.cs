// [MODIFIED]
using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using GFC.Core.DTOs;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/HallRentalInquiry")]
    public class HallRentalInquiryController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly INotificationService _notificationService;

        public HallRentalInquiryController(IRentalService rentalService, INotificationService notificationService)
        {
            _rentalService = rentalService;
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HallRentalRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            // Final submission
            await _rentalService.CreateRentalRequestAsync(request);

            // Send confirmation email
            await _notificationService.SendRentalConfirmationEmailAsync(request);

            return Ok();
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] JsonElement formData)
        {
            var inquiry = await _rentalService.SaveInquiryAsync(formData.ToString());

            var resumeUrl = Url.Action("Resume", "HallRentalInquiry", new { token = inquiry.ResumeToken }, Request.Scheme);

            // Extract email from formData
            var email = formData.GetProperty("email").GetString();

            if (!string.IsNullOrEmpty(email))
            {
                var subject = "GFC Hall Rental - Save & Resume";
                var body = $"You can resume your hall rental application by clicking this link: {resumeUrl}";
                await _notificationService.SendEmailAsync(email, subject, body);
            }

            return Ok(new { resumeToken = inquiry.ResumeToken });
        }

        [HttpGet("resume/{token}")]
        public async Task<IActionResult> Resume(string token)
        {
            var inquiry = await _rentalService.GetInquiryAsync(token);
            if (inquiry == null)
            {
                return NotFound();
            }
            return Ok(JsonDocument.Parse(inquiry.FormData));
        }
    }
}
