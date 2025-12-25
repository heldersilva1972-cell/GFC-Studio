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
        public virtual Form Form { get; set; }

        [Required]
        [StringLength(100)]
        public string FieldType { get; set; } // e.g., "text", "email", "date", "textarea"

        [Required]
        [StringLength(100)]
        public string Label { get; set; }

        public string Placeholder { get; set; }

        public bool IsRequired { get; set; }

        public int Order { get; set; }
    }
}
