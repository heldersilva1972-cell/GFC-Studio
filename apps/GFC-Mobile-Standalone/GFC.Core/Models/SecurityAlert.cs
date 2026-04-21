// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("SecurityAlerts")]
    public class SecurityAlert
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string AlertType { get; set; } // 'FailedLogin', 'UnusualLocation', 'ExpiredAccess', etc.

        [Required]
        [MaxLength(20)]
        public string Severity { get; set; } // 'Info', 'Warning', 'Critical'

        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        [MaxLength(255)]
        public string Username { get; set; }

        [MaxLength(50)]
        public string ClientIP { get; set; }

        public string Details { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReviewedAt { get; set; }

        public int? ReviewedBy { get; set; }

        [ForeignKey("ReviewedBy")]
        public AppUser ReviewedByUser { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "New"; // 'New', 'Reviewed', 'Resolved', 'Dismissed'
    }
}
