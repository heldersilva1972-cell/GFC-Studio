// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioSectionAsset
    {
        [Key]
        public int Id { get; set; }

        public int StudioSectionId { get; set; }
        [ForeignKey("StudioSectionId")]
        public virtual StudioSection StudioSection { get; set; }

        public int MediaAssetId { get; set; }
        [ForeignKey("MediaAssetId")]
        public virtual MediaAsset MediaAsset { get; set; }
    }
}
