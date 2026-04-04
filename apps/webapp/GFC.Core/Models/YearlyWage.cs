using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class YearlyWage : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [Required]
        [Range(0, 1000, ErrorMessage = "Rate must be a positive number")]
        public decimal HourlyRate { get; set; }
    }
}
