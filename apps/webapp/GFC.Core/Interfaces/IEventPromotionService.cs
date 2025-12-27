// [NEW]
using GFC.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IEventPromotionService
    {
        Task<List<EventPromotion>> GetUpcomingEventPromotionsAsync(DateTime startDate);
    }
}
