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
        public virtual StudioPage StudioPage { get; set; }

        [StringLength(70)]
        public string MetaTitle { get; set; }

        [StringLength(160)]
        public string MetaDescription { get; set; }

        [StringLength(1024)]
        public string OpenGraphImageUrl { get; set; }
    }
}
