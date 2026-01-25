namespace GFC.Core.Models
{
    public class ProductTrendDTO
    {
        public int ItemId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CurrentPeriodUsage { get; set; }
        public int PreviousPeriodUsage { get; set; }
        public double PercentageChange { get; set; }
        public bool IsUp => PercentageChange > 0;
    }
}
