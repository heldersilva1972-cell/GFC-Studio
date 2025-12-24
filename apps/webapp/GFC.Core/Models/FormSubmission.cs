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
        public string SubmittedData { get; set; } // JSON string of the submitted form data

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
