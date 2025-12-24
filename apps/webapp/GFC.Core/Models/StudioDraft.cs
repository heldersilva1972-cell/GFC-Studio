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

        [Required]
        public int PageId { get; set; }

        [ForeignKey("PageId")]
        public StudioPage StudioPage { get; set; }

        public string ContentSnapshotJson { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
