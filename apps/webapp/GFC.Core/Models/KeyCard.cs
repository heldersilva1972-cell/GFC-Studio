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
    public bool IsControllerSynced { get; set; }
    public DateTime? LastControllerSyncDate { get; set; }
    public string? CardType { get; set; } // "Card" or "Fob"
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
}



