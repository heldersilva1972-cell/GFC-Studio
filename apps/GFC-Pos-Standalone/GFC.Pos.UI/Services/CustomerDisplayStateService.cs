using Microsoft.JSInterop;
using System.Text.Json;

namespace GFC.Pos.UI.Services;

public class CustomerDisplayStateService : ICustomerDisplayStateService
{
    private readonly IStationSettingsService _settings;
    private readonly IJSRuntime? _js;
    private System.Threading.Timer? _resetTimer;
    public const string StorageKey = "gfc_pos_cfd_snapshot";

    public CustomerDisplaySnapshot Current { get; private set; } = new();

    public event Action? OnStateChanged;

    public CustomerDisplayStateService(IStationSettingsService settings, IJSRuntime js)
    {
        _settings = settings;
        _js = js;
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();

        if (_js != null)
        {
            try
            {
                var json = JsonSerializer.Serialize(Current);
                _ = _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
            }
            catch
            {
                // Non-critical: ignore in environments where JS is not yet ready
            }
        }
    }

    public void SetWelcome(string? message = null, bool force = false)
    {
        // If a transaction was just completed and the countdown is showing "THANK YOU",
        // do not wipe the screen with Welcome unless explicitly forced or next order starts
        if (!force && Current.Mode == CustomerDisplayMode.Completed && _resetTimer != null)
        {
            return;
        }

        _resetTimer?.Dispose();
        _resetTimer = null;

        Current = new CustomerDisplaySnapshot
        {
            Mode = CustomerDisplayMode.Welcome,
            HeaderMessage = message ?? "Welcome!",
            Items = new List<CustomerDisplayItem>()
        };

        NotifyStateChanged();
    }

    public void UpdateCart(IEnumerable<CustomerDisplayItem> items, decimal subtotal, decimal tax = 0m, decimal? cashGiven = null, decimal? changeDue = null)
    {
        _resetTimer?.Dispose();
        _resetTimer = null;

        var itemList = items.ToList();

        if (itemList.Count == 0)
        {
            SetWelcome();
            return;
        }

        Current = new CustomerDisplaySnapshot
        {
            Mode = CustomerDisplayMode.ActiveOrder,
            Items = itemList,
            Subtotal = subtotal,
            Tax = tax,
            TotalDue = subtotal + tax,
            AmountPaid = cashGiven,
            ChangeDue = changeDue
        };

        NotifyStateChanged();
    }

    public void CompleteTransaction(decimal? totalPaid, decimal? changeDue, string? paymentNote = null, int? durationSeconds = null)
    {
        _resetTimer?.Dispose();

        Current.Mode = CustomerDisplayMode.Completed;
        Current.AmountPaid = totalPaid;
        Current.ChangeDue = changeDue;
        Current.PaymentNote = paymentNote;

        NotifyStateChanged();

        // Auto-reset to welcome screen after configured duration (default: 20 seconds)
        int delayMs = (durationSeconds.HasValue && durationSeconds.Value > 0 ? durationSeconds.Value : 20) * 1000;
        _resetTimer = new System.Threading.Timer(_ =>
        {
            SetWelcome(null, force: true);
        }, null, delayMs, System.Threading.Timeout.Infinite);
    }
}
