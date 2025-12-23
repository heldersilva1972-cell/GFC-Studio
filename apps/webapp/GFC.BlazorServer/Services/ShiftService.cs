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
            return await _context.ShiftReports.FirstOrDefaultAsync(r => r.StaffShiftId == shiftId);
        }

        public async Task<IEnumerable<StaffShift>> GetStaffShiftsAsync()
        {
            return await _context.StaffShifts.ToListAsync();
        }
    }
}
