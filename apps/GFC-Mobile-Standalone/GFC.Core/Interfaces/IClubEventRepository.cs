using System.Collections.Generic;
using System.Threading.Tasks;
using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IClubEventRepository
{
    Task<IEnumerable<ClubEvent>> GetAllEventsAsync(bool includeArchived = false);
    Task<ClubEvent?> GetEventByIdAsync(int id);
    Task<int> CreateEventAsync(ClubEvent clubEvent);
    Task UpdateEventAsync(ClubEvent clubEvent);
    Task DeleteEventAsync(int id);
    
    Task<IEnumerable<ClubEventTransaction>> GetTransactionsForEventAsync(int eventId);
    Task<int> AddTransactionAsync(ClubEventTransaction transaction);
    Task DeleteTransactionAsync(int id);
    
    Task<IEnumerable<EventPerformanceDto>> GetPerformanceHistoryAsync(string eventGroup);
}
