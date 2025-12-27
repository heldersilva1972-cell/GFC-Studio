// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    /// <summary>
    /// Represents a dynamic form definition, stored as a JSON schema.
    /// </summary>
    public class DynamicForm
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// A unique name to identify the form, e.g., "hall-rental" or "contact-us".
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// The JSON schema that defines the form's structure, fields, validation, and conditional logic.
        /// </summary>
        [Required]
        public string SchemaJson { get; set; }
    }
}
