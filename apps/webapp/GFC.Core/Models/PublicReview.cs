// [MODIFIED]
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class PublicReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }

        [StringLength(100)]
        public string? EventType { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public bool IsApproved { get; set; }

        public bool IsFeatured { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
