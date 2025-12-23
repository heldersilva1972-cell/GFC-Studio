// [NEW]
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
        public int? StudioPageId { get; set; }
        public virtual StudioPage StudioPage { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime LastSaved { get; set; }
    }
}
