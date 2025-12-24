// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class MediaAsset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(100)]
        public string FileType { get; set; } // e.g., "image/jpeg", "video/mp4"

        public long FileSize { get; set; } // in bytes

        [Required]
        public string FilePath { get; set; } // Relative path to the original file

        // For responsive image variants
        public string FilePath_xl { get; set; }
        public string FilePath_lg { get; set; }
        public string FilePath_md { get; set; }
        public string FilePath_sm { get; set; }

        public int? AssetFolderId { get; set; }

        [ForeignKey("AssetFolderId")]
        public virtual AssetFolder AssetFolder { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
