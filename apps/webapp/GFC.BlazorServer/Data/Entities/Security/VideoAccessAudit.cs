// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GFC.Core.Models;

namespace GFC.BlazorServer.Data.Entities.Security
{
    [Table("VideoAccessAudit")]
    public class VideoAccessAudit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        [Required]
        [MaxLength(50)]
        public string AccessType { get; set; } // 'LiveView', 'Recording', 'Download', 'Snapshot'

        public int? CameraId { get; set; }

        [ForeignKey("CameraId")]
        public Camera Camera { get; set; }

        [MaxLength(255)]
        public string CameraName { get; set; }

        [Required]
        [MaxLength(50)]
        public string ConnectionType { get; set; } // 'LAN', 'VPN', 'Blocked'

        [Required]
        [MaxLength(50)]
        public string ClientIP { get; set; }

        public DateTime SessionStart { get; set; } = DateTime.UtcNow;

        public DateTime? SessionEnd { get; set; }

        public int? DurationSeconds { get; set; }

        [MaxLength(500)]
        public string RecordingFile { get; set; } // If downloaded

        public string Notes { get; set; }
    }
}
