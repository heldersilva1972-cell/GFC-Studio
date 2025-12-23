// [NEW]
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    /// <summary>
    /// Represents a section within a Studio page.
    /// </summary>
    public class StudioSection
    {
        [Key]
        public int Id { get; set; }

        public Guid ClientId { get; set; } = Guid.NewGuid();

        [Required]
        public int PageIndex { get; set; }

        public string Content { get; set; }

        public string AnimationSettings { get; set; }

        public int StudioPageId { get; set; }
        public StudioPage StudioPage { get; set; }
    }
}
