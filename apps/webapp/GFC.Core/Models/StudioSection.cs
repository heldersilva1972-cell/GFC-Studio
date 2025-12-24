// [MODIFIED]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class StudioSection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudioPageId { get; set; }

        [ForeignKey("StudioPageId")]
        public StudioPage StudioPage { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = "New Section";

        public string Content { get; set; } = ""; // To store simple text content

        // Renamed from Type to avoid keyword conflict and match usage (sectionType)
        [Required]
        [StringLength(50)]
        public string sectionType { get; set; } = "RichTextBlock"; 

        [NotMapped] 
        public Guid ClientId { get; set; } = Guid.NewGuid();

        public int PageIndex { get; set; } // Renamed from OrderIndex to match usage

        // Properties dictionary for dynamic content (JSON serialized in DB)
        [NotMapped]
        public Dictionary<string, object> properties { get; set; } = new Dictionary<string, object>();
        
        public string PropertiesJson { get; set; } // Backing field for DB

        public string AnimationSettings { get; set; }
    }
}
