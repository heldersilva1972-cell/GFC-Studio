using System;

namespace GFC.Core.DTOs
{
    public class UnavailableDateDto
    {
        public DateTime Date { get; set; }
        public string Status { get; set; } // "Booked", "Pending", "Blackout"
        public string? EventType { get; set; } // e.g., "Wedding", "Birthday Party", etc.
        public string? EventTime { get; set; } // e.g., "2:00 PM - 10:00 PM"
    }
}
