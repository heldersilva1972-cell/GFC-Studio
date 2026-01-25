namespace GFC.Core.Models
{
    public class LiquorUsageDTO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string BottleSize { get; set; } = string.Empty;
        public int TotalConsumed { get; set; } // Absolute value of negative changes
    }
}
