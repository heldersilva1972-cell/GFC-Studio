// [NEW]
using GFC.Core.Interfaces;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly GfcDbContext _context;

        public AvailabilityService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<List<DateTime>> GetReservedDatesAsync()
        {
            return await _context.AvailabilityCalendars
                .Where(ac => ac.Status == "Booked")
                .Select(ac => ac.Date.Date)
                .ToListAsync();
        }
    }
}
