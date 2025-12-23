// [MODIFIED]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models
{
    public class StudioDraft
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("StudioPage")]
        public int PageId { get; set; }
        public virtual StudioPage StudioPage { get; set; }

        public string ContentSnapshotJson { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
