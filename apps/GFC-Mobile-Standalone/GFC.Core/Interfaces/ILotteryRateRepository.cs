using GFC.Core.Models;
using System.Collections.Generic;

namespace GFC.Core.Interfaces
{
    public interface ILotteryRateRepository
    {
        List<LotteryCommissionRate> GetAll();
        LotteryCommissionRate? GetByYear(int year);
        void Save(LotteryCommissionRate rate);
        LotteryCommissionRate GetApplicableRate(int year);
    }
}
