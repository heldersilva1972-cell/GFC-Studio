// [MODIFIED]
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
        Task<bool> UpdateRentalRequestAsync(HallRentalRequest request);
        Task DeleteRentalRequestAsync(int id);
        Task UpdateCalendarAvailabilityAsync(System.DateTime date, string status);
    }
}
