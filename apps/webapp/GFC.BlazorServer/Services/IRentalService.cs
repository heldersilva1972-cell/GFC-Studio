// [MODIFIED]
using GFC.Core.Models;
using GFC.Core.DTOs;
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
        Task UpdateCalendarAvailabilityAsync(System.DateTime date, string status, string? description = null, string? startTime = null, string? endTime = null);
        Task<bool> ApproveRentalRequestAsync(int requestId, string adminNotes, string approvedBy = "Admin");
        Task<bool> DenyRentalRequestAsync(int requestId, string adminNotes, string deniedBy = "Admin");
        Task<IEnumerable<HallRentalRequest>> GetApprovedRentalsAsync();
        Task CreateRentalInquiryAsync(HallRentalInquiry inquiry);
        Task<List<System.DateTime>> GetReservedDatesAsync();
        Task<List<AvailabilityCalendar>> GetBlackoutEventsAsync();
        Task AddBlackoutDateAsync(System.DateTime date, string? description = null, string? startTime = null, string? endTime = null);
        Task RemoveBlackoutDateAsync(System.DateTime date);
        Task<List<UnavailableDateDto>> GetUnavailableDatesAsync();
        Task<HallRentalInquiry> SaveInquiryAsync(string formData);
        Task<HallRentalInquiry> GetInquiryAsync(string resumeToken);
    }
}
