using System;

namespace GFC.Core.Models
{
    public class TemporaryCard
    {
        public int TemporaryCardId { get; set; }
        public string? CardNumber { get; set; }
        public string HolderName { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
