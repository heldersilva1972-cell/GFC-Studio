// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class BartenderShiftService : IBartenderShiftService
    {
        private readonly GfcDbContext _context;

        public BartenderShiftService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<StaffShift?> GetCurrentShiftAsync(string userName)
        {
            var staffMember = await _context.StaffMembers.FirstOrDefaultAsync(s => s.Email == userName);
            if (staffMember == null) return null;

            return await _context.StaffShifts
                .Where(s => s.StaffMemberId == staffMember.Id && s.ClockOutTime == null)
                .OrderByDescending(s => s.ClockInTime)
                .FirstOrDefaultAsync();
        }

        public async Task<StaffShift> ClockInAsync(string userName)
        {
            var existingShift = await GetCurrentShiftAsync(userName);
            if (existingShift != null)
            {
                throw new InvalidOperationException("An active shift already exists for this user. Please clock out before clocking in again.");
            }

            var staffMember = await _context.StaffMembers.FirstOrDefaultAsync(s => s.Email == userName);
            if (staffMember == null) throw new InvalidOperationException("Staff member not found.");

            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            var newShift = new StaffShift
            {
                StaffMemberId = staffMember.Id,
                Date = easternTime.Date,
                ShiftType = easternTime.Hour < 17 ? 1 : 2, // 1=Day (ends 5pm), 2=Night
                ClockInTime = DateTime.UtcNow,
                Status = "Active"
            };

            _context.StaffShifts.Add(newShift);
            await _context.SaveChangesAsync();
            return newShift;
        }

        public async Task<StaffShift> ClockOutAsync(int shiftId)
        {
            var shift = await _context.StaffShifts.FindAsync(shiftId);
            if (shift == null)
            {
                throw new InvalidOperationException($"Shift with ID {shiftId} not found.");
            }

            shift.ClockOutTime = DateTime.UtcNow;
            shift.Status = "Completed";
            await _context.SaveChangesAsync();
            return shift;
        }

        public async Task<ShiftReport> SubmitReportAsync(ShiftReport report)
        {
            report.SubmittedAt = DateTime.UtcNow;
            _context.ShiftReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }
    }
}
