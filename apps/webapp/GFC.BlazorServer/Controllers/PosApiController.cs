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

    public PosApiController(
        IDbContextFactory<GfcDbContext> dbFactory,
        ILiquorService liquorService,
        ILogger<PosApiController> logger)
    {
        _dbFactory = dbFactory;
        _liquorService = liquorService;
        _logger = logger;
    }

    [HttpGet("menu")]
    public async Task<ActionResult<PosMenuDto>> GetMenu()
    {
        using var db = await _dbFactory.CreateDbContextAsync();
        
        var categories = await db.PosCategories
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync();

        var items = await db.LiquorItems
            .Where(i => i.ShowInPos)
            .OrderBy(i => i.DisplayOrder)
            .ThenBy(i => i.Name)
            .Select(i => new PosItemDto 
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.RetailPrice,
                Category = i.Category ?? "MISC",
                DisplayOrder = i.DisplayOrder
            })
            .ToListAsync();

        var tokens = await db.PosTokens
            .Where(t => t.IsActive)
            .ToListAsync();

        var categoryNames = categories.Select(c => c.Name).ToList();
        categoryNames.Add("TOKENS");

        return Ok(new PosMenuDto
        {
            Categories = categoryNames,
            Items = items,
            Tokens = tokens
        });
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
                IsSynced = true
            };

            db.PosSales.Add(sale);

            // Handle inventory if not in training mode
            if (!saleDto.TerminalName.Contains("(TRAINING)"))
            {
                var items = JsonSerializer.Deserialize<List<PosSaleItemDto>>(saleDto.ItemsJson);
                if (items != null)
                {
                    foreach (var item in items.Where(i => i.Id > 0))
                    {
                        // Note: Using a default system user ID (1) for POS adjustments
                        await _liquorService.AdjustStockAsync(item.Id, 1, -item.Quantity, $"POS Sale: {item.Name}");
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
                IsSynced = true
            };

            db.PosZReports.Add(report);

            // Handle Inventory Pulls
            if (!string.IsNullOrEmpty(reportDto.InventoryPullsJson))
            {
                var pulls = JsonSerializer.Deserialize<Dictionary<int, int>>(reportDto.InventoryPullsJson);
                if (pulls != null)
                {
                    foreach (var pull in pulls)
                    {
                        for (int i = 0; i < pull.Value; i++)
                        {
                            await _liquorService.CheckoutBottleAsync(pull.Key, 1, $"[{reportDto.TerminalName}] POS End-of-Shift Removal");
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
            ItemsJson = sale.ItemsJson
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
}

