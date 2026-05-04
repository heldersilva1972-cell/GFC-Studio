using System;

namespace GFC.Core.DTOs
{
    public class MobileKeyCardDashboardRow
    {
        public int CardId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string MemberStatus { get; set; } = string.Empty;
        public bool IsEligible { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public bool IsDelinquent { get; set; }
        public bool InGracePeriod { get; set; }
        public string? DeactivationReason { get; set; }
        public int Year { get; set; }
        public bool IsSyncPending { get; set; }
        public bool IsDirector { get; set; }
        public bool IsLifeMember { get; set; }
        public bool HasCard => CardId > 0;
    }

    public class ToggleCardRequest 
    { 
        public int CardId { get; set; } 
    }

    public class AssignCardRequest 
    { 
        public int MemberId { get; set; } 
        public string CardNumber { get; set; } = ""; 
        public string? Notes { get; set; } 
        public string? CardType { get; set; } 
    }
}
