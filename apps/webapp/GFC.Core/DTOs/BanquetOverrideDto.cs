namespace GFC.Core.DTOs;

public class BanquetOverrideDto
{
    public int ItemId { get; set; }
    public bool IsVisible { get; set; }
    public decimal OverridePrice { get; set; }
    public bool TrackTally { get; set; }
    public bool EnableClubSplit { get; set; }
    public decimal ClubSplitAmount { get; set; }
}
