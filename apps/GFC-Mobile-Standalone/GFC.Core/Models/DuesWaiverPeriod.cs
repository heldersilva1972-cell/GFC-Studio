namespace GFC.Core.Models;

/// <summary>
/// Represents a multi-year dues waiver granted to a member.
/// </summary>
public class DuesWaiverPeriod
{
    public int WaiverId { get; set; }
    public int MemberId { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
}



