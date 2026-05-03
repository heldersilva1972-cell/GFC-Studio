// [NEW]
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class Form
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        // Navigation property for the fields in this form
        public virtual ICollection<FormField> FormFields { get; set; } = new List<FormField>();

        // For routing submissions
        [StringLength(100)]
        public string SubmissionTarget { get; set; } = string.Empty; // e.g., "RentalInquiries", "ContactSubmissions"
    }
}
