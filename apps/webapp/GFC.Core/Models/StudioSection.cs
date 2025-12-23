// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioSection
    {
        [Key]
        public int Id { get; set; }

        public Guid ClientId { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = "New Section";

        public string? Content { get; set; }

        public int PageIndex { get; set; } = 0;

        public string? AnimationSettings { get; set; }

        [ForeignKey("StudioPage")]
        public int StudioPageId { get; set; }
        public virtual StudioPage StudioPage { get; set; }
    }
}
