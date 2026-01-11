using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class BarSaleEntry
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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
