using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Repositories;

public class ClubEventRepository : IClubEventRepository
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;

    public ClubEventRepository(IDbContextFactory<GfcDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<ClubEvent>> GetAllEventsAsync(bool includeArchived = false)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var query = context.ClubEvents
            .Include(e => e.Transactions)
            .Where(e => !e.IsDeleted)
            .AsQueryable();
        
        if (!includeArchived)
            query = query.Where(e => !e.IsArchived);
            
        return await query.OrderByDescending(e => e.EventDate).ToListAsync();
    }

    public async Task<ClubEvent?> GetEventByIdAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.ClubEvents
            .Include(e => e.Transactions)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<int> CreateEventAsync(ClubEvent clubEvent)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.ClubEvents.Add(clubEvent);
        await context.SaveChangesAsync();
        return clubEvent.Id;
    }

    public async Task UpdateEventAsync(ClubEvent clubEvent)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.ClubEvents.Update(clubEvent);
        await context.SaveChangesAsync();
    }

    public async Task DeleteEventAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var clubEvent = await context.ClubEvents.FindAsync(id);
        if (clubEvent != null)
        {
            clubEvent.IsDeleted = true;
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ClubEventTransaction>> GetTransactionsForEventAsync(int eventId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.ClubEventTransactions
            .Where(t => t.EventId == eventId)
            .OrderBy(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<int> AddTransactionAsync(ClubEventTransaction transaction)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.ClubEventTransactions.Add(transaction);
        await context.SaveChangesAsync();
        return transaction.Id;
    }

    public async Task DeleteTransactionAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var transaction = await context.ClubEventTransactions.FindAsync(id);
        if (transaction != null)
        {
            transaction.IsDeleted = true;
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<EventPerformanceDto>> GetPerformanceHistoryAsync(string eventGroup)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        
        // Find all events in this group (including archived, but NOT deleted)
        var performanceByYear = await context.ClubEvents
            .Where(e => e.EventGroup == eventGroup)
            .Select(e => new { e.Id, e.EventDate.Year })
            .ToListAsync();

        var eventIds = performanceByYear.Select(x => x.Id).ToList();

        // Get all transactions for these events
        var transactions = await context.ClubEventTransactions
            .Where(t => eventIds.Contains(t.EventId))
            .ToListAsync();

        // Group by year - EXCLUDE opening balances from performance
        var history = performanceByYear.Select(eventInfo => new EventPerformanceDto
        {
            Year = eventInfo.Year,
            TotalIncome = transactions.Where(t => t.EventId == eventInfo.Id && t.Amount > 0 && t.Category != "Opening Balance" && t.Description != "Opening Balance").Sum(t => t.Amount),
            TotalExpense = transactions.Where(t => t.EventId == eventInfo.Id && t.Amount < 0 && t.Category != "Opening Balance" && t.Description != "Opening Balance").Sum(t => t.Amount)
        })
        .OrderBy(h => h.Year)
        .ToList();

        return history;
    }
}


