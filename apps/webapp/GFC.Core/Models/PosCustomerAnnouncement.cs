using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("PosCustomerAnnouncements")]
    public class PosCustomerAnnouncement : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        [StringLength(50)]
        public string Theme { get; set; } = "Bakery"; // Bakery, Notice, Event, Promo, General

        [StringLength(500)]
        public string? IconOrImageUrl { get; set; }

        [StringLength(100)]
        public string? BadgeText { get; set; } // e.g. "FRESH DAILY", "URGENT", "SPECIAL"

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}

