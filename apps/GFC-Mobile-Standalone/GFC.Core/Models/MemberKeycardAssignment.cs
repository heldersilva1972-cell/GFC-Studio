namespace GFC.Core.Models;

/// <summary>
/// Represents the history record for a key card assignment to a member.
/// </summary>
public class MemberKeycardAssignment
{
    public int AssignmentId { get; set; }
    public int MemberId { get; set; }
    public int KeyCardId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? ChangedBy { get; set; }

    public KeyCard? KeyCard { get; set; }
}



