// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    [Table("PagePermissions")]
    public class PagePermission
    {
        [Key]
        public int PermissionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; } = default!;

        [Required]
        public int PageId { get; set; }

        [ForeignKey("PageId")]
        public virtual AppPage Page { get; set; } = default!;

        [Required]
        public bool ReceivePush { get; set; } = false;
    }
}
