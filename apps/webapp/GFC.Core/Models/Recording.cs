// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class Recording
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int CameraId { get; set; }

        [ForeignKey("CameraId")]
        public Camera Camera { get; set; }

        [Required]
        [StringLength(1000)]
        public string FilePath { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public long FileSize { get; set; }
    }
}
