// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class ShiftService : IShiftService
    {
        private readonly GfcDbContext _context;

        public ShiftService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<StaffShift> GetStaffShiftAsync(int id)
        {
            return await _context.StaffShifts.FindAsync(id);
        }

        public async Task CreateStaffShiftAsync(StaffShift shift)
        {
            _context.StaffShifts.Add(shift);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStaffShiftAsync(StaffShift shift)
        {
            _context.Entry(shift).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStaffShiftAsync(int id)
        {
            var shift = await _context.StaffShifts.FindAsync(id);
            if (shift != null)
            {
                _context.StaffShifts.Remove(shift);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SubmitShiftReportAsync(ShiftReport report)
        {
            _context.ShiftReports.Add(report);
            await _context.SaveChangesAsync();
        }

        public async Task<ShiftReport> GetShiftReportAsync(int shiftId)
        {
            return await _context.ShiftReports.FirstOrDefaultAsync(r => r.ShiftId == shiftId);
        }

        public async Task<IEnumerable<StaffShift>> GetStaffShiftsAsync()
        {
            var shifts = await _context.StaffShifts
                .Include(s => s.StaffMember)
                .ToListAsync();

            foreach (var shift in shifts)
            {
                if (shift.StaffMember != null)
                {
                    shift.StaffName = shift.StaffMember.Username; // Or a display name if available
                }
            }

            return shifts;
        }

        public async Task<IEnumerable<StaffShift>> GetShiftsForWeekAsync(System.DateTime startDate)
        {
            var endDate = startDate.AddDays(7);
            return await _context.StaffShifts
                .Include(s => s.StaffMember)
                .Where(s => s.ShiftDate >= startDate && s.ShiftDate < endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShiftReport>> GetShiftReportsForExportAsync(System.DateTime startDate, System.DateTime endDate)
        {
            return await _context.ShiftReports
                .Include(r => r.Shift)
                .ThenInclude(s => s.StaffMember)
                .Where(r => r.Shift.ShiftDate >= startDate && r.Shift.ShiftDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetAssignableStaffAsync()
        {
            // In a real app, you might filter this to users with a 'Staff' role.
            return await _context.Users.ToListAsync();
        }

        public async Task<IEnumerable<StaffShift>> GetShiftsForTomorrowAsync()
        {
            var tomorrow = DateTime.Today.AddDays(1);
            return await _context.StaffShifts
                .Include(s => s.StaffMember)
                .Where(s => s.ShiftDate.Date == tomorrow)
                .ToListAsync();
        }
    }
}
