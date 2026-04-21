// [NEW]
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

public class StudioLock
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("StudioPage")]
    public int PageId { get; set; }

    [Required]
    public string LockedBy { get; set; }

    public DateTime LockedAt { get; set; }
}
