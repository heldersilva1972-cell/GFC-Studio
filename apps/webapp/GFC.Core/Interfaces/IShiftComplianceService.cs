using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces;

public record MissingShiftAlert(DateTime Date, string ShiftType, string Message);

public interface IShiftComplianceService
{
    Task<List<MissingShiftAlert>> GetMissingShiftsAsync(int lookbackDays = 2);
}
