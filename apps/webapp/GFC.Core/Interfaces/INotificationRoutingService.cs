// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface INotificationRoutingService
    {
        Task<List<NotificationRouting>> GetAllRoutingsAsync();
        Task<NotificationRouting> GetRoutingByIdAsync(int id);
        Task CreateRoutingAsync(NotificationRouting routing);
        Task UpdateRoutingAsync(NotificationRouting routing);
        Task DeleteRoutingAsync(int id);
        Task<string> GetEmailForActionAsync(string actionName);
    }
}
