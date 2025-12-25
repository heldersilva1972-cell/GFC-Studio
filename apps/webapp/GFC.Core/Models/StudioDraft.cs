using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioDraft
    {
        [Key]
        public int Id { get; set; }

        feature/gfc-studio-phase-1-668448862994436057
        [ForeignKey("StudioPage")]
        public int PageId { get; set; }

        public int StudioPageId { get; set; } // Renamed from PageId to match common pattern if needed, but keeping PageId mapping valid

        [ForeignKey("StudioPageId")]
        public StudioPage StudioPage { get; set; }
         master

        // Mapped column
        public string ContentSnapshotJson { get; set; }

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

        [Required]
       feature/gfc-studio-phase-1-668448862994436057
        public string ContentJson { get; set; } = "[]";

        [StringLength(500)]
        public string? ChangeDescription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public int Version { get; set; }
        
        public bool IsPublished { get; set; }
        
        public DateTime? PublishedAt { get; set; }
     master
    }
}
