// [MODIFIED]
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Services;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly IStudioService _studioService;
        private readonly GfcDbContext _context;
        private readonly ILogger<ContentController> _logger;

        public ContentController(IStudioService studioService, GfcDbContext context, ILogger<ContentController> logger)
        {
            _studioService = studioService;
            _context = context;
            _logger = logger;
        }

        [HttpGet("page/{slug}")]
        public async Task<IActionResult> GetPage(string slug)
        {
            var page = await _studioService.GetPublishedPageAsync(slug);
            if (page == null)
            {
                return NotFound();
            }
            return Ok(page);
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.EventPromotions
                .Select(e => new EventDto
                {
                    Title = e.Title,
                    EventDate = e.EventDate,
                    Description = "" // No description field in EventPromotion model
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("rental-availability")]
        public async Task<IActionResult> GetRentalAvailability()
        {
            var bookedDates = await _context.HallRentals
                .Where(r => r.Status == "Approved")
                .Select(r => new RentalAvailabilityDto
                {
                    Date = r.EventDate,
                    IsAvailable = false
                })
                .ToListAsync();

            return Ok(bookedDates);
        }

        [HttpPost("contact")]
        public IActionResult SubmitContactForm([FromBody] ContactFormDto contactForm)
        {
            _logger.LogInformation("New contact form submission: {Name} - {Email}", contactForm.Name, contactForm.Email);
            // In a real application, you would save this to a database or send an email.
            return Ok();
        }
    }
}
