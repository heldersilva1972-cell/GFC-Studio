// [NEW]
using System;

namespace GFC.Core.Models;

public class HallRentalRequest
{
    public int Id { get; set; }
    public int GuestCount { get; set; }
    public DateTime EventDate { get; set; }
    public string Status { get; set; }
    public string ApprovedBy { get; set; }
    public DateTime? ApprovalDate { get; set; }
}
