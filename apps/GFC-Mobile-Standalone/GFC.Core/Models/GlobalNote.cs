namespace GFC.Core.Models;

/// <summary>
/// Represents a global note entry for system-wide logging.
/// </summary>
public class GlobalNote
{
    public int GlobalNoteID { get; set; }
    public DateTime NoteDate { get; set; }
    public string? Category { get; set; }
    public string Text { get; set; } = string.Empty;
}



