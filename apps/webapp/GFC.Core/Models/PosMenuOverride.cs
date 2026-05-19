using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("PosMenuOverrides")]
    public class PosMenuOverride : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MenuProfileId { get; set; }

        [ForeignKey("MenuProfileId")]
        public virtual PosMenuProfile? MenuProfile { get; set; }

        [Required]
        public int LiquorItemId { get; set; }

        [ForeignKey("LiquorItemId")]
        public virtual LiquorItem? LiquorItem { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? OverridePrice { get; set; }

        [StringLength(100)]
        public string? OverrideCategory { get; set; }

        public bool? IsVisible { get; set; }

        public int? DisplayOrder { get; set; }
    }
}
