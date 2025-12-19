namespace GFC.Core.Models;

/// <summary>
/// Represents a dues payment record for a member for a specific year.
/// </summary>
public class DuesPayment
{
    public int DuesPaymentID { get; set; }
    public int MemberID { get; set; }
    public int Year { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? PaymentType { get; set; } // CASH, CHECK, CARD, WAIVED
    public string? Notes { get; set; }
}



