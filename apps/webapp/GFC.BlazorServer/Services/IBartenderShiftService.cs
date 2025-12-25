// [NEW]
using GFC.Core.Models;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IBartenderShiftService
    {
        Task<StaffShift?> GetCurrentShiftAsync(string userName);
        Task<StaffShift> ClockInAsync(string userName);
        Task<StaffShift> ClockOutAsync(int shiftId);
        Task<ShiftReport> SubmitReportAsync(ShiftReport report);
    }
}
