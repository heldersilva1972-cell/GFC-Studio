namespace GFC.Core.DTOs;

public class AddFundsRequest
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
}

public class UpdateEventTallyRequest
{
    public int Id { get; set; }
    public string BeerTalliesJson { get; set; } = string.Empty;
    public decimal InitialAmount { get; set; }
    public decimal CurrentBalance { get; set; }
    public bool CloseEvent { get; set; }
    public int DonatedBeerClaimedCount { get; set; }
    public int DonatedBeerReDonatedCount { get; set; }
    public int DonatedBeerSoldCount { get; set; }
    public string? DonatedItemIdsJson { get; set; }
    public string? DonatedItemTalliesJson { get; set; }
    public bool IsRecurring { get; set; }
}
