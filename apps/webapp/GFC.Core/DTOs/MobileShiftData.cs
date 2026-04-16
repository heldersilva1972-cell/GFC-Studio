namespace GFC.Core.DTOs;

public class MobileShiftData
{
    public DateTime Date { get; set; }
    public string ShiftType { get; set; } = string.Empty;
    public bool IsRentalHall { get; set; }
    
    // Bar Data
    public decimal? BarSales { get; set; }
    public decimal? TotalHours { get; set; }
    public string? Notes { get; set; }
    
    // Lottery Data
    public decimal? LottoSales { get; set; }
    public decimal? LottoCashes { get; set; }
    public decimal? LottoInstantTickets { get; set; }
    public decimal? LottoNetDue { get; set; }
    public decimal? LottoOpeningCash { get; set; }
    public decimal? LottoBackupBag { get; set; }
    public decimal? LottoCashCounted { get; set; }
    
    // Metadata/Status
    public string Status { get; set; } = "Draft";
    public string? ModifiedBy { get; set; }
    public bool IsLocked { get; set; }
    public string? LockOwner { get; set; }
    public bool ExistingEntryFound { get; set; }
}
