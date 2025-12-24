// [NEW]
using System;

namespace GFC.Core.Models;

public class StaffShift
{
    public int Id { get; set; }
    public int StaffId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ShiftReport ShiftReport { get; set; }
}
