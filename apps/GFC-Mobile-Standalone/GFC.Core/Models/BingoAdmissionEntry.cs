using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class BingoAdmissionEntry : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BingoSessionId { get; set; }

        [Required]
        public int AdmissionDefinitionId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceAtTime { get; set; }

        // Navigation properties
        [ForeignKey("BingoSessionId")]
        public virtual BingoSession? Session { get; set; }

        [ForeignKey("AdmissionDefinitionId")]
        public virtual BingoAdmissionDefinition? Definition { get; set; }
    }
}
