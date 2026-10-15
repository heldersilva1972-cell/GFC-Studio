using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class GoogleEventOverride
    {
        [Key]
        [MaxLength(255)]
        public string GoogleEventId { get; set; } = string.Empty;

        public decimal EventFee { get; set; } = 0.00m;

        public bool BarRequested { get; set; } = false;

        public string? DisabledTimeSlotsJson { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
    }
}
