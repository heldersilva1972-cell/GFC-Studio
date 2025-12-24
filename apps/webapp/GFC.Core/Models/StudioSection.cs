// [NEW]
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioSection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudioPageId { get; set; }

        [ForeignKey("StudioPageId")]
        public StudioPage StudioPage { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        public int OrderIndex { get; set; }

        public string Data { get; set; }

        public string AnimationSettingsJson { get; set; }
    }
}
