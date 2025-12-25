// [MODIFIED]
using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class StudioTemplate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? ThumbnailUrl { get; set; }

        [Required]
        [StringLength(100)]
        public string ComponentType { get; set; } = string.Empty;

        [Required]
        public string ContentJson { get; set; } = "[]";

        public bool IsPublic { get; set; } = false;

        public int UsageCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
