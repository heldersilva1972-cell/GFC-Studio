// [NEW]
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class StudioPage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsPublished { get; set; }

        public virtual ICollection<StudioSection> Sections { get; set; } = new List<StudioSection>();
    }
}
