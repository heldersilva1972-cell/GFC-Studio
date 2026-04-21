// [NEW]
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class SystemNotification
    {
        [Key]
        public int Id { get; set; }

        public string? RecipientEmail { get; set; }
        
        public string? Subject { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Channel { get; set; } // e.g., "Email", "SMS", "InApp"

        public string Status { get; set; } // e.g., "Pending", "Sent", "Failed"
        
        public DateTime? SentAt { get; set; }

        // Announcement properties
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
    }
}
