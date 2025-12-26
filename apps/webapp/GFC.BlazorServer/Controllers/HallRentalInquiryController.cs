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
        public async Task<IActionResult> Post([FromBody] JsonElement body)
        {
            try
            {
                var request = new HallRentalRequest();

                // Safe extraction of properties
                if (body.TryGetProperty("requesterName", out var nameProp)) 
                    request.RequesterName = nameProp.GetString() ?? string.Empty;
                    
                if (body.TryGetProperty("requesterEmail", out var emailProp)) 
                    request.RequesterEmail = emailProp.GetString() ?? string.Empty;
                    
                if (body.TryGetProperty("requesterPhone", out var phoneProp)) 
                    request.RequesterPhone = phoneProp.GetString() ?? string.Empty;

                if (body.TryGetProperty("requestedDate", out var dateProp) && dateProp.TryGetDateTime(out var dateVal))
                {
                    request.RequestedDate = dateVal;
                    // FIX 1: Populate EventDate to prevent '0001-01-01' SQL range error
                    request.EventDate = dateVal; 
                }
                else
                {
                    // Fallback to safe date if missing
                    request.RequestedDate = DateTime.UtcNow;
                    request.EventDate = DateTime.UtcNow;
                }

                if (body.TryGetProperty("status", out var statusProp))
                    request.Status = statusProp.GetString() ?? "Pending";

                // Parse event details
                if (body.TryGetProperty("eventType", out var eventTypeProp))
                    request.EventType = eventTypeProp.GetString();

                if (body.TryGetProperty("startTime", out var startTimeProp))
                {
                    var timeStr = startTimeProp.GetString();
                    request.StartTime = ConvertTo12HourFormat(timeStr);
                }

                if (body.TryGetProperty("endTime", out var endTimeProp))
                {
                    var timeStr = endTimeProp.GetString();
                    request.EndTime = ConvertTo12HourFormat(timeStr);
                }

                // CRITICAL: Capture the full application details
                if (body.TryGetProperty("eventDetails", out var detailsProp))
                {
                    var details = detailsProp.GetString();
                    request.InternalNotes = $"--- ONLINE APPLICATION DETAILS ---\n{details}";
                }

                // Default required fields
                request.ApplicantName = !string.IsNullOrWhiteSpace(request.RequesterName) ? request.RequesterName : "Unknown Applicant";
                request.GuestCount = 0; 
                request.MemberStatus = false; 
                request.RulesAgreed = true; 

                // FIX 2: Ensure we don't save invalid dates
                if (request.EventDate.Year < 1753) request.EventDate = DateTime.UtcNow;
                if (request.RequestedDate.Year < 1753) request.RequestedDate = DateTime.UtcNow;

                // Final submission to Database
                await _rentalService.CreateRentalRequestAsync(request);

                // FIX 3: Wrap Email in try-catch to prevent crash if SMTP is down
                try 
                {
                    await _notificationService.SendRentalConfirmationEmailAsync(request);
                }
                catch (System.Exception emailEx)
                {
                    // Log email failure but don't fail the request
                    System.Console.WriteLine($"[WARNING] Automated email failed: {emailEx.Message}");
                }

                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred submitting the application.", details = ex.Message });
            }
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

        private string? ConvertTo12HourFormat(string? time24)
        {
            if (string.IsNullOrEmpty(time24)) return time24;
            
            // If already in 12-hour format (contains AM/PM), return as-is
            if (time24.Contains("AM") || time24.Contains("PM") || time24.Contains("am") || time24.Contains("pm"))
                return time24;
            
            // Try to parse 24-hour format (HH:mm)
            if (TimeSpan.TryParse(time24, out var timeSpan))
            {
                var hours = timeSpan.Hours;
                var minutes = timeSpan.Minutes;
                var period = hours >= 12 ? "PM" : "AM";
                
                // Convert to 12-hour format
                if (hours == 0) hours = 12; // Midnight
                else if (hours > 12) hours -= 12; // Afternoon/Evening
                
                return $"{hours}:{minutes:D2} {period}";
            }
            
            // If parsing fails, return original
            return time24;
        }
    }
}
