// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("AppPages")]
    public class AppPage
    {
        [Key]
        public int PageId { get; set; }

        [Required]
        [StringLength(100)]
        public string PageName { get; set; }

        [Required]
        [StringLength(255)]
        public string PageRoute { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        public bool RequiresAdmin { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
}
