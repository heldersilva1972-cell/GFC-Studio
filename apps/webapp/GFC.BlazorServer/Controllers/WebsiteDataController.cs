// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebsiteDataController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        private readonly IAvailabilityService _availabilityService;
        private readonly IPublicReviewService _publicReviewService;
        private readonly IRentalService _rentalService;
        private readonly GFC.Core.Interfaces.IWebsiteSettingsService _websiteSettingsService;

        public WebsiteDataController(
            IAnnouncementService announcementService,
            IAvailabilityService availabilityService,
            IPublicReviewService publicReviewService,
            IRentalService rentalService,
            GFC.Core.Interfaces.IWebsiteSettingsService websiteSettingsService)
        {
            _announcementService = announcementService;
            _availabilityService = availabilityService;
            _publicReviewService = publicReviewService;
            _rentalService = rentalService;
            _websiteSettingsService = websiteSettingsService;
        }

        [HttpGet("Announcements")]
        public async Task<IActionResult> GetAnnouncements()
        {
            var announcements = await _announcementService.GetActiveAnnouncementsAsync();
            return Ok(announcements);
        }

        [HttpGet("Availability")]
        public async Task<IActionResult> GetAvailability()
        {
            var dates = await _availabilityService.GetReservedDatesAsync();
            return Ok(dates);
        }

        [HttpGet("PublicReviews")]
        public async Task<IActionResult> GetPublicReviews()
        {
            var reviews = await _publicReviewService.GetApprovedAndFeaturedReviewsAsync();
            return Ok(reviews);
        }

        [HttpGet("WebsiteSettings")]
        public async Task<IActionResult> GetWebsiteSettings()
        {
            var settings = await _websiteSettingsService.GetWebsiteSettingsAsync();
            return Ok(settings);
        }

        [HttpPost("HallRentalInquiry")]
        public async Task<IActionResult> PostHallRentalInquiry([FromBody] HallRentalInquiry inquiry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _rentalService.CreateRentalInquiryAsync(inquiry);
            return Ok();
        }
    }
}
