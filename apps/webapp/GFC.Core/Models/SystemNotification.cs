// [NEW]
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class SystemNotification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Channel { get; set; } // e.g., "Email", "SMS", "InApp"

        public string Status { get; set; } // e.g., "Pending", "Sent", "Failed"
    }
}
