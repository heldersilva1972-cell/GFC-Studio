using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class ReportingApiKey
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string KeyHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string KeyPrefix { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Label { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? AllowedDatasets { get; set; } // Comma-separated list of dataset keys, or null/empty for ALL

        [MaxLength(100)]
        public string? AllowedIpAddress { get; set; }

        [MaxLength(100)]
        public string? BoundDeviceName { get; set; }

        public int CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public AppUser? CreatedByUser { get; set; }

        public int? AssignedUserId { get; set; }

        [ForeignKey("AssignedUserId")]
        public AppUser? AssignedUser { get; set; }

        public string? PendingClaimRawKey { get; set; } // Available only until the user views/claims it

        public DateTime? ClaimedAtUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? ExpiresAtUtc { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? LastUsedAtUtc { get; set; }

        public DateTime? RevokedAtUtc { get; set; }

        public int? RevokedByUserId { get; set; }

        [MaxLength(255)]
        public string? RevokedReason { get; set; }
    }
}
