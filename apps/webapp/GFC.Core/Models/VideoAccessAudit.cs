// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class VideoAccessAudit
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        [Required]
        [StringLength(50)]
        public string AccessType { get; set; }

        public int? CameraId { get; set; }

        [ForeignKey("CameraId")]
        public Camera Camera { get; set; }

        [StringLength(255)]
        public string CameraName { get; set; }

        [Required]
        [StringLength(50)]
        public string ConnectionType { get; set; }

        [Required]
        [StringLength(50)]
        public string ClientIP { get; set; }

        public DateTime SessionStart { get; set; } = DateTime.UtcNow;

        public DateTime? SessionEnd { get; set; }

        public int? DurationSeconds { get; set; }

        [StringLength(500)]
        public string RecordingFile { get; set; }

        public string Notes { get; set; }
    }
}
