using GFC.Core.Models;

namespace GFC.Core.Interfaces
{
    public interface ILotteryShiftRepository
    {
        LotteryShift? GetById(int shiftId);
        List<LotteryShift> GetByDateRange(DateTime startDate, DateTime endDate);
        List<LotteryShift> GetByEmployee(string employeeName, DateTime? startDate = null, DateTime? endDate = null);
        List<LotteryShift> GetAll();
        int Create(LotteryShift shift);
        void Update(LotteryShift shift);
        void Delete(int shiftId);
        List<LotteryShift> GetUnreconciled();
        bool Exists(int shiftId);
    }
}

