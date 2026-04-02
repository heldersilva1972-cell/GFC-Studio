using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class BarSaleEntry : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Required]
        [Range(0, 100000, ErrorMessage = "Sales must be a positive number")]
        public decimal TotalSales { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Total items sold must be a positive number")]
        public int TotalItemsSold { get; set; }

        public string? Notes { get; set; }

        public string Shift { get; set; } = "Day";
        
        public DateTime? AdjustedSaleDate { get; set; }

        public bool IsAudited { get; set; }

        public decimal? OriginalTotalSales { get; set; }

        public decimal? TotalHours { get; set; }
        public bool IsRentalHall { get; set; }
        public string? BarLocation { get; set; }

        public decimal? HourlyRate_AtTimeOfShift { get; set; }
        public decimal? TotalEmployeeTaxes_AtTimeOfShift { get; set; }
        public decimal? TotalEmployerTaxes_AtTimeOfShift { get; set; }

        public string Status { get; set; } = "Draft";
    }
}
