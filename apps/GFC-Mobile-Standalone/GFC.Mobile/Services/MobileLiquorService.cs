using GFC.Core.Interfaces;
using GFC.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GFC.Mobile.Services
{
    public class MobileLiquorService : ILiquorService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        
        private const string ItemsCacheKey = "gfc_liquor_items";
        private const string VendorsCacheKey = "gfc_liquor_vendors";
        private const string OrdersCacheKey = "gfc_liquor_orders";
        private const string BackordersCacheKey = "gfc_liquor_backorders";
        private const string TransactionsCacheKey = "gfc_liquor_transactions";
        private const string TrendsCacheKey = "gfc_liquor_trends";

        public MobileLiquorService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }

        private async Task<T> GetCachedOrFetchAsync<T>(string cacheKey, string apiEndpoint, T fallback)
        {
            // 1. Network-First: Try to fetch fresh data from the server first when online
            try
            {
                using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(3));
                var data = await _http.GetFromJsonAsync<T>(apiEndpoint, cts.Token);
                if (data != null)
                {
                    await SaveToCacheAsync(cacheKey, data);
                    return data;
                }
            }
            catch { }

            // 2. Cache-Fallback: Fall back to local storage cache if offline or server is down
            try
            {
                var cachedJson = await _js.InvokeAsync<string>("localStorage.getItem", cacheKey);
                if (!string.IsNullOrEmpty(cachedJson))
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<T>(cachedJson, options) ?? fallback;
                }
            }
            catch { }

            return fallback;
        }

        private async Task SaveToCacheAsync<T>(string cacheKey, T data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                await _js.InvokeVoidAsync("localStorage.setItem", cacheKey, json);
            }
            catch { }
        }

        // Implementation of ILiquorService
        public async Task<IEnumerable<LiquorItem>> GetAllItemsAsync()
        {
            return await GetCachedOrFetchAsync<List<LiquorItem>>(ItemsCacheKey, "api/liquor/items", new List<LiquorItem>());
        }

        public async Task<LiquorItem?> GetItemByIdAsync(int id)
        {
            var items = await GetAllItemsAsync();
            foreach (var item in items)
            {
                if (item.Id == id) return item;
            }
            return null;
        }

        public async Task<LiquorItem?> GetItemByUpcAsync(string upc)
        {
            var items = await GetAllItemsAsync();
            foreach (var item in items)
            {
                if (item.UpcCode == upc) return item;
            }
            return null;
        }

        public async Task<LiquorItem> CreateItemAsync(LiquorItem item, int? userId = null)
        {
            // For offline support, add to local list
            var items = new List<LiquorItem>(await GetAllItemsAsync());
            item.Id = new Random().Next(-100000, -1); // Temporary negative ID
            items.Add(item);
            await SaveToCacheAsync(ItemsCacheKey, items);

            try
            {
                var response = await _http.PostAsJsonAsync($"api/liquor/items?userId={userId}", item);
                if (response.IsSuccessStatusCode)
                {
                    var saved = await response.Content.ReadFromJsonAsync<LiquorItem>();
                    if (saved != null)
                    {
                        // Replace temporary item with real saved item
                        items.Remove(item);
                        items.Add(saved);
                        await SaveToCacheAsync(ItemsCacheKey, items);
                        return saved;
                    }
                }
            }
            catch { }

            return item;
        }

        public async Task UpdateItemAsync(LiquorItem item, int? userId = null)
        {
            var items = new List<LiquorItem>(await GetAllItemsAsync());
            var existing = items.Find(i => i.Id == item.Id);
            if (existing != null)
            {
                items.Remove(existing);
                items.Add(item);
                await SaveToCacheAsync(ItemsCacheKey, items);
            }

            try
            {
                await _http.PutAsJsonAsync($"api/liquor/items/{item.Id}?userId={userId}", item);
            }
            catch { }
        }

        public async Task BulkSaveLiquorItemsAsync(IEnumerable<LiquorItem> items, int? userId = null)
        {
            await SaveToCacheAsync(ItemsCacheKey, items);
            try
            {
                await _http.PostAsJsonAsync($"api/liquor/items/bulk?userId={userId}", items);
            }
            catch { }
        }

        public async Task DeleteItemAsync(int id)
        {
            var items = new List<LiquorItem>(await GetAllItemsAsync());
            items.RemoveAll(i => i.Id == id);
            await SaveToCacheAsync(ItemsCacheKey, items);

            try
            {
                await _http.DeleteAsync($"api/liquor/items/{id}");
            }
            catch { }
        }

        public async Task<IEnumerable<LiquorVendor>> GetAllVendorsAsync()
        {
            return await GetCachedOrFetchAsync<List<LiquorVendor>>(VendorsCacheKey, "api/liquor/vendors", new List<LiquorVendor>());
        }

        public async Task<LiquorVendor?> GetVendorByIdAsync(int id)
        {
            var vendors = await GetAllVendorsAsync();
            foreach (var v in vendors)
            {
                if (v.Id == id) return v;
            }
            return null;
        }

        public async Task<LiquorVendor> CreateVendorAsync(LiquorVendor vendor)
        {
            var vendors = new List<LiquorVendor>(await GetAllVendorsAsync());
            vendor.Id = new Random().Next(-100000, -1);
            vendors.Add(vendor);
            await SaveToCacheAsync(VendorsCacheKey, vendors);

            try
            {
                var response = await _http.PostAsJsonAsync("api/liquor/vendors", vendor);
                if (response.IsSuccessStatusCode)
                {
                    var saved = await response.Content.ReadFromJsonAsync<LiquorVendor>();
                    if (saved != null)
                    {
                        vendors.Remove(vendor);
                        vendors.Add(saved);
                        await SaveToCacheAsync(VendorsCacheKey, vendors);
                        return saved;
                    }
                }
            }
            catch { }

            return vendor;
        }

        public async Task UpdateVendorAsync(LiquorVendor vendor)
        {
            var vendors = new List<LiquorVendor>(await GetAllVendorsAsync());
            var existing = vendors.Find(v => v.Id == vendor.Id);
            if (existing != null)
            {
                vendors.Remove(existing);
                vendors.Add(vendor);
                await SaveToCacheAsync(VendorsCacheKey, vendors);
            }

            try
            {
                await _http.PutAsJsonAsync($"api/liquor/vendors/{vendor.Id}", vendor);
            }
            catch { }
        }

        public async Task DeleteVendorAsync(int id)
        {
            var vendors = new List<LiquorVendor>(await GetAllVendorsAsync());
            vendors.RemoveAll(v => v.Id == id);
            await SaveToCacheAsync(VendorsCacheKey, vendors);

            try
            {
                await _http.DeleteAsync($"api/liquor/vendors/{id}");
            }
            catch { }
        }

        public async Task<LiquorTransaction> CheckoutBottleAsync(int itemId, int userId, string? notes = null)
        {
            try
            {
                var response = await _http.PostAsync($"api/liquor/checkout/{itemId}?userId={userId}&notes={Uri.EscapeDataString(notes ?? "")}", null);
                if (response.IsSuccessStatusCode)
                {
                    var tx = await response.Content.ReadFromJsonAsync<LiquorTransaction>();
                    if (tx != null) return tx;
                }
            }
            catch { }

            return new LiquorTransaction { ItemId = itemId, UserId = userId, Notes = notes, Timestamp = DateTime.UtcNow };
        }

        public async Task<LiquorTransaction> RestockItemAsync(int itemId, int userId, int amount, string? notes = null)
        {
            try
            {
                var response = await _http.PostAsync($"api/liquor/restock/{itemId}?userId={userId}&amount={amount}&notes={Uri.EscapeDataString(notes ?? "")}", null);
                if (response.IsSuccessStatusCode)
                {
                    var tx = await response.Content.ReadFromJsonAsync<LiquorTransaction>();
                    if (tx != null) return tx;
                }
            }
            catch { }

            return new LiquorTransaction { ItemId = itemId, UserId = userId, Timestamp = DateTime.UtcNow };
        }

        public async Task<LiquorTransaction> AdjustStockAsync(int itemId, int userId, int delta, string reason, string? locationName = null)
        {
            try
            {
                string url = $"api/liquor/adjust/{itemId}?userId={userId}&delta={delta}&reason={Uri.EscapeDataString(reason)}";
                if (!string.IsNullOrEmpty(locationName))
                {
                    url += $"&locationName={Uri.EscapeDataString(locationName)}";
                }
                var response = await _http.PostAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    var tx = await response.Content.ReadFromJsonAsync<LiquorTransaction>();
                    if (tx != null) return tx;
                }
            }
            catch { }

            return new LiquorTransaction { ItemId = itemId, UserId = userId, Timestamp = DateTime.UtcNow };
        }

        public async Task<IEnumerable<LiquorTransaction>> GetRecentTransactionsAsync(int count = 50)
        {
            return await GetCachedOrFetchAsync<List<LiquorTransaction>>(TransactionsCacheKey, $"api/liquor/transactions?count={count}", new List<LiquorTransaction>());
        }

        public async Task<IEnumerable<LiquorOrder>> GetAllOrdersAsync()
        {
            return await GetCachedOrFetchAsync<List<LiquorOrder>>(OrdersCacheKey, "api/liquor/orders", new List<LiquorOrder>());
        }

        public async Task<LiquorOrder?> GetOrderByIdAsync(int id)
        {
            var orders = await GetAllOrdersAsync();
            foreach (var o in orders)
            {
                if (o.Id == id) return o;
            }
            return null;
        }

        public async Task<LiquorOrder> CreateOrderAsync(LiquorOrder order, bool sendEmail = false)
        {
            var orders = new List<LiquorOrder>(await GetAllOrdersAsync());
            order.Id = new Random().Next(-100000, -1);
            orders.Add(order);
            await SaveToCacheAsync(OrdersCacheKey, orders);

            try
            {
                var response = await _http.PostAsJsonAsync($"api/liquor/orders?sendEmail={sendEmail}", order);
                if (response.IsSuccessStatusCode)
                {
                    var saved = await response.Content.ReadFromJsonAsync<LiquorOrder>();
                    if (saved != null)
                    {
                        orders.Remove(order);
                        orders.Add(saved);
                        await SaveToCacheAsync(OrdersCacheKey, orders);
                        return saved;
                    }
                }
            }
            catch { }

            return order;
        }

        public async Task ResendOrderEmailAsync(int orderId)
        {
            try
            {
                await _http.PostAsync($"api/liquor/orders/{orderId}/resend-email", null);
            }
            catch { }
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status, string? invoiceNumber = null, decimal? taxAmount = null, decimal? additionalCosts = null)
        {
            var orders = new List<LiquorOrder>(await GetAllOrdersAsync());
            var existing = orders.Find(o => o.Id == orderId);
            if (existing != null)
            {
                existing.Status = status;
                existing.InvoiceNumber = invoiceNumber;
                existing.TaxAmount = taxAmount ?? 0;
                existing.AdditionalCosts = additionalCosts ?? 0;
                await SaveToCacheAsync(OrdersCacheKey, orders);
            }

            try
            {
                await _http.PostAsJsonAsync($"api/liquor/orders/{orderId}/status", new { status, invoiceNumber, taxAmount, additionalCosts });
            }
            catch { }
        }

        public async Task MarkOrderAsPaidAsync(int orderId, DateTime paidDate, decimal paidAmount, int paidByUserId)
        {
            var orders = new List<LiquorOrder>(await GetAllOrdersAsync());
            var existing = orders.Find(o => o.Id == orderId);
            if (existing != null)
            {
                existing.IsPaid = true;
                existing.PaidDate = paidDate;
                existing.ActualPaidAmount = paidAmount;
                existing.PaidByUserId = paidByUserId;
                await SaveToCacheAsync(OrdersCacheKey, orders);
            }

            try
            {
                await _http.PostAsJsonAsync($"api/liquor/orders/{orderId}/paid", new { paidDate, paidAmount, paidByUserId });
            }
            catch { }
        }

        public async Task ReceiveOrderAsync(int orderId, int userId)
        {
            try
            {
                await _http.PostAsync($"api/liquor/orders/{orderId}/receive-process?userId={userId}", null);
            }
            catch { }
        }

        public async Task UpdateOrderItemsBackorderAsync(int orderId, List<int> backorderedOrderItemIds)
        {
            try
            {
                await _http.PostAsJsonAsync($"api/liquor/orders/{orderId}/backorders", backorderedOrderItemIds);
            }
            catch { }
        }

        public async Task<IEnumerable<LiquorOrderItem>> GetPendingBackordersAsync()
        {
            return await GetCachedOrFetchAsync<List<LiquorOrderItem>>(BackordersCacheKey, "api/liquor/backorders", new List<LiquorOrderItem>());
        }

        public async Task ResolveBackorderAsync(int orderItemId, int userId, bool adjustStock = false)
        {
            try
            {
                await _http.PostAsync($"api/liquor/backorders/{orderItemId}/resolve?userId={userId}&adjustStock={adjustStock}", null);
            }
            catch { }
        }

        public async Task<LiquorNotificationRule?> GetNotificationRuleAsync(int userId)
        {
            try
            {
                return await _http.GetFromJsonAsync<LiquorNotificationRule>($"api/liquor/notifications/rules/{userId}");
            }
            catch { }
            return null;
        }

        public async Task UpsertNotificationRuleAsync(LiquorNotificationRule rule)
        {
            try
            {
                await _http.PostAsJsonAsync("api/liquor/notifications/rules", rule);
            }
            catch { }
        }

        public async Task<IEnumerable<LiquorNotificationRule>> GetSubscribedUsersAsync()
        {
            try
            {
                return await _http.GetFromJsonAsync<List<LiquorNotificationRule>>("api/liquor/notifications/subscribed") ?? new List<LiquorNotificationRule>();
            }
            catch { }
            return new List<LiquorNotificationRule>();
        }

        public async Task ReconcileStockAsync(IEnumerable<StockReconcileEntry> entries, int userId)
        {
            try
            {
                await _http.PostAsJsonAsync($"api/liquor/reconcile?userId={userId}", entries);
            }
            catch { }
        }

        public async Task<List<ProductTrendDTO>> GetProductTrendsAsync(int daysLookback = 30)
        {
            return await GetCachedOrFetchAsync<List<ProductTrendDTO>>(TrendsCacheKey, $"api/liquor/trends?daysLookback={daysLookback}", new List<ProductTrendDTO>());
        }

        public async Task<IEnumerable<PosCategory>> GetAllCategoriesAsync()
        {
            return await GetCachedOrFetchAsync<List<PosCategory>>("gfc_liquor_categories", "api/liquor/categories", new List<PosCategory>());
        }

        public async Task<IEnumerable<LiquorLocationStock>> GetLocationStocksAsync(string? locationName = null)
        {
            try
            {
                string url = "api/liquor/locations/stocks";
                if (!string.IsNullOrEmpty(locationName))
                {
                    url += $"?locationName={Uri.EscapeDataString(locationName)}";
                }
                return await _http.GetFromJsonAsync<List<LiquorLocationStock>>(url) ?? new List<LiquorLocationStock>();
            }
            catch { }
            return new List<LiquorLocationStock>();
        }

        public async Task<IEnumerable<LiquorLocationStock>> GetItemStocksAsync(int itemId)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<LiquorLocationStock>>($"api/liquor/item/stocks/{itemId}") ?? new List<LiquorLocationStock>();
            }
            catch { }
            return new List<LiquorLocationStock>();
        }

        public async Task ReconcileLocationStockAsync(string locationName, IEnumerable<StockReconcileEntry> entries, int userId)
        {
            try
            {
                await _http.PostAsJsonAsync($"api/liquor/reconcile/location/{Uri.EscapeDataString(locationName)}?userId={userId}", entries);
            }
            catch { }
        }

        public async Task TransferStockAsync(int itemId, string fromLocation, string toLocation, decimal amount, int userId, string? notes = null)
        {
            try
            {
                string url = $"api/liquor/transfer?itemId={itemId}&fromLocation={Uri.EscapeDataString(fromLocation)}&toLocation={Uri.EscapeDataString(toLocation)}&amount={amount}&userId={userId}";
                if (!string.IsNullOrEmpty(notes))
                {
                    url += $"&notes={Uri.EscapeDataString(notes)}";
                }
                await _http.PostAsync(url, null);
            }
            catch { }
        }
    }
}
