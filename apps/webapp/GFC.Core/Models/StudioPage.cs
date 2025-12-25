// [MODIFIED]
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class StudioPage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Slug { get; set; }

        public bool IsPublished { get; set; }

        public ICollection<StudioSection> Sections { get; set; } = new List<StudioSection>();

        public ICollection<StudioDraft> Drafts { get; set; } = new List<StudioDraft>();
    }
}
