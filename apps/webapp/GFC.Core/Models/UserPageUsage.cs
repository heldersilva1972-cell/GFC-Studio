using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class UserPageUsage
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string PageIdentifier { get; set; } = string.Empty;

        public int UsageCount { get; set; }

        public DateTime LastUsedUtc { get; set; }

        // Navigation property
        // [ForeignKey("UserId")]
        // public virtual AppUser? User { get; set; }
    }
}
