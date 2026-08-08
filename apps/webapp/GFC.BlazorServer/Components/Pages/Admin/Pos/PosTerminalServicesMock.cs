using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.EntityFrameworkCore;
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using GFC.Core.DTOs;
using GFC.Core.Interfaces;

namespace GFC.Pos.UI.Services
{
    public interface IPosTerminalService
    {
        Task<PosMenuDto> GetMenuAsync(bool force = false);
        Task<PosMenuDto?> GetCachedMenuAsync();
        Task SaveMenuToVaultAsync(PosMenuDto menu);
        Task SaveSaleAsync(PosSaleDto sale);
        Task<PosZReportDto?> GetZReportAsync(Guid id);
        Task<List<PosZReportDto>> GetZReportsAsync(string terminalName);
        Task SaveZReportAsync(PosZReportDto report);
        event Action? OutboxChanged;
        event Action<PosMenuDto>? MenuRefreshed;
        int TotalPendingCount { get; }
        DateTime? LastSynced { get; }
        Task<int> GetTotalPendingAsync();
        Task<DateTime> GetLastZTimeAsync(string terminalName);
        Task<PosSaleDto?> GetDartsRoundTodayAsync(string terminalName);
        Task<List<PosSaleDto>> GetDartsRoundsTodayAsync(string terminalName);
        Task<bool> CheckConnectivityAsync();
        Task<List<LiquorOrder>> GetPendingLiquorOrdersAsync(bool force = false);
        Task ReceiveLiquorOrderAsync(LiquorOrderReceiptDto receipt);
        Task<List<UserListItemDto>> GetAuthorizedUsersAsync();

        // LOCAL SHIFT DATABASE
        Task AddSaleToShiftAsync(PosSaleDto sale);
        Task VoidSaleAsync(Guid saleId, string reason);
        Task<ShiftAuditDto> GetShiftAuditAsync();
        Task ClearShiftAsync();
        Task FlushAllPendingAsync();
        Task<string> GetServerVersionAsync();
        Task<BanquetMasterSummaryDto?> GetBanquetMasterSummaryAsync(int eventId);
        Task<List<PosSaleDto>> GetUnsyncedSalesAsync();
        Task<MemberDrawPoolDto?> GetMemberDrawPoolAsync();
        Task<MemberDrawStatusDto?> GetMemberDrawStatusAsync(int memberId);
        Task<List<LiquorItem>> GetLiquorInventoryAsync(bool force = false);
    }

    public class ShiftAuditDto
    {
        public decimal CashTotal { get; set; }
        public decimal GrossTotal { get; set; }
        public decimal TokenCredits { get; set; }
        public Dictionary<string, int> ItemSummary { get; set; } = new();
        public Dictionary<string, decimal> ItemTotals { get; set; } = new();
        public Dictionary<string, int> RegularItemSummary { get; set; } = new();
        public Dictionary<string, decimal> RegularItemTotals { get; set; } = new();
        public List<PosSaleDto> VoidedSales { get; set; } = new();
        public PosSaleDto? LatestSale { get; set; }
        public List<BanquetShiftReportDto> Banquets { get; set; } = new();
        public decimal PayoutTotal { get; set; }
        public List<PosSaleDto> Payouts { get; set; } = new();
    }

    public class BanquetShiftReportDto
    {
        public int? ActiveEventId { get; set; }
        public string EventName { get; set; } = "";
        public List<decimal> Deposits { get; set; } = new();
        public decimal TotalSpent { get; set; }
        public Dictionary<string, int> ItemSummary { get; set; } = new();
        public Dictionary<string, decimal> ItemTotals { get; set; } = new();
        public string EventType { get; set; } = "RunningTab";
    }

    public interface IStationSettingsService
    {
        Task<string> GetTerminalNameAsync();
        Task SaveTerminalNameAsync(string name);
    }

    public interface IPrinterService
    {
        Task<bool> PrintReceiptAsync(string content);
        Task<bool> PrintRawDataAsync(byte[] data, global::System.Threading.CancellationToken cancellationToken = default);
        Task<bool> KickDrawerAsync();
        Task<List<UsbDeviceDto>> GetConnectedDevicesAsync();
    }

    public class UsbDeviceDto
    {
        public string Name { get; set; } = "";
        public int VendorId { get; set; }
        public int ProductId { get; set; }
        public string VidHex => VendorId.ToString("X4");
        public string PidHex => ProductId.ToString("X4");
    }

    public enum PrinterType { USB, Ethernet }

