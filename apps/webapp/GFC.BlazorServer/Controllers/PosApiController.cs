using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Core.DTOs;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GFC.BlazorServer.Controllers;

[ApiController]
[Route("api/pos")]
[Microsoft.AspNetCore.Authorization.AllowAnonymous] // Login coming in the future
public class PosApiController : ControllerBase
{
    private readonly IDbContextFactory<GfcDbContext> _dbFactory;
    private readonly ILiquorService _liquorService;
    private readonly ILogger<PosApiController> _logger;
    private readonly IPagePermissionRepository _pagePermissionRepo;

    [HttpGet("debug-pages")]
    public async Task<IActionResult> DebugPages()
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var pages = await db.AppPages.ToListAsync();
        var perms = await db.UserPagePermissions
            .Include(p => p.User)
            .Include(p => p.Page)
            .Take(100)
            .Select(p => new { p.PageId, p.UserId, p.User.Username, p.Page.PageRoute, p.CanAccess })
            .ToListAsync();
            
        return Ok(new { Pages = pages, RecentPermissions = perms });
    }

    public PosApiController(
        IDbContextFactory<GfcDbContext> dbFactory,
        ILiquorService liquorService,
        ILogger<PosApiController> logger,
        IPagePermissionRepository pagePermissionRepo)
    {
        _dbFactory = dbFactory;
        _liquorService = liquorService;
        _logger = logger;
        _pagePermissionRepo = pagePermissionRepo;
    }

    [HttpGet("menu")]
    public async Task<IActionResult> GetMenu([FromQuery] string? terminalName = null)
    {
        try 
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            int? profileId = null;
            string profileName = "Default Retail (No Profile)";
            if (!string.IsNullOrWhiteSpace(terminalName))
            {
                var searchName = terminalName.Trim();
                var terminal = await db.PosTerminals
                    .Include(t => t.MenuProfile)
                    .FirstOrDefaultAsync(t => t.TerminalName.Trim() == searchName);
                
                // Fallback to case-insensitive match if not found exactly
                if (terminal == null)
                {
                    terminal = await db.PosTerminals
                        .Include(t => t.MenuProfile)
                        .FirstOrDefaultAsync(t => t.TerminalName.ToLower().Trim() == searchName.ToLower());
                }

                if (terminal == null)
                {
                    terminal = new PosTerminal
                    {
                        TerminalName = searchName,
                        MenuProfileId = null,
                        LastSeenAt = DateTime.UtcNow
                    };
                    db.PosTerminals.Add(terminal);
                    await db.SaveChangesAsync();
                }
                else
                {
                    terminal.LastSeenAt = DateTime.UtcNow;
                    await db.SaveChangesAsync();
                }
                profileId = terminal.MenuProfileId;
                if (terminal.MenuProfile != null)
                {
                    profileName = terminal.MenuProfile.Name;
                }
            }

            var categoriesQuery = db.PosCategories
                .Where(c => c.IsActive && !c.IsModifierCategory);

            if (profileId.HasValue)
            {
                categoriesQuery = categoriesQuery.Where(c => c.MenuProfileId == null || c.MenuProfileId == profileId.Value);
            }
            else
            {
                categoriesQuery = categoriesQuery.Where(c => c.MenuProfileId == null);
            }

            var categories = await categoriesQuery
                .OrderBy(c => c.DisplayOrder)
                .Select(c => c.Name)
                .ToListAsync();

            var modifierCategoriesList = await db.PosCategories
                .Where(c => c.IsActive && c.IsModifierCategory)
                .ToListAsync();

            var modifierCategories = modifierCategoriesList.Select(c => c.Name).ToList();

            var flatModifiers = new List<PosModifierDto>();
            foreach (var cat in modifierCategoriesList)
            {
                if (!string.IsNullOrWhiteSpace(cat.ModifiersJson))
                {
                    try
                    {
                        var mods = System.Text.Json.JsonSerializer.Deserialize<List<PosModifierDto>>(cat.ModifiersJson);
                        if (mods != null)
                        {
                            flatModifiers.AddRange(mods);
                        }
                    }
                    catch (Exception jsonEx)
                    {
                        _logger.LogWarning("Failed to deserialize ModifiersJson for category {Id}: {Msg}", cat.Id, jsonEx.Message);
                    }
                }
            }

            var liquorItems = await db.LiquorItems.ToListAsync();
            var overrides = profileId.HasValue 
                ? await db.PosMenuOverrides.Where(o => o.MenuProfileId == profileId.Value).ToListAsync()
                : new List<PosMenuOverride>();

            var overrideMap = overrides.ToDictionary(o => o.LiquorItemId);
            var items = new List<PosItemDto>();

            foreach (var i in liquorItems)
            {
                bool showInPos = i.ShowInPos;
                decimal price = i.RetailPrice;
                string category = i.Category ?? "MISC";
                int displayOrder = i.DisplayOrder;

                if (overrideMap.TryGetValue(i.Id, out var o))
                {
                    if (o.IsVisible.HasValue)
                    {
                        showInPos = o.IsVisible.Value;
                    }
                    if (o.OverridePrice.HasValue)
                    {
                        price = o.OverridePrice.Value;
                    }
                    if (!string.IsNullOrWhiteSpace(o.OverrideCategory))
                    {
                        category = o.OverrideCategory;
                    }
                    if (o.DisplayOrder.HasValue)
                    {
                        displayOrder = o.DisplayOrder.Value;
                    }
                }

                if (showInPos)
                {
                    items.Add(new PosItemDto 
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Price = price,
                        Category = category.Trim().ToUpper(),
                        DisplayOrder = displayOrder
                    });
                }
            }

            items = items.OrderBy(x => x.DisplayOrder).ToList();

            if (!categories.Contains("TOKENS")) categories.Add("TOKENS");

            var tokens = await db.PosTokens.Where(t => t.IsActive).OrderBy(t => t.Name).ToListAsync();
            var activeEvents = await db.ActiveEvents.Where(e => e.Status == GFC.Core.Enums.EventTabStatus.Open && !e.IsDeleted).ToListAsync();
            var templates = await db.EventTemplates.Where(t => !t.IsDeleted).ToListAsync();

            var settings = await db.SystemSettings.FirstOrDefaultAsync();
            var payoutCategories = new List<string>();
            if (settings != null && !string.IsNullOrEmpty(settings.PayoutCategories))
            {
                payoutCategories = settings.PayoutCategories
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.Trim().ToUpper())
                    .ToList();
            }
            else
            {
                payoutCategories = new List<string> { "FOOD", "SUPPLIES", "MAINTENANCE", "REBATE/REFUND", "OTHER" };
            }

            return Ok(new PosMenuDto
            {
                Categories = categories,
                Items = items,
                Tokens = tokens,
                ActiveEvents = activeEvents,
                EventTemplates = templates,
                ModifierCategories = modifierCategories,
                Modifiers = flatModifiers,
                PayoutCategories = payoutCategories,
                ProfileName = profileName
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching POS menu");
            return StatusCode(500, $"Internal Server Error: {ex.Message} {(ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "")}");
        }
    }

    [HttpPost("events/start")]
    public async Task<IActionResult> StartEvent([FromBody] ActiveEvent newEvent)
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            newEvent.Id = 0; // Force new identity
            newEvent.CreatedAt = DateTime.UtcNow;
            db.ActiveEvents.Add(newEvent);
            await db.SaveChangesAsync();
            return Ok(newEvent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("events/add-funds")]
    public async Task<IActionResult> AddFunds([FromBody] AddFundsRequest? request)
    {
        if (request == null) return BadRequest("Missing request body");

        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var ev = await db.ActiveEvents.FindAsync(request.Id);
            if (ev == null) return NotFound($"Event with ID {request.Id} not found");

            ev.CurrentBalance += request.Amount;
            await db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add funds to event {Id}", request?.Id);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("events/close/{id}")]
    public async Task<IActionResult> CloseEvent(int id)
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var ev = await db.ActiveEvents.FindAsync(id);
            if (ev != null)
            {
                ev.Status = GFC.Core.Enums.EventTabStatus.Closed;
                await db.SaveChangesAsync();
            }
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("sale")]
    public async Task<IActionResult> SaveSale([FromBody] PosSaleDto saleDto)
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            // [IDEMPOTENCY] Check if this sale already arrived
            if (await db.PosSales.AnyAsync(s => s.Id == saleDto.Id))
            {
                _logger.LogInformation("POS Sale {Id} already exists, skipping duplicate save.", saleDto.Id);
                return Ok();
            }

            var sale = new PosSale
            {
                Id = saleDto.Id,
                Timestamp = saleDto.Timestamp,
                TerminalName = saleDto.TerminalName,
                BartenderName = saleDto.BartenderName,
                TotalAmount = saleDto.TotalAmount,
                PaymentType = saleDto.PaymentType,
                ItemsJson = saleDto.ItemsJson,
                IsSynced = true,
                AmountReceived = saleDto.AmountReceived,
                ChangeDue = saleDto.ChangeDue,
                OriginalTotal = saleDto.OriginalTotal,
                IsVoided = saleDto.IsVoided,
                IsCorrection = saleDto.IsCorrection,
                OriginalSaleId = saleDto.OriginalSaleId,
                AdjustmentReason = saleDto.AdjustmentReason,
                ActiveEventId = (saleDto.ActiveEventId.HasValue && saleDto.ActiveEventId.Value > 0 && 
                                 await db.ActiveEvents.AnyAsync(e => e.Id == saleDto.ActiveEventId.Value)) 
                                 ? saleDto.ActiveEventId.Value 
                                 : null
            };

            db.PosSales.Add(sale);

            // Handle inventory if not in training mode
            if (!saleDto.TerminalName.Contains("(TRAINING)"))
            {
                var items = JsonSerializer.Deserialize<List<PosSaleItemDto>>(saleDto.ItemsJson);
                if (items != null)
                {
                    foreach (var parent in items)
                    {
                        if (parent.Id > 0)
                        {
                            try {
                                // Note: Using a default system user ID (1) for POS adjustments
                                await _liquorService.AdjustStockAsync(parent.Id, 1, -parent.Quantity, $"POS Sale: {parent.Name}");
                            } catch (Exception invEx) {
                                _logger.LogWarning("Could not adjust stock for item {Id} ({Name}): {Msg}", parent.Id, parent.Name, invEx.Message);
                            }
                        }

                        if (parent.Modifiers != null)
                        {
                            foreach (var mod in parent.Modifiers.Where(m => m.Id > 0))
                            {
                                try {
                                    // Total deduction is parent quantity * modifier quantity
                                    int totalModQty = parent.Quantity * mod.Quantity;
                                    await _liquorService.AdjustStockAsync(mod.Id, 1, -totalModQty, $"POS Sale (Add-on): {mod.Name} (for {parent.Name})");
                                } catch (Exception invEx) {
                                    _logger.LogWarning("Could not adjust stock for modifier item {Id} ({Name}): {Msg}", mod.Id, mod.Name, invEx.Message);
                                }
                            }
                        }
                    }
                }
            }

            // Handle Event Tab Deduction
            if (saleDto.PaymentType == "TAB" && saleDto.ActiveEventId.HasValue)
            {
                var activeEvent = await db.ActiveEvents.FindAsync(saleDto.ActiveEventId.Value);
                if (activeEvent != null)
                {
                    if (saleDto.IsVoided)
                    {
                        activeEvent.CurrentBalance += saleDto.TotalAmount;
                    }
                    else
                    {
                        activeEvent.CurrentBalance -= saleDto.TotalAmount;
                    }
                }
            }

            await db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save POS sale");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("z-report")]
    public async Task<IActionResult> SaveZReport([FromBody] PosZReportDto reportDto)
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            
            // [IDEMPOTENCY] Check if this report already arrived
            if (await db.PosZReports.AnyAsync(r => r.Id == reportDto.Id))
            {
                _logger.LogInformation("Z-Report {Id} already exists, skipping duplicate save.", reportDto.Id);
                return Ok();
            }

            var report = new PosZReport
            {
                Id = reportDto.Id,
                Timestamp = reportDto.Timestamp,
                TerminalName = reportDto.TerminalName,
                BartenderName = reportDto.BartenderName,
                CashTotal = reportDto.CashTotal,
                TotalGrossSales = reportDto.TotalGrossSales,
                InventoryPullsJson = reportDto.InventoryPullsJson,
                SalesSummaryJson = reportDto.SalesSummaryJson,
                BanquetSummaryJson = reportDto.BanquetSummaryJson,
                TokenCredits = reportDto.TokenCredits,
                IsSynced = true
            };

            db.PosZReports.Add(report);

            // Handle Inventory Pulls
            if (!string.IsNullOrEmpty(reportDto.InventoryPullsJson))
            {
                var pulls = JsonSerializer.Deserialize<Dictionary<int, int>>(reportDto.InventoryPullsJson);
                if (pulls != null)
                {
                    // Resolve user ID dynamically using BartenderName, falling back to first available user
                    int userId = 1;
                    if (!string.IsNullOrEmpty(reportDto.BartenderName))
                    {
                        var userObj = await db.AppUsers.FirstOrDefaultAsync(u => u.Username == reportDto.BartenderName);
                        if (userObj != null)
                        {
                            userId = userObj.UserId;
                        }
                        else
                        {
                            var fallbackUser = await db.AppUsers.OrderBy(u => u.UserId).FirstOrDefaultAsync();
                            if (fallbackUser != null)
                            {
                                userId = fallbackUser.UserId;
                            }
                        }
                    }
                    else
                    {
                        var fallbackUser = await db.AppUsers.OrderBy(u => u.UserId).FirstOrDefaultAsync();
                        if (fallbackUser != null)
                        {
                            userId = fallbackUser.UserId;
                        }
                    }

                    foreach (var pull in pulls)
                    {
                        for (int i = 0; i < pull.Value; i++)
                        {
                            await _liquorService.CheckoutBottleAsync(pull.Key, userId, $"[{reportDto.TerminalName}] POS End-of-Shift Removal");
                        }
                    }
                }
            }

            await db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save Z-Report");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("darts-check/{terminalName}")]
    public async Task<ActionResult<PosSaleDto?>> GetDartsRoundToday(string terminalName)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var today = DateTime.Today;
        // A Darts Round sale is marked as TOKEN/COMP and contains (DARTS) items
        var sale = await db.PosSales
            .OrderByDescending(s => s.Timestamp)
            .FirstOrDefaultAsync(s => s.TerminalName == terminalName && 
                                     s.Timestamp >= today && 
                                     s.PaymentType == "TOKEN/COMP" && 
                                     s.ItemsJson.Contains("(DARTS)"));
        
        if (sale == null) return Ok(null);

        return Ok(new PosSaleDto
        {
            Id = sale.Id,
            Timestamp = sale.Timestamp,
            TerminalName = sale.TerminalName,
            BartenderName = sale.BartenderName,
            TotalAmount = sale.TotalAmount,
            PaymentType = sale.PaymentType,
            ItemsJson = sale.ItemsJson,
            AmountReceived = sale.AmountReceived,
            ChangeDue = sale.ChangeDue,
            OriginalTotal = sale.OriginalTotal
        });
    }

    [HttpGet("z-reports/{terminalName}")]
    public async Task<ActionResult<List<PosZReportDto>>> GetZReports(string terminalName)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var reports = await db.PosZReports
            .Where(r => r.TerminalName == terminalName)
            .OrderByDescending(r => r.Timestamp)
            .Take(100)
            .Select(r => new PosZReportDto
            {
                Id = r.Id,
                Timestamp = r.Timestamp,
                TerminalName = r.TerminalName,
                BartenderName = r.BartenderName,
                CashTotal = r.CashTotal,
                TotalGrossSales = r.TotalGrossSales,
                InventoryPullsJson = r.InventoryPullsJson,
                SalesSummaryJson = r.SalesSummaryJson
            })
            .ToListAsync();
        return Ok(reports);
    }

    [HttpGet("z-report/{id}")]
    public async Task<ActionResult<PosZReportDto?>> GetZReport(Guid id)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var r = await db.PosZReports.FindAsync(id);
        if (r == null) return NotFound();
        return Ok(new PosZReportDto
        {
            Id = r.Id,
            Timestamp = r.Timestamp,
            TerminalName = r.TerminalName,
            BartenderName = r.BartenderName,
            CashTotal = r.CashTotal,
            TotalGrossSales = r.TotalGrossSales,
            InventoryPullsJson = r.InventoryPullsJson,
            SalesSummaryJson = r.SalesSummaryJson
        });
    }

    [HttpGet("last-z/{terminalName}")]
    public async Task<ActionResult<DateTime>> GetLastZTime(string terminalName)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var lastZ = await db.PosZReports
            .Where(z => z.TerminalName == terminalName)
            .OrderByDescending(z => z.Timestamp)
            .Select(z => z.Timestamp)
            .FirstOrDefaultAsync();

        return Ok(lastZ == default ? DateTime.Today : lastZ);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAuthorizedUsers()
    {
        try
        {
            // The POS page can be identified by either the legacy "pos" route or the canonical "/admin/pos-terminal"
            var posPage = _pagePermissionRepo.GetPageByRoute("/admin/pos-terminal") ?? _pagePermissionRepo.GetPageByRoute("pos");

            if (posPage == null)
            {
                _logger.LogWarning("[POS API] No page found with route '/admin/pos-terminal' or 'pos'. Ensure the page is registered in AppPages.");
                return Ok(new List<UserListItemDto>());
            }

            _logger.LogInformation($"[POS API] Fetching authorized users for page: {posPage.PageName} (ID: {posPage.PageId}, Route: {posPage.PageRoute})");

            // 2. Get active users with permission using the Repository
            var permissions = _pagePermissionRepo.GetPagePermissions(posPage.PageId);
            
            using var db = await _dbFactory.CreateDbContextAsync();
            var userIds = permissions.Select(p => p.UserId).ToList();
            
            // [HARDENED SYNC] Fetch users and cards separately to avoid EF join complexity with nullable MemberIds
            var allActiveUsers = await db.AppUsers.Where(u => u.IsActive).ToListAsync();
            var allActiveCards = await db.KeyCards.Where(k => k.IsActive).ToListAsync();

            var users = allActiveUsers
                .Where(u => userIds.Contains(u.UserId) || u.IsAdmin || (u.MemberId.HasValue && allActiveCards.Any(kc => kc.MemberId == u.MemberId.Value)))
                .Select(u => {
                    var kc = u.MemberId.HasValue ? allActiveCards.FirstOrDefault(k => k.MemberId == u.MemberId.Value) : null;
                    return new UserListItemDto(
                        u.UserId,
                        u.Username,
                        u.IsAdmin,
                        u.IsActive,
                        u.MemberId,
                        null, // MemberName
                        u.LastLoginDate,
                        u.Notes,
                        u.Email ?? "",
                        false, // IsDirector
                        kc?.CardNumber
                    );
                })
                .OrderBy(u => u.Username)
                .ToList();

            _logger.LogInformation($"[POS API] Found {users.Count} authorized users: {string.Join(", ", users.Select(u => u.Username))}");
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[POS API] Error in GetAuthorizedUsers");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("events/summary/{eventId}")]
    public async Task<IActionResult> GetBanquetMasterSummary(int eventId)
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var activeEvent = await db.ActiveEvents.FindAsync(eventId);
            if (activeEvent == null) return NotFound("Event not found");

            var summary = new BanquetMasterSummaryDto
            {
                EventName = activeEvent.Name,
                TotalSpent = 0,
                TotalDeposited = activeEvent.InitialAmount
            };

            if (activeEvent.InitialAmount > 0)
            {
                summary.Deposits.Add(new BanquetDepositDetailDto
                {
                    Timestamp = activeEvent.CreatedAt,
                    Amount = activeEvent.InitialAmount,
                    IsInitial = true
                });
            }

            var sales = await db.PosSales
                .Where(s => s.ActiveEventId == eventId && !s.IsVoided)
                .ToListAsync();

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            foreach (var sale in sales)
            {
                if (string.IsNullOrEmpty(sale.ItemsJson)) continue;
                try
                {
                    var items = JsonSerializer.Deserialize<List<PosSaleItemDto>>(sale.ItemsJson, jsonOptions);
                    if (items != null)
                    {
                        foreach (var i in items)
                        {
                            if (i.Name.StartsWith("TAB DEPOSIT:"))
                            {
                                summary.TotalDeposited += i.Price;
                                summary.Deposits.Add(new BanquetDepositDetailDto
                                {
                                    Timestamp = sale.Timestamp,
                                    Amount = i.Price,
                                    IsInitial = false
                                });
                            }
                            else if (sale.PaymentType == "TAB")
                            {
                                if (!summary.ItemQuantities.ContainsKey(i.Name))
                                {
                                    summary.ItemQuantities[i.Name] = 0;
                                    summary.ItemTotals[i.Name] = 0;
                                }
                                summary.ItemQuantities[i.Name] += i.Quantity;
                                summary.ItemTotals[i.Name] += (i.Price * i.Quantity);
                                summary.TotalSpent += (i.Price * i.Quantity);
                            }

                            if (i.Modifiers != null && sale.PaymentType == "TAB")
                            {
                                foreach (var m in i.Modifiers)
                                {
                                    if (!summary.ItemQuantities.ContainsKey(m.Name))
                                    {
                                        summary.ItemQuantities[m.Name] = 0;
                                        summary.ItemTotals[m.Name] = 0;
                                    }
                                    summary.ItemQuantities[m.Name] += m.Quantity;
                                    summary.ItemTotals[m.Name] += (m.Price * m.Quantity);
                                    summary.TotalSpent += (m.Price * m.Quantity);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to deserialize ItemsJson in banquet summary for sale {Id}: {Msg}", sale.Id, ex.Message);
                }
            }

            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[POS API] Error compiling banquet master summary for event {EventId}", eventId);
            return StatusCode(500, "Internal Server Error");
        }
    }
}
