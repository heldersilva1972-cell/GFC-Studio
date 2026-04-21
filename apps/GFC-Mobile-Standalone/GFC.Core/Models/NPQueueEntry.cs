namespace GFC.Core.Models
{
    public class NPQueueEntry
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        // NP queue position at that time; can change over time.
        public int Position { get; set; }

        // Whether the entry is still active (currently in NP queue).
        public bool IsActive { get; set; }

        // Reason for the entry or change (optional).
        public string? Note { get; set; }

        public DateTime CreatedUtc { get; set; }
        public DateTime? DeactivatedUtc { get; set; }
    }
}
