using GFC.Core.Models;
using System;
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
        Task<LiquorItem> CreateItemAsync(LiquorItem item, int? userId = null);
        Task UpdateItemAsync(LiquorItem item, int? userId = null);
        Task BulkSaveLiquorItemsAsync(IEnumerable<LiquorItem> items, int? userId = null);
        Task DeleteItemAsync(int id);

        // Vendor Management
        Task<IEnumerable<LiquorVendor>> GetAllVendorsAsync();
        Task<LiquorVendor?> GetVendorByIdAsync(int id);
        Task<LiquorVendor> CreateVendorAsync(LiquorVendor vendor);
        Task UpdateVendorAsync(LiquorVendor vendor);
        Task DeleteVendorAsync(int id);

        // Transaction Management
        Task<LiquorTransaction> CheckoutBottleAsync(int itemId, int userId, string? notes = null);
        Task<LiquorTransaction> RestockItemAsync(int itemId, int userId, int amount, string? notes = null);
        Task<LiquorTransaction> AdjustStockAsync(int itemId, int userId, int delta, string reason);
        Task<IEnumerable<LiquorTransaction>> GetRecentTransactionsAsync(int count = 50);

        // Order Management
        Task<IEnumerable<LiquorOrder>> GetAllOrdersAsync();
        Task<LiquorOrder?> GetOrderByIdAsync(int id);
        Task<LiquorOrder> CreateOrderAsync(LiquorOrder order, bool sendEmail = false);
        Task<EmailResult> ResendOrderEmailAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId, string status, string? invoiceNumber = null, decimal? taxAmount = null, decimal? additionalCosts = null);
        Task MarkOrderAsPaidAsync(int orderId, DateTime paidDate, decimal paidAmount, int paidByUserId);
        Task ReceiveOrderAsync(int orderId, int userId);
        Task UpdateOrderItemsBackorderAsync(int orderId, List<int> backorderedOrderItemIds);
        Task<IEnumerable<LiquorOrderItem>> GetPendingBackordersAsync();
        Task ResolveBackorderAsync(int orderItemId, int userId, bool adjustStock = false);

        // Notification Rules
        Task<LiquorNotificationRule?> GetNotificationRuleAsync(int userId);
        Task UpsertNotificationRuleAsync(LiquorNotificationRule rule);
        Task<IEnumerable<LiquorNotificationRule>> GetSubscribedUsersAsync();

        // Inventory Audit (Reconciliation)
        Task ReconcileStockAsync(IEnumerable<GFC.Core.Models.StockReconcileEntry> entries, int userId);

        // Analytics
        Task<List<ProductTrendDTO>> GetProductTrendsAsync(int daysLookback = 30);

        // Categories
        Task<IEnumerable<PosCategory>> GetAllCategoriesAsync();
    }
}
