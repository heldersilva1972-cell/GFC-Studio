// [MODIFIED]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioSection
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("StudioPage")]
        public int PageId { get; set; }
        public virtual StudioPage StudioPage { get; set; }

        [Required]
        [StringLength(100)]
        public string ComponentType { get; set; } = string.Empty;

        public int OrderIndex { get; set; } = 0;

        [Required]
        public string ContentJson { get; set; } = "{}";

        public string? StylesJson { get; set; }

        public string? AnimationJson { get; set; }

        public string? ResponsiveJson { get; set; }

        public bool IsVisible { get; set; } = true;

        public bool VisibleOnDesktop { get; set; } = true;

        public bool VisibleOnTablet { get; set; } = true;

        public bool VisibleOnMobile { get; set; } = true;

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
