using System;

namespace GFC.Core.Models
{
    public class HallRentalRequest
    {
        public int Id { get; set; }
        
        // Applicant Information
        public string ApplicantName { get; set; } = string.Empty;
        public string RequesterName { get; set; } = string.Empty;
        public string RequesterEmail { get; set; } = string.Empty;
        public string RequesterPhone { get; set; } = string.Empty;
        
        // Event Details
        public DateTime EventDate { get; set; }
        public string? EventType { get; set; } // e.g., "Wedding", "Birthday Party"
        public string? StartTime { get; set; } // e.g., "2:00 PM"
        public string? EndTime { get; set; } // e.g., "10:00 PM"
        public bool MemberStatus { get; set; }
        public int GuestCount { get; set; }
        public bool RulesAgreed { get; set; }
        public bool KitchenUsage { get; set; }
        public DateTime RequestedDate { get; set; }
        
        // Pricing
        public decimal TotalPrice { get; set; }
        
        // Status and Approval
        public string Status { get; set; } = "Pending";
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? DeniedBy { get; set; }
        public DateTime? DenialDate { get; set; }
        public string? StatusChangedBy { get; set; }
        public DateTime? StatusChangedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Admin Notes
        public string? InternalNotes { get; set; }
    }
}
