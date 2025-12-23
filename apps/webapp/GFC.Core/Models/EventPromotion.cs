// [NEW]
using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class EventPromotion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime EventDate { get; set; }

        public string ImageUrl { get; set; }

        public string Skin { get; set; } // e.g., "Gold Border", "Standard"
    }
}
