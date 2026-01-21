using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class LiquorItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? UpcCode { get; set; }

        [StringLength(50)]
        public string? BottleSize { get; set; } // e.g. "750ml", "1.75L"

        [StringLength(100)]
        public string? Category { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public int CurrentStock { get; set; }

        [Required]
        public int MinStockLimit { get; set; } = 2;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
