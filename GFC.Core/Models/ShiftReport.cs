// [NEW]
namespace GFC.Core.Models;

public class ShiftReport
{
    public int Id { get; set; }
    public decimal TotalSales { get; set; }
    public string Notes { get; set; }
    public int StaffShiftId { get; set; }
    public StaffShift StaffShift { get; set; }
}
