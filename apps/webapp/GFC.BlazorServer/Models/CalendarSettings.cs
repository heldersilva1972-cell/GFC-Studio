using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GFC.Core.Models;

namespace GFC.BlazorServer.Models
{
    /// <summary>
    /// Settings that control the appearance and behaviour of the public-facing rental calendar.
    /// Stored as JSON in App_Data/calendar_ui_settings.json and edited via HallRentalSettings.
    /// </summary>
    public class CalendarSettings
    {
        [Key]
        public int Id { get; set; } = 1;

        /// <summary>
        /// Primary colour (hex) used for standard rental event backgrounds and primary calendar accents (default: GFC Gold).
        /// </summary>
        [Required]
        public string PrimaryColor { get; set; } = "#C49A49";

        /// <summary>
        /// Text colour (hex) for standard rental event titles and cards.
        /// </summary>
        [Required]
        public string TextColor { get; set; } = "#FFFFFF";

        /* ── Event Color Coding Rules ────────────────────────────────────── */
        /// <summary>
        /// Whether to enable distinct custom colors for internal Club Events and Secondary Space meetings on the public calendar.
        /// </summary>
        public bool ColorCodeClubEvents { get; set; } = true;

        /// <summary>
        /// Background color for internal Club Events (Function Hall).
        /// </summary>
        public string ClubEventColor { get; set; } = "#7c3aed";

        /// <summary>
        /// Text color for internal Club Events.
        /// </summary>
        public string ClubEventTextColor { get; set; } = "#FFFFFF";

        /// <summary>
        /// Background color for Secondary Space meetings/events (e.g. Office, Directors Room).
        /// </summary>
        public string SecondarySpaceEventColor { get; set; } = "#0284c7";

        /// <summary>
        /// Text color for Secondary Space meetings/events.
        /// </summary>
        public string SecondarySpaceEventTextColor { get; set; } = "#FFFFFF";

        /// <summary>
        /// Default view: "dayGridMonth", "listMonth".
        /// </summary>
        [Required]
        public string DefaultView { get; set; } = "dayGridMonth";

        /// <summary>
        /// Whether to show a loading spinner while events are fetched.
        /// </summary>
        public bool ShowLoadingSpinner { get; set; } = true;

        /// <summary>
        /// Display mode for day grid cells: "multiline" (expanded cards) or "compact" (single-line badge).
        /// </summary>
        public string MonthDisplayMode { get; set; } = "multiline";

        /// <summary>
        /// Configurable and re-orderable fields to display inside the month grid event cards.
        /// </summary>
        public List<CalendarFieldDisplayConfig> MonthCalendarFields { get; set; } = GetDefaultPublicMonthCalendarFields();

        /// <summary>
        /// Whether to show unapproved/pending applications on the public calendar.
        /// </summary>
        public bool ShowPendingApplications { get; set; } = true;

        /// <summary>
        /// Custom badge text displayed for unapproved / pending applications.
        /// </summary>
        public string PendingBadgeText { get; set; } = "PENDING";

        /// <summary>
        /// Custom badge text displayed for approved / confirmed bookings.
        /// </summary>
        public string ApprovedBadgeText { get; set; } = "RESERVED";

        /// <summary>
        /// Whether to show the PENDING pill badge on unapproved events.
        /// </summary>
        public bool ShowPendingBadge { get; set; } = true;

        /// <summary>
        /// Whether to show the RESERVED pill badge on approved events.
        /// </summary>
        public bool ShowApprovedBadge { get; set; } = true;

        /* ── Partial Day Availability Pill Settings ──────────────────────── */
        /// <summary>
        /// Whether to show a green "Slot Open" / "Partial Day Available" pill on dates with existing bookings that still have open rental slots.
        /// </summary>
        public bool ShowPartialAvailabilityBadge { get; set; } = true;

        /// <summary>
        /// Custom text template for the partial availability badge (e.g. "Slot Open" or "{count} Slot Open").
        /// </summary>
        public string PartialAvailabilityBadgeText { get; set; } = "Slot Open";

