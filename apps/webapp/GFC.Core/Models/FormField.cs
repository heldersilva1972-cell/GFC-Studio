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
        [StringLength(50)]
        public string FieldType { get; set; } // e.g., "text", "email", "date", "dropdown"

        [Required]
        [StringLength(100)]
        public string Label { get; set; }

        public string Options { get; set; } // For dropdowns, radios, etc. (e.g., JSON or CSV)

        public bool IsRequired { get; set; }

        [StringLength(200)]
        public string ValidationPattern { get; set; } // Regex for validation

        public int Order { get; set; }
    }
}
