using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("PosCategories")]
    public class PosCategory : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public bool IsModifierCategory { get; set; } = false;

        public int InventoryTrackType { get; set; } = 0; // 0 = None, 1 = Liquor, 2 = Food/General

        public int ZReportGroup { get; set; } = 0; // 0 = Auto, 1 = Food & Non-Alcoholic, 2 = Liquor & Alcohol, 3 = Excluded

        public string? ModifiersJson { get; set; }

        public int? MenuProfileId { get; set; }

        [ForeignKey("MenuProfileId")]
        public virtual PosMenuProfile? MenuProfile { get; set; }
    }
}
