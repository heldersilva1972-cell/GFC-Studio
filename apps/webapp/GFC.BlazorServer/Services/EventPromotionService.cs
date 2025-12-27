// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class EventPromotionService : IEventPromotionService
    {
        private readonly IDbContextFactory<GfcDbContext> _contextFactory;

        public EventPromotionService(IDbContextFactory<GfcDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<EventPromotion>> GetUpcomingEventPromotionsAsync(DateTime startDate)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.EventPromotions
                .Where(e => e.EventDate >= startDate)
                .OrderBy(e => e.EventDate)
                .ToListAsync();
        }
    }
}
