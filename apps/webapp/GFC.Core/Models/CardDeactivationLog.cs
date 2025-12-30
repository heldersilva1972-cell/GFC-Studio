using System;

namespace GFC.Core.Models
{
    public class CardDeactivationLog
    {
        public int LogId { get; set; }
        public int KeyCardId { get; set; }
        public int MemberId { get; set; }
        public DateTime DeactivatedDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public bool ControllerSynced { get; set; }
        public DateTime? SyncedDate { get; set; }
        public string? Notes { get; set; }
    }
}
