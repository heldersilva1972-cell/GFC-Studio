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
        private readonly INotificationService _notificationService;

        public LiquorService(IDbContextFactory<GfcDbContext> dbFactory, INotificationService notificationService)
        {
            _dbFactory = dbFactory;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<LiquorItem>> GetAllItemsAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorItems.Where(i => i.IsActive).ToListAsync();
        }

        public async Task<LiquorItem?> GetItemByIdAsync(int id)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorItems.FindAsync(id);
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

            // Trigger Notifications
            await CheckAndNotifyAsync(item);

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
            if (actualDelta < 0) await CheckAndNotifyAsync(item);

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

        private async Task CheckAndNotifyAsync(LiquorItem item)
        {
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

            using var db = await _dbFactory.CreateDbContextAsync();
            var subscribers = await db.LiquorNotificationRules.ToListAsync();

            // Deduplicate by UserId to prevent sending multiple alerts to the same person if legacy data exists
            var uniqueSubscribers = subscribers
                .GroupBy(s => s.UserId)
                .Select(g => g.First())
                .ToList();

            foreach (var sub in uniqueSubscribers)
            {
                if (isCritical && !sub.NotifyOnEmpty) continue;
                if (!isCritical && !sub.NotifyOnLowStock) continue;

                var user = await db.AppUsers.FindAsync(sub.UserId);
                if (user == null) continue;

                // 1. Email (Prioritize reliability)
                if (sub.ReceiveEmail && !string.IsNullOrEmpty(user.Email))
                {
                    try 
                    {
                        await _notificationService.SendEmailAsync(user.Email, title, body);
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
                        await _notificationService.SendPushNotificationAsync(user.UserId, title, body, "/mobile/liquor/manage");
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
                            RecipientEmail = user.Email, // SMS doesn't have a dedicated field in SystemNotification yet
                            Subject = title,
                            Message = body,
                            Channel = "SMS",
                            Status = "Pending"
                        };
                        await _notificationService.DispatchNotificationAsync(smsNotification);
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
