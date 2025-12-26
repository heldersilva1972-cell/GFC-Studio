// [NEW]
using System;
using System.Collections.Generic;
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
        public string FileName { get; set; } = string.Empty; // Original name or unique name depending on usage

        [StringLength(255)]
        public string? StoredFileName { get; set; } // Unique name on the server

        [StringLength(255)]
        public string? FilePath { get; set; } // URL path

        [StringLength(100)]
        public string? Tag { get; set; }

        [StringLength(100)]
        public string? ContentType { get; set; }

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? UploadedBy { get; set; }

        public int? AssetFolderId { get; set; }

        [ForeignKey("AssetFolderId")]
        public virtual AssetFolder? AssetFolder { get; set; }

        // Navigation property for different versions (renditions) of the asset
        public virtual ICollection<MediaRendition> Renditions { get; set; } = new List<MediaRendition>();

        // For tracking where the image is used, e.g., "Home Page", "About Us"
        public string? Usage { get; set; }

        [StringLength(100)]
        public string? RequiredRole { get; set; }
    }
}
