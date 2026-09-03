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

    [HttpGet("debug-menu")]
    public async Task<IActionResult> DebugMenu([FromQuery] string? terminalName = null)
    {
        var steps = new List<string>();
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            steps.Add("DB context created");

            steps.Add("Loading PosTerminal...");
            if (!string.IsNullOrWhiteSpace(terminalName))
            {
                var t = await db.PosTerminals.Include(x => x.MenuProfile).FirstOrDefaultAsync(x => x.TerminalName == terminalName);
                steps.Add($"Terminal: {(t == null ? "not found" : t.TerminalName)}");
            }

            steps.Add("Loading PosCategories (active, non-modifier)...");
            var cats = await db.PosCategories.Where(c => c.IsActive && !c.IsModifierCategory).ToListAsync();
            steps.Add($"PosCategories OK: {cats.Count}");

            steps.Add("Loading modifier categories...");
            var modCats = await db.PosCategories.Where(c => c.IsActive && c.IsModifierCategory).ToListAsync();
            steps.Add($"Modifier categories OK: {modCats.Count}");

            steps.Add("Loading LiquorItems...");
            var liquorItems = await db.LiquorItems.AsNoTracking().ToListAsync();
            steps.Add($"LiquorItems OK: {liquorItems.Count}");

            steps.Add("Loading PosMenuOverrides...");
            var overrides = await db.PosMenuOverrides.AsNoTracking().ToListAsync();
            steps.Add($"PosMenuOverrides OK: {overrides.Count}");

            steps.Add("Building override map...");
            var overrideMap = overrides.GroupBy(o => o.LiquorItemId).ToDictionary(g => g.Key, g => g.First());
            steps.Add($"Override map OK: {overrideMap.Count} entries");

            steps.Add("Loading PosTokens...");
            var tokens = await db.PosTokens.AsNoTracking().Where(t => t.IsActive).ToListAsync();
            steps.Add($"PosTokens OK: {tokens.Count}");

            steps.Add("Loading ActiveEvents (ALL statuses)...");
            var allEvents = await db.ActiveEvents.AsNoTracking().ToListAsync();
            steps.Add($"Total ActiveEvents in DB: {allEvents.Count}");
            foreach (var ev in allEvents)
                steps.Add($"  Event '{ev.Name}' | Status={ev.Status} | IsDeleted={ev.IsDeleted} | Id={ev.Id}");

            steps.Add("Loading ActiveEvents (Open + not deleted)...");
            var activeEvents = await db.ActiveEvents.AsNoTracking()
                .Where(e => e.Status == GFC.Core.Enums.EventTabStatus.Open && !e.IsDeleted)
                .ToListAsync();
            steps.Add($"ActiveEvents OK: {activeEvents.Count}");

            steps.Add("Loading EventTemplates...");
            var templates = await db.EventTemplates.AsNoTracking().Where(t => !t.IsDeleted).ToListAsync();
            steps.Add($"EventTemplates OK: {templates.Count}");

            steps.Add("Loading SystemSettings...");
            var settings = await db.SystemSettings.AsNoTracking().FirstOrDefaultAsync();
            steps.Add($"SystemSettings OK: {(settings != null ? "found" : "null")}");

            steps.Add("Building PosMenuDto...");
            var dto = new GFC.Core.DTOs.PosMenuDto
            {
                Categories = cats.Select(c => c.Name).ToList(),
                Items = new(),
                Tokens = tokens,
                ActiveEvents = activeEvents,
                EventTemplates = templates,
                PayoutCategories = new List<string> { "FOOD" },
                ProfileName = "Debug"
            };
            steps.Add("PosMenuDto built OK");

            steps.Add("Serializing to JSON...");
            var json = System.Text.Json.JsonSerializer.Serialize(dto, new System.Text.Json.JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
            steps.Add($"Serialization OK: {json.Length} chars");

            return Ok(new { Success = true, Steps = steps });
        }
        catch (Exception ex)
        {
            steps.Add($"FAILED: {ex.Message}");
            if (ex.InnerException != null) steps.Add($"Inner: {ex.InnerException.Message}");
            return StatusCode(500, new { Success = false, Steps = steps, Error = ex.Message, InnerError = ex.InnerException?.Message });
        }
    }

    [HttpGet("debug-create-event")]
    public async Task<IActionResult> DebugCreateEvent()
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        // Create a test event with Open status
        var testEvent = new GFC.Core.Models.ActiveEvent
        {
            Name = "DEBUG_TEST_EVENT",
            Type = GFC.Core.Enums.EventTabType.RunningTab,
            Status = GFC.Core.Enums.EventTabStatus.Open,
            InitialAmount = 0,
            CurrentBalance = 0,
            CreatedAt = DateTime.UtcNow
        };
        db.ActiveEvents.Add(testEvent);
        await db.SaveChangesAsync();

        // Read it back from a fresh context
        using var db2 = await _dbFactory.CreateDbContextAsync();
        var saved = await db2.ActiveEvents.AsNoTracking().FirstOrDefaultAsync(e => e.Id == testEvent.Id);

        // Clean up
        using var db3 = await _dbFactory.CreateDbContextAsync();
        var toDelete = await db3.ActiveEvents.FindAsync(testEvent.Id);
        if (toDelete != null) { db3.ActiveEvents.Remove(toDelete); await db3.SaveChangesAsync(); }

        return Ok(new
        {
            CreatedWithStatus = testEvent.Status.ToString(),
            CreatedWithStatusInt = (int)testEvent.Status,
            SavedInDbStatus = saved?.Status.ToString() ?? "NOT FOUND",
            SavedInDbStatusInt = saved != null ? (int)saved.Status : -1,
            Match = saved?.Status == GFC.Core.Enums.EventTabStatus.Open
        });
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
                try
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
                        try
                        {
                            await db.SaveChangesAsync();
                        }
                        catch (Exception innerEx)
                        {
                            // Clean up tracking of the failed entity
                            db.Entry(terminal).State = EntityState.Detached;
                            // Retrieve the one that was inserted by the concurrent request
                            terminal = await db.PosTerminals
                                .Include(t => t.MenuProfile)
                                .FirstOrDefaultAsync(t => t.TerminalName.ToLower().Trim() == searchName.ToLower());
                            
                            _logger.LogWarning(innerEx, "Handled terminal creation conflict for {TerminalName}", searchName);
                        }
                    }
                    else
                    {
                        if (terminal.IsDeleted)
                        {
                            terminal.IsDeleted = false;
                        }
                        terminal.LastSeenAt = DateTime.UtcNow;
                        try
                        {
                            await db.SaveChangesAsync();
                        }
                        catch (Exception innerEx)
                        {
                            // Ignore concurrency/db conflicts on LastSeenAt updates
                            _logger.LogWarning(innerEx, "Handled terminal update conflict for {TerminalName}", searchName);
                        }
                    }
                    
                    if (terminal != null)
                    {
                        profileId = terminal.MenuProfileId;
                        if (terminal.MenuProfile != null)
                        {
                            profileName = terminal.MenuProfile.Name;
                        }
                    }
                }
                catch (Exception termEx)
                {
                    _logger.LogWarning(termEx, "Error during terminal registration for '{TerminalName}'. Proceeding with default menu profile.", terminalName);
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

            var overrideMap = overrides.GroupBy(o => o.LiquorItemId).ToDictionary(g => g.Key, g => g.First());
            var resolvedVisibility = new Dictionary<int, bool>();
            foreach (var i in liquorItems)
            {
                bool showInPos = i.ShowInPos && i.IsActive;
                if (overrideMap.TryGetValue(i.Id, out var o) && o.IsVisible.HasValue)
                {
                    showInPos = o.IsVisible.Value;
                }
                resolvedVisibility[i.Id] = showInPos;
            }

            var parentIdsWithChildren = liquorItems
                .Where(x => x.ParentItemId.HasValue && resolvedVisibility.TryGetValue(x.Id, out var cv) && cv && resolvedVisibility.TryGetValue(x.ParentItemId.Value, out var pv) && pv)
                .Select(x => x.ParentItemId.Value)
                .ToHashSet();

            var items = new List<PosItemDto>();

            var posCats = await db.PosCategories.Where(c => c.IsActive && !string.IsNullOrWhiteSpace(c.Name)).ToListAsync();
            var catZGroupMap = posCats
                .GroupBy(c => (c.Name ?? "MISC").Trim().ToUpper(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().ZReportGroup, StringComparer.OrdinalIgnoreCase);

            foreach (var i in liquorItems)
            {
                bool showInPos = resolvedVisibility[i.Id];
                if (i.ParentItemId.HasValue)
                {
                    // Cascade visibility: hide child if parent is hidden
                    bool parentVisible = resolvedVisibility.TryGetValue(i.ParentItemId.Value, out var pv) && pv;
                    if (!parentVisible)
                    {
                        showInPos = false;
                    }
                }

                decimal price = i.RetailPrice;
                string category = i.Category ?? "MISC";
                int displayOrder = i.DisplayOrder;

                if (overrideMap.TryGetValue(i.Id, out var o))
                {
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
                    string displayName = i.Name ?? "";
                    string cleanCat = (category ?? "MISC").Trim().ToUpper();
                    if (cleanCat == "WINE" && parentIdsWithChildren.Contains(i.Id))
                    {
                        displayName = displayName + " (Bottle)";
                    }

                    int effectiveZGroup = i.ZReportGroup;
                    if (effectiveZGroup == 0 && catZGroupMap.TryGetValue(cleanCat, out var catZ))
                    {
                        effectiveZGroup = catZ;
                    }

                    items.Add(new PosItemDto 
                    {
                        Id = i.Id,
                        Name = displayName,
                        Price = price,
                        Category = cleanCat,
                        DisplayOrder = displayOrder,
                        ZReportGroup = effectiveZGroup
                    });
                }
            }

            var resolvedDisplayOrders = new Dictionary<int, int>();
            foreach (var i in liquorItems)
            {
                int order = i.DisplayOrder;
                if (overrideMap.TryGetValue(i.Id, out var o) && o.DisplayOrder.HasValue)
                {
                    order = o.DisplayOrder.Value;
                }
                resolvedDisplayOrders[i.Id] = order;
            }

            var parentIdLookup = liquorItems
                .Where(x => x.ParentItemId.HasValue)
                .GroupBy(x => x.Id)
                .ToDictionary(g => g.Key, g => g.First().ParentItemId!.Value);

            var liquorItemMap = liquorItems.GroupBy(x => x.Id).ToDictionary(g => g.Key, g => g.First());


            items = items
                .OrderBy(x => {
                    if (parentIdLookup.TryGetValue(x.Id, out var parentId))
                    {
                        return resolvedDisplayOrders.TryGetValue(parentId, out var parentOrder) ? parentOrder : x.DisplayOrder;
                    }
                    return x.DisplayOrder;
                })
                .ThenBy(x => {
                    if (parentIdLookup.TryGetValue(x.Id, out var parentId) && liquorItemMap.TryGetValue(parentId, out var parent))
                    {
                        return parent.Name ?? "";
                    }
                    return x.Name ?? "";
                })
                .ThenBy(x => parentIdLookup.ContainsKey(x.Id) ? 1 : 0)
                .ThenBy(x => x.Name ?? "")
                .ThenBy(x => x.Id)
                .ToList();


            if (!categories.Contains("TOKENS")) categories.Add("TOKENS");

            var tokens = await db.PosTokens.Where(t => t.IsActive).OrderBy(t => t.Name).ToListAsync();
            
            var activeEvents = new List<ActiveEvent>();
            try {
                activeEvents = await db.ActiveEvents
                    .AsNoTracking()
                    .Where(e => e.Status == GFC.Core.Enums.EventTabStatus.Open && !e.IsDeleted)
                    .ToListAsync();
                // Null out navigation properties to prevent JSON circular reference during serialization
                foreach (var ae in activeEvents) ae.Template = null;
            } catch (Exception ex) {
                _logger.LogError(ex, "Error querying ActiveEvents table in GetMenu");
            }

            var templates = new List<EventTemplate>();
            try {
                templates = await db.EventTemplates
                    .AsNoTracking()
                    .Where(t => !t.IsDeleted)
                    .ToListAsync();
            } catch (Exception ex) {
                _logger.LogError(ex, "Error querying EventTemplates table in GetMenu");
            }

            var settings = await db.SystemSettings.AsNoTracking().FirstOrDefaultAsync();

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
            var existingOpen = await db.ActiveEvents.FirstOrDefaultAsync(e => e.Status == GFC.Core.Enums.EventTabStatus.Open && !e.IsDeleted);
            if (existingOpen != null)
            {
                return BadRequest($"An event is already active ('{existingOpen.Name}'). Please close the active event before starting a new one.");
            }

            newEvent.Id = 0; // Force new identity
            newEvent.Template = null; // Clear navigation property to prevent EF Core identity insert errors
            newEvent.CreatedAt = DateTime.UtcNow;

            // Carry over remaining donated beer pool from the latest closed session of this event
            if (string.IsNullOrEmpty(newEvent.DonatedItemIdsJson))
            {
                var cleanName = (newEvent.Name ?? "").Replace(" [Direct]", "").Replace("[Direct]", "").Trim();
                var prevSession = await db.ActiveEvents
                    .Where(e => ((newEvent.TemplateId.HasValue && e.TemplateId.HasValue && e.TemplateId.Value > 0 && e.TemplateId == newEvent.TemplateId.Value) || e.Name == cleanName || (e.Name != null && e.Name.StartsWith(cleanName))) && e.Status == GFC.Core.Enums.EventTabStatus.Closed && !e.IsDeleted && !string.IsNullOrEmpty(e.DonatedItemIdsJson))
                    .OrderByDescending(e => e.Id)
                    .FirstOrDefaultAsync();

                if (prevSession != null && !string.IsNullOrEmpty(prevSession.DonatedItemIdsJson))
                {
                    newEvent.DonatedItemIdsJson = prevSession.DonatedItemIdsJson;
                    newEvent.Enable100PercentDonatedProceeds = true;
                }
            }

            if (newEvent.TemplateId.HasValue && newEvent.TemplateId.Value > 0)
            {
                var tmpl = await db.EventTemplates.FindAsync(newEvent.TemplateId.Value);
                if (tmpl != null)
                {
                    newEvent.PromptPrintSummaryOnClose = tmpl.PromptPrintSummaryOnClose;
                }
            }

            db.ActiveEvents.Add(newEvent);
            await db.SaveChangesAsync();
            return Ok(newEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting active event");
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

    [HttpPost("events/update-tally")]
    public async Task<IActionResult> UpdateEventTally([FromBody] UpdateEventTallyRequest? request)
    {
        if (request == null) return BadRequest("Missing request body");

        string diagMessage = string.Empty;
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var ev = await db.ActiveEvents.FindAsync(request.Id);
            if (ev == null)
            {
                if (request.CloseEvent)
                {
                    // Event was canceled or closed and already removed — return Ok so outbox clears!
                    return Ok();
                }
                return NotFound($"Event with ID {request.Id} not found");
            }

            ev.BeerTalliesJson = request.BeerTalliesJson;
            ev.InitialAmount = request.InitialAmount;
            ev.CurrentBalance = request.CurrentBalance;
            ev.DonatedBeerClaimedCount = request.DonatedBeerClaimedCount;
            ev.DonatedBeerReDonatedCount = request.DonatedBeerReDonatedCount;
            ev.DonatedBeerSoldCount = request.DonatedBeerSoldCount;
            ev.DonatedItemTalliesJson = request.DonatedItemTalliesJson;

            if (request.DonatedItemIdsJson != null)
            {
                ev.DonatedItemIdsJson = string.IsNullOrEmpty(request.DonatedItemIdsJson) ? null : request.DonatedItemIdsJson;
                ev.Enable100PercentDonatedProceeds = !string.IsNullOrEmpty(request.DonatedItemIdsJson);
            }

            if (request.IsRecurring)
            {
                ev.IsRecurring = true;
            }

            if (request.CloseEvent)
            {
                ev.Status = GFC.Core.Enums.EventTabStatus.Closed;
                ev.IsDeleted = false;
                if (!string.IsNullOrEmpty(request.BeerTalliesJson))
                {
                    ev.BeerTalliesJson = request.BeerTalliesJson;
                }
            }

            await db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update tally for event {Id}", request?.Id);
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
            if (ev == null)
            {
                return Ok(new { message = $"Event {id} is already closed/deleted." });
            }
            ev.Status = GFC.Core.Enums.EventTabStatus.Closed;
            ev.IsDeleted = false; // Preserved for audit review
            await db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to close event {Id}", id);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("events/cancel/{id}")]
    public async Task<IActionResult> CancelEvent(int id)
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var ev = await db.ActiveEvents.FindAsync(id);
            if (ev == null) return Ok();

            ev.Status = GFC.Core.Enums.EventTabStatus.Closed;
            ev.IsDeleted = true;
            ev.DonatedItemTalliesJson = null;
            ev.DonatedItemIdsJson = null;
            ev.BeerTalliesJson = null;
            ev.DonatedBeerClaimedCount = 0;
            ev.DonatedBeerReDonatedCount = 0;
            ev.DonatedBeerSoldCount = 0;
            ev.CurrentBalance = 0;

            await db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cancel event {Id}", id);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("events/close")]
    public async Task<IActionResult> CloseEventFromBody([FromBody] ActiveEvent? ev)
    {
        if (ev == null || ev.Id == 0) return Ok();
        return await CloseEvent(ev.Id);
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
                // Resolve user ID dynamically using BartenderName, falling back to 1
                int userId = 1;
                if (!string.IsNullOrEmpty(saleDto.BartenderName))
                {
                    var userObj = await db.AppUsers.FirstOrDefaultAsync(u => u.Username == saleDto.BartenderName);
                    if (userObj != null)
                    {
                        userId = userObj.UserId;
                    }
                    else
                    {
                        // Case-insensitive fallback
                        var userObjCI = await db.AppUsers.FirstOrDefaultAsync(u => u.Username.ToLower() == saleDto.BartenderName.ToLower());
                        if (userObjCI != null)
                        {
                            userId = userObjCI.UserId;
                        }
                    }
                }

                var items = JsonSerializer.Deserialize<List<PosSaleItemDto>>(saleDto.ItemsJson);
                if (items != null)
                {
                    foreach (var parent in items)
                    {
                        if (parent.Id > 0)
                        {
                            try {
                                var liquorItem = await db.LiquorItems.FindAsync(parent.Id);
                                if (liquorItem != null)
                                {
                                    if (liquorItem.ParentItemId.HasValue)
                                    {
                                        var parentItem = await db.LiquorItems.FindAsync(liquorItem.ParentItemId.Value);
                                        if (parentItem != null)
                                        {
                                            decimal pourSize = liquorItem.PourVolumeOunces ?? 5.0m;
                                            decimal bottleVolume = parentItem.BottleVolumeOunces ?? 25.0m;
                                            
                                            parentItem.OuncesAccumulator += (parent.Quantity * pourSize);
                                            
                                            int bottlesToDeduct = 0;
                                            if (bottleVolume > 0)
                                            {
                                                bottlesToDeduct = (int)(parentItem.OuncesAccumulator / bottleVolume);
                                                parentItem.OuncesAccumulator %= bottleVolume;
                                            }
                                            
                                            await db.SaveChangesAsync();
                                            
                                            if (bottlesToDeduct > 0)
                                            {
                                                await _liquorService.AdjustStockAsync(parentItem.Id, userId, -bottlesToDeduct, $"POS Sale: {parent.Quantity}x {parent.Name} (Accumulated depletion)");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        await _liquorService.AdjustStockAsync(parent.Id, userId, -parent.Quantity, $"POS Sale: {parent.Name}");
                                    }
                                }
                            } catch (Exception invEx) {
                                _logger.LogWarning("Could not adjust stock for item {Id} ({Name}): {Msg}", parent.Id, parent.Name, invEx.Message);
                            }
                        }

                        if (parent.Modifiers != null)
                        {
                            foreach (var mod in parent.Modifiers.Where(m => m.Id > 0))
                            {
                                try {
                                    int totalModQty = parent.Quantity * mod.Quantity;
                                    var liquorItem = await db.LiquorItems.FindAsync(mod.Id);
                                    if (liquorItem != null)
                                    {
                                        if (liquorItem.ParentItemId.HasValue)
                                        {
                                            var parentItem = await db.LiquorItems.FindAsync(liquorItem.ParentItemId.Value);
                                            if (parentItem != null)
                                            {
                                                decimal pourSize = liquorItem.PourVolumeOunces ?? 5.0m;
                                                decimal bottleVolume = parentItem.BottleVolumeOunces ?? 25.0m;
                                                
                                                parentItem.OuncesAccumulator += (totalModQty * pourSize);
                                                
                                                int bottlesToDeduct = 0;
                                                if (bottleVolume > 0)
                                                {
                                                    bottlesToDeduct = (int)(parentItem.OuncesAccumulator / bottleVolume);
                                                    parentItem.OuncesAccumulator %= bottleVolume;
                                                }
                                                
                                                await db.SaveChangesAsync();
                                                
                                                if (bottlesToDeduct > 0)
                                                {
                                                    await _liquorService.AdjustStockAsync(parentItem.Id, userId, -bottlesToDeduct, $"POS Sale (Add-on): {totalModQty}x {mod.Name} (for {parent.Name}) (Accumulated depletion)");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            await _liquorService.AdjustStockAsync(mod.Id, userId, -totalModQty, $"POS Sale (Add-on): {mod.Name} (for {parent.Name})");
                                        }
                                    }
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
                ItemTotalsJson = reportDto.ItemTotalsJson,
                BanquetSummaryJson = reportDto.BanquetSummaryJson,
                TokenCredits = reportDto.TokenCredits,
                HoursWorked = reportDto.HoursWorked,
                ShiftType = reportDto.ShiftType,
                RecordSalesToBar = reportDto.RecordSalesToBar,
                IsSynced = true
            };

            db.PosZReports.Add(report);

                // Sync with BarSaleEntries if hours are provided OR record sales is enabled
                if (reportDto.HoursWorked.HasValue || reportDto.RecordSalesToBar)
                {
                    var localTime = reportDto.Timestamp;
                    var localHour = localTime.Hour;
                    DateTime targetDate = localTime.Date;
                    string targetShift = "Day";

                    if (localHour >= 0 && localHour < 5)
                    {
                        targetDate = targetDate.AddDays(-1);
                        targetShift = "Night";
                    }
                    else if (localHour >= 5 && localHour < 19)
                    {
                        targetShift = "Day";
                    }
                    else
                    {
                        targetShift = "Night";
                    }

                    var barEntry = await db.BarSaleEntries
                        .FirstOrDefaultAsync(e => (e.AdjustedSaleDate ?? e.SaleDate).Date == targetDate && e.Shift == targetShift && e.IsRentalHall == false);

                    // Skip creating a new blank entry if this is a $0.00 quick reset Z-report with no hours worked
                    bool isZeroGrossQuickReset = reportDto.TotalGrossSales == 0 && (!reportDto.HoursWorked.HasValue || reportDto.HoursWorked.Value == 0);

                    if (barEntry == null && !isZeroGrossQuickReset)
                    {
                        barEntry = new BarSaleEntry
                        {
                            SaleDate = targetDate,
                            AdjustedSaleDate = targetDate,
                            Shift = targetShift,
                            IsRentalHall = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = reportDto.BartenderName,
                            Status = reportDto.RecordSalesToBar ? "Submitted" : "Draft"
                        };
                        db.BarSaleEntries.Add(barEntry);
                    }

                    if (barEntry != null)
                    {
                        if (reportDto.HoursWorked.HasValue && (reportDto.HoursWorked.Value > 0 || barEntry.TotalHours == 0))
                        {
                            barEntry.TotalHours = reportDto.HoursWorked.Value;
                        }

                        if (reportDto.RecordSalesToBar)
                        {
                            if (reportDto.TotalGrossSales > 0)
                            {
                                if (barEntry.TotalSales > 0 && barEntry.Status == "Submitted")
                                {
                                    barEntry.TotalSales += reportDto.TotalGrossSales;
                                }
                                else
                                {
                                    barEntry.TotalSales = reportDto.TotalGrossSales;
                                }
                            }
                            barEntry.Status = "Submitted";
                        }

                        barEntry.ModifiedAt = DateTime.UtcNow;
                        barEntry.ModifiedBy = reportDto.BartenderName;
                        if (!string.IsNullOrWhiteSpace(reportDto.BartenderName) && (string.IsNullOrWhiteSpace(barEntry.EmployeeUsername) || reportDto.TotalGrossSales > 0))
                        {
                            barEntry.EmployeeUsername = reportDto.BartenderName;
                        }
                    }
                }

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

    [HttpGet("darts-rounds-today/{terminalName}")]
    public async Task<ActionResult<List<PosSaleDto>>> GetDartsRoundsToday(string terminalName)
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        var today = DateTime.Today;
        var sales = await db.PosSales
            .Where(s => s.TerminalName == terminalName && 
                        s.Timestamp >= today && 
                        s.PaymentType == "TOKEN/COMP" && 
                        s.ItemsJson.Contains("(DARTS"))
            .OrderByDescending(s => s.Timestamp)
            .Select(s => new PosSaleDto
            {
                Id = s.Id,
                Timestamp = s.Timestamp,
                TerminalName = s.TerminalName,
                BartenderName = s.BartenderName,
                TotalAmount = s.TotalAmount,
                PaymentType = s.PaymentType,
                ItemsJson = s.ItemsJson,
                AmountReceived = s.AmountReceived,
                ChangeDue = s.ChangeDue,
                OriginalTotal = s.OriginalTotal
            })
            .ToListAsync();
        
        return Ok(sales);
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
                SalesSummaryJson = r.SalesSummaryJson,
                ItemTotalsJson = r.ItemTotalsJson ?? "{}",
                BanquetSummaryJson = r.BanquetSummaryJson ?? "[]",
                TokenCredits = r.TokenCredits ?? 0,
                HoursWorked = r.HoursWorked,
                ShiftType = r.ShiftType,
                RecordSalesToBar = r.RecordSalesToBar
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
            InventoryPullsJson = r.InventoryPullsJson ?? "{}",
            SalesSummaryJson = r.SalesSummaryJson ?? "{}",
            ItemTotalsJson = r.ItemTotalsJson ?? "{}",
            BanquetSummaryJson = r.BanquetSummaryJson ?? "[]",
            TokenCredits = r.TokenCredits ?? 0,
            HoursWorked = r.HoursWorked,
            ShiftType = r.ShiftType,
            RecordSalesToBar = r.RecordSalesToBar
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
            // The POS page permission can be identified by either "/admin/pos-terminal" or "pos"
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
                .Where(u => userIds.Contains(u.UserId) || u.IsAdmin)
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
                            if (i.Name.StartsWith("TAB DEPOSIT:") || i.Name.StartsWith("DEPOSIT CORRECTION:") || i.Name.StartsWith("REFUND:") || i.Name.StartsWith("BALANCE RETURNED:") || i.Name.StartsWith("RETURNED FUNDS:"))
                            {
                                if (i.Price >= 0)
                                {
                                    summary.TotalDeposited += i.Price;
                                }
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

    [HttpGet("members/draw-pool")]
    public async Task<IActionResult> GetMemberDrawPool()
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var members = await db.Members
                .AsNoTracking()
                .ToListAsync();

            var currentYear = DateTime.Now.Year;

            // Pre-fetch collections to do fast in-memory sets/lookups rather than N+1 queries
            var boardMemberIds = await db.BoardAssignments
                .AsNoTracking()
                .Where(ba => ba.TermYear == currentYear)
                .Select(ba => ba.MemberID)
                .ToListAsync();

            var duesPayments = await db.DuesPayments
                .AsNoTracking()
                .Where(dp => dp.Year == currentYear)
                .ToListAsync();

            var waiverPeriods = await db.DuesWaiverPeriods
                .AsNoTracking()
                .Where(dwp => dwp.StartYear <= currentYear && dwp.EndYear >= currentYear)
                .ToListAsync();

            var boardMemberSet = new HashSet<int>(boardMemberIds);
            
            // A member has their dues paid if they have a payment with a PaidDate
            var duesPaidSet = new HashSet<int>(
                duesPayments
                    .Where(dp => dp.PaidDate.HasValue)
                    .Select(dp => dp.MemberId)
            );

            // A member is explicitly waived if their payment type is "WAIVED"
            var duesExplicitWaivedSet = new HashSet<int>(
                duesPayments
                    .Where(dp => dp.PaymentType != null && dp.PaymentType.Equals("WAIVED", StringComparison.OrdinalIgnoreCase))
                    .Select(dp => dp.MemberId)
            );

            var multiYearWaiverSet = new HashSet<int>(
                waiverPeriods
                    .Select(dwp => dwp.MemberId)
            );

            var poolItems = new List<MemberDrawPoolItemDto>();
            var memberIds = new List<int>();

            foreach (var m in members)
            {
                var isActive = (m.Status == "REGULAR" || m.Status == "REGULAR-NP" || m.Status == "LIFE" || m.Status == "GUEST")
                    && m.Status != "INACTIVE"
                    && m.Status != "DECEASED"
                    && m.Status != "REJECTED";

                var isLife = m.Status == "LIFE";
                var isBoard = boardMemberSet.Contains(m.MemberID);
                var isAutoWaived = isLife || isBoard;
                var isDuesPaid = duesPaidSet.Contains(m.MemberID);
                var isExplicitWaived = duesExplicitWaivedSet.Contains(m.MemberID);
                var hasMultiYearWaiver = multiYearWaiverSet.Contains(m.MemberID);

                var isWaived = isAutoWaived || isExplicitWaived || hasMultiYearWaiver;
                var isEligible = isActive && (isDuesPaid || isWaived);

                memberIds.Add(m.MemberID);
                poolItems.Add(new MemberDrawPoolItemDto
                {
                    MemberId = m.MemberID,
                    FirstName = m.FirstName ?? string.Empty,
                    LastName = m.LastName ?? string.Empty,
                    Suffix = m.Suffix,
                    IsEligible = isEligible
                });
            }

            var maxId = memberIds.Any() ? memberIds.Max() : 0;

            return Ok(new MemberDrawPoolDto
            {
                MemberIds = memberIds,
                MaxMemberId = maxId,
                Members = poolItems
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[POS API] Error fetching member draw pool");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("members/{id}/draw-status")]
    public async Task<IActionResult> GetMemberDrawStatus(int id)
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var member = await db.Members
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MemberID == id);

            if (member == null)
            {
                return NotFound($"Member with ID {id} not found");
            }

            var currentYear = DateTime.Now.Year;

            // 1. Is the member active?
            var isActive = (member.Status == "REGULAR" || member.Status == "REGULAR-NP" || member.Status == "LIFE" || member.Status == "GUEST")
                && member.Status != "INACTIVE"
                && member.Status != "DECEASED"
                && member.Status != "REJECTED";

            // 2. Is the member waived automatically (LIFE or Board Director)?
            var isLife = member.Status == "LIFE";
            var isBoardMember = await db.BoardAssignments
                .AsNoTracking()
                .AnyAsync(ba => ba.MemberID == member.MemberID && ba.TermYear == currentYear);

            var isAutoWaived = isLife || isBoardMember;
            var waiverReason = isLife ? "Life Member Waiver" : (isBoardMember ? "Board Director Waiver" : "");

            // 3. Does the member have direct dues paid/waived record for this year?
            var dues = await db.DuesPayments
                .AsNoTracking()
                .FirstOrDefaultAsync(dp => dp.MemberId == member.MemberID && dp.Year == currentYear);

            var isDuesPaidRecord = dues != null && dues.PaidDate.HasValue;
            var isExplicitWaivedRecord = dues != null && dues.PaymentType != null && dues.PaymentType.Equals("WAIVED", StringComparison.OrdinalIgnoreCase);

            // 4. Does the member have a multi-year waiver covering the current year?
            var hasMultiYearWaiver = false;
            if (!isAutoWaived && !isExplicitWaivedRecord)
            {
                var multiWaiver = await db.DuesWaiverPeriods
                    .AsNoTracking()
                    .FirstOrDefaultAsync(dwp => dwp.MemberId == member.MemberID && dwp.StartYear <= currentYear && dwp.EndYear >= currentYear);
                
                if (multiWaiver != null)
                {
                    hasMultiYearWaiver = true;
                    waiverReason = $"Waiver Period: {multiWaiver.Reason}";
                }
            }

            var isDuesPaid = isDuesPaidRecord;
            var isWaived = isAutoWaived || isExplicitWaivedRecord || hasMultiYearWaiver;

            if (isExplicitWaivedRecord && string.IsNullOrEmpty(waiverReason))
            {
                waiverReason = dues?.Notes ?? "Explicit Dues Waiver";
            }

            var isEligible = isActive && (isDuesPaid || isWaived);

            var dto = new MemberDrawStatusDto
            {
                MemberId = member.MemberID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Suffix = member.Suffix,
                Status = member.Status,
                IsActive = isActive,
                DuesPaid = isDuesPaid,
                IsWaived = isWaived,
                WaiverReason = waiverReason,
                IsEligible = isEligible
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[POS API] Error looking up member draw status for ID {MemberId}", id);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("employee-shifts")]
    public async Task<IActionResult> GetEmployeeShifts([FromQuery] string username, [FromQuery] int year, [FromQuery] int month)
    {
        try
        {
            using var db = await _dbFactory.CreateDbContextAsync();
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var entries = await db.BarSaleEntries.IgnoreQueryFilters().AsNoTracking()
                .Where(e => !e.IsDeleted &&
                       (e.AdjustedSaleDate ?? e.SaleDate).Date >= startDate.Date &&
                       (e.AdjustedSaleDate ?? e.SaleDate).Date <= endDate.Date &&
                       (e.EmployeeUsername == username || e.CreatedBy == username || e.ModifiedBy == username) &&
                       (e.TotalHours.HasValue && e.TotalHours.Value > 0))
                .OrderBy(e => e.AdjustedSaleDate ?? e.SaleDate)
                .ToListAsync();

            var shifts = entries.Select(e => new EmployeeShiftItemDto
            {
                Date = (e.AdjustedSaleDate ?? e.SaleDate).Date,
                DayOfWeek = (e.AdjustedSaleDate ?? e.SaleDate).Date.ToString("ddd"),
                FormattedDate = (e.AdjustedSaleDate ?? e.SaleDate).Date.ToString("MM/dd/yyyy"),
                ShiftType = e.Shift ?? "Day",
                Location = (e.IsRentalHall || e.Shift == "Hall") ? "Hall" : (e.BarLocation ?? "Main"),
                Hours = e.TotalHours ?? 0,
                Notes = e.Notes
            }).ToList();

            var user = await db.AppUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
            Member? member = null;
            if (user?.MemberId.HasValue == true)
            {
                member = await db.Members.AsNoTracking().FirstOrDefaultAsync(m => m.MemberID == user.MemberId.Value);
            }
            string displayName = member != null ? $"{member.FirstName} {member.LastName}".Trim() : username;

            var result = new EmployeeMonthlyShiftsDto
            {
                Username = username,
                DisplayName = displayName,
                Month = month,
                Year = year,
                MonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                TotalDaysWorked = shifts.Select(s => s.Date).Distinct().Count(),
                TotalShifts = shifts.Count,
                TotalHours = shifts.Sum(s => s.Hours),
                Shifts = shifts
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[POS API] Error retrieving employee shifts for {Username} in {Month}/{Year}", username, month, year);
            return StatusCode(500, "Internal Server Error");
        }
    }

    /// <summary>Minimal DTO for deserializing a single BeerTallyItem entry from the client's BeerTalliesJson.</summary>
    private class TallyEntry
    {
        public int ItemId { get; set; }
        public string? Name { get; set; }
        public int QuantityTaken { get; set; }
        public int QuantityReturned { get; set; }
    }

    private class DonatedLogEntry
    {
        public string? RecordId { get; set; }
        public int ItemId { get; set; }
        public string? BeerName { get; set; }
        public int Cases { get; set; }
        public int Cans { get; set; }
        public string? ActionType { get; set; }
        public string? RecipientEventName { get; set; }
    }
}
