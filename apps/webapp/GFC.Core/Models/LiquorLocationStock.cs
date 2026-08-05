using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class LiquorLocationStock
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [ForeignKey("ItemId")]
        public virtual LiquorItem? Item { get; set; }

        [Required]
        [StringLength(100)]
        public string LocationName { get; set; } = string.Empty; // MAIN_STORAGE, DOWNSTAIRS_BAR, UPSTAIRS_BAR

        [Column(TypeName = "decimal(18,4)")]
        public decimal Stock { get; set; } = 0;
    }
}
