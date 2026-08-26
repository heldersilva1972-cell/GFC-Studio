using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GFC.Core.Enums;

namespace GFC.Core.Models;

public class ActiveEvent : BaseEntity
{
    [Key]
    public int Id { get; set; }

    public int? TemplateId { get; set; }
    [ForeignKey(nameof(TemplateId))]
    public EventTemplate? Template { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public EventTabType Type { get; set; } = EventTabType.RunningTab;

    [Column(TypeName = "decimal(18,2)")]
    public decimal InitialAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentBalance { get; set; }

    public EventTabStatus Status { get; set; } = EventTabStatus.Open;

    public string? BeerTalliesJson { get; set; }

    public bool EnableBeerTally { get; set; }

    public int ClubDonatedCasesCap { get; set; } = 0;

    public string? DonatedItemIdsJson { get; set; }

    public string? DonatedItemTalliesJson { get; set; }

    public int DonatedBeerClaimedCount { get; set; }


    public int DonatedBeerReDonatedCount { get; set; }

    public int DonatedBeerSoldCount { get; set; }

    public bool Enable100PercentDonatedProceeds { get; set; } = false;

    public bool IsRecurring { get; set; }

    public string? ActiveGrantJson { get; set; }

    [MaxLength(100)]
    public string? RecipientEventName { get; set; }
}
