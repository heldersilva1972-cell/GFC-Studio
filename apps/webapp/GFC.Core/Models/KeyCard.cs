namespace GFC.Core.Models;

/// <summary>
/// Represents a physical EM-ID key card.
/// </summary>
public class KeyCard
{
    public int KeyCardId { get; set; }
    public int MemberId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
}



