namespace GFC.Core.Models;

/// <summary>
/// Represents a board position (e.g., President, Vice President, Trustee).
/// </summary>
public class BoardPosition
{
    public int PositionID { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public int MaxSeats { get; set; } = 1;
}



