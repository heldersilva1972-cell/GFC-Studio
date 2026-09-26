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

    public class CalendarFieldDisplayConfig
    {
        public string FieldKey { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public int Order { get; set; } = 0;
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

        // Privacy & Field Visibility Options for Google Calendar Events
        public bool PushApplicantNameToTitle { get; set; } = true;
        public bool PushApplicantNameToDescription { get; set; } = true;
        public bool PushApplicantPhoneToDescription { get; set; } = false;
        public bool PushApplicantEmailToDescription { get; set; } = false;
        public bool PushPricingQuoteToDescription { get; set; } = false;
        public bool PushGuestCountToDescription { get; set; } = true;
        public bool PushServicesToDescription { get; set; } = true;
        public bool PushAddressToDescription { get; set; } = false;
        public bool PushNotesToDescription { get; set; } = true;
        public string GoogleEventVisibility { get; set; } = "default"; // "default", "private", "public"

        // Configurable & Ordered Title Fields for Google Calendar (Pipe '|' delimited)
        public List<CalendarFieldDisplayConfig> EventTitleFields { get; set; } = GetDefaultGoogleTitleFields();

        public static List<CalendarFieldDisplayConfig> GetDefaultGoogleTitleFields()
        {
            return new List<CalendarFieldDisplayConfig>
            {
                new() { FieldKey = "applicant_name", Label = "Applicant / Organization Name", Description = "Full name of applicant or organization", IsEnabled = true, Order = 1 },
                new() { FieldKey = "event_type", Label = "Event Type / Occasion", Description = "Category or occasion of the celebration", IsEnabled = true, Order = 2 },
                new() { FieldKey = "room_location", Label = "Room / Facility Space", Description = "Selected rental room or hall space", IsEnabled = true, Order = 3 },
                new() { FieldKey = "time_window", Label = "Reserved Time Window", Description = "Event start and end time", IsEnabled = false, Order = 4 },
                new() { FieldKey = "services", Label = "Add-on Services & Amenities", Description = "Requested services (Bar, Kitchen, A/V)", IsEnabled = true, Order = 5 },
                new() { FieldKey = "guest_count", Label = "Guest Attendance Count", Description = "Number of anticipated attendees", IsEnabled = false, Order = 6 },
                new() { FieldKey = "pricing_quote", Label = "Total Price / Financial Quote", Description = "Total rental price quote ($)", IsEnabled = false, Order = 7 },
                new() { FieldKey = "contact_phone", Label = "Contact Phone Number", Description = "Applicant telephone number", IsEnabled = false, Order = 8 },
                new() { FieldKey = "contact_email", Label = "Contact Email Address", Description = "Applicant email address", IsEnabled = false, Order = 9 }
            };
        }

        // Granular Add-on Services Visibility & Custom Display Names for Google Calendar
        public bool PushBarServiceToDescription { get; set; } = true;
        public string BarServiceCalendarDisplayName { get; set; } = "Bar / Bartender Service";

        public bool PushKitchenUsageToDescription { get; set; } = true;
        public string KitchenUsageCalendarDisplayName { get; set; } = "Kitchen Access";

        public bool PushAvEquipmentToDescription { get; set; } = true;
        public string AvEquipmentCalendarDisplayName { get; set; } = "A/V Equipment";
    }
}

