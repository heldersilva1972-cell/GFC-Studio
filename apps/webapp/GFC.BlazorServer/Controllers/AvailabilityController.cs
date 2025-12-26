// [NEW]
using GFC.BlazorServer.Services;
using GFC.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public AvailabilityController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UnavailableDateDto>>> Get()
        {
            Console.WriteLine("=== AVAILABILITY CONTROLLER GET CALLED ===");
            var unavailableDates = await _rentalService.GetUnavailableDatesAsync();
            Console.WriteLine($"=== RETURNING {unavailableDates.Count} DATES ===");
            foreach (var d in unavailableDates)
            {
                Console.WriteLine($"Date: {d.Date}, Status: {d.Status}, EventType: {d.EventType}, EventTime: {d.EventTime}");
            }
            return Ok(unavailableDates);
        }

        [HttpGet("test")]
        public async Task<ActionResult> Test()
        {
            var requests = await _rentalService.GetRentalRequestsAsync();
            var result = requests.Select(r => new {
                r.Id,
                r.EventType,
                r.StartTime,
                r.EndTime,
                r.Status
            });
            return Ok(result);
        }
    }
}
