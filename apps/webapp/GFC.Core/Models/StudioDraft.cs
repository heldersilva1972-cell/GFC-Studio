using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioDraft
    {
        [Key]
        public int Id { get; set; }

        public int StudioPageId { get; set; } // Renamed from PageId to match common pattern if needed, but keeping PageId mapping valid

        [ForeignKey("StudioPageId")]
        public StudioPage StudioPage { get; set; }

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
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public int Version { get; set; }
        
        public bool IsPublished { get; set; }
        
        public DateTime? PublishedAt { get; set; }
    }
}
