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
                    shift.StaffName = shift.StaffMember.Name ?? "Unknown";
                }
                else
                {
                    shift.StaffName = "Unassigned";
                }
            }

            return shifts;
        }

        public async Task<IEnumerable<StaffShift>> GetShiftsForWeekAsync(System.DateTime startDate)
        {
            try
            {
                var endDate = startDate.AddDays(7);
                return await GetShiftsForRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetShiftsForWeekAsync Error: {ex}");
                return new List<StaffShift>();
            }
        }

        public async Task<IEnumerable<StaffShift>> GetShiftsForRangeAsync(System.DateTime startDate, System.DateTime endDate)
        {
            try
            {
                // Load shifts with navigation properties in a single efficient query
                var shifts = await _context.StaffShifts
                    .Include(s => s.StaffMember)
                    .Where(s => s.Date >= startDate && s.Date < endDate)
                    .ToListAsync();

                foreach (var shift in shifts)
                {
                    if (shift.StaffMember != null)
                        shift.StaffName = shift.StaffMember.Name ?? "Unknown";
                    else
                        shift.StaffName = "Unassigned";
                }

                return shifts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetShiftsForRangeAsync Error: {ex}");
                return new List<StaffShift>();
            }
        }

        public async Task<IEnumerable<ShiftReport>> GetShiftReportsForExportAsync(System.DateTime startDate, System.DateTime endDate)
        {
            return await _context.ShiftReports
                .Include(r => r.StaffShift)
                .ThenInclude(s => s.StaffMember)
                .Where(r => r.StaffShift.Date >= startDate && r.StaffShift.Date <= endDate)
                .ToListAsync();
        }


        public async Task<IEnumerable<StaffMember>> GetAssignableStaffAsync()
        {
            return await _context.StaffMembers
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<StaffShift>> GetShiftsForTomorrowAsync()
        {
            var tomorrow = DateTime.Today.AddDays(1);
            return await _context.StaffShifts
                .Include(s => s.StaffMember)
                .Where(s => s.Date.Date == tomorrow)
                .ToListAsync();
        }

        // Staff Member Management Methods
        public async Task<StaffMember> GetStaffMemberAsync(int id)
        {
            return await _context.StaffMembers.FindAsync(id);
        }

        public async Task<IEnumerable<StaffMember>> GetAllStaffMembersAsync()
        {
            return await _context.StaffMembers
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<StaffMember>> GetActiveStaffMembersAsync()
        {
            return await _context.StaffMembers
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task CreateStaffMemberAsync(StaffMember staffMember)
        {
            staffMember.CreatedAt = DateTime.UtcNow;
            _context.StaffMembers.Add(staffMember);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStaffMemberAsync(StaffMember staffMember)
        {
            staffMember.UpdatedAt = DateTime.UtcNow;
            _context.Entry(staffMember).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStaffMemberAsync(int id)
        {
            var staffMember = await _context.StaffMembers.FindAsync(id);
            if (staffMember != null)
            {
                _context.StaffMembers.Remove(staffMember);
                await _context.SaveChangesAsync();
            }
        }
    }
}
