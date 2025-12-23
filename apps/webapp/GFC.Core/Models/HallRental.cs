// [MODIFIED]
using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class HallRental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ApplicantName { get; set; }

        [Required]
        public string ContactInfo { get; set; }

        public DateTime EventDate { get; set; }

        [Required]
        public string Status { get; set; } // Pending/Approved/Denied/Completed

        [Range(1, 180, ErrorMessage = "Guest count must be between 1 and 180.")]
        public int GuestCount { get; set; }

        public bool KitchenUsed { get; set; }

        public decimal TotalPrice { get; set; }

        public string InternalNotes { get; set; }
    }
}
