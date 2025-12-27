// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class FormSubmission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FormId { get; set; }

        [ForeignKey("FormId")]
        public virtual Form Form { get; set; }

        [Required]
        public string SubmissionData { get; set; } // JSON string of submitted data

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        [EmailAddress]
        public string SubmitterEmail { get; set; }

        [StringLength(50)]
        public string Status { get; set; } // e.g., "Complete", "Incomplete"

        [StringLength(128)]
        public string ResumeToken { get; set; }
    }
}
