namespace GFC.Core.Models;

/// <summary>
/// Represents a board assignment of a member to a position for a specific term year.
/// </summary>
public class BoardAssignment
{
    public int AssignmentID { get; set; }
    public int MemberID { get; set; }
    public int PositionID { get; set; }
    public int TermYear { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }
    
    // Navigation properties (populated when needed)
    public string? MemberName { get; set; }
    public string? PositionName { get; set; }
}



