using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface ILiquorService
    {
        // Item Management
        Task<IEnumerable<LiquorItem>> GetAllItemsAsync();
        Task<LiquorItem?> GetItemByIdAsync(int id);
        Task<LiquorItem?> GetItemByUpcAsync(string upc);
        Task<LiquorItem> CreateItemAsync(LiquorItem item);
        Task UpdateItemAsync(LiquorItem item);
        Task DeleteItemAsync(int id);

        // Transaction Management
        Task<LiquorTransaction> CheckoutBottleAsync(int itemId, int userId, string? notes = null);
        Task<LiquorTransaction> RestockItemAsync(int itemId, int userId, int amount, string? notes = null);
        Task<IEnumerable<LiquorTransaction>> GetRecentTransactionsAsync(int count = 50);

        // Notification Rules
        Task<LiquorNotificationRule?> GetNotificationRuleAsync(int userId);
        Task UpsertNotificationRuleAsync(LiquorNotificationRule rule);
        Task<IEnumerable<LiquorNotificationRule>> GetSubscribedUsersAsync();
    }
}
