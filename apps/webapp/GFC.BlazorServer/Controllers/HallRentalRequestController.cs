// [NEW]
using GFC.Core.Models;
using GFC.BlazorServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallRentalRequestController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public HallRentalRequestController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] HallRentalRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            await _rentalService.CreateRentalRequestAsync(request);
            return Ok();
        }
    }
}
