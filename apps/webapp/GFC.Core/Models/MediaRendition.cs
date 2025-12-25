// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class MediaRendition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MediaAssetId { get; set; }

        [ForeignKey("MediaAssetId")]
        public virtual MediaAsset MediaAsset { get; set; }

        [Required]
        [StringLength(50)]
        public string RenditionType { get; set; } // e.g., "thumbnail", "medium", "large", "webp"

        [Required]
        [StringLength(1024)]
        public string Url { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
