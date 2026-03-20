using GFC.BlazorServer.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models;
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

        public LiquorService(IDbContextFactory<GfcDbContext> dbFactory, IServiceScopeFactory scopeFactory)
        {
            _dbFactory = dbFactory;
            _scopeFactory = scopeFactory;
        }

        public async Task<IEnumerable<LiquorItem>> GetAllItemsAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorItems
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
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<LiquorItem?> GetItemByUpcAsync(string upc)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorItems.FirstOrDefaultAsync(i => i.UpcCode == upc && i.IsActive);
        }

        public async Task<LiquorItem> CreateItemAsync(LiquorItem item)
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
                db.LiquorItems.Add(item);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Liquor Create Failed: {innerMessage}");
            }
            return item;
        }

        public async Task UpdateItemAsync(LiquorItem item)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            // Check for duplicate UPC on OTHER items
            if (!string.IsNullOrEmpty(item.UpcCode))
            {
                var exists = await db.LiquorItems.AnyAsync(i => i.UpcCode == item.UpcCode && i.Id != item.Id && i.IsActive);
                if (exists) throw new Exception($"The UPC code '{item.UpcCode}' is already assigned to another product.");
            }

            var existing = await db.LiquorItems.FindAsync(item.Id);
            if (existing != null)
            {
            try 
            {
                // Safety: Entry tracking check
                db.Entry(existing).CurrentValues.SetValues(item);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Liquor Update Failed: {innerMessage}");
            }
            }
            else
            {
                throw new Exception("Item not found in database.");
            }
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
            return await db.LiquorVendors.OrderBy(v => v.Name).ToListAsync();
        }

        public async Task<LiquorVendor?> GetVendorByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorVendors.Include(v => v.Items).FirstOrDefaultAsync(v => v.Id == id);
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
                .Include(o => o.Vendor)
                .Include(o => o.User)
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
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<LiquorOrder> CreateOrderAsync(LiquorOrder order)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            order.Status = "Placed";
            order.OrderDate = DateTime.UtcNow;
            
            db.LiquorOrders.Add(order);
            await db.SaveChangesAsync();
            return order;
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

        public async Task MarkOrderAsPaidAsync(int orderId, DateTime paidDate)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var order = await db.LiquorOrders.FindAsync(orderId);
            if (order != null)
            {
                order.IsPaid = true;
                order.PaidDate = paidDate;
                await db.SaveChangesAsync();
            }
        }

        public async Task ReceiveOrderAsync(int orderId, int userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var order = await db.LiquorOrders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) throw new Exception("Order not found");
            if (order.Status == "Received") return; // Already processed

            foreach (var orderItem in order.OrderItems)
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
                        Notes = $"Order #{order.Id} Received (Invoice: {order.InvoiceNumber})",
                        Timestamp = DateTime.UtcNow
                    };
                    db.LiquorTransactions.Add(transaction);
                }
            }

            order.Status = "Received";
            await db.SaveChangesAsync();
        }

        public async Task<LiquorNotificationRule?> GetNotificationRuleAsync(int userId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorNotificationRules.FirstOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task UpsertNotificationRuleAsync(LiquorNotificationRule rule)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var existing = await db.LiquorNotificationRules.FirstOrDefaultAsync(r => r.UserId == rule.UserId);
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
