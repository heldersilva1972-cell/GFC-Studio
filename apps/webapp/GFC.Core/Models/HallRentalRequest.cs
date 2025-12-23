// [NEW]
using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class HallRentalRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RequesterName { get; set; }

        [Required]
        public string RequesterEmail { get; set; }

        [Required]
        public string RequesterPhone { get; set; }

        public bool MemberStatus { get; set; }

        [Range(1, 180, ErrorMessage = "Guest count must be between 1 and 180.")]
        public int GuestCount { get; set; }

        public bool RulesAgreed { get; set; }

        public bool KitchenUsage { get; set; }

        public DateTime RequestedDate { get; set; }

        public string Status { get; set; } // e.g., Pending, Approved, Denied

        public decimal CalculatedRate { get; set; }
    }
}
