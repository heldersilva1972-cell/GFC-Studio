using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GFC.Core.Models;

public class BylawDocument
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = "General";

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Content { get; set; }

    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? LastUpdatedBy { get; set; }

    public int CurrentVersion { get; set; } = 1;
}
