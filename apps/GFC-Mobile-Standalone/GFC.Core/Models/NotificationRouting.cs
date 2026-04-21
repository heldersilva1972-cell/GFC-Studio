// [NEW]
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class NotificationRouting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ActionName { get; set; }

        [Required]
        [StringLength(200)]
        public string DirectorEmail { get; set; }
    }
}
