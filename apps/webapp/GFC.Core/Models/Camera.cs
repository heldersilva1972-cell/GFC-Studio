// [NEW]
using System;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models
{
    public class Camera
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string RtspUrl { get; set; } = string.Empty;

        [StringLength(45)]
        public string IpAddress { get; set; } = string.Empty;

        [StringLength(100)]
        public string Manufacturer { get; set; } = string.Empty;

        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        public bool? IsEnabled { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
