// [NEW]
using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class ProtectedDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ContentType { get; set; } = string.Empty;

        [Required]
        [StringLength(1024)]
        public string FilePath { get; set; } = string.Empty; // Relative path to the stored file

        [Required]
        [StringLength(50)]
        public string Visibility { get; set; } = string.Empty; // e.g., "Public", "MembersOnly"

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
