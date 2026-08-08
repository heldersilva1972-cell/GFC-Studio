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

        public async Task<IEnumerable<LiquorItem>> GetAllItemsAsync(bool includeInactive = false)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var query = db.LiquorItems
                .AsNoTracking()
                .Include(i => i.Vendor)
                .Where(i => !i.ParentItemId.HasValue);

            if (!includeInactive)
            {
                query = query.Where(i => i.IsActive);
            }

            return await query.OrderBy(i => i.Name).ToListAsync();
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
            if (item.OrderByCaseOnly)
            {
                if ((!item.CasePrice.HasValue || item.CasePrice.Value == 0) && item.CurrentPrice > 0)
                {
                    item.CasePrice = item.CurrentPrice;
                }
                else if (item.CasePrice.HasValue && item.CasePrice.Value > 0)
                {
                    item.CurrentPrice = item.CasePrice.Value;
                }
            }

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
                // structural sync: ensure case-only products keep their prices aligned
                if (item.OrderByCaseOnly)
                {
                    if ((!item.CasePrice.HasValue || item.CasePrice.Value == 0) && item.CurrentPrice > 0)
                    {
                        item.CasePrice = item.CurrentPrice;
                    }
                    else if (item.CasePrice.HasValue && item.CasePrice.Value > 0)
                    {
                        item.CurrentPrice = item.CasePrice.Value;
                    }
                }

                var existing = await db.LiquorItems.AsNoTracking().OrderBy(i => i.Id).FirstOrDefaultAsync(i => i.Id == item.Id);
                if (existing == null) continue;

                // Detect Changes
                bool costChanged = existing.CurrentPrice != item.CurrentPrice;
                bool priceChanged = existing.RetailPrice != item.RetailPrice;
                bool minStockChanged = existing.MinStockLimit != item.MinStockLimit;
                bool vendorChanged = existing.VendorId != item.VendorId;
                bool nameChanged = existing.Name != item.Name;
                bool sizeChanged = existing.BottleSize != item.BottleSize;
                bool looseChanged = existing.AllowLooseReconciliation != item.AllowLooseReconciliation;
                bool unitChanged = existing.IsUnitBased != item.IsUnitBased;
                bool activeChanged = existing.IsActive != item.IsActive;
                bool beerChanged = existing.IsBeer != item.IsBeer;
                bool posChanged = existing.ShowInPos != item.ShowInPos;
                bool packChanged = existing.PackSize != item.PackSize;
                bool pourChanged = existing.PourSize != item.PourSize;
                bool minOrderChanged = existing.MinimumOrderQuantity != item.MinimumOrderQuantity;
                bool casePriceChanged = existing.CasePrice != item.CasePrice;
                bool orderByCaseOnlyChanged = existing.OrderByCaseOnly != item.OrderByCaseOnly;
                bool bulkDiscountThresholdChanged = existing.BulkDiscountThreshold != item.BulkDiscountThreshold;
                bool bulkDiscountPriceChanged = existing.BulkDiscountPrice != item.BulkDiscountPrice;
                bool minOrderCasesChanged = existing.MinOrderCases != item.MinOrderCases;

                if (!costChanged && !priceChanged && !minStockChanged && !vendorChanged && !nameChanged && !sizeChanged &&
                    !looseChanged && !unitChanged && !activeChanged && !beerChanged && !posChanged && !packChanged &&
                    !pourChanged && !minOrderChanged && !casePriceChanged && !orderByCaseOnlyChanged && 
                    !bulkDiscountThresholdChanged && !bulkDiscountPriceChanged && !minOrderCasesChanged) continue;

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
                        if (looseChanged) auditNotes.Add($"Loose Audit: {existing.AllowLooseReconciliation} -> {item.AllowLooseReconciliation}");
                        if (unitChanged) auditNotes.Add($"Unit Based: {existing.IsUnitBased} -> {item.IsUnitBased}");
                        if (activeChanged) auditNotes.Add($"Active: {existing.IsActive} -> {item.IsActive}");
                        if (beerChanged) auditNotes.Add($"Beer: {existing.IsBeer} -> {item.IsBeer}");
                        if (posChanged) auditNotes.Add($"Show in POS: {existing.ShowInPos} -> {item.ShowInPos}");
                        if (casePriceChanged) auditNotes.Add($"Case Price: {existing.CasePrice:C} -> {item.CasePrice:C}");
                        if (orderByCaseOnlyChanged) auditNotes.Add($"Case Only: {existing.OrderByCaseOnly} -> {item.OrderByCaseOnly}");
                        if (bulkDiscountThresholdChanged || bulkDiscountPriceChanged) auditNotes.Add($"Bulk Discount: {existing.BulkDiscountPrice:C} @ {existing.BulkDiscountThreshold} -> {item.BulkDiscountPrice:C} @ {item.BulkDiscountThreshold}");
                        if (minOrderCasesChanged) auditNotes.Add($"Min Cases: {existing.MinOrderCases} -> {item.MinOrderCases}");

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

            // Decrement master CurrentStock for compatibility
            item.CurrentStock--;
            if (item.CurrentStock < 0) item.CurrentStock = 0;

            // Route to correct location
            string targetLocation = "DOWNSTAIRS_BAR"; // Default to Downstairs if unspecified
            if (notes != null)
            {
                if (notes.Contains("Upstairs Bar", StringComparison.OrdinalIgnoreCase) || notes.Contains("UPSTAIRS", StringComparison.OrdinalIgnoreCase))
                {
                    targetLocation = "UPSTAIRS_BAR";
                }
                else if (notes.Contains("Downstairs Bar", StringComparison.OrdinalIgnoreCase) || notes.Contains("DOWNSTAIRS", StringComparison.OrdinalIgnoreCase))
                {
                    targetLocation = "DOWNSTAIRS_BAR";
                }
            }

            // Transfer: Decrease from MAIN_STORAGE
            var source = await db.LiquorLocationStocks.FirstOrDefaultAsync(ls => ls.ItemId == itemId && ls.LocationName == "MAIN_STORAGE");
            if (source == null)
            {
                source = new LiquorLocationStock { ItemId = itemId, LocationName = "MAIN_STORAGE", Stock = item.CurrentStock + 1 };
                db.LiquorLocationStocks.Add(source);
            }
            source.Stock -= 1;
            if (source.Stock < 0) source.Stock = 0;

            // Transfer: Increase in targetLocation
            var dest = await db.LiquorLocationStocks.FirstOrDefaultAsync(ls => ls.ItemId == itemId && ls.LocationName == targetLocation);
            if (dest == null)
            {
                dest = new LiquorLocationStock { ItemId = itemId, LocationName = targetLocation, Stock = 0 };
                db.LiquorLocationStocks.Add(dest);
            }
            dest.Stock += 1;

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

            // Trigger Notifications in background
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

            // Add restocked items to MAIN_STORAGE
            var locStock = await db.LiquorLocationStocks.FirstOrDefaultAsync(ls => ls.ItemId == itemId && ls.LocationName == "MAIN_STORAGE");
            if (locStock == null)
            {
                locStock = new LiquorLocationStock { ItemId = itemId, LocationName = "MAIN_STORAGE", Stock = item.CurrentStock };
                db.LiquorLocationStocks.Add(locStock);
            }
            else
            {
                locStock.Stock += amount;
            }

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

        public async Task<LiquorTransaction> AdjustStockAsync(int itemId, int userId, int delta, string reason, string? locationName = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var item = await db.LiquorItems.FindAsync(itemId);
            if (item == null) throw new Exception("Item not found");

            // Route to correct location
            string targetLocation = locationName ?? "MAIN_STORAGE";
            if (locationName == null && reason.StartsWith("POS Sale", StringComparison.OrdinalIgnoreCase))
            {
                if (item.IsUnitBased || item.IsBeer)
                {
                    targetLocation = "MAIN_STORAGE";
                }
                else
                {
                    targetLocation = "DOWNSTAIRS_BAR"; // Default to downstairs for POS sales of pour drinks
                    if (reason.Contains("Upstairs Bar", StringComparison.OrdinalIgnoreCase) || reason.Contains("UPSTAIRS", StringComparison.OrdinalIgnoreCase))
                    {
                        targetLocation = "UPSTAIRS_BAR";
                    }
                }
            }

            var locStock = await db.LiquorLocationStocks.FirstOrDefaultAsync(ls => ls.ItemId == itemId && ls.LocationName == targetLocation);
            if (locStock == null)
            {
                decimal initialStock = targetLocation == "MAIN_STORAGE" ? item.CurrentStock : 0;
                locStock = new LiquorLocationStock { ItemId = itemId, LocationName = targetLocation, Stock = initialStock };
                db.LiquorLocationStocks.Add(locStock);
            }

            var oldStock = locStock.Stock;
            locStock.Stock += delta;
            if (locStock.Stock < 0) locStock.Stock = 0; // Prevent negative stock

            var actualDelta = locStock.Stock - oldStock;

            // Sync master CurrentStock only for MAIN_STORAGE
            if (targetLocation == "MAIN_STORAGE")
            {
                item.CurrentStock = (int)Math.Max(0, Math.Round(locStock.Stock));
            }

            var transaction = new LiquorTransaction
            {
                ItemId = itemId,
                UserId = userId,
                ChangeAmount = delta,
                TransactionType = "Adjustment",
                Notes = $"{reason} (Location: {targetLocation}, From {oldStock:F2} to {locStock.Stock:F2})",
                Timestamp = DateTime.UtcNow
            };

            db.LiquorTransactions.Add(transaction);
            await db.SaveChangesAsync();

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
                try {
                    var settings = await _settingsService.GetAsync();
                    var vendor = await db.LiquorVendors.FindAsync(order.VendorId);
                    
                    if (vendor != null && !string.IsNullOrEmpty(vendor.Email))
                    {
                        var subject = $"Liquor Order #{order.Id} - GFC System";
                        
                        // Re-fetch order with items for email body
                        var fullOrder = await db.LiquorOrders
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
                                order.IsEmailed = true;
                                order.LastEmailedDate = fullOrder.LastEmailedDate;
                                await db.SaveChangesAsync();
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
                    Console.WriteLine($"[LiquorService] Emailing failed for Order #{order.Id}: {ex}");
                }
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

            var cleanInstructions = order.SpecialInstructions;
            if (!string.IsNullOrEmpty(cleanInstructions))
            {
                cleanInstructions = cleanInstructions.Replace("Placed via Mobile", "").Trim().Trim(',', ';', ' ').Trim();
            }

            if (!string.IsNullOrEmpty(cleanInstructions))
            {
                sb.AppendLine("<div style='background-color: #fff9db; border-left: 4px solid #fcc419; padding: 15px; border-radius: 4px; margin-bottom: 30px;'>");
                sb.AppendLine("<div style='font-size: 11px; font-weight: bold; color: #856404; text-transform: uppercase; margin-bottom: 5px;'>Delivery Instructions:</div>");
                sb.AppendLine($"<div style='font-size: 14px; color: #2c3e50;'>{cleanInstructions}</div>");
                sb.AppendLine("</div>");
            }

            // Items Table
            sb.AppendLine("<table style='width: 100%; border-collapse: collapse;'>");
            sb.AppendLine("<thead><tr style='border-bottom: 2px solid #edf2f7;'>");
            sb.AppendLine("<th style='padding: 12px 5px; text-align: left; font-size: 12px; color: #718096; text-transform: uppercase;'>Qty</th>");
            sb.AppendLine("<th style='padding: 12px 5px; text-align: left; font-size: 12px; color: #718096; text-transform: uppercase;'>Product Description</th>");
            sb.AppendLine("<th style='padding: 12px 5px; text-align: right; font-size: 12px; color: #718096; text-transform: uppercase;'>Unit Price</th>");
            sb.AppendLine("<th style='padding: 12px 5px; text-align: right; font-size: 12px; color: #718096; text-transform: uppercase;'>Deposit/Fee</th>");
            sb.AppendLine("<th style='padding: 12px 5px; text-align: right; font-size: 12px; color: #718096; text-transform: uppercase;'>Subtotal</th>");
            sb.AppendLine("</tr></thead><tbody>");

            foreach (var item in order.OrderItems)
            {
                var packSize = item.LiquorItem?.PackSize ?? 1;
                var isCase = packSize > 1;
                var displayUnits = isCase ? (decimal)item.Quantity / packSize : item.Quantity;
                var unitLabel = isCase ? (displayUnits == 1 ? "Case" : "Cases") : (displayUnits == 1 ? "Bottle" : "Bottles");
                
                var depositFee = item.BottleFeeAtTimeOfOrder * item.Quantity;
                var subtotal = (displayUnits * item.UnitPriceAtTimeOfOrder) + depositFee;

                sb.AppendLine("<tr style='border-bottom: 1px solid #edf2f7;'>");
                sb.AppendLine($"<td style='padding: 15px 5px; vertical-align: top; font-weight: bold; color: #1a1a1a; white-space: nowrap;'>{displayUnits:G29} {unitLabel}</td>");
                sb.AppendLine("<td style='padding: 15px 5px; vertical-align: top;'>");
                sb.AppendLine($"<div style='font-weight: bold; color: #1a1a1a; font-size: 15px;'>{item.LiquorItem?.Name}</div>");
                sb.AppendLine($"<div style='font-size: 12px; color: #718096; margin-top: 2px;'>{item.LiquorItem?.BottleSize}</div>");
                sb.AppendLine("</td>");
                sb.AppendLine($"<td style='padding: 15px 5px; vertical-align: top; text-align: right; color: #4a5568;'>{item.UnitPriceAtTimeOfOrder:C}</td>");
                sb.AppendLine($"<td style='padding: 15px 5px; vertical-align: top; text-align: right; color: #4a5568;'>{(depositFee > 0 ? depositFee.ToString("C") : "-")}</td>");
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
            
            var alreadyReceived = await db.LiquorTransactions.AnyAsync(t => t.TransactionType == "Restock" && t.Notes.Contains($"Order #{order.Id} Received"));
            if (alreadyReceived) return; // Already processed

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

        public async Task ResolveBackorderAsync(int orderItemId, int userId, bool adjustStock = false)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var orderItem = await db.LiquorOrderItems
                .Include(oi => oi.Order)
                .OrderBy(oi => oi.Id)
                .FirstOrDefaultAsync(oi => oi.Id == orderItemId);

            if (orderItem == null || orderItem.IsResolved) return;

            if (adjustStock)
            {
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

        public async Task<List<LiquorRecommendationDTO>> GetOrderRecommendationsAsync(int? vendorId = null, string mode = "Recent", int recentDays = 30, DateTime? seasonalTargetDate = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            var itemsQuery = db.LiquorItems
                 .Include(i => i.Vendor)
                 .Where(i => i.IsActive);

            if (vendorId.HasValue && vendorId.Value > 0)
            {
                itemsQuery = itemsQuery.Where(i => i.VendorId == vendorId.Value);
            }

            var items = await itemsQuery.ToListAsync();
            var recommendations = new List<LiquorRecommendationDTO>();

            // Date ranges for calculations
            var now = DateTime.UtcNow;
            var recentStart = now.AddDays(-recentDays);
            
            // YoY variables
            double yoyMultiplier = 1.0;
            var current30Start = now.AddDays(-30);
            var prev30Start = now.AddDays(-395); // Approx same time last year
            var prev30End = now.AddDays(-365);

            if (mode == "Seasonal")
            {
                // Calculate overall YoY growth factor for clamp checks
                var currentSalesTotal = await db.LiquorTransactions
                    .Where(t => t.Timestamp >= current30Start && t.ChangeAmount < 0)
                    .SumAsync(t => Math.Abs(t.ChangeAmount));

                var prevSalesTotal = await db.LiquorTransactions
                    .Where(t => t.Timestamp >= prev30Start && t.Timestamp <= prev30End && t.ChangeAmount < 0)
                    .SumAsync(t => Math.Abs(t.ChangeAmount));

                if (prevSalesTotal > 0 && currentSalesTotal > 0)
                {
                    yoyMultiplier = (double)currentSalesTotal / prevSalesTotal;
                    // Clamp to prevent wild seasonal spikes
                    yoyMultiplier = Math.Clamp(yoyMultiplier, 0.5, 2.0);
                }
            }

            // Get historical transactions for usages (checkouts & negative adjustments)
            var rawTransactions = await db.LiquorTransactions
                .Include(t => t.Item)
                .Where(t => t.ChangeAmount < 0 && t.Item != null && t.Notes != null)
                .ToListAsync();

            var allTransactions = rawTransactions.Where(t => {
                var cat = t.Item.Category?.ToUpper() ?? "";
                return ((cat == "BEER" || cat == "SELTZER" || cat == "CIDER") && t.Notes.Contains("POS Sale")) ||
                       ((cat == "LIQUOR" || cat == "WINE") && (t.Notes.Contains("POS End-of-Shift Removal") || t.TransactionType == "Checkout"));
            }).ToList();

            // Get recent order history for feedback loops (last 4 weeks)
            var fourWeeksAgo = now.AddDays(-28);
            var recentOrders = await db.LiquorOrders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderDate >= fourWeeksAgo && o.Status == "Received")
                .ToListAsync();

            foreach (var item in items)
            {
                double avgWeeklyUsage = 0;
                double totalPeriodUsage = 0;

                // 1. Calculate historical baseline usage
                if (mode == "Seasonal")
                {
                    var targetDate = seasonalTargetDate ?? now;
                    var seasonalStart = targetDate.AddYears(-1).AddDays(-15);
                    var seasonalEnd = targetDate.AddYears(-1).AddDays(15);

                    var seasonalCheckouts = allTransactions
                        .Where(t => t.ItemId == item.Id && t.Timestamp >= seasonalStart && t.Timestamp <= seasonalEnd)
                        .Sum(t => Math.Abs(t.ChangeAmount));

                    // 30 days = ~4.28 weeks
                    avgWeeklyUsage = (seasonalCheckouts / 4.28) * yoyMultiplier;
                    totalPeriodUsage = seasonalCheckouts;
                }
                else // Recent Trends
                {
                    var itemTx = allTransactions.Where(t => t.ItemId == item.Id).ToList();
                    
                    double w1Usage = itemTx.Where(t => t.Timestamp >= now.AddDays(-7)).Sum(t => Math.Abs(t.ChangeAmount));
                    double w2Usage = itemTx.Where(t => t.Timestamp >= now.AddDays(-14) && t.Timestamp < now.AddDays(-7)).Sum(t => Math.Abs(t.ChangeAmount));
                    double w3Usage = itemTx.Where(t => t.Timestamp >= now.AddDays(-21) && t.Timestamp < now.AddDays(-14)).Sum(t => Math.Abs(t.ChangeAmount));
                    double w4Usage = itemTx.Where(t => t.Timestamp >= recentStart && t.Timestamp < now.AddDays(-21)).Sum(t => Math.Abs(t.ChangeAmount));
                    
                    if (recentDays <= 7)
                    {
                        avgWeeklyUsage = w1Usage;
                    }
                    else if (recentDays <= 14)
                    {
                        avgWeeklyUsage = (w1Usage * 0.70) + (w2Usage * 0.30);
                    }
                    else if (recentDays <= 21)
                    {
                        avgWeeklyUsage = (w1Usage * 0.60) + (w2Usage * 0.30) + (w3Usage * 0.10);
                    }
                    else
                    {
                        double w4Weeks = (recentDays - 21) / 7.0;
                        double w4Normalized = w4Usage / (w4Weeks > 0 ? w4Weeks : 1.0);
                        avgWeeklyUsage = (w1Usage * 0.50) + (w2Usage * 0.30) + (w3Usage * 0.15) + (w4Normalized * 0.05);
                    }
                    totalPeriodUsage = w1Usage + w2Usage + w3Usage + w4Usage;
                }

                double trueAvgWeeklyUsage = avgWeeklyUsage;

                // 2. Error-Correction Loop (Adjust based on recent order vs. usage variance)
                var receivedInPeriod = recentOrders
                    .SelectMany(o => o.OrderItems)
                    .Where(oi => oi.LiquorItemId == item.Id)
                    .Sum(oi => oi.Quantity);

                var checkedOutInPeriod = allTransactions
                    .Where(t => t.ItemId == item.Id && t.Timestamp >= fourWeeksAgo)
                    .Sum(t => Math.Abs(t.ChangeAmount));

                // Weekly average error = (weekly ordered) - (weekly consumed)
                double weeklyOrderAvg = receivedInPeriod / 4.0;
                double weeklyUsageAvg = checkedOutInPeriod / 4.0;
                double error = weeklyOrderAvg - weeklyUsageAvg;

                if (error > 0)
                {
                    // Over-ordered in the past; reduce predicted usage to compensate
                    avgWeeklyUsage = Math.Max(0, avgWeeklyUsage - error);
                }
                else if (error < 0)
                {
                    // Under-ordered (possible stockout); boost predicted usage
                    avgWeeklyUsage += Math.Abs(error);
                }

                // 3. Recommended order calculation
                // Target stock includes 20% safety buffer + minimum limit
                double targetStock = (avgWeeklyUsage * 1.20) + item.MinStockLimit;
                int recommendedQty = (int)Math.Max(0, Math.Ceiling(targetStock - item.CurrentStock));

                // Apply minimum order and case rounding constraints
                if (recommendedQty > 0)
                {
                    if (item.OrderByCaseOnly || item.PackSize > 1)
                    {
                        int packSize = item.PackSize > 0 ? item.PackSize : 1;
                        int neededCases = (int)Math.Ceiling((double)recommendedQty / packSize);
                        
                        if (item.MinOrderCases.HasValue && neededCases < item.MinOrderCases.Value)
                        {
                            neededCases = item.MinOrderCases.Value;
                        }

                        recommendedQty = neededCases * packSize;
                    }

                    if (recommendedQty < item.MinimumOrderQuantity)
                    {
                        recommendedQty = item.MinimumOrderQuantity;
                        
                        // Re-align to case size if needed
                        if (item.OrderByCaseOnly && item.PackSize > 1)
                        {
                            int neededCases = (int)Math.Ceiling((double)recommendedQty / item.PackSize);
                            recommendedQty = neededCases * item.PackSize;
                        }
                    }
                }

                recommendations.Add(new LiquorRecommendationDTO
                {
                    ItemId = item.Id,
                    ProductName = item.Name,
                    Category = item.Category ?? "Uncategorized",
                    CurrentStock = item.CurrentStock,
                    MinStockLimit = item.MinStockLimit,
                    MinimumOrderQuantity = item.MinimumOrderQuantity,
                    AvgWeeklyUsage = Math.Round(trueAvgWeeklyUsage, 2),
                    RecommendedQuantity = recommendedQty,
                    UnitPrice = item.CurrentPrice,
                    CasePrice = item.CasePrice ?? (item.CurrentPrice * item.PackSize),
                    PackSize = item.PackSize,
                    OrderByCaseOnly = item.OrderByCaseOnly,
                    VendorId = item.VendorId,
                    VendorName = item.Vendor?.Name ?? "Unassigned",
                    IsTopUpSuggestion = false,
                    ExcludeFromPredictions = item.ExcludeFromPredictions,
                    Reason = recommendedQty > 0 ? "Below minimum stock or projected by usage trends." : string.Empty,
                    IsUnitBased = item.IsUnitBased,
                    TotalPeriodUsage = Math.Round(totalPeriodUsage, 2)
                });
            }

            // 4. Vendor Minimum Order Fill Top-ups
            // Group recommendations by vendor to check if they meet the minimum order limit
            var recommendationsByVendor = recommendations.GroupBy(r => r.VendorId);
            foreach (var vendorGroup in recommendationsByVendor)
            {
                if (!vendorGroup.Key.HasValue) continue;

                var firstRec = vendorGroup.First();
                var vendor = items.FirstOrDefault(i => i.VendorId == vendorGroup.Key)?.Vendor;
                if (vendor == null || vendor.MinimumOrderAmount <= 0) continue;

                // Calculate current recommended order total for this vendor
                decimal currentOrderTotal = 0;
                foreach (var rec in vendorGroup)
                {
                    if (rec.RecommendedQuantity <= 0) continue;

                    if (rec.PackSize > 1 && rec.OrderByCaseOnly)
                    {
                        int cases = rec.RecommendedQuantity / rec.PackSize;
                        currentOrderTotal += cases * rec.CasePrice;
                    }
                    else
                    {
                        currentOrderTotal += rec.RecommendedQuantity * rec.UnitPrice;
                    }
                }

                // If below the minimum order limit, suggest top-up items
                if (currentOrderTotal > 0 && currentOrderTotal < vendor.MinimumOrderAmount)
                {
                    decimal remainingNeeded = vendor.MinimumOrderAmount - currentOrderTotal;

                    // Get items from this vendor that currently have no recommended quantity
                    var topUpCandidates = vendorGroup
                        .Where(r => r.RecommendedQuantity == 0)
                        .Select(r => new
                        {
                            Rec = r,
                            // Rank by days until stock runs out (CurrentStock / WeeklyUsage)
                            DaysRemaining = r.AvgWeeklyUsage > 0 ? (r.CurrentStock / r.AvgWeeklyUsage) * 7.0 : 9999.0
                        })
                        .OrderBy(c => c.DaysRemaining) // Run out soonest first
                        .ToList();

                    foreach (var candidate in topUpCandidates)
                    {
                        if (remainingNeeded <= 0) break;

                        // Recommend 1 Case or MinimumOrderQuantity
                        int suggestQty = candidate.Rec.PackSize > 1 ? candidate.Rec.PackSize : candidate.Rec.MinimumOrderQuantity;
                        decimal itemCost = candidate.Rec.PackSize > 1 ? candidate.Rec.CasePrice : (suggestQty * candidate.Rec.UnitPrice);

                        candidate.Rec.RecommendedQuantity = suggestQty;
                        candidate.Rec.IsTopUpSuggestion = true;
                        candidate.Rec.Reason = $"Suggested top-up to meet vendor minimum order limit ({vendor.MinimumOrderAmount:C}). Est. run-out: {Math.Round(candidate.DaysRemaining)} days.";

                        remainingNeeded -= itemCost;
                    }
                }
            }

            return recommendations.OrderByDescending(r => r.RecommendedQuantity > 0).ThenBy(r => r.ProductName).ToList();
        }

        public async Task<IEnumerable<PosCategory>> GetAllCategoriesAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.PosCategories.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LiquorLocationStock>> GetLocationStocksAsync(string? locationName = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var query = db.LiquorLocationStocks
                .Include(ls => ls.Item)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(locationName))
            {
                query = query.Where(ls => ls.LocationName == locationName);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<LiquorLocationStock>> GetItemStocksAsync(int itemId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorLocationStocks
                .Include(ls => ls.Item)
                .Where(ls => ls.ItemId == itemId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task ReconcileLocationStockAsync(string locationName, IEnumerable<GFC.Core.Models.StockReconcileEntry> entries, int userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            foreach (var entry in entries)
            {
                var locStock = await db.LiquorLocationStocks
                    .Include(ls => ls.Item)
                    .FirstOrDefaultAsync(ls => ls.ItemId == entry.ItemId && ls.LocationName == locationName);

                if (locStock == null)
                {
                    locStock = new LiquorLocationStock
                    {
                        ItemId = entry.ItemId,
                        LocationName = locationName,
                        Stock = 0
                    };
                    db.LiquorLocationStocks.Add(locStock);
                }

                var oldStock = locStock.Stock;
                decimal newStock = locationName == "MAIN_STORAGE" ? entry.ActualCount : entry.ActualCountDecimal;
                decimal delta = newStock - oldStock;

                if (delta == 0) continue;

                locStock.Stock = newStock;

                if (locationName == "MAIN_STORAGE")
                {
                    var item = await db.LiquorItems.FindAsync(entry.ItemId);
                    if (item != null)
                    {
                        item.CurrentStock = (int)newStock;
                    }
                }

                var transaction = new LiquorTransaction
                {
                    ItemId = entry.ItemId,
                    UserId = userId,
                    ChangeAmount = (int)Math.Round(delta),
                    TransactionType = "Reconcile",
                    Notes = $"Reconcile [{locationName}]: {newStock:F2} (Adjusted from {oldStock:F2})",
                    Timestamp = DateTime.UtcNow
                };
                db.LiquorTransactions.Add(transaction);
            }
            await db.SaveChangesAsync();
        }

        public async Task TransferStockAsync(int itemId, string fromLocation, string toLocation, decimal amount, int userId, string? notes = null)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            var source = await db.LiquorLocationStocks.FirstOrDefaultAsync(ls => ls.ItemId == itemId && ls.LocationName == fromLocation);
            if (source == null)
            {
                source = new LiquorLocationStock { ItemId = itemId, LocationName = fromLocation, Stock = 0 };
                db.LiquorLocationStocks.Add(source);
            }
            source.Stock -= amount;
            if (source.Stock < 0) source.Stock = 0;

            var dest = await db.LiquorLocationStocks.FirstOrDefaultAsync(ls => ls.ItemId == itemId && ls.LocationName == toLocation);
            if (dest == null)
            {
                dest = new LiquorLocationStock { ItemId = itemId, LocationName = toLocation, Stock = 0 };
                db.LiquorLocationStocks.Add(dest);
            }
            dest.Stock += amount;

            var item = await db.LiquorItems.FindAsync(itemId);
            if (item != null)
            {
                if (fromLocation == "MAIN_STORAGE")
                {
                    item.CurrentStock = (int)Math.Max(0, Math.Round(source.Stock));
                }
                else if (toLocation == "MAIN_STORAGE")
                {
                    item.CurrentStock = (int)Math.Max(0, Math.Round(dest.Stock));
                }
            }

            var transaction = new LiquorTransaction
            {
                ItemId = itemId,
                UserId = userId,
                ChangeAmount = (int)Math.Round(amount),
                TransactionType = "Adjustment",
                Notes = notes ?? $"Transfer: {amount:F2} from {fromLocation} to {toLocation}",
                Timestamp = DateTime.UtcNow
            };
            db.LiquorTransactions.Add(transaction);

            await db.SaveChangesAsync();
        }
    }
}


