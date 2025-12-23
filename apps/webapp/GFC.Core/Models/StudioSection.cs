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
        public string Title { get; set; }

        public string Content { get; set; }

        public int PageIndex { get; set; }

        public string AnimationSettings { get; set; }

        [ForeignKey("StudioPage")]
        public int StudioPageId { get; set; }
        public virtual StudioPage StudioPage { get; set; }
    }
}
