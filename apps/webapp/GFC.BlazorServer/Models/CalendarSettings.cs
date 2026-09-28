using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.BlazorServer.Models
{
    /// <summary>
    /// Settings that control the appearance and behaviour of the rental calendar.
    /// Stored in the database and edited via the Settings UI.
    /// </summary>
    public class CalendarSettings
    {
        [Key]
        public int Id { get; set; } = 1; // singleton row

        /// <summary>
        /// Primary colour (hex) used for event backgrounds.
        /// </summary>
        [Required]
        public string PrimaryColor { get; set; } = "#C49A49"; // dark‑gold theme

        /// <summary>
        /// Text colour (hex) for event titles.
        /// </summary>
        [Required]
        public string TextColor { get; set; } = "#FFFFFF";

        /// <summary>
        /// Default view: "dayGridMonth", "timeGridWeek", etc.
        /// </summary>
        [Required]
        public string DefaultView { get; set; } = "dayGridMonth";

        /// <summary>
        /// Whether to show a loading spinner while events are fetched.
        /// </summary>
        public bool ShowLoadingSpinner { get; set; } = true;
    }
}
