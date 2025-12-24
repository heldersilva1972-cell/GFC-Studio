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

        [ForeignKey("StudioPage")]
        public int PageId { get; set; }

        public int Version { get; set; }

        [Required]
        public string ContentJson { get; set; } = "[]";

        [StringLength(500)]
        public string? ChangeDescription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = string.Empty;
    }
}
