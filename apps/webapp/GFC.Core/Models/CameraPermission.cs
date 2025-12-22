// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public enum CameraAccessLevel
    {
        View,
        Playback,
        Download,
        Manage
    }

    public class CameraPermission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CameraId { get; set; }

        [ForeignKey("CameraId")]
        public Camera Camera { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        [Required]
        public CameraAccessLevel AccessLevel { get; set; }
    }
}