        /* ── Header Top Notice Banner (Configured Independently) ──────────── */
        /// <summary>
        /// Whether to show the introductory notification/notice banner at the top of the calendar.
        /// </summary>
        public bool ShowHeaderNoticeBanner { get; set; } = true;

        /// <summary>
        /// The message text displayed in the header notice banner at the top of the calendar.
        /// </summary>
        public string HeaderNoticeBannerText { get; set; } =
            "Thank you for visiting our Calendar of Events. You can view from this calendar which dates are potentially available for your event.\n" +
            "If you see an available date that fits your needs, please fill out an online application and someone will get back to you to confirm the date.";

        /* ── Header Top Buttons (Configured Independently) ─────────────────── */
        /// <summary>
        /// Whether to show the GFC Home button in the header at the top of the calendar.
        /// </summary>
        public bool ShowHeaderHomeButton { get; set; } = true;
        public string HeaderHomeButtonText { get; set; } = "GFC Home";
        public string HeaderHomeButtonUrl { get; set; } = "https://gloucesterfraternityclub.com";

        /// <summary>
        /// Whether to show the Home button on mobile view / phone preview bar.
        /// </summary>
        public bool ShowMobileHomeButton { get; set; } = true;

        /// <summary>
        /// Whether to show the Book the Hall CTA button in the header at the top of the calendar.
        /// </summary>
        public bool ShowHeaderBookButton { get; set; } = true;
        public string HeaderBookButtonText { get; set; } = "Book the Hall";
        public string HeaderBookButtonUrl { get; set; } = "/rentals/apply";

        /// <summary>
        /// Whether to show the "Ask a Question" button in the header at the top of the calendar.
        /// </summary>
        public bool ShowHeaderQuestionButton { get; set; } = true;
        public string HeaderQuestionButtonText { get; set; } = "Ask a Question";
        public string HeaderQuestionButtonUrl { get; set; } = "/rentals/apply?mode=inquiry";

        /* ── Modal Popup Action Buttons (Configured Independently) ───────── */
        /// <summary>
        /// Whether to show the booking button inside the event detail popup modal.
        /// </summary>
        public bool ShowModalBookButton { get; set; } = true;
        public string ModalBookButtonText { get; set; } = "Book this Hall";
        public string ModalBookButtonUrl { get; set; } = "/rentals/apply";

        /// <summary>
        /// Whether to show the "Ask a Question" button inside the event detail popup modal.
        /// </summary>
        public bool ShowModalQuestionButton { get; set; } = true;
        public string ModalQuestionButtonText { get; set; } = "Ask a Question";
        public string ModalQuestionButtonUrl { get; set; } = "/rentals/apply?mode=inquiry";

        /* Legacy compatibility properties */
        public bool ShowCtaButton
        {
            get => ShowHeaderBookButton;
            set => ShowHeaderBookButton = value;
        }
        public string CtaButtonText
        {
            get => HeaderBookButtonText;
            set => HeaderBookButtonText = value;
        }
        public string CtaButtonUrl
        {
            get => HeaderBookButtonUrl;
            set => HeaderBookButtonUrl = value;
        }

        /* ── Event Detail Popup Modal Display Options ────────────────────── */
        /// <summary>
        /// Whether to show the event detail popup modal on click.
        /// </summary>
        public bool ShowEventModal { get; set; } = true;

        public bool ShowModalTitle { get; set; } = true;
        public bool ShowModalTime { get; set; } = true;
        public bool ShowModalLocation { get; set; } = true;
        public bool ShowModalStatus { get; set; } = true;
        public bool ShowModalDescription { get; set; } = true;
        public bool ShowModalSource { get; set; } = false;

        /* ── Mobile Calendar Specific Preferences ────────────────────────── */
        /// <summary>
        /// Display mode on mobile devices: "drawer" (Mini-Month Grid + Selected Day Drawer) or "standard" (Desktop-style grid).
        /// </summary>
        public string MobileDisplayMode { get; set; } = "drawer";

