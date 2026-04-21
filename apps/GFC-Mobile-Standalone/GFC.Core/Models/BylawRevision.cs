using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

public class BylawRevision
{
    [Key]
    public int Id { get; set; }

    public int DocumentId { get; set; }

    public string? Content { get; set; }

    public DateTime RevisionDate { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? RevisionBy { get; set; }

    public int Version { get; set; }

    [MaxLength(500)]
    public string? ChangeReason { get; set; }
}