    public interface IPrinterConfigService
    {
        PrinterType CurrentPrinterType { get; set; }
        string PrinterVendorId { get; set; }
        string PrinterProductId { get; set; }
        string PrinterIpAddress { get; set; }
        
        void SaveSettings(string vid, string pid, string ip, PrinterType type);
        Task LoadSettingsAsync();
        (int? vid, int? pid) GetParsedSettings();
    }

    public class ConnectivityService : IAsyncDisposable
    {
        public bool IsOnline => true;
        public bool IsHardwareOnline => true;
        public bool IsServerReachable => true;
        public string EnvironmentName => "SIMULATOR";
        public event Action<bool>? ConnectivityChanged;

        public ConnectivityService(IJSRuntime js, HttpClient http) {}
        public Task InitializeAsync() => Task.CompletedTask;
        public Task<bool> CheckServerReachableAsync() => Task.FromResult(true);
        public Task<bool> GateAsync(string actionName) => Task.FromResult(true);
        public void OnConnectivityChanged(bool isOnline) {}
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}

namespace GFC.BlazorServer.Components.Pages.Admin.Pos
{
    using GFC.Pos.UI.Services;

    public class MockStationSettingsService : IStationSettingsService
    {
        private string _terminalName = "SIM-TERMINAL-01";
        public Task<string> GetTerminalNameAsync() => Task.FromResult(_terminalName);
        public Task SaveTerminalNameAsync(string name) { _terminalName = name; return Task.CompletedTask; }
    }

    public class MockPrinterService : IPrinterService
    {
        private readonly IJSRuntime _js;

        public MockPrinterService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<bool> PrintReceiptAsync(string content)
        {
            Console.WriteLine($"[SIMULATED PRINT] {content}");
            try
            {
                // Serialize content outside of the string interpolation to avoid verbatim string parsing issues
                string contentJson = global::System.Text.Json.JsonSerializer.Serialize(content);

                // For Web/PWA, we create a temporary hidden iframe with the content and print it
                await _js.InvokeVoidAsync("eval", $@"
                    (function(content) {{
                        const iframe = document.createElement('iframe');
                        iframe.style.position = 'fixed';
                        iframe.style.right = '0';
                        iframe.style.bottom = '0';
                        iframe.style.width = '0';
                        iframe.style.height = '0';
                        iframe.style.border = '0';
                        document.body.appendChild(iframe);
                        
                        const doc = iframe.contentWindow.document;
                        doc.open();
                        doc.write(content);
                        doc.close();
                        
                        iframe.contentWindow.focus();
                        iframe.contentWindow.print();
                        
                        // Remove iframe after print dialog is handled
                        setTimeout(() => document.body.removeChild(iframe), 1000);
                    }})({contentJson})");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SIMULATED PRINT] Browser printing failed: {ex.Message}");
                return false;
            }
        }
        public Task<bool> PrintRawDataAsync(byte[] data, global::System.Threading.CancellationToken cancellationToken = default) => Task.FromResult(true);
        public Task<bool> KickDrawerAsync()
        {
            Console.WriteLine("[SIMULATED KICK DRAWER]");
            return Task.FromResult(true);
        }
        public Task<List<UsbDeviceDto>> GetConnectedDevicesAsync() => Task.FromResult(new List<UsbDeviceDto>());
    }

    public class MockPrinterConfigService : IPrinterConfigService
    {
        public PrinterType CurrentPrinterType { get; set; } = PrinterType.USB;
        public string PrinterVendorId { get; set; } = "";
        public string PrinterProductId { get; set; } = "";
        public string PrinterIpAddress { get; set; } = "";
        
        public void SaveSettings(string vid, string pid, string ip, PrinterType type)
        {
            PrinterVendorId = vid;
            PrinterProductId = pid;
            PrinterIpAddress = ip;
            CurrentPrinterType = type;
        }
        public Task LoadSettingsAsync() => Task.CompletedTask;
        public (int? vid, int? pid) GetParsedSettings() => (null, null);
    }

    public class MockPosTerminalService : IPosTerminalService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbFactory;
        private readonly IVersionService _versionService;
        private ShiftAuditDto _currentShift = new();
        private List<PosSaleDto> _sales = new();
        private List<PosZReportDto> _zReports = new();

        public MockPosTerminalService(IDbContextFactory<GfcDbContext> dbFactory, IVersionService versionService)
        {
            _dbFactory = dbFactory;
            _versionService = versionService;
        }

        public event Action? OutboxChanged;
        public event Action<PosMenuDto>? MenuRefreshed;

        public int TotalPendingCount => 0;
        public DateTime? LastSynced => DateTime.Now;

