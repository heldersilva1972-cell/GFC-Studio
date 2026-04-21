using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/liquor")]
    public class LiquorApiController : ControllerBase
    {
        private readonly ILiquorService _liquorService;
        private readonly ILogger<LiquorApiController> _logger;

        public LiquorApiController(ILiquorService liquorService, ILogger<LiquorApiController> logger)
        {
            _liquorService = liquorService;
            _logger = logger;
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                var items = await _liquorService.GetAllItemsAsync();
                return Ok(items);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching liquor items for POS");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("checkout/{itemId}")]
        public async Task<IActionResult> CheckoutBottle(int itemId, [FromQuery] int userId, [FromQuery] string? notes = null)
        {
            try
            {
                var transaction = await _liquorService.CheckoutBottleAsync(itemId, userId, notes);
                return Ok(new { success = true, transactionId = transaction.Id });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error during bottle checkout for Item {ItemId}", itemId);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("bulk-checkout")]
        public async Task<IActionResult> BulkCheckout([FromBody] List<InventoryPullRequest> items, [FromQuery] int userId)
        {
            try
            {
                foreach (var item in items)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        await _liquorService.CheckoutBottleAsync(item.ItemId, userId, "POS Shift Close-out");
                    }
                }
                return Ok(new { success = true });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error during bulk checkout");
                return BadRequest(ex.Message);
            }
        }
    }

    public class InventoryPullRequest
    {
        public int ItemId { get; set; }
        public int Count { get; set; }
    }
}


