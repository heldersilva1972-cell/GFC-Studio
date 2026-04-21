// [NEW]
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IAvailabilityService
    {
        Task<List<DateTime>> GetReservedDatesAsync();
    }
}
