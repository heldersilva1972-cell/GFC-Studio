// [MODIFIED]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioDraft
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PageId { get; set; }
        [ForeignKey("PageId")]
        public virtual StudioPage StudioPage { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        public string ContentJson { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ChangeDescription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = string.Empty;
    }
}
