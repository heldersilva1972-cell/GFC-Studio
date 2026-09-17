using System;

namespace GFC.Core.Models
{
    public class CalendarEventItem
    {
        public string Uid { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsAllDay { get; set; }
        public string Source { get; set; } = "Google Calendar"; // e.g. "Google Calendar", "Hall Booking", "Imported File"
        public string? Status { get; set; } = "CONFIRMED";
        public string ColorCategory { get; set; } = "badge-primary";
        public string? GoogleEventId { get; set; }
        public string? CalendarId { get; set; }
    }
}
