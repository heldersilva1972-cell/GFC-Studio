using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class PullTabPrizeOption : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GameDefinitionId { get; set; }

        [ForeignKey("GameDefinitionId")]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual PullTabGameDefinition? GameDefinition { get; set; }

        [Required]
        [MaxLength(255)]
        public string OptionLabel { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PayoutAmount { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
