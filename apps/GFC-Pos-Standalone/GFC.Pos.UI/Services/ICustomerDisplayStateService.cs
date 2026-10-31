namespace GFC.Pos.UI.Services;

public enum CustomerDisplayMode
{
    Welcome,
    ActiveOrder,
    Completed
}

public class CustomerDisplayItem
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total => Quantity * Price;
    public List<string> Modifiers { get; set; } = new();
}

public class CustomerDisplaySnapshot
{
    public CustomerDisplayMode Mode { get; set; } = CustomerDisplayMode.Welcome;
    public List<CustomerDisplayItem> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal TotalDue { get; set; }
    public decimal? AmountPaid { get; set; }
    public decimal? ChangeDue { get; set; }
    public string? PaymentNote { get; set; }
    public string StoreName { get; set; } = "GFC CLUB";
    public string HeaderMessage { get; set; } = "Welcome!";
}

public interface ICustomerDisplayStateService
{
    CustomerDisplaySnapshot Current { get; }
    event Action? OnStateChanged;

    void SetWelcome(string? message = null, bool force = false);
    void UpdateCart(IEnumerable<CustomerDisplayItem> items, decimal subtotal, decimal tax = 0m, decimal? cashGiven = null, decimal? changeDue = null);
    void CompleteTransaction(decimal? totalPaid, decimal? changeDue, string? paymentNote = null, int? durationSeconds = null);
}
