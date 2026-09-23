using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/rentals")]
    public class RentalsApiController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly GFC.Core.Interfaces.IWebsiteSettingsService _settingsService;

        public RentalsApiController(IRentalService rentalService, GFC.Core.Interfaces.IWebsiteSettingsService settingsService)
        {
            _rentalService = rentalService;
            _settingsService = settingsService;
        }

        public class GoogleFormWebhookPayload
        {
            public string? ApiKey { get; set; }
            public string? ApplicantName { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? Address { get; set; }
            public DateTime? EventDate { get; set; }
            public string? EventType { get; set; }
            public string? StartTime { get; set; }
            public string? EndTime { get; set; }
            public string? RoomSelected { get; set; }
            public bool IsClubMember { get; set; }
            public int GuestCount { get; set; }
            public bool BarService { get; set; }
            public bool KitchenAccess { get; set; }
            public bool AvEquipment { get; set; }
            public bool IsTest { get; set; } = false;
        }

        [HttpPost("google-form")]
        public async Task<IActionResult> IngestGoogleForm([FromBody] GoogleFormWebhookPayload payload)
        {
            if (payload == null)
            {
                return BadRequest(new { success = false, message = "Empty payload received." });
            }

            var settings = await _settingsService.GetWebsiteSettingsAsync();

            // Validate API Key if configured
            if (!string.IsNullOrEmpty(settings?.WebhookApiKey) && !string.Equals(settings.WebhookApiKey, payload.ApiKey, StringComparison.Ordinal))
            {
                return Unauthorized(new { success = false, message = "Invalid webhook API key." });
            }

            var request = new HallRentalRequest
            {
                ApplicantName = payload.ApplicantName ?? "Applicant",
                RequesterName = payload.ApplicantName ?? "Applicant",
                RequesterEmail = payload.Email ?? "",
                RequesterPhone = payload.Phone ?? "",
                RequesterAddress = payload.Address,
                EventDate = payload.EventDate ?? DateTime.Today.AddDays(14),
                RequestedDate = payload.EventDate ?? DateTime.Today.AddDays(14),
                EventType = payload.EventType ?? "Hall Rental",
                StartTime = payload.StartTime ?? "2:00 PM",
                EndTime = payload.EndTime ?? "7:00 PM",
                RoomSelected = string.IsNullOrEmpty(payload.RoomSelected) ? "Function Hall" : payload.RoomSelected,
                MemberStatus = payload.IsClubMember,
                RenterType = payload.IsClubMember ? "Member" : "Non-Member",
                GuestCount = payload.GuestCount > 0 ? payload.GuestCount : 50,
                BartenderRequested = payload.BarService,
                KitchenUsage = payload.KitchenAccess,
                AvEquipmentUsage = payload.AvEquipment,
                RulesAgreed = true,
                Status = "Pending",
                IsTestRecord = payload.IsTest || (settings?.EnableSandboxMode == true)
            };

            // Calculate pricing based on settings
            decimal basePrice = payload.IsClubMember ? (settings?.FunctionHallMemberRate ?? 300) : (settings?.FunctionHallNonMemberRate ?? 400);
            decimal addOns = 0;
            if (payload.BarService) addOns += (settings?.BartenderServiceFee ?? 100);
            if (payload.KitchenAccess) addOns += (settings?.KitchenFee ?? 0);
            if (payload.AvEquipment) addOns += (settings?.AvEquipmentFee ?? 0);

            request.TotalPrice = basePrice + addOns;
            request.SecurityDepositAmount = settings?.SecurityDepositAmount ?? 200;

            var created = await _rentalService.SubmitPublicRentalRequestAsync(request, request.IsTestRecord);

            return Ok(new
            {
                success = true,
                requestId = created.Id,
                status = created.Status,
                message = "Hall rental application ingested successfully."
            });
        }
    }
}
