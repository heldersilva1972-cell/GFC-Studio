using System.ComponentModel.DataAnnotations;
using GFC.Core.Enums;

namespace GFC.Core.Models;

public class EventTemplate : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public EventTabType DefaultType { get; set; } = EventTabType.RunningTab;

    public string? ItemsOverrideJson { get; set; }

    public bool EnableBeerTally { get; set; }

    public int ClubDonatedCasesCap { get; set; } = 3;

    public string? DonatedItemIdsJson { get; set; }

    public bool Enable100PercentDonatedProceeds { get; set; } = true;


    public bool IsRecurring { get; set; }

    [MaxLength(100)]
    public string? RecipientEventName { get; set; }
}
