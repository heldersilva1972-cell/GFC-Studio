// [MODIFIED]
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class StudioPage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Slug { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public string ThemeConfig { get; set; }

        public ICollection<StudioSection> Sections { get; set; } = new List<StudioSection>();
    }
}
