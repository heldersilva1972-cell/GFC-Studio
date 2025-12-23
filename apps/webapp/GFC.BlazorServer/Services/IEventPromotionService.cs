// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IEventPromotionService
    {
        Task<EventPromotion> GetEventPromotionAsync(int id);
        Task<IEnumerable<EventPromotion>> GetEventPromotionsAsync();
        Task CreateEventPromotionAsync(EventPromotion request);
        Task UpdateEventPromotionAsync(EventPromotion request);
        Task DeleteEventPromotionAsync(int id);
    }
}
