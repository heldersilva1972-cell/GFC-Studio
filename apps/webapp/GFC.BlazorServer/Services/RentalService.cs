// [MODIFIED]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class RentalService : IRentalService
    {
        private readonly GfcDbContext _context;

        public RentalService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<HallRentalRequest> GetRentalRequestAsync(int id)
        {
            return await _context.HallRentalRequests.FindAsync(id);
        }

        public async Task<IEnumerable<HallRentalRequest>> GetRentalRequestsAsync()
        {
            return await _context.HallRentalRequests.ToListAsync();
        }

        public async Task CreateRentalRequestAsync(HallRentalRequest request)
        {
            _context.HallRentalRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRentalRequestAsync(HallRentalRequest request)
        {
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
    }
}
