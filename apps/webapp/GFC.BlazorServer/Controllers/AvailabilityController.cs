// [NEW]
using GFC.BlazorServer.Services;
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
        public async Task<ActionResult<List<DateTime>>> Get()
        {
            var unavailableDates = await _rentalService.GetUnavailableDatesAsync();
            return Ok(unavailableDates);
        }
    }
}
