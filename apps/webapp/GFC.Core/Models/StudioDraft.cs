// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioDraft
    {
        [Key]
        public int Id { get; set; } // Changed to int to match usage (StudioService uses FindAsync with int, Studio.razor uses draft.Id == 0)

        [Required]
        public int StudioPageId { get; set; }

        [ForeignKey("StudioPageId")]
        public StudioPage StudioPage { get; set; }

        public string ContentJson { get; set; } // Renamed from Payload

        public int Version { get; set; }

        public string CreatedBy { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedAt { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
