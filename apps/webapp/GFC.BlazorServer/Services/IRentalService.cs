// [MODIFIED]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IRentalService
    {
        Task<HallRental> GetRentalRequestAsync(int id);
        Task<IEnumerable<HallRental>> GetRentalRequestsAsync();
        Task CreateRentalRequestAsync(HallRental request);
        Task UpdateRentalRequestAsync(HallRental request);
        Task DeleteRentalRequestAsync(int id);
        Task UpdateCalendarAvailabilityAsync(System.DateTime date, string status);
    }
}
