using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("PosTerminals")]
    public class PosTerminal : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TerminalName { get; set; } = string.Empty;

        public int? MenuProfileId { get; set; }

        [ForeignKey("MenuProfileId")]
        public virtual PosMenuProfile? MenuProfile { get; set; }

        public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;
    }
}
