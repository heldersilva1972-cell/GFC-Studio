// [NEW]
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    /// <summary>
    /// Represents a page created in the GFC Studio.
    /// </summary>
    public class StudioPage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public ICollection<StudioSection> Sections { get; set; } = new List<StudioSection>();
    }
}
