// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("MagicLinkTokens")]
    public class MagicLinkToken
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        [Required]
        [StringLength(128)]
        public string Token { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAtUtc { get; set; }

        public bool IsUsed { get; set; } = false;

        [StringLength(45)]
        public string IpAddress { get; set; }
    }
}
