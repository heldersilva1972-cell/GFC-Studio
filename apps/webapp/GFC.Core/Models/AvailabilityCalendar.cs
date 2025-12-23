// [NEW]
using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class AvailabilityCalendar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Status { get; set; } // e.g., Available, Booked, Pending
    }
}
