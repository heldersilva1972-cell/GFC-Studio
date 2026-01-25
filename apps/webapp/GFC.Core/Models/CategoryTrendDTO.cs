namespace GFC.Core.Models
{
    public class CategoryTrendDTO
    {
        public string Category { get; set; } = string.Empty;
        public int CurrentPeriodUsage { get; set; }
        public int PreviousPeriodUsage { get; set; }
        public double PercentageChange { get; set; }
        public bool IsUp => PercentageChange > 0;
    }
}
