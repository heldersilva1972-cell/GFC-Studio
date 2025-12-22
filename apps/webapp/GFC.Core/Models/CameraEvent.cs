// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public enum CameraEventType
    {
        Motion,
        Access,
        Alert
    }

    public class CameraEvent
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public int CameraId { get; set; }

        [ForeignKey("CameraId")]
        public Camera Camera { get; set; }

        [Required]
        public CameraEventType EventType { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}
