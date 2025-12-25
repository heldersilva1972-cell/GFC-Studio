// [MODIFIED]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class RentalService : IRentalService
    {
        private readonly GfcDbContext _context;
        private readonly INotificationService _notificationService;

        public RentalService(GfcDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<HallRentalRequest> GetRentalRequestAsync(int id)
        {
            return await _context.HallRentalRequests.FindAsync(id);
        }

        public async Task<IEnumerable<HallRentalRequest>> GetRentalRequestsAsync()
        {
            return await _context.HallRentalRequests.ToListAsync();
        }

        public async Task<IEnumerable<HallRentalRequest>> GetRentalRequestsByStatusAsync(string status)
        {
            return await _context.HallRentalRequests.Where(r => r.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<HallRentalRequest>> GetRentalRequestsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.HallRentalRequests
                .Where(r => r.RequestedDate.Date >= startDate.Date && r.RequestedDate.Date <= endDate.Date)
                .ToListAsync();
        }

        public async Task<bool> IsDateAlreadyBookedAsync(DateTime date)
        {
            return await _context.HallRentalRequests
                .AnyAsync(r => r.RequestedDate.Date == date.Date && r.Status == RentalStatus.Approved);
        }

        public async Task CreateRentalRequestAsync(HallRentalRequest request)
        {
            _context.HallRentalRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateRentalRequestAsync(HallRentalRequest request)
        {
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true; // Indicate success
        }

        public async Task<bool> ApproveRentalRequestAsync(int requestId, string adminNotes)
        {
            var request = await _context.HallRentalRequests.FindAsync(requestId);
            if (request == null) return false;

            if (await IsDateAlreadyBookedAsync(request.RequestedDate))
            {
                return false; // Double booking detected
            }

            request.Status = RentalStatus.Approved;
            request.InternalNotes = adminNotes;
            _context.Entry(request).State = EntityState.Modified;

            await UpdateCalendarAvailabilityAsync(request.RequestedDate, "Booked");
            await _context.SaveChangesAsync();

            // Fire and forget email notification
            _ = _notificationService.SendRentalConfirmationEmailAsync(request);

            return true;
        }

        public async Task<bool> DenyRentalRequestAsync(int requestId, string adminNotes)
        {
            var request = await _context.HallRentalRequests.FindAsync(requestId);
            if (request == null) return false;

            request.Status = RentalStatus.Denied;
            request.InternalNotes = adminNotes;
            _context.Entry(request).State = EntityState.Modified;

            await UpdateCalendarAvailabilityAsync(request.RequestedDate, "Available");
            await _context.SaveChangesAsync();

            // Fire and forget email notification
            _ = _notificationService.SendRentalDenialEmailAsync(request, "Request denied by administrator");

            return true;
        }

        public async Task DeleteRentalRequestAsync(int id)
        {
            var request = await _context.HallRentalRequests.FindAsync(id);
            if (request != null)
            {
                _context.HallRentalRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCalendarAvailabilityAsync(DateTime date, string status)
        {
            var calendarEntry = await _context.AvailabilityCalendars.FirstOrDefaultAsync(c => c.Date.Date == date.Date);

            if (calendarEntry != null)
            {
                calendarEntry.Status = status;
            }
            else
            {
                _context.AvailabilityCalendars.Add(new AvailabilityCalendar { Date = date, Status = status });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<HallRentalRequest>> GetApprovedRentalsAsync()
        {
            return await _context.HallRentalRequests
                .Where(r => r.Status == RentalStatus.Approved)
                .ToListAsync();
        }

        public async Task CreateRentalInquiryAsync(HallRentalInquiry inquiry)
        {
            _context.HallRentalInquiries.Add(inquiry);
            await _context.SaveChangesAsync();
        public async Task<List<DateTime>> GetReservedDatesAsync()
        {
            return await _context.AvailabilityCalendars
                                 .Where(d => d.Status == "Booked")
                                 .Select(d => d.Date)
                                 .ToListAsync();
        }
    }
}