        /// <summary>
        /// Show available slot booking action button when a date has remaining availability on mobile.
        /// </summary>
        public bool MobileShowBookAvailableSlot { get; set; } = true;
        public string MobileBookAvailableSlotText { get; set; } = "Book Available Slot";

        /// <summary>
        /// Show question action button when a date has remaining availability on mobile.
        /// </summary>
        public bool MobileShowQuestionAvailableSlot { get; set; } = true;
        public string MobileQuestionAvailableSlotText { get; set; } = "Ask a Question";

        /// <summary>
        /// Text shown when a date has open availability.
        /// </summary>
        public string MobileOpenSlotMessage { get; set; } = "Remaining time slot(s) are available for booking on this date!";

        public static List<CalendarFieldDisplayConfig> GetDefaultPublicMonthCalendarFields()
        {
            return new List<CalendarFieldDisplayConfig>
            {
                new() { FieldKey = "time_window", Label = "Reserved Time Window", Description = "Start and end time (e.g., 2 PM - 7 PM)", IsEnabled = true, Order = 1 },
                new() { FieldKey = "applicant_name", Label = "Applicant / Organization Name", Description = "Full name of applicant or organization", IsEnabled = true, Order = 2 },
                new() { FieldKey = "event_type", Label = "Event Type / Occasion", Description = "Category or occasion of celebration", IsEnabled = true, Order = 3 },
                new() { FieldKey = "room_location", Label = "Room / Facility Space", Description = "Main Function Hall, Coalition Room, etc.", IsEnabled = true, Order = 4 },
                new() { FieldKey = "booking_status", Label = "Booking Status Badge (Pending / Reserved)", Description = "Dynamic PENDING / RESERVED status pill", IsEnabled = true, Order = 5 },
                new() { FieldKey = "public_notes", Label = "Public Description / Note", Description = "Event details or public summary", IsEnabled = false, Order = 6 }
            };
        }

        public static List<CalendarColorPreset> GetDefaultColorPresets()
        {
            return new List<CalendarColorPreset>
            {
                new("Warm Gold, Soft Lilac & Sky Mist", "#FEF3C7", "#92400E", "#F3E8FF", "#6B21A8", "#E0F2FE", "#0369A1", "Soft Gold tint, Translucent Lilac, Light Sky Blue"),
                new("Cream Amber, Lavender & Mint", "#FFFBEB", "#B45309", "#EDE9FE", "#5B21B6", "#D1FAE5", "#065F46", "Cream Amber, Soft Lavender, Gentle Mint Green"),
                new("Peach Blossom & Ice Blue", "#FFEDD5", "#9A3412", "#E0E7FF", "#3730A3", "#CFFAFE", "#155E75", "Translucent Peach, Light Periwinkle, Ice Cyan"),
                new("Soft Rose, Orchid & Cloud Blue", "#FFE4E6", "#9F1239", "#FAE8FF", "#86198F", "#E0F2FE", "#075985", "Soft Rose tint, Light Orchid, Clear Cloud Blue"),
                new("Buttercup Gold & Soft Slate", "#FEF9C3", "#854D0E", "#F1F5F9", "#334155", "#DBEAFE", "#1E40AF", "Light Buttercup, Clean Slate, Soft Azure"),
                new("Seafoam Teal, Cotton Candy & Honey", "#CCFBF1", "#115E59", "#FCE7F3", "#9D174D", "#FEF3C7", "#92400E", "Soft Seafoam, Light Pastel Pink, Gentle Honey"),
                new("Translucent Sage & Soft Violet", "#DCFCE7", "#166534", "#F3E8FF", "#7E22CE", "#E0F2FE", "#0284C7", "Translucent Sage Green, Soft Violet, Crisp Light Sky"),
                new("Apricot Glow & Soft Indigo", "#FFEDD5", "#C2410C", "#EEF2FF", "#4338CA", "#E0F2FE", "#0369A1", "Light Apricot, Translucent Indigo, Cool Blue"),
                new("Muted Champagne & Heather Purple", "#F5EFE6", "#7A5C1E", "#F5F3FF", "#6D28D9", "#E0F2FE", "#0284C7", "Champagne Gold, Soft Heather Purple, Light Azure"),
                new("Clean Sky Cyan & Soft Blush", "#E0F2FE", "#0369A1", "#FCE7F3", "#BE185D", "#FEF9C3", "#854D0E", "Translucent Sky Cyan, Soft Blush, Light Honey")
            };
        }

