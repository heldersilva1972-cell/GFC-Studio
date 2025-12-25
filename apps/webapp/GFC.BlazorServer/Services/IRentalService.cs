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
        Task<IEnumerable<HallRentalRequest>> GetRentalRequestsByStatusAsync(string status);
        Task<IEnumerable<HallRentalRequest>> GetRentalRequestsByDateRangeAsync(System.DateTime startDate, System.DateTime endDate);
        Task<bool> IsDateAlreadyBookedAsync(System.DateTime date);
        Task CreateRentalRequestAsync(HallRentalRequest request);
        Task<bool> UpdateRentalRequestAsync(HallRentalRequest request);
        Task DeleteRentalRequestAsync(int id);
        Task UpdateCalendarAvailabilityAsync(System.DateTime date, string status);
        Task<bool> ApproveRentalRequestAsync(int requestId, string adminNotes);
        Task<bool> DenyRentalRequestAsync(int requestId, string adminNotes);
        Task<IEnumerable<HallRentalRequest>> GetApprovedRentalsAsync();
        Task CreateRentalInquiryAsync(HallRentalInquiry inquiry);
    }
}
