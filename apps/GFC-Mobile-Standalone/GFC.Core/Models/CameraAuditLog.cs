// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class CameraAuditLog
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public int CameraId { get; set; }

        [ForeignKey("CameraId")]
        public Camera Camera { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [StringLength(1000)]
        public string Details { get; set; }
    }
}
