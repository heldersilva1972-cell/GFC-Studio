using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class StudioPage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = "New Page";

        [Required]
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;

        [StringLength(200)]
        public string? MetaTitle { get; set; }

        [StringLength(500)]
        public string? MetaDescription { get; set; }

        [StringLength(500)]
        public string? OgImage { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Draft"; // Draft, Published, Archived

        public DateTime? PublishedAt { get; set; }

        [StringLength(100)]
        public string? PublishedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string UpdatedBy { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedAt { get; set; }

        [StringLength(100)]
        public string? DeletedBy { get; set; }

        public ICollection<StudioSection> Sections { get; set; } = new List<StudioSection>();

        public ICollection<StudioDraft> Drafts { get; set; } = new List<StudioDraft>();
    }
}
