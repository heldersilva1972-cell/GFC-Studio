using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces;

public record MissingShiftAlert(DateTime Date, string ShiftType, string Message, string? AssignedBartender = null);

public interface IShiftComplianceService
{
    Task<List<MissingShiftAlert>> GetMissingShiftsAsync(int lookbackDays = 2);
}
