using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class BingoGameDefinition : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SheetDefinitionId { get; set; }

        [ForeignKey("SheetDefinitionId")]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual BingoSheetDefinition? Sheet { get; set; }

        [Required]
        [MaxLength(255)]
        public string GameName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DefaultPayout { get; set; }

        public bool IsVariablePayout { get; set; }
    }
}
