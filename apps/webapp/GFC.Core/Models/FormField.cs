// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class FormField
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FormId { get; set; }

        [ForeignKey("FormId")]
        public virtual Form Form { get; set; } = default!;

        [Required]
        [StringLength(100)]
        public string FieldType { get; set; } = string.Empty; // e.g., "text", "email", "date", "textarea"

        [Required]
        [StringLength(100)]
        public string Label { get; set; } = string.Empty;

        public string Placeholder { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        public int Order { get; set; }

        public string Options { get; set; } = string.Empty; // Comma-separated for dropdowns
    }
}
