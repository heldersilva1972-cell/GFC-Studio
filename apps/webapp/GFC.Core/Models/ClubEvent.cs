using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GFC.Core.Models;

[Table("ClubEvents")]
public class ClubEvent : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string EventName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string EventGroup { get; set; } = string.Empty;

    [Required]
    public DateTime EventDate { get; set; }

    public bool IsArchived { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? InitialBudget { get; set; }

    public string? Note { get; set; }

    public string? ConfigurationJson { get; set; }

    public virtual ICollection<ClubEventTransaction> Transactions { get; set; } = new List<ClubEventTransaction>();
}
