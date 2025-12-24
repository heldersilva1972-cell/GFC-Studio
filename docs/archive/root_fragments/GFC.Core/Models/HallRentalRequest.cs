// [NEW]
using System;

namespace GFC.Core.Models;

public class HallRentalRequest
{
    public int Id { get; set; }
    public string RequesterName { get; set; } = string.Empty;
    public string RequesterEmail { get; set; } = string.Empty;
    public string RequesterPhone { get; set; } = string.Empty;
    public bool MemberStatus { get; set; }
    public int GuestCount { get; set; }
    public bool RulesAgreed { get; set; }
    public bool KitchenUsage { get; set; }
    public DateTime RequestedDate { get; set; }
    public string Status { get; set; } = "Pending";
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovalDate { get; set; }
}