        public Task<int> GetTotalPendingAsync() => Task.FromResult(0);
        public Task<DateTime> GetLastZTimeAsync(string terminalName) => Task.FromResult(DateTime.Now.AddDays(-1));
        public Task<bool> CheckConnectivityAsync() => Task.FromResult(true);
        public Task<string> GetServerVersionAsync() => Task.FromResult(_versionService.GetPosVersion());

        public async Task<PosMenuDto> GetMenuAsync(bool force = false)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var menu = new PosMenuDto();
            
            // 1. Categories
            menu.Categories = await db.PosCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .Select(c => c.Name)
                .ToListAsync();
            
            if (!menu.Categories.Contains("TOKENS"))
                menu.Categories.Add("TOKENS");
            
            // Add Dart Team tab if it's Thursday or in simulator (training mode)
            if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday || true)
            {
                if (!menu.Categories.Contains("DARTS CLUB ROUND"))
                    menu.Categories.Add("DARTS CLUB ROUND");
            }
            
            // 2. Items
            var items = await db.LiquorItems
                .Where(i => i.ShowInPos)
                .OrderBy(i => i.DisplayOrder)
                .ThenBy(i => i.Name)
                .ToListAsync();

            menu.Items = items.Select(i => new PosItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.RetailPrice,
                Category = i.Category ?? "MISC",
                DisplayOrder = i.DisplayOrder
            }).ToList();

            // 3. Tokens
            menu.Tokens = await db.PosTokens.Where(t => t.IsActive).ToListAsync();

            // 4. Active Events & Templates
            menu.ActiveEvents = await db.ActiveEvents.Where(e => e.Status == GFC.Core.Enums.EventTabStatus.Open).ToListAsync();
            menu.EventTemplates = await db.EventTemplates.Where(t => !t.IsDeleted).ToListAsync();
            
            // 5. Modifiers
            menu.ModifierCategories = new List<string> { "POURS", "MIXERS" };
            menu.Modifiers = new List<PosModifierDto>();
            
            // 6. Payout categories
            menu.PayoutCategories = new List<string> { "FOOD", "SUPPLIES", "MAINTENANCE", "REBATE/REFUND", "OTHER" };
            
            // 7. Profile name
            try
            {
                var terminal = await db.PosTerminals
                    .Include(t => t.MenuProfile)
                    .FirstOrDefaultAsync(t => !t.IsDeleted);
                if (terminal != null && terminal.MenuProfile != null)
                {
                    menu.ProfileName = terminal.MenuProfile.Name;
                }
            }
            catch {}
            
            return menu;
        }

        public Task<PosMenuDto?> GetCachedMenuAsync() => GetMenuAsync().ContinueWith(t => (PosMenuDto?)t.Result);
        public Task SaveMenuToVaultAsync(PosMenuDto menu) => Task.CompletedTask;

        // Simulator Mode: Save in memory
        public Task SaveSaleAsync(PosSaleDto sale)
        {
            if (_sales.Any(s => s.Id == sale.Id)) return Task.CompletedTask;
            _sales.Add(sale);
            _currentShift.LatestSale = sale;
            _currentShift.GrossTotal += sale.TotalAmount;
            _currentShift.CashTotal += sale.PaymentType == "CASH" ? sale.TotalAmount : 0;
            
            int tokensUsed = 0;
            try
            {
                if (!string.IsNullOrEmpty(sale.ItemsJson))
                {
                    var items = global::System.Text.Json.JsonSerializer.Deserialize<List<PosTerminal.ProductItem>>(sale.ItemsJson);
                    if (items != null)
                    {
                        foreach (var item in items)
                        {
                            if (item.IsTokenApplied || item.AppliedTokenId != null)
                            {
                                tokensUsed += item.Quantity;
                            }
                            
                            if (!_currentShift.ItemSummary.ContainsKey(item.Name))
                            {
                                _currentShift.ItemSummary[item.Name] = 0;
                                _currentShift.ItemTotals[item.Name] = 0;
                            }
                            _currentShift.ItemSummary[item.Name] += item.Quantity;
                            _currentShift.ItemTotals[item.Name] += item.Price * item.Quantity;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SIMULATOR] Error deserializing items in SaveSaleAsync: {ex.Message}");
            }
            
            _currentShift.TokenCredits += tokensUsed;
            
            OutboxChanged?.Invoke();
            return Task.CompletedTask;
        }

        public Task<PosZReportDto?> GetZReportAsync(Guid id) => Task.FromResult(_zReports.FirstOrDefault(z => z.Id == id));
        public Task<List<PosZReportDto>> GetZReportsAsync(string terminalName) => Task.FromResult(_zReports);
        public Task SaveZReportAsync(PosZReportDto report) { _zReports.Add(report); return Task.CompletedTask; }

        public Task<PosSaleDto?> GetDartsRoundTodayAsync(string terminalName) => Task.FromResult<PosSaleDto?>(null);
        public Task<List<PosSaleDto>> GetDartsRoundsTodayAsync(string terminalName) => Task.FromResult(new List<PosSaleDto>());

        public async Task<List<LiquorOrder>> GetPendingLiquorOrdersAsync(bool force = false)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorOrders
                .Include(o => o.OrderItems)
                .Where(o => o.Status != "RECEIVED")
                .ToListAsync();
        }

        // Simulator Mode: Fake receive order
        public Task ReceiveLiquorOrderAsync(LiquorOrderReceiptDto receipt) => Task.CompletedTask;

        public async Task<List<UserListItemDto>> GetAuthorizedUsersAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var users = await db.AppUsers.Where(u => u.IsActive).ToListAsync();
            return users.Select(u => new UserListItemDto(
                u.UserId,
                u.Username,
                u.IsAdmin,
                u.IsActive,
                u.MemberId,
                null, // MemberName
                u.LastLoginDate,
                u.Notes,
                u.Email,
                false, // IsDirector
                null // CardNumber
            )).ToList();
        }

        public Task AddSaleToShiftAsync(PosSaleDto sale) => SaveSaleAsync(sale);

        public Task VoidSaleAsync(Guid saleId, string reason)
        {
            var sale = _sales.FirstOrDefault(s => s.Id == saleId);
            if (sale != null)
            {
                _sales.Remove(sale);
                _currentShift.VoidedSales.Add(sale);
                _currentShift.GrossTotal -= sale.TotalAmount;
                _currentShift.CashTotal -= sale.PaymentType == "CASH" ? sale.TotalAmount : 0;
                
                int tokensUsed = 0;
                try
                {
                    if (!string.IsNullOrEmpty(sale.ItemsJson))
                    {
                        var items = global::System.Text.Json.JsonSerializer.Deserialize<List<PosTerminal.ProductItem>>(sale.ItemsJson);
                        if (items != null)
                        {
                            foreach (var item in items)
                            {
                                if (item.IsTokenApplied || item.AppliedTokenId != null)
                                {
                                    tokensUsed += item.Quantity;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SIMULATOR] Error deserializing items in VoidSaleAsync: {ex.Message}");
                }
                
                _currentShift.TokenCredits -= tokensUsed;
            }
            return Task.CompletedTask;
        }

        public Task<ShiftAuditDto> GetShiftAuditAsync() => Task.FromResult(_currentShift);
        
        public Task ClearShiftAsync()
        {
            _currentShift = new ShiftAuditDto();
            _sales.Clear();
            return Task.CompletedTask;
        }

        public Task FlushAllPendingAsync() => Task.CompletedTask;

        public Task<BanquetMasterSummaryDto?> GetBanquetMasterSummaryAsync(int eventId) => Task.FromResult<BanquetMasterSummaryDto?>(null);
        public Task<List<PosSaleDto>> GetUnsyncedSalesAsync() => Task.FromResult(new List<PosSaleDto>());

        public async Task<MemberDrawPoolDto?> GetMemberDrawPoolAsync()
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var members = await db.AppUsers.Where(u => u.IsActive).ToListAsync();
            var ids = members.Select(u => u.UserId).ToList();
            var maxId = ids.Any() ? ids.Max() : 0;
            var poolItems = members.Select(m => new MemberDrawPoolItemDto
            {
                MemberId = m.UserId,
                FirstName = m.Username,
                LastName = "",
                Suffix = "",
                IsEligible = true
            }).ToList();

            return new MemberDrawPoolDto
            {
                MemberIds = ids,
                MaxMemberId = maxId,
                Members = poolItems
            };
        }

        public Task<MemberDrawStatusDto?> GetMemberDrawStatusAsync(int memberId)
        {
            return Task.FromResult<MemberDrawStatusDto?>(new MemberDrawStatusDto
            {
                MemberId = memberId,
                FirstName = "John",
                LastName = "Doe",
                Suffix = "Jr",
                Status = "Active",
                IsActive = true,
                DuesPaid = true,
                IsEligible = true
            });
        }

        public async Task<List<LiquorItem>> GetLiquorInventoryAsync(bool force = false)
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            return await db.LiquorItems.Where(i => i.ShowInPos).ToListAsync();
        }
    }
}
