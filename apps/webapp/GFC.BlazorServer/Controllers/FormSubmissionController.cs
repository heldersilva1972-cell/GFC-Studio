// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/form-submissions")]
    public class FormSubmissionController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly INotificationService _notificationService;

        public FormSubmissionController(IFormService formService, INotificationService notificationService)
        {
            _formService = formService;
            _notificationService = notificationService;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SavePartialSubmission([FromBody] FormSubmission submission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            submission.Status = "Incomplete";
            submission.ResumeToken = GenerateSecureToken();
            submission.SubmittedAt = DateTime.UtcNow;

            await _formService.SaveSubmissionAsync(submission);

            // Send email with resume link
            var resumeUrl = Url.Action("Resume", "HallRentals", new { token = submission.ResumeToken }, Request.Scheme);
            await _notificationService.SendFormResumeEmailAsync(submission.SubmitterEmail, resumeUrl);

            return Ok(new { message = "Your progress has been saved. A link to resume has been sent to your email." });
        }

        [HttpGet("resume/{token}")]
        public async Task<IActionResult> GetSubmissionByToken(string token)
        {
            var submission = await _formService.GetSubmissionByTokenAsync(token);
            if (submission == null)
            {
                return NotFound();
            }
            return Ok(submission);
        }

        private string GenerateSecureToken()
        {
            using var randomNumberGenerator = new RNGCryptoServiceProvider();
            var randomNumber = new byte[32];
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}
