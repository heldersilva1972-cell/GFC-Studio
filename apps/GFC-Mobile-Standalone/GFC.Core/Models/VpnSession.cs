// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("VpnSessions")]
    public class VpnSession
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int VpnProfileId { get; set; }

        [ForeignKey("VpnProfileId")]
        public VpnProfile VpnProfile { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

        public DateTime? DisconnectedAt { get; set; }

        [Required]
        [MaxLength(50)]
        public string ClientIP { get; set; }

        public long BytesReceived { get; set; } = 0;

        public long BytesSent { get; set; } = 0;
    }
}
