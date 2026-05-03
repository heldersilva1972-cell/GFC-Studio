// [VERIFIED FIX]
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace GFC.Core.Models
{
    public class MediaAsset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty; // Original file name

        [Required]
        [StringLength(255)]
        public string StoredFileName { get; set; } = string.Empty; // Actual file name on disk

        [Required]
        [StringLength(100)]
        public string ContentType { get; set; } = string.Empty;

        // [Fix] Schema has BOTH 'Size' and 'FileSize' as bigint NOT NULL. 
        // We map both to ensure inserts succeed regardless of which one is legacy.
        [Column("Size")]
        public long Size { get; set; }

        [Column("FileSize")]
        public long FileSize { get; set; }

        [Required]
        public string Url { get; set; } = string.Empty;

        // [Fix] Schema has both CreatedAt and UploadedAt as NOT NULL
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        
        // [Fix] Schema has UploadedBy (nvarchar(100), NOT NULL). Must be mapped and required.
        [Required]
        [StringLength(100)]
        public string UploadedBy { get; set; } = string.Empty;

        // [Fix] Schema has Tag (nvarchar(100), NULL). Should be mapped.
        [StringLength(100)]
        public string? Tag { get; set; }

        [Required]
        public string Usage { get; set; } = string.Empty; 

        [NotMapped]
        public string FilePath 
        { 
            get => $"/uploads/{StoredFileName}";
            set { /* Compatibility with older code */ }
        }

        public int? AssetFolderId { get; set; }

        [ForeignKey("AssetFolderId")]
        public virtual AssetFolder AssetFolder { get; set; } = default!;

        // Mapped in DbContext but potentially missing from DB. 
        // We will keep it but STOP the service from trying to save settings into it.
        public virtual ICollection<MediaRendition> Renditions { get; set; } = new List<MediaRendition>();

        [StringLength(100)]
        public string? RequiredRole { get; set; }
    }
}
