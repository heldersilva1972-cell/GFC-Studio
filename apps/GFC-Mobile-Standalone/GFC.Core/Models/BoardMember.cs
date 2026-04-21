namespace GFC.Core.Models;

/// <summary>
/// Represents a board member role assignment.
/// </summary>
public class BoardMember
{
    public int BoardMemberID { get; set; }
    public int MemberID { get; set; }
    public string Role { get; set; } = string.Empty; // PRESIDENT, VP, TREASURER, etc.
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}



