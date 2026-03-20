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

        public virtual ICollection<LiquorItem> Items { get; set; } = new List<LiquorItem>();
        public virtual ICollection<LiquorOrder> Orders { get; set; } = new List<LiquorOrder>();
    }
}
