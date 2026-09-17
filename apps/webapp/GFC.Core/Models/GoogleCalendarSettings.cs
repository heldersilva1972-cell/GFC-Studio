using System;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class CalendarFeedConfig
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "Main Calendar";
        public string Url { get; set; } = string.Empty;
        public string Color { get; set; } = "#0d6efd"; // Hex color for calendar tags
        public bool IsEnabled { get; set; } = true;
        public DateTime? LastSyncedAt { get; set; }
        public int EventCount { get; set; } = 0;
        public string LastStatus { get; set; } = "Ready";
    }

    public class GoogleCalendarSettings
    {
        public int Id { get; set; } = 1;
        public string CalendarName { get; set; } = "Public Google Calendar";
        public string PublicCalendarUrl { get; set; } = string.Empty;
        public List<CalendarFeedConfig> Feeds { get; set; } = new();
        public bool AutoSyncEnabled { get; set; } = true;
        public int SyncIntervalMinutes { get; set; } = 30;
        public DateTime? LastSyncedAt { get; set; }
        public string LastSyncStatus { get; set; } = "Not synced yet";
        public bool LastSyncSuccess { get; set; } = false;
        public int TotalEventsCount { get; set; } = 0;

        // Google Service Account 2-Way Write / Update API Configuration
        public bool EnableWriteApi { get; set; } = false;
        public string ServiceAccountEmail { get; set; } = string.Empty;
        public string ServiceAccountPrivateKey { get; set; } = string.Empty;
        public string PrimaryGoogleCalendarId { get; set; } = string.Empty;
        public string ServiceAccountProjectNumber { get; set; } = string.Empty;
    }
}
