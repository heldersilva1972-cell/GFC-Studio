namespace GFC.Core.Models
{
    /// <summary>
    /// Represents a cash collection from the Scratch Ticket Vending Machine (ITVM).
    /// </summary>
    public class LotteryVendingCollection
    {
        public int Id { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
        public decimal AmountCollected { get; set; }
        public string? EnteredBy { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
