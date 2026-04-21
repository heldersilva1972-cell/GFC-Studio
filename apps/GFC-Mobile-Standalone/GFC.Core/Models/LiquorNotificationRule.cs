using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class LiquorNotificationRule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public bool NotifyOnLowStock { get; set; } = true;

        public bool NotifyOnEmpty { get; set; } = true;

        public bool ReceivePush { get; set; } = true;

        public bool ReceiveSms { get; set; } = false;

        public bool ReceiveEmail { get; set; } = false;
    }
}
