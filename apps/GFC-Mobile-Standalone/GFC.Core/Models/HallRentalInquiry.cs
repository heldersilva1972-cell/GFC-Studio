// [NEW]
using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    /// <summary>
    /// Represents an incomplete Hall Rental application that has been saved for later.
    /// </summary>
    public class HallRentalInquiry
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// A unique, non-guessable token used to generate the "magic link" for resuming the application.
        /// </summary>
        [Required]
        [StringLength(128)]
        public string ResumeToken { get; set; }

        /// <summary>
        /// A JSON serialized string representing the data the user has entered so far.
        /// </summary>
        [Required]
        public string FormData { get; set; }

        /// <summary>
        /// The date and time when this inquiry was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date and time when this inquiry and its resume link should expire.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
