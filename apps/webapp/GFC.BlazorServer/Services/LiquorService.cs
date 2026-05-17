using GFC.BlazorServer.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class LiquorService : ILiquorService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEmailService _emailService;
        private readonly IBlazorSystemSettingsService _settingsService;

        public LiquorService(IDbContextFactory<GfcDbContext> dbFactory, IServiceScopeFactory scopeFactory, IEmailService emailService, IBlazorSystemSettingsService settingsService)
        {
            _dbFactory = dbFactory;
            _scopeFactory = scopeFactory;
            _emailService = emailService;
            _settingsService = settingsService;
        }

        public async Task<IEnumerable<LiquorItem>> GetAllItemsAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorItems
                .AsNoTracking()
                .Include(i => i.Vendor)
                .Where(i => i.IsActive)
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<LiquorItem?> GetItemByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorItems
                .Include(i => i.Vendor)
                .OrderBy(i => i.Id)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<LiquorItem?> GetItemByUpcAsync(string upc)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorItems.OrderBy(i => i.Id).FirstOrDefaultAsync(i => i.UpcCode == upc && i.IsActive);
        }

        public async Task<LiquorItem> CreateItemAsync(LiquorItem item, int? userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            // Check for duplicate UPC
            if (!string.IsNullOrEmpty(item.UpcCode))
            {
                var exists = await db.LiquorItems.AnyAsync(i => i.UpcCode == item.UpcCode && i.IsActive);
                if (exists) throw new Exception($"A product with UPC code '{item.UpcCode}' already exists.");
            }

            try 
            {
                // [POS SYNC] Default to showing in POS when created via Hub
                item.ShowInPos = true;
                
                db.LiquorItems.Add(item);
                await db.SaveChangesAsync();

                // Log Creation if userId is provided
                if (userId.HasValue && userId > 0)
                {
                    var transaction = new LiquorTransaction
                    {
                        ItemId = item.Id,
                        UserId = userId.Value,
                        ChangeAmount = item.CurrentStock,
                        TransactionType = "Creation",
                        Notes = $"Item Created: Initial Stock @ {item.CurrentStock}",
                        Timestamp = DateTime.UtcNow
                    };
                    db.LiquorTransactions.Add(transaction);
                    await db.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Liquor Create Failed: {innerMessage}");
            }
            return item;
        }

        public async Task UpdateItemAsync(LiquorItem item, int? userId)
        {
            await BulkSaveLiquorItemsAsync(new List<LiquorItem> { item }, userId);
        }

        public async Task BulkSaveLiquorItemsAsync(IEnumerable<LiquorItem> items, int? userId = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var timestamp = DateTime.UtcNow;

            foreach (var item in items)
            {
                var existing = await db.LiquorItems.AsNoTracking().OrderBy(i => i.Id).FirstOrDefaultAsync(i => i.Id == item.Id);
                if (existing == null) continue;

                // Detect Changes
                bool costChanged = existing.CurrentPrice != item.CurrentPrice;
                bool priceChanged = existing.RetailPrice != item.RetailPrice;
                bool minStockChanged = existing.MinStockLimit != item.MinStockLimit;
                bool vendorChanged = existing.VendorId != item.VendorId;
                bool nameChanged = existing.Name != item.Name;
                bool sizeChanged = existing.BottleSize != item.BottleSize;

                if (!costChanged && !priceChanged && !minStockChanged && !vendorChanged && !nameChanged && !sizeChanged) continue;

                try
                {
                    db.LiquorItems.Update(item);
                    
                    if (userId.HasValue && userId > 0)
                    {
                        var auditNotes = new List<string>();
                        if (costChanged) auditNotes.Add($"Bottle Cost: {existing.CurrentPrice:C} -> {item.CurrentPrice:C}");
                        if (priceChanged) auditNotes.Add($"Drink Price: {existing.RetailPrice:C} -> {item.RetailPrice:C}");
                        if (minStockChanged) auditNotes.Add($"Min: {existing.MinStockLimit} -> {item.MinStockLimit}");
                        if (vendorChanged) auditNotes.Add("Vendor Updated");
                        if (nameChanged) auditNotes.Add($"Name: {existing.Name} -> {item.Name}");

                        if (auditNotes.Any())
                        {
                            var transaction = new LiquorTransaction
                            {
                                ItemId = item.Id,
                                UserId = userId.Value,
                                ChangeAmount = 0,
                                TransactionType = "Update",
                                Notes = string.Join(" | ", auditNotes),
                                Timestamp = timestamp
                            };
                            db.LiquorTransactions.Add(transaction);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    var innerMessage = ex.InnerException?.Message ?? ex.Message;
                    throw new Exception($"Bulk update for item '{item.Name}' failed: {innerMessage}");
                }
            }

            await db.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var item = await db.LiquorItems.FindAsync(id);
            if (item != null)
            {
                item.IsActive = false; // Soft delete
                await db.SaveChangesAsync();
            }
        }

        // Vendor Management
        public async Task<IEnumerable<LiquorVendor>> GetAllVendorsAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorVendors.AsNoTracking().OrderBy(v => v.Name).ToListAsync();
        }

        public async Task<LiquorVendor?> GetVendorByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorVendors.Include(v => v.Items).OrderBy(v => v.Id).FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<LiquorVendor> CreateVendorAsync(LiquorVendor vendor)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            db.LiquorVendors.Add(vendor);
            await db.SaveChangesAsync();
            return vendor;
        }

        public async Task UpdateVendorAsync(LiquorVendor vendor)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.LiquorVendors.FindAsync(vendor.Id);
            if (existing != null)
            {
                db.Entry(existing).CurrentValues.SetValues(vendor);
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteVendorAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var vendor = await db.LiquorVendors.FindAsync(id);
            if (vendor != null)
            {
                // check if items are linked
                var hasItems = await db.LiquorItems.AnyAsync(i => i.VendorId == id && i.IsActive);
                if (hasItems) throw new Exception("Cannot delete vendor while products are assigned to it.");
                
                db.LiquorVendors.Remove(vendor);
                await db.SaveChangesAsync();
            }
        }

        public async Task<LiquorTransaction> CheckoutBottleAsync(int itemId, int userId, string? notes = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var item = await db.LiquorItems.FindAsync(itemId);
            if (item == null) throw new Exception("Item not found");

            item.CurrentStock--;
            
            var transaction = new LiquorTransaction
            {
                ItemId = itemId,
                UserId = userId,
                ChangeAmount = -1,
                TransactionType = "Checkout",
                Notes = notes,
                Timestamp = DateTime.UtcNow
            };

            db.LiquorTransactions.Add(transaction);
            await db.SaveChangesAsync();

            // Trigger Notifications in background to avoid blocking UI (e.g. slow SMTP)
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var scopedLiquorService = (LiquorService)scope.ServiceProvider.GetRequiredService<ILiquorService>();
                    await scopedLiquorService.CheckAndNotifyAsync(itemId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[LiquorService] Background notification failed: {ex.Message}");
                }
            });

            return transaction;
        }

        public async Task<LiquorTransaction> RestockItemAsync(int itemId, int userId, int amount, string? notes = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var item = await db.LiquorItems.FindAsync(itemId);
            if (item == null) throw new Exception("Item not found");

            item.CurrentStock += amount;

            var transaction = new LiquorTransaction
            {
                ItemId = itemId,
                UserId = userId,
                ChangeAmount = amount,
                TransactionType = "Restock",
                Notes = notes,
                Timestamp = DateTime.UtcNow
            };

            db.LiquorTransactions.Add(transaction);
            await db.SaveChangesAsync();

            // Restock rarely triggers low-stock alerts unless it's a correction, 
            // but we'll check anyway if someone wants to know when stuff arrives.
            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var scopedLiquorService = (LiquorService)scope.ServiceProvider.GetRequiredService<ILiquorService>();
                    await scopedLiquorService.CheckAndNotifyAsync(itemId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[LiquorService] Background notification failed: {ex.Message}");
                }
            });

            return transaction;
        }

        public async Task<LiquorTransaction> AdjustStockAsync(int itemId, int userId, int delta, string reason)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var item = await db.LiquorItems.FindAsync(itemId);
            if (item == null) throw new Exception("Item not found");

            var oldStock = item.CurrentStock;
            var newStock = oldStock + delta;
            
            // Prevent negative stock
            if (newStock < 0) newStock = 0;
            
            // Update item
            item.CurrentStock = newStock;
            
            // Calculate actual effective change (in case it was clamped)
            var actualDelta = newStock - oldStock;

            // If no actual change happened (e.g., trying to reduce 0 stock), we can still log it or return null. 
            // For now, let's log it as 0 change if that happens so the audit trail exists.

            var transaction = new LiquorTransaction
            {
                ItemId = itemId,
                UserId = userId,
                ChangeAmount = actualDelta,
                TransactionType = "Adjustment", 
                Notes = $"{reason} (From {oldStock} to {newStock})",
                Timestamp = DateTime.UtcNow
            };

            db.LiquorTransactions.Add(transaction);
            await db.SaveChangesAsync();

            // Trigger Notifications if stock dropped significantly or is critical
            if (actualDelta < 0)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var scopedLiquorService = (LiquorService)scope.ServiceProvider.GetRequiredService<ILiquorService>();
                        await scopedLiquorService.CheckAndNotifyAsync(itemId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[LiquorService] Background notification failed: {ex.Message}");
                    }
                });
            }

            return transaction;
        }

        public async Task<IEnumerable<LiquorTransaction>> GetRecentTransactionsAsync(int count = 50)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorTransactions
                .AsNoTracking()
                .Include(t => t.Item)
                .Include(t => t.User)
                .OrderByDescending(t => t.Timestamp)
                .Take(count)
                .ToListAsync();
        }

        // Order Management
        public async Task<IEnumerable<LiquorOrder>> GetAllOrdersAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorOrders
                .AsNoTracking()
                .Include(o => o.Vendor)
                .Include(o => o.User)
                .Include(o => o.PaidByUser)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.LiquorItem)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<LiquorOrder?> GetOrderByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorOrders
                .Include(o => o.Vendor)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.LiquorItem)
                .OrderBy(o => o.Id)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<LiquorOrder> CreateOrderAsync(LiquorOrder order, bool sendEmail = false)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            order.Status = "Placed";
            order.OrderDate = DateTime.UtcNow;
            
            db.LiquorOrders.Add(order);
            await db.SaveChangesAsync();

            if (sendEmail)
            {
                _ = Task.Run(async () => {
                    try {
                        var settings = await _settingsService.GetAsync();
                        
                        // Use a fresh DB context inside the background task to avoid "Disposed" errors
                        using var taskDb = await _dbFactory.CreateDbContextAsync();
                        var vendor = await taskDb.LiquorVendors.FindAsync(order.VendorId);
                        
                        if (vendor != null && !string.IsNullOrEmpty(vendor.Email))
                        {
                            var subject = $"Liquor Order #{order.Id} - GFC System";
                            
                            // Re-fetch order with items for email body
                            var fullOrder = await taskDb.LiquorOrders
                                .Include(o => o.OrderItems)
                                    .ThenInclude(oi => oi.LiquorItem)
                                .FirstOrDefaultAsync(o => o.Id == order.Id);

                            if (fullOrder != null)
                            {
                                var body = GetOrderEmailHtmlBody(fullOrder, vendor, settings);
                                var result = await _emailService.SendEmailAsync(vendor.Email, subject, body, ccEmail: settings.LiquorEmailCc);
                                
                                if (result.Success)
                                {
                                    // Mark as emailed ONLY if successful
                                    fullOrder.IsEmailed = true;
                                    fullOrder.LastEmailedDate = DateTime.UtcNow;
                                    await taskDb.SaveChangesAsync();
                                }
                                else
                                {
                                    Console.WriteLine($"[LiquorService] Email delivery failed for Order #{order.Id}: {result.ErrorMessage}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[LiquorService] Async emailing failed for Order #{order.Id}: {ex}");
                        
                        // Revert the intent flag if it failed completely
                        try {
                            using var errorDb = await _dbFactory.CreateDbContextAsync();
                            var o = await errorDb.LiquorOrders.FindAsync(order.Id);
                            if (o != null)
                            {
                                o.IsEmailed = false;
                                await errorDb.SaveChangesAsync();
                            }
                        } catch { /* Silent fail on revert attempt */ }
                    }
                });
            }

            return order;
        }

        private string GetOrderEmailHtmlBody(LiquorOrder order, LiquorVendor vendor, SystemSettings settings)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("<html><body style='font-family: Arial, sans-serif; color: #333; margin: 0; padding: 20px; background-color: #f4f7f6;'>");
            
            // Outer table to force width in Outlook
            sb.AppendLine("<table cellpadding='0' cellspacing='0' border='0' width='100%' style='background-color: #f4f7f6;'>");
            sb.AppendLine("<tr><td align='center'>");
            
            sb.AppendLine("<table cellpadding='0' cellspacing='0' border='0' width='650' style='background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 15px rgba(0,0,0,0.05); border: 1px solid #e1e4e8;'>");
            sb.AppendLine("<tr><td>");

            // Header - Clean & Professional
            sb.AppendLine("<div style='padding: 30px; border-bottom: 3px solid #f1f3f5;'>");
            sb.AppendLine("<h1 style='margin: 0; font-size: 26px; color: #1a1a1a; text-transform: uppercase; letter-spacing: 1px;'>Liquor Purchase Order</h1>");
            sb.AppendLine($"<div style='font-size: 14px; color: #718096; margin-top: 8px;'>Order ID: <strong style='color: #2d3748;'>#{order.Id}</strong> &bull; {order.OrderDate:MMMM dd, yyyy}</div>");
            sb.AppendLine("</div>");

            sb.AppendLine("<div style='padding: 30px;'>");
            
            // Vendor & Sender Info
            sb.AppendLine("<table style='width: 100%; margin-bottom: 30px;'>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style='width: 50%; vertical-align: top;'>");
            sb.AppendLine("<div style='font-size: 12px; font-weight: bold; color: #95a5a6; text-transform: uppercase; margin-bottom: 5px;'>To Vendor:</div>");
            sb.AppendLine($"<div style='font-size: 16px; font-weight: bold; color: #2c3e50;'>{vendor.Name}</div>");
            if (!string.IsNullOrEmpty(vendor.ContactName)) sb.AppendLine($"<div style='font-size: 14px; color: #7f8c8d;'>Attn: {vendor.ContactName}</div>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td style='width: 50%; vertical-align: top; text-align: right;'>");
            sb.AppendLine("<div style='font-size: 12px; font-weight: bold; color: #95a5a6; text-transform: uppercase; margin-bottom: 5px;'>Ordered By:</div>");
            sb.AppendLine("<div style='font-size: 16px; font-weight: bold; color: #2c3e50;'>GFC System</div>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");

            if (!string.IsNullOrEmpty(order.SpecialInstructions))
            {
                sb.AppendLine("<div style='background-color: #fff9db; border-left: 4px solid #fcc419; padding: 15px; border-radius: 4px; margin-bottom: 30px;'>");
                sb.AppendLine("<div style='font-size: 11px; font-weight: bold; color: #856404; text-transform: uppercase; margin-bottom: 5px;'>Delivery Instructions:</div>");
                sb.AppendLine($"<div style='font-size: 14px; color: #2c3e50;'>{order.SpecialInstructions}</div>");
                sb.AppendLine("</div>");
            }

            // Items Table
            sb.AppendLine("<table style='width: 100%; border-collapse: collapse;'>");
            sb.AppendLine("<thead><tr style='border-bottom: 2px solid #edf2f7;'>");
            sb.AppendLine("<th style='padding: 12px 5px; text-align: left; font-size: 12px; color: #718096; text-transform: uppercase;'>Qty</th>");
            sb.AppendLine("<th style='padding: 12px 5px; text-align: left; font-size: 12px; color: #718096; text-transform: uppercase;'>Product Description</th>");
            sb.AppendLine("<th style='padding: 12px 5px; text-align: right; font-size: 12px; color: #718096; text-transform: uppercase;'>Unit Price</th>");
            sb.AppendLine("<th style='padding: 12px 5px; text-align: right; font-size: 12px; color: #718096; text-transform: uppercase;'>Subtotal</th>");
            sb.AppendLine("</tr></thead><tbody>");

            foreach (var item in order.OrderItems)
            {
                var packSize = item.LiquorItem?.PackSize ?? 1;
                var isCase = packSize > 1;
                var displayUnits = isCase ? (decimal)item.Quantity / packSize : item.Quantity;
                var unitLabel = isCase ? (displayUnits == 1 ? "Case" : "Cases") : (displayUnits == 1 ? "Unit" : "Units");
                
                var subtotal = (displayUnits * item.UnitPriceAtTimeOfOrder) + (item.BottleFeeAtTimeOfOrder * item.Quantity);

                sb.AppendLine("<tr style='border-bottom: 1px solid #edf2f7;'>");
                sb.AppendLine($"<td style='padding: 15px 5px; vertical-align: top; font-weight: bold; color: #1a1a1a; white-space: nowrap;'>{displayUnits:G29} {unitLabel}</td>");
                sb.AppendLine("<td style='padding: 15px 5px; vertical-align: top;'>");
                sb.AppendLine($"<div style='font-weight: bold; color: #1a1a1a; font-size: 15px;'>{item.LiquorItem?.Name}</div>");
                sb.AppendLine($"<div style='font-size: 12px; color: #718096; margin-top: 2px;'>{item.LiquorItem?.BottleSize}</div>");
                sb.AppendLine("</td>");
                sb.AppendLine($"<td style='padding: 15px 5px; vertical-align: top; text-align: right; color: #4a5568;'>{item.UnitPriceAtTimeOfOrder:C}</td>");
                sb.AppendLine($"<td style='padding: 15px 5px; vertical-align: top; text-align: right; font-weight: bold; color: #1a1a1a;'>{subtotal:C}</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody></table>");

            // Totals
            sb.AppendLine("<div style='margin-top: 30px; border-top: 2px solid #edf2f7; padding-top: 20px;'>");
            sb.AppendLine("<table style='width: 100%;'>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style='text-align: right; font-size: 16px; color: #718096;'>Total Order Value:</td>");
            sb.AppendLine($"<td style='text-align: right; font-size: 22px; font-weight: 800; color: #2c3e50; padding-left: 20px;'>{order.TotalCost:C}</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");

            if (!string.IsNullOrEmpty(settings.LiquorEmailSignature))
            {
                sb.AppendLine("<div style='margin-top: 40px; padding-top: 25px; border-top: 2px solid #edf2f7; color: #1a1a1a; font-size: 16px; line-height: 1.6; font-weight: bold;'>");
                sb.AppendLine(settings.LiquorEmailSignature.Replace("\n", "<br/>"));
                sb.AppendLine("</div>");
            }
            
            sb.AppendLine("</div>"); // padding div
            
            var footerText = !string.IsNullOrEmpty(settings.LiquorEmailFooter) 
                ? settings.LiquorEmailFooter 
                : "This purchase order was generated automatically. Please contact us directly if there are any discrepancies.";

            sb.AppendLine("<div style='background-color: #f1f5f9; padding: 25px; text-align: center; font-size: 13px; color: #334155; border-top: 1px solid #e2e8f0; font-weight: bold;'>");
            sb.AppendLine(footerText);
            sb.AppendLine("</div>");
            
            sb.AppendLine("</td></tr></table>"); // inner table
            sb.AppendLine("</td></tr></table>"); // outer table
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }

        public async Task<EmailResult> ResendOrderEmailAsync(int orderId)
        {
            try 
            {
                using var db = await _dbFactory.CreateDbContextAsync();
                var order = await db.LiquorOrders
                    .Include(o => o.Vendor)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.LiquorItem)
                    .OrderBy(o => o.Id)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null) return EmailResult.Failure("Order not found.");
                if (order.Vendor == null) return EmailResult.Failure("Vendor not found for this order.");
                if (string.IsNullOrEmpty(order.Vendor.Email)) return EmailResult.Failure("Vendor email address is missing.");

                var settings = await _settingsService.GetAsync();
                var subject = $"Liquor Order #{order.Id} (RESENT) - GFC System";
                var body = GetOrderEmailHtmlBody(order, order.Vendor, settings);
                var result = await _emailService.SendEmailAsync(order.Vendor.Email, subject, body, ccEmail: settings.LiquorEmailCc);
                
                if (result.Success)
                {
                    order.IsEmailed = true;
                    order.LastEmailedDate = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LIQUOR SERVICE ERROR] {DateTime.Now}: {ex}");
                return EmailResult.Failure($"System Error: {ex.Message}");
            }
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status, string? invoiceNumber = null, decimal? taxAmount = null, decimal? additionalCosts = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var order = await db.LiquorOrders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                if (invoiceNumber != null) order.InvoiceNumber = invoiceNumber;
                if (taxAmount.HasValue) order.TaxAmount = taxAmount.Value;
                if (additionalCosts.HasValue) order.AdditionalCosts = additionalCosts.Value;
                
                order.TotalCost = order.ItemsTotal + order.TaxAmount + order.AdditionalCosts;
                
                await db.SaveChangesAsync();
            }
        }

        public async Task MarkOrderAsPaidAsync(int orderId, DateTime paidDate, decimal paidAmount, int paidByUserId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var order = await db.LiquorOrders.FindAsync(orderId);
            if (order != null)
            {
                order.IsPaid = true;
                order.PaidDate = paidDate;
                order.ActualPaidAmount = paidAmount;
                order.PaidByUserId = paidByUserId;
                await db.SaveChangesAsync();
            }
        }

        public async Task ReceiveOrderAsync(int orderId, int userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var order = await db.LiquorOrders
                .Include(o => o.OrderItems)
                .OrderBy(o => o.Id)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) throw new Exception("Order not found");
            if (order.Status == "Received") return; // Already processed

            foreach (var orderItem in order.OrderItems)
            {
                if (orderItem.IsBackordered) continue;

                var liquor = await db.LiquorItems.FindAsync(orderItem.LiquorItemId);
                if (liquor != null)
                {
                    liquor.CurrentStock += orderItem.Quantity;
                    
                    // Log transaction
                    var transaction = new LiquorTransaction
                    {
                        ItemId = liquor.Id,
                        UserId = userId,
                        ChangeAmount = orderItem.Quantity,
                        TransactionType = "Restock",
                        Notes = $"Order #{order.Id} Received (Invoice: {order.InvoiceNumber})",
                        Timestamp = DateTime.UtcNow
                    };
                    db.LiquorTransactions.Add(transaction);
                }
            }

            order.Status = "Received";
            await db.SaveChangesAsync();
        }

        public async Task UpdateOrderItemsBackorderAsync(int orderId, List<int> backorderedOrderItemIds)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var order = await db.LiquorOrders
                .Include(o => o.OrderItems)
                .OrderBy(o => o.Id)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order != null)
            {
                foreach (var item in order.OrderItems)
                {
                    item.IsBackordered = backorderedOrderItemIds.Contains(item.Id);
                }
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LiquorOrderItem>> GetPendingBackordersAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorOrderItems
                .Include(oi => oi.LiquorItem)
                .Include(oi => oi.Order)
                    .ThenInclude(o => o!.Vendor)
                .Where(oi => oi.IsBackordered && !oi.IsResolved)
                .OrderByDescending(oi => oi.Order!.OrderDate)
                .ToListAsync();
        }

        public async Task ResolveBackorderAsync(int orderItemId, int userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var orderItem = await db.LiquorOrderItems
                .Include(oi => oi.Order)
                .OrderBy(oi => oi.Id)
                .FirstOrDefaultAsync(oi => oi.Id == orderItemId);

            if (orderItem == null || orderItem.IsResolved) return;

            var liquor = await db.LiquorItems.FindAsync(orderItem.LiquorItemId);
            if (liquor != null)
            {
                liquor.CurrentStock += orderItem.Quantity;
                
                // Log transaction
                var transaction = new LiquorTransaction
                {
                    ItemId = liquor.Id,
                    UserId = userId,
                    ChangeAmount = orderItem.Quantity,
                    TransactionType = "Restock",
                    Notes = $"Backorder from Order #{orderItem.OrderId} Received",
                    Timestamp = DateTime.UtcNow
                };
                db.LiquorTransactions.Add(transaction);
            }

            orderItem.IsResolved = true;
            await db.SaveChangesAsync();
        }

        public async Task<LiquorNotificationRule?> GetNotificationRuleAsync(int userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorNotificationRules.OrderBy(r => r.Id).FirstOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task UpsertNotificationRuleAsync(LiquorNotificationRule rule)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.LiquorNotificationRules.OrderBy(r => r.Id).FirstOrDefaultAsync(r => r.UserId == rule.UserId);
            if (existing == null)
            {
                db.LiquorNotificationRules.Add(rule);
            }
            else
            {
                db.Entry(existing).CurrentValues.SetValues(rule);
            }
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<LiquorNotificationRule>> GetSubscribedUsersAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorNotificationRules.ToListAsync();
        }

        public async Task CheckAndNotifyAsync(int itemId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var item = await db.LiquorItems.FindAsync(itemId);
            if (item == null) return;

            if (item.CurrentStock > item.MinStockLimit && item.CurrentStock > 0) return;

            string title = "";
            string body = "";
            bool isCritical = item.CurrentStock <= 0;

            if (isCritical)
            {
                title = $"CRITICAL: {item.Name} is Out of Stock!";
                body = $"The inventory for {item.Name} ({item.BottleSize}) has reached ZERO. Immediate restock required.";
            }
            else if (item.CurrentStock <= item.MinStockLimit)
            {
                title = $"Low Stock Alert: {item.Name}";
                body = $"{item.Name} ({item.BottleSize}) is down to {item.CurrentStock} bottles. Limit is {item.MinStockLimit}.";
            }

            if (string.IsNullOrEmpty(title)) return;

            var subscribers = await db.LiquorNotificationRules.ToListAsync();

            // Deduplicate by UserId to prevent sending multiple alerts to the same person if legacy data exists
            var uniqueSubscribers = subscribers
                .GroupBy(s => s.UserId)
                .Select(g => g.First())
                .ToList();

            // Create one scope for all notifications in this run
            using var notificationScope = _scopeFactory.CreateScope();
            var notificationService = notificationScope.ServiceProvider.GetRequiredService<INotificationService>();

            foreach (var sub in uniqueSubscribers)
            {
                if (isCritical && !sub.NotifyOnEmpty) continue;
                if (!isCritical && !sub.NotifyOnLowStock) continue;

                var user = await db.AppUsers.FindAsync(sub.UserId);
                if (user == null || !user.IsActive) 
                {
                    Console.WriteLine($"[LiquorService] Skipping user {sub.UserId} (Not found or Inactive)");
                    continue;
                }

                // 1. Email (Prioritize reliability)
                if (sub.ReceiveEmail && !string.IsNullOrEmpty(user.Email))
                {
                    try 
                    {
                        await notificationService.SendEmailAsync(user.Email, title, body);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[LiquorService] Email failed for user {user.UserId}: {ex.Message}");
                    }
                }

                // 2. Push Notification
                if (sub.ReceivePush)
                {
                    try
                    {
                        await notificationService.SendPushNotificationAsync(user.UserId, title, body, "/mobile/liquor/manage");
                    }
                    catch (Exception ex)
                    {
                         // Push often fails due to missing subscriptions or keys. Log but don't crash.
                         Console.WriteLine($"[LiquorService] Push failed for user {user.UserId}: {ex.Message}");
                    }
                }

                // 3. SMS (Future)
                if (sub.ReceiveSms) 
                {
                    try
                    {
                        var smsNotification = new SystemNotification
                        {
                            RecipientEmail = user.Email, 
                            Subject = title,
                            Message = body,
                            Channel = "SMS",
                            Status = "Pending"
                        };
                        await notificationService.DispatchNotificationAsync(smsNotification);
                    }
                    catch (Exception ex)
                    {
                         Console.WriteLine($"[LiquorService] SMS failed for user {user.UserId}: {ex.Message}");
                    }
                }
            }
        }


        public async Task ReconcileStockAsync(IEnumerable<GFC.Core.Models.StockReconcileEntry> entries, int userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            foreach (var entry in entries)
            {
                var item = await db.LiquorItems.FindAsync(entry.ItemId);
                if (item == null) continue;

                var oldStock = item.CurrentStock;
                var delta = entry.ActualCount - oldStock;
                
                if (delta == 0) continue; // No change needed

                // Update system stock
                item.CurrentStock = entry.ActualCount;

                // Log as reconciliation transaction
                var transaction = new LiquorTransaction
                {
                    ItemId = item.Id,
                    UserId = userId,
                    ChangeAmount = delta,
                    TransactionType = "Reconcile",
                    Notes = $"Physical Inventory: {entry.ActualCount} (Adjusted from {oldStock})",
                    Timestamp = DateTime.UtcNow
                };

                db.LiquorTransactions.Add(transaction);

                // Quick notification check if it's now low
                if (delta < 0)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            using var scope = _scopeFactory.CreateScope();
                            var scopedLiquorService = (LiquorService)scope.ServiceProvider.GetRequiredService<ILiquorService>();
                            await scopedLiquorService.CheckAndNotifyAsync(item.Id);
                        }
                        catch { /* Fire and forget */ }
                    });
                }
            }

            await db.SaveChangesAsync();
        }

        public async Task<List<ProductTrendDTO>> GetProductTrendsAsync(int daysLookback = 30)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            var endDate = DateTime.UtcNow;
            var startDate = endDate.AddDays(-daysLookback);
            var previousStartDate = startDate.AddDays(-daysLookback);

            // Fetch Usage Transactions
            var transactions = await db.LiquorTransactions
                .Include(t => t.Item)
                .Where(t => t.Timestamp >= previousStartDate && t.ChangeAmount < 0)
                .ToListAsync();

            var trends = transactions
                .Where(t => t.Item != null)
                .GroupBy(t => t.Item)
                .Select(g => 
                {
                    var currentUsage = g.Where(t => t.Timestamp >= startDate).Sum(t => Math.Abs(t.ChangeAmount));
                    var prevUsage = g.Where(t => t.Timestamp < startDate).Sum(t => Math.Abs(t.ChangeAmount));
                    
                    double percentChange = 0;
                    if (prevUsage > 0)
                    {
                        percentChange = ((double)(currentUsage - prevUsage) / prevUsage) * 100;
                    }
                    else if (currentUsage > 0)
                    {
                        percentChange = 100;
                    }

                    return new ProductTrendDTO
                    {
                        ItemId = g.Key!.Id,
                        ProductName = g.Key.Name,
                        Category = g.Key.Category ?? "Uncategorized",
                        CurrentPeriodUsage = currentUsage,
                        PreviousPeriodUsage = prevUsage,
                        PercentageChange = Math.Round(percentChange, 1)
                    };
                })
                .OrderByDescending(t => t.CurrentPeriodUsage)
                .ToList();

            return trends;
        }
    }
}


