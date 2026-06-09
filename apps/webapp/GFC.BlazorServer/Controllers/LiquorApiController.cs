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
                var dbItems = await _liquorService.GetAllItemsAsync();
                var cleanItems = new List<LiquorItem>();
                foreach (var item in dbItems)
                {
                    cleanItems.Add(new LiquorItem
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        UpcCode = item.UpcCode,
                        BottleSize = item.BottleSize,
                        Category = item.Category,
                        ImageUrl = item.ImageUrl,
                        VendorId = item.VendorId,
                        CurrentPrice = item.CurrentPrice,
                        CurrentStock = item.CurrentStock,
                        MinStockLimit = item.MinStockLimit,
                        MinimumOrderQuantity = item.MinimumOrderQuantity,
                        PackSize = item.PackSize,
                        RetailPrice = item.RetailPrice,
                        PourSize = item.PourSize,
                        IsUnitBased = item.IsUnitBased,
                        IsActive = item.IsActive,
                        IsBeer = item.IsBeer,
                        ShowInPos = item.ShowInPos,
                        AllowLooseReconciliation = item.AllowLooseReconciliation,
                        DisplayOrder = item.DisplayOrder,
                        CreatedAt = item.CreatedAt,
                        Vendor = null, // Break EF circular reference cycle
                        OrderHistory = new List<LiquorOrderItem>() // Break cycle
                    });
                }
                return Ok(cleanItems);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching liquor items for POS");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("vendors")]
        public async Task<IActionResult> GetVendors()
        {
            try
            {
                var dbVendors = await _liquorService.GetAllVendorsAsync();
                var cleanVendors = new List<LiquorVendor>();
                foreach (var v in dbVendors)
                {
                    cleanVendors.Add(new LiquorVendor
                    {
                        Id = v.Id,
                        Name = v.Name,
                        ContactName = v.ContactName,
                        MinimumOrderAmount = v.MinimumOrderAmount,
                        PhoneNumber = v.PhoneNumber,
                        Email = v.Email,
                        Website = v.Website,
                        Items = new List<LiquorItem>(),
                        Orders = new List<LiquorOrder>()
                    });
                }
                return Ok(cleanVendors);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching liquor vendors");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("vendors")]
        public async Task<IActionResult> CreateVendor([FromBody] LiquorVendor vendor)
        {
            try
            {
                if (vendor == null) return BadRequest("Vendor data is required");
                var dbVendor = await _liquorService.CreateVendorAsync(vendor);
                var cleanVendor = new LiquorVendor
                {
                    Id = dbVendor.Id,
                    Name = dbVendor.Name,
                    ContactName = dbVendor.ContactName,
                    MinimumOrderAmount = dbVendor.MinimumOrderAmount,
                    PhoneNumber = dbVendor.PhoneNumber,
                    Email = dbVendor.Email,
                    Website = dbVendor.Website,
                    Items = new List<LiquorItem>(),
                    Orders = new List<LiquorOrder>()
                };
                return Ok(cleanVendor);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error creating liquor vendor");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("vendors/{id}")]
        public async Task<IActionResult> UpdateVendor(int id, [FromBody] LiquorVendor vendor)
        {
            try
            {
                if (vendor == null) return BadRequest("Vendor data is required");
                vendor.Id = id;
                await _liquorService.UpdateVendorAsync(vendor);
                return Ok(new { success = true });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error updating liquor vendor");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("vendors/{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            try
            {
                await _liquorService.DeleteVendorAsync(id);
                return Ok(new { success = true });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error deleting liquor vendor");
                return BadRequest(ex.Message);
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

        [HttpGet("orders/pending")]
        public async Task<IActionResult> GetPendingOrders()
        {
            try
            {
                var orders = await _liquorService.GetAllOrdersAsync();
                var pendingOrders = new List<LiquorOrder>();
                foreach (var dbOrder in orders)
                {
                    if (dbOrder.Status == "Placed" || dbOrder.Status == "Sent")
                    {
                        // Map to a completely clean POCO graph to bypass EF tracking proxies & cycles
                        var cleanOrder = new LiquorOrder
                        {
                            Id = dbOrder.Id,
                            VendorId = dbOrder.VendorId,
                            OrderDate = dbOrder.OrderDate,
                            Status = dbOrder.Status,
                            ItemsTotal = dbOrder.ItemsTotal,
                            TaxAmount = dbOrder.TaxAmount,
                            AdditionalCosts = dbOrder.AdditionalCosts,
                            TotalCost = dbOrder.TotalCost,
                            InvoiceNumber = dbOrder.InvoiceNumber,
                            IsPaid = dbOrder.IsPaid,
                            PaidDate = dbOrder.PaidDate,
                            ActualPaidAmount = dbOrder.ActualPaidAmount,
                            PaidByUserId = dbOrder.PaidByUserId,
                            UserId = dbOrder.UserId,
                            IsEmailed = dbOrder.IsEmailed,
                            LastEmailedDate = dbOrder.LastEmailedDate,
                            SpecialInstructions = dbOrder.SpecialInstructions,
                            
                            // Flat Vendor
                            Vendor = dbOrder.Vendor == null ? null : new LiquorVendor
                            {
                                Id = dbOrder.Vendor.Id,
                                Name = dbOrder.Vendor.Name,
                                ContactName = dbOrder.Vendor.ContactName,
                                MinimumOrderAmount = dbOrder.Vendor.MinimumOrderAmount,
                                PhoneNumber = dbOrder.Vendor.PhoneNumber,
                                Email = dbOrder.Vendor.Email,
                                Website = dbOrder.Vendor.Website,
                                Items = new List<LiquorItem>(),
                                Orders = new List<LiquorOrder>()
                            },
                            
                            OrderItems = new List<LiquorOrderItem>()
                        };
                        
                        if (dbOrder.OrderItems != null)
                        {
                            foreach (var dbItem in dbOrder.OrderItems)
                            {
                                var cleanItem = new LiquorOrderItem
                                {
                                    Id = dbItem.Id,
                                    OrderId = dbItem.OrderId,
                                    LiquorItemId = dbItem.LiquorItemId,
                                    Quantity = dbItem.Quantity,
                                    UnitPriceAtTimeOfOrder = dbItem.UnitPriceAtTimeOfOrder,
                                    BottleFeeAtTimeOfOrder = dbItem.BottleFeeAtTimeOfOrder,
                                    IsBackordered = dbItem.IsBackordered,
                                    IsResolved = dbItem.IsResolved,
                                    Order = null, // Break parent cycle
                                    
                                    // Flat LiquorItem
                                    LiquorItem = dbItem.LiquorItem == null ? null : new LiquorItem
                                    {
                                        Id = dbItem.LiquorItem.Id,
                                        Name = dbItem.LiquorItem.Name,
                                        Description = dbItem.LiquorItem.Description,
                                        UpcCode = dbItem.LiquorItem.UpcCode,
                                        BottleSize = dbItem.LiquorItem.BottleSize,
                                        Category = dbItem.LiquorItem.Category,
                                        ImageUrl = dbItem.LiquorItem.ImageUrl,
                                        VendorId = dbItem.LiquorItem.VendorId,
                                        CurrentPrice = dbItem.LiquorItem.CurrentPrice,
                                        CurrentStock = dbItem.LiquorItem.CurrentStock,
                                        MinStockLimit = dbItem.LiquorItem.MinStockLimit,
                                        MinimumOrderQuantity = dbItem.LiquorItem.MinimumOrderQuantity,
                                        PackSize = dbItem.LiquorItem.PackSize,
                                        RetailPrice = dbItem.LiquorItem.RetailPrice,
                                        PourSize = dbItem.LiquorItem.PourSize,
                                        IsUnitBased = dbItem.LiquorItem.IsUnitBased,
                                        IsActive = dbItem.LiquorItem.IsActive,
                                        IsBeer = dbItem.LiquorItem.IsBeer,
                                        ShowInPos = dbItem.LiquorItem.ShowInPos,
                                        DisplayOrder = dbItem.LiquorItem.DisplayOrder,
                                        CreatedAt = dbItem.LiquorItem.CreatedAt,
                                        OrderHistory = new List<LiquorOrderItem>(),
                                        Vendor = null // Break cycle
                                    }
                                };
                                cleanOrder.OrderItems.Add(cleanItem);
                            }
                        }
                        
                        pendingOrders.Add(cleanOrder);
                    }
                }
                return Ok(pendingOrders);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error fetching pending liquor orders for POS");
                return StatusCode(500, $"Internal server error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        [HttpPost("orders/{orderId}/receive")]
        public async Task<IActionResult> ReceiveOrder(int orderId, [FromBody] LiquorOrderReceiptDto receipt)
        {
            try
            {
                if (receipt == null) return BadRequest("Receipt data is required");
                if (string.IsNullOrWhiteSpace(receipt.InvoiceNumber))
                {
                    receipt.InvoiceNumber = "N/A";
                }
                if (receipt.TotalDue < 0) return BadRequest("Invoice total cannot be negative");

                var order = await _liquorService.GetOrderByIdAsync(orderId);
                if (order == null) return NotFound($"Order with ID {orderId} not found");
                if (order.Status == "Received") return Ok(new { success = true, message = "Order already received" });

                // 1. Update order backordered items
                await _liquorService.UpdateOrderItemsBackorderAsync(orderId, receipt.BackorderedOrderItemIds);

                // Calculate discrepancy (Tax + Fees) on RECEIVED items only
                var receivedItemsTotal = 0m;
                foreach (var item in order.OrderItems)
                {
                    if (!receipt.BackorderedOrderItemIds.Contains(item.Id))
                    {
                        receivedItemsTotal += item.UnitPriceAtTimeOfOrder * item.Quantity;
                    }
                }
                var additionalCosts = receipt.TotalDue - receivedItemsTotal;

                // 2. Update order meta
                await _liquorService.UpdateOrderStatusAsync(orderId, "Received", receipt.InvoiceNumber, 0, additionalCosts);

                // 3. Process stock arrival
                await _liquorService.ReceiveOrderAsync(orderId, receipt.UserId);

                return Ok(new { success = true, message = "Order received successfully" });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error processing liquor order receipt for Order {OrderId}", orderId);
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


