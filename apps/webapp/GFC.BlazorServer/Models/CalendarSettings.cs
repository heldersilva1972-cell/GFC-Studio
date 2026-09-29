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
        /// Primary colour (hex) used for event backgrounds and buttons (default: GFC Gold).
        /// </summary>
        [Required]
        public string PrimaryColor { get; set; } = "#C49A49";

        /// <summary>
        /// Text colour (hex) for event titles.
        /// </summary>
        [Required]
        public string TextColor { get; set; } = "#FFFFFF";

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

        /* ── Header Top Buttons (Configured Independently) ─────────────────── */
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
                new() { FieldKey = "time_window", Label = "Reserved Time Window", Description = "Start and end time (e.g., 2:00 PM - 7:00 PM)", IsEnabled = true, Order = 1 },
                new() { FieldKey = "event_title", Label = "Event Title / Designation", Description = "Event title or rental designation", IsEnabled = true, Order = 2 },
                new() { FieldKey = "room_location", Label = "Room / Facility Space", Description = "Main Function Hall, Coalition Room, etc.", IsEnabled = true, Order = 3 },
                new() { FieldKey = "booking_status", Label = "Booking Status Badge (Pending / Reserved)", Description = "Dynamic PENDING / RESERVED status pill", IsEnabled = true, Order = 4 },
                new() { FieldKey = "public_notes", Label = "Public Description / Note", Description = "Event details or public summary", IsEnabled = false, Order = 5 }
            };
        }
    }
}
