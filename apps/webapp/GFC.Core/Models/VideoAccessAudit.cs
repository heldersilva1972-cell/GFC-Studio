// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("VideoAccessAudit")]
    public class VideoAccessAudit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string AccessType { get; set; } = string.Empty; // 'LiveView', 'Recording', 'Download', 'Snapshot'

        public int? CameraId { get; set; }

        [ForeignKey("CameraId")]
        public Camera Camera { get; set; } = default!;

        [MaxLength(255)]
        public string CameraName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string ConnectionType { get; set; } = string.Empty; // 'LAN', 'VPN', 'Blocked'

        [Required]
        [MaxLength(50)]
        public string ClientIP { get; set; } = string.Empty;

        public DateTime SessionStart { get; set; } = DateTime.UtcNow;

        public DateTime? SessionEnd { get; set; }

        public int? DurationSeconds { get; set; }

        [MaxLength(500)]
        public string RecordingFile { get; set; } = string.Empty; // If downloaded

        public string Notes { get; set; } = string.Empty;
    }
}
