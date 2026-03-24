using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("LiquorVendors")]
    public class LiquorVendor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? ContactName { get; set; }

        public decimal MinimumOrderAmount { get; set; } = 0;

        [StringLength(50)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Website { get; set; }

        public virtual ICollection<LiquorItem> Items { get; set; } = new List<LiquorItem>();
        public virtual ICollection<LiquorOrder> Orders { get; set; } = new List<LiquorOrder>();
    }
}
