// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class EventPromotionService : IEventPromotionService
    {
        private readonly GfcDbContext _context;

        public EventPromotionService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<EventPromotion> GetEventPromotionAsync(int id)
        {
            return await _context.EventPromotions.FindAsync(id);
        }

        public async Task<IEnumerable<EventPromotion>> GetEventPromotionsAsync()
        {
            return await _context.EventPromotions.ToListAsync();
        }

        public async Task CreateEventPromotionAsync(EventPromotion promotion)
        {
            _context.EventPromotions.Add(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEventPromotionAsync(EventPromotion promotion)
        {
            _context.Entry(promotion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEventPromotionAsync(int id)
        {
            var promotion = await _context.EventPromotions.FindAsync(id);
            if (promotion != null)
            {
                _context.EventPromotions.Remove(promotion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
