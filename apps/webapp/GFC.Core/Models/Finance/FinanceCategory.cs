using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models.Finance
{
    [Table("FinanceCategories")]
    public class FinanceCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string Icon { get; set; } = "bi-tag";

        [StringLength(20)]
        public string Color { get; set; } = "#3b82f6";

        public bool IsActive { get; set; } = true;

        public virtual ICollection<FinanceBill> Bills { get; set; } = new List<FinanceBill>();
    }
}
