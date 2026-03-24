using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("LiquorOrderItems")]
    public class LiquorOrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual LiquorOrder? Order { get; set; }

        [Required]
        public int LiquorItemId { get; set; }

        [ForeignKey("LiquorItemId")]
        public virtual LiquorItem? LiquorItem { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPriceAtTimeOfOrder { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BottleFeeAtTimeOfOrder { get; set; } = 0;

        public bool IsBackordered { get; set; } = false;

        public bool IsResolved { get; set; } = false;
    }
}
