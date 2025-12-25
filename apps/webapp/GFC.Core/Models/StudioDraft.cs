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
        public int StudioPageId { get; set; }

        [ForeignKey("StudioPageId")]
        public virtual StudioPage StudioPage { get; set; }

        [Required]
        public string ContentSnapshotJson { get; set; } = "[]";

        // Alias for UI compatibility
        [NotMapped]
        public string ContentJson 
        { 
            get => ContentSnapshotJson; 
            set => ContentSnapshotJson = value; 
        }

        // Additional properties required by UI
        [NotMapped]
        public int PageId 
        { 
            get => StudioPageId; 
            set => StudioPageId = value; 
        }

        [StringLength(500)]
        public string? ChangeDescription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public int Version { get; set; }
        
        [StringLength(100)]
        public string? Name { get; set; }
        
        public bool IsPublished { get; set; }
        
        public DateTime? PublishedAt { get; set; }
    }
}
