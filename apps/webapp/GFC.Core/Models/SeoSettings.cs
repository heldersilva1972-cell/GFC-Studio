// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class SeoSettings
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudioPageId { get; set; }

        [ForeignKey("StudioPageId")]
        public virtual StudioPage StudioPage { get; set; } = default!;

        [StringLength(70)]
        public string MetaTitle { get; set; } = string.Empty;

        [StringLength(160)]
        public string MetaDescription { get; set; } = string.Empty;

        [StringLength(1024)]
        public string OpenGraphImageUrl { get; set; } = string.Empty;
    }
}
