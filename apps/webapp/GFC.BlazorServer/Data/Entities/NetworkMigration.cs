using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.BlazorServer.Data.Entities
{
    /// <summary>
    /// Tracks network configuration migrations for controllers
    /// </summary>
    public class NetworkMigration
    {
        [Key]
        public int Id { get; set; }

        public int ControllerId { get; set; }
        
        /// <summary>
        /// Type of migration: LAN_TO_VPN, VPN_TO_LAN, IP_CHANGE
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MigrationType { get; set; }

        /// <summary>
        /// Previous network type: LAN, VPN, Remote
        /// </summary>
        [MaxLength(50)]
        public string PreviousNetworkType { get; set; }

        /// <summary>
        /// Previous IP address
        /// </summary>
        [MaxLength(50)]
        public string PreviousIpAddress { get; set; }

        /// <summary>
        /// Previous port
        /// </summary>
        public int PreviousPort { get; set; }

        /// <summary>
        /// Previous VPN profile ID (if applicable)
        /// </summary>
        public int? PreviousVpnProfileId { get; set; }

        /// <summary>
        /// New network type: LAN, VPN, Remote
        /// </summary>
        [MaxLength(50)]
        public string NewNetworkType { get; set; }

        /// <summary>
        /// New IP address
        /// </summary>
        [MaxLength(50)]
        public string NewIpAddress { get; set; }

        /// <summary>
        /// New port
        /// </summary>
        public int NewPort { get; set; }

        /// <summary>
        /// New VPN profile ID (if applicable)
        /// </summary>
        public int? NewVpnProfileId { get; set; }

        /// <summary>
        /// When migration was initiated
        /// </summary>
        public DateTime InitiatedUtc { get; set; }

        /// <summary>
        /// When migration was completed
        /// </summary>
        public DateTime? CompletedUtc { get; set; }

        /// <summary>
        /// Migration status: Pending, InProgress, Completed, Failed, RolledBack
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        /// <summary>
        /// User who initiated the migration
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string InitiatedByUser { get; set; }

        /// <summary>
        /// Connection test results (JSON)
        /// </summary>
        public string TestResultsJson { get; set; }

        /// <summary>
        /// Error message if migration failed
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// When the backup configuration expires (for rollback)
        /// </summary>
        public DateTime? BackupExpiresUtc { get; set; }

        /// <summary>
        /// Whether this migration can be rolled back
        /// </summary>
        public bool CanRollback { get; set; }

        /// <summary>
        /// Notes or comments about the migration
        /// </summary>
        public string Notes { get; set; }

        // Navigation properties
        public virtual ControllerDevice Controller { get; set; }
    }
}
