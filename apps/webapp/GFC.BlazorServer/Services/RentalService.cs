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

        public async Task<HallRental> GetRentalRequestAsync(int id)
        {
            return await _context.HallRentals.FindAsync(id);
        }

        public async Task<IEnumerable<HallRental>> GetRentalRequestsAsync()
        {
            return await _context.HallRentals.ToListAsync();
        }

        public async Task CreateRentalRequestAsync(HallRental request)
        {
            _context.HallRentals.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRentalRequestAsync(HallRental request)
        {
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRentalRequestAsync(int id)
        {
            var request = await _context.HallRentals.FindAsync(id);
            if (request != null)
            {
                _context.HallRentals.Remove(request);
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
