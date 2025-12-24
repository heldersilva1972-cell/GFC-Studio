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
        public virtual StudioPage StudioPage { get; set; }

        public byte[]? ContentCompressed { get; set; }

        [NotMapped]
        public string? ContentJson { get; set; }

        public int Version { get; set; }

        public string? Name { get; set; }

        [Required]
        public string CreatedBy { get; set; } = "System";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
