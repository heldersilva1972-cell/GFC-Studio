// [NEW]
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class UrlRedirect
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(2048)]
        public string OldUrl { get; set; }

        [Required]
        [StringLength(2048)]
        public string NewUrl { get; set; }

        [Required]
        public int RedirectType { get; set; } = 301; // Permanent Redirect
    }
}
