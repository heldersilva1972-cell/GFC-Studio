// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class VpnProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        [Required]
        [MaxLength(255)]
        public string PublicKey { get; set; }

        [Required]
        [MaxLength(255)]
        public string PrivateKey { get; set; } // Encrypted at rest

        [Required]
        [MaxLength(50)]
        public string AssignedIP { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastUsedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public int? RevokedBy { get; set; }

        [ForeignKey("RevokedBy")]
        public AppUser RevokedByUser { get; set; }

        [MaxLength(500)]
        public string RevokedReason { get; set; }

        [MaxLength(255)]
        public string DeviceName { get; set; }

        [MaxLength(50)]
        public string DeviceType { get; set; }
    }
}
