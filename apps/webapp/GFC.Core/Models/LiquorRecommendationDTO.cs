using System;

namespace GFC.Core.Models
{
    public class LiquorRecommendationDTO
    {
        public int ItemId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int MinStockLimit { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public double AvgWeeklyUsage { get; set; }
        public int RecommendedQuantity { get; set; } // Recommended quantity in bottles/units
        public decimal UnitPrice { get; set; }
        public decimal CasePrice { get; set; }
        public int PackSize { get; set; }
        public bool OrderByCaseOnly { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public bool IsTopUpSuggestion { get; set; }
        public bool ExcludeFromPredictions { get; set; }
        public string Reason { get; set; } = string.Empty;
        public bool IsUnitBased { get; set; }
        public double TotalPeriodUsage { get; set; }
    }
}
