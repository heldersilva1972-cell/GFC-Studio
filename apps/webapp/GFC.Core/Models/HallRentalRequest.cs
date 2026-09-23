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
        public string? RequesterAddress { get; set; }
        public string? RequesterCity { get; set; }
        public string? RequesterState { get; set; }
        public string? RequesterZip { get; set; }
        public string? ApplicantSignature { get; set; }
        
        // Event Details
        public DateTime EventDate { get; set; }
        public DateTime? AlternateEventDate { get; set; }
        public string? RoomSelected { get; set; } = "Function Hall"; // "Function Hall", "Coalition Room", "Youth Organization"
        public string? EventType { get; set; } // e.g., "Wedding", "Birthday Party"
        public string? EventDescription { get; set; }
        public string? StartTime { get; set; } // e.g., "2:00 PM"
        public string? EndTime { get; set; } // e.g., "10:00 PM"
        public string RenterType { get; set; } = "Non-Member"; // Member, Non-Member, Non-Profit
        public bool MemberStatus { get; set; }
        public int GuestCount { get; set; }
        public bool RulesAgreed { get; set; }
        public bool TermsAgreed { get; set; }
        public bool CancellationPolicyAgreed { get; set; }
        public bool KitchenPolicyAgreed { get; set; }
        public bool BartenderRequested { get; set; }
        public bool KitchenUsage { get; set; }
        public bool AvEquipmentUsage { get; set; }
        public bool RequiresSetupTime { get; set; }
        public bool SecurityDepositPaid { get; set; }
        public decimal SecurityDepositAmount { get; set; }
        public DateTime RequestedDate { get; set; }
        
        // Pricing & Payment
        public decimal TotalPrice { get; set; }
        public decimal AmountPaid { get; set; } = 0;
        public bool IsPaid { get; set; }
        public string? PaymentMethod { get; set; } // e.g., "Check", "Cash", "Online"
        public DateTime? PaymentDate { get; set; }
        
        // Status and Approval
        public string Status { get; set; } = "Pending";
        public string? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string? DeniedBy { get; set; }
        public DateTime? DenialDate { get; set; }
        public string? StatusChangedBy { get; set; }
        public DateTime? StatusChangedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Sandbox / Test Tracking
        public bool IsTestRecord { get; set; } = false;

        // Admin Notes
        public string? InternalNotes { get; set; }

        // Accepted Terms and Policies Details (JSON list of agreed policy items or titles)
        public string? AgreedPoliciesJson { get; set; }
    }
}
