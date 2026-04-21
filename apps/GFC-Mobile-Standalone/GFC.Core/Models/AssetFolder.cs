// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class AssetFolder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int? ParentFolderId { get; set; }

        [ForeignKey("ParentFolderId")]
        public virtual AssetFolder ParentFolder { get; set; }

        public virtual ICollection<AssetFolder> SubFolders { get; set; } = new List<AssetFolder>();

        public virtual ICollection<MediaAsset> MediaAssets { get; set; } = new List<MediaAsset>();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
