// [NEW]
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class NavMenuEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Label { get; set; }

        [Required]
        public string Url { get; set; }

        public int? ParentId { get; set; }
    }
}
