namespace GFC.Core.Models
{
    public class KeyHistoryEntry
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int? OldKeyCardId { get; set; }
        public KeyCard? OldKeyCard { get; set; }

        public int? NewKeyCardId { get; set; }
        public KeyCard? NewKeyCard { get; set; }

        // Short reason (e.g. "Lost card", "Upgrade", etc.)
        public string? Reason { get; set; }

        public DateTime ChangedUtc { get; set; }
    }
}
