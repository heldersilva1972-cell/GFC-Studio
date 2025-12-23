// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioSection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public int Order { get; set; }

        [ForeignKey("StudioPage")]
        public int StudioPageId { get; set; }
        public virtual StudioPage StudioPage { get; set; }
    }
}
