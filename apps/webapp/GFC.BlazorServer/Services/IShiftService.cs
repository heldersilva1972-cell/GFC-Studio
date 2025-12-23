// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IShiftService
    {
        Task<StaffShift> GetStaffShiftAsync(int id);
        Task<IEnumerable<StaffShift>> GetStaffShiftsAsync();
        Task<IEnumerable<StaffShift>> GetShiftsForWeekAsync(System.DateTime startDate);
        Task CreateStaffShiftAsync(StaffShift shift);
        Task UpdateStaffShiftAsync(StaffShift shift);
        Task<IEnumerable<ShiftReport>> GetShiftReportsForExportAsync(System.DateTime startDate, System.DateTime endDate);
        Task DeleteStaffShiftAsync(int id);
        Task SubmitShiftReportAsync(ShiftReport report);
        Task<ShiftReport> GetShiftReportAsync(int shiftId);
        Task<IEnumerable<AppUser>> GetAssignableStaffAsync();
        Task<IEnumerable<StaffShift>> GetShiftsForTomorrowAsync();
    }
}
