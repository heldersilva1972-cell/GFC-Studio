namespace GFC.Core.Models
{
    public class MemberDoorAccess
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public int DoorId { get; set; }

        public string CardNumber { get; set; } = string.Empty;

        public int? TimeProfileId { get; set; }

        public TimeProfile? TimeProfile { get; set; }

        public bool IsEnabled { get; set; } = true;

        public DateTime? LastSyncedAt { get; set; }

        public string? LastSyncResult { get; set; }
    }
}