        public static List<ColorSwatchItem> GetStandardRentalSwatches()
        {
            return new List<ColorSwatchItem>
            {
                new("Translucent Gold", "#FEF3C7", "#92400E"),
                new("Cream Amber", "#FFFBEB", "#B45309"),
                new("Soft Champagne", "#F5EFE6", "#7A5C1E"),
                new("Light Buttercup", "#FEF9C3", "#854D0E"),
                new("Translucent Sky", "#E0F2FE", "#0369A1"),
                new("Soft Ice Cyan", "#CFFAFE", "#155E75"),
                new("Light Seafoam", "#CCFBF1", "#115E59"),
                new("Translucent Sage", "#DCFCE7", "#166534"),
                new("Soft Peach", "#FFEDD5", "#9A3412"),
                new("Clean Slate Tint", "#F1F5F9", "#334155")
            };
        }

        public static List<ColorSwatchItem> GetClubEventSwatches()
        {
            return new List<ColorSwatchItem>
            {
                new("Translucent Lilac", "#F3E8FF", "#6B21A8"),
                new("Soft Lavender", "#EDE9FE", "#5B21B6"),
                new("Light Orchid", "#FAE8FF", "#86198F"),
                new("Pastel Violet", "#F5F3FF", "#6D28D9"),
                new("Soft Blush Pink", "#FCE7F3", "#9D174D"),
                new("Translucent Rose", "#FFE4E6", "#9F1239"),
                new("Light Amber Glow", "#FEF3C7", "#B45309"),
                new("Soft Periwinkle", "#E0E7FF", "#3730A3"),
                new("Light Emerald Tint", "#D1FAE5", "#065F46"),
                new("Pale Coral", "#FFEDD5", "#C2410C")
            };
        }

        public static List<ColorSwatchItem> GetSecondarySpaceSwatches()
        {
            return new List<ColorSwatchItem>
            {
                new("Translucent Sky Blue", "#E0F2FE", "#0369A1"),
                new("Light Ice Cyan", "#CFFAFE", "#155E75"),
                new("Soft Seafoam", "#CCFBF1", "#115E59"),
                new("Light Mint Green", "#D1FAE5", "#065F46"),
                new("Soft Indigo Tint", "#EEF2FF", "#4338CA"),
                new("Periwinkle Mist", "#E0E7FF", "#3730A3"),
                new("Clean Light Slate", "#F1F5F9", "#334155"),
                new("Light Cream Tint", "#FFFBEB", "#92400E"),
                new("Soft Lilac Tint", "#F3E8FF", "#7E22CE"),
                new("Gentle Sage Tint", "#DCFCE7", "#166534")
            };
        }
    }

    public class ColorSwatchItem
    {
        public string Name { get; set; }
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }

        public ColorSwatchItem(string name, string bg, string text)
        {
            Name = name;
            BackgroundColor = bg;
            TextColor = text;
        }
    }

    public class CalendarColorPreset
    {
        public string Name { get; set; }
        public string PrimaryColor { get; set; }
        public string TextColor { get; set; }
        public string ClubColor { get; set; }
        public string ClubTextColor { get; set; }
        public string SecondaryColor { get; set; }
        public string SecondaryTextColor { get; set; }
        public string Description { get; set; }

        public CalendarColorPreset(string name, string primary, string text, string club, string clubText, string secondary, string secondaryText, string desc)
        {
            Name = name;
            PrimaryColor = primary;
            TextColor = text;
            ClubColor = club;
            ClubTextColor = clubText;
            SecondaryColor = secondary;
            SecondaryTextColor = secondaryText;
            Description = desc;
        }
    }
}
