// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("VpnProfiles")]
    public class VpnProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [Required]
        [StringLength(255)]
        public string PublicKey { get; set; }

        [Required]
        [StringLength(255)]
        public string PrivateKey { get; set; }

        [Required]
        [StringLength(50)]
        public string AssignedIP { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastUsedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public int? RevokedBy { get; set; }

        [ForeignKey("RevokedBy")]
        public virtual AppUser RevokedByUser { get; set; }

        [StringLength(500)]
        public string RevokedReason { get; set; }

        [StringLength(255)]
        public string DeviceName { get; set; }

        [StringLength(50)]
        public string DeviceType { get; set; }
    }
}
