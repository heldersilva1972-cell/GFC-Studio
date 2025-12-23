// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IRentalService
    {
        Task<HallRentalRequest> GetRentalRequestAsync(int id);
        Task<IEnumerable<HallRentalRequest>> GetRentalRequestsAsync();
        Task CreateRentalRequestAsync(HallRentalRequest request);
        Task UpdateRentalRequestAsync(HallRentalRequest request);
        Task DeleteRentalRequestAsync(int id);
        Task UpdateCalendarAvailabilityAsync(DateTime date, string status);
    }
}
