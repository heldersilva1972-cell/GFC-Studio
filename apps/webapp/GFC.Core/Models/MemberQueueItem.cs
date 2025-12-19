namespace GFC.Core.Models;

/// <summary>
/// Represents a member waiting in the Non-Portuguese queue.
/// </summary>
public class MemberQueueItem
{
    public int MemberID { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateTime? AcceptedDate { get; set; }
    public int Position { get; set; }
}

