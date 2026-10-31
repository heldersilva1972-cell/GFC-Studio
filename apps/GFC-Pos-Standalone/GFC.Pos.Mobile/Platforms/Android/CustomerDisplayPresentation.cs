#if ANDROID
using Android.App;
using Android.Content;
using Android.Hardware.Display;
using Android.OS;
using Android.Views;
using Android.Webkit;
using GFC.Pos.UI.Services;
using Microsoft.Maui.Storage;
using AndroidWebView = Android.Webkit.WebView;

namespace GFC.Pos.Mobile.Platforms.Android;

public class CustomerDisplayPresentation : Presentation
{
    private AndroidWebView? _webView;
    private readonly ICustomerDisplayStateService? _stateService;
    private bool _pageLoaded = false;

    public CustomerDisplayPresentation(Context outerContext, Display display, ICustomerDisplayStateService? stateService = null) 
        : base(outerContext, display)
    {
        _stateService = stateService;
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Remove title bar and set full screen
        RequestWindowFeature((int)WindowFeatures.NoTitle);
        Window?.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

        _webView = new AndroidWebView(Context);
        _webView.Settings.JavaScriptEnabled = true;
        _webView.Settings.DomStorageEnabled = true;
        _webView.Settings.DatabaseEnabled = true;
        _webView.Settings.MediaPlaybackRequiresUserGesture = false;
        _webView.Settings.UseWideViewPort = true;
        _webView.Settings.LoadWithOverviewMode = true;
        _webView.HorizontalScrollBarEnabled = false;
        _webView.VerticalScrollBarEnabled = false;
        _webView.OverScrollMode = OverScrollMode.Never;

        _webView.SetWebViewClient(new CfdWebViewClient(() =>
        {
            _pageLoaded = true;
            if (_stateService != null)
            {
                PushStateToWebView(_stateService.Current);
            }
        }));

        SetContentView(_webView);

        // Load self-contained offline customer display template
        _webView.LoadDataWithBaseURL("https://pos.lovanow.com", GetOfflineHtmlTemplate(), "text/html", "UTF-8", null);

        if (_stateService != null)
        {
            _stateService.OnStateChanged += HandleStateChanged;
        }
    }

    private void HandleStateChanged()
    {
        if (!_pageLoaded || _webView == null || _stateService == null) return;
        var snapshot = _stateService.Current;
        
        // Push state directly on the Android UI thread
        _webView.Post(() => PushStateToWebView(snapshot));
    }

    private void PushStateToWebView(CustomerDisplaySnapshot snapshot)
    {
        try
        {
            if (_webView == null) return;
            var json = System.Text.Json.JsonSerializer.Serialize(snapshot);
            // Escape json for JS string literal
            var escaped = json.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "");
            _webView.EvaluateJavascript($"window.updateCfdState && window.updateCfdState('{escaped}');", null);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[CFD] PushState error: {ex.Message}");
        }
    }

    public override void Dismiss()
    {
        if (_stateService != null)
        {
            _stateService.OnStateChanged -= HandleStateChanged;
        }
        try
        {
            _webView?.Destroy();
        }
        catch { }
        base.Dismiss();
    }

    private class CfdWebViewClient : WebViewClient
    {
        private readonly Action _onLoaded;
        public CfdWebViewClient(Action onLoaded) => _onLoaded = onLoaded;

        public override void OnPageFinished(AndroidWebView? view, string? url)
        {
            base.OnPageFinished(view, url);
            _onLoaded.Invoke();
        }
    }

    private static string GetOfflineHtmlTemplate()
    {
        return @"<!DOCTYPE html>
<html lang=""en"">
<head>
<meta charset=""utf-8""/>
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no""/>
<title>GFC Customer Display</title>
<style>
  * { box-sizing: border-box; margin: 0; padding: 0; }
  html, body {
    width: 100vw; height: 100vh;
    background: #090d16; color: #f8fafc;
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;
    user-select: none; overflow: hidden; display: flex; flex-direction: column;
  }
  .cfd-header {
    height: 48px; background: #0b1329; border-bottom: 2px solid #1e293b;
    display: flex; align-items: center; justify-content: space-between;
    padding: 0 20px; flex-shrink: 0;
  }
  .cfd-brand { font-size: 1.35rem; font-weight: 900; letter-spacing: 2px; color: #38bdf8; }
  .cfd-welcome-badge {
    font-size: 0.95rem; font-weight: 800; color: #cbd5e1;
    background: rgba(30, 41, 59, 0.8); border: 1px solid #334155;
    padding: 4px 14px; border-radius: 999px; letter-spacing: 0.5px;
  }
  .cfd-welcome {
    flex: 1; display: flex; align-items: center; justify-content: center;
    background: radial-gradient(circle at center, #172554 0%, #090d16 100%); padding: 24px;
    width: 100%; height: 100%;
  }
  .cfd-welcome-card { text-align: center; max-width: 580px; animation: cfdPixelShift 28s ease-in-out infinite; }
  .cfd-welcome-icon {
    display: inline-flex; align-items: center; justify-content: center;
    width: 110px; height: 110px; background: rgba(59, 130, 246, 0.15);
    color: #38bdf8; border-radius: 50%; margin-bottom: 22px;
    box-shadow: 0 0 50px rgba(56, 189, 248, 0.25);
    animation: cfdIconPulse 6s ease-in-out infinite;
  }
  .cfd-welcome-icon svg { width: 68px; height: 68px; stroke: #38bdf8; stroke-width: 2.2; fill: none; }
  .cfd-welcome-title { font-size: 2.8rem; font-weight: 900; letter-spacing: 3px; color: #ffffff; margin-bottom: 10px; text-transform: uppercase; }
  .cfd-welcome-subtitle { font-size: 1.4rem; color: #94a3b8; margin-bottom: 24px; }
  .cfd-welcome-time { font-size: 1rem; font-weight: 700; color: #64748b; text-transform: uppercase; letter-spacing: 1.5px; }

  @keyframes cfdPixelShift {
    0%, 100% { transform: translate(0, 0); }
    25% { transform: translate(3px, -2px); }
    50% { transform: translate(-2px, 3px); }
    75% { transform: translate(2px, 2px); }
  }
  @keyframes cfdIconPulse {
    0%, 100% { box-shadow: 0 0 35px rgba(56, 189, 248, 0.2); transform: scale(1); }
    50% { box-shadow: 0 0 55px rgba(56, 189, 248, 0.4); transform: scale(1.03); }
  }

  .cfd-split { flex: 1; display: flex; overflow: hidden; width: 100%; height: 100%; }
  .cfd-left { width: 60%; background: #0f172a; border-right: 2px solid rgba(255, 255, 255, 0.08); display: flex; flex-direction: column; overflow: hidden; padding: 16px 20px; }
  .cfd-left-header {
    height: 40px; display: flex; align-items: center; justify-content: space-between;
    padding-bottom: 10px; border-bottom: 2px solid rgba(255, 255, 255, 0.08); font-size: 0.9rem; font-weight: 900; color: #94a3b8;
    text-transform: uppercase; letter-spacing: 2px; flex-shrink: 0; margin-bottom: 10px;
  }
  .cfd-header-badge {
    font-size: 0.8rem; font-weight: 800; background: #1e293b; color: #38bdf8;
    padding: 3px 12px; border-radius: 999px; border: 1px solid rgba(56, 189, 248, 0.3);
  }
  .cfd-item-list { flex: 1; overflow-y: auto; padding-right: 4px; display: flex; flex-direction: column; gap: 8px; }
  .cfd-item-row {
    display: flex; align-items: center; justify-content: space-between;
    background: rgba(30, 41, 59, 0.75); border: 1px solid rgba(255, 255, 255, 0.08);
    border-left: 5px solid #2563eb; border-radius: 12px;
    padding: 10px 14px; box-shadow: 0 4px 12px rgba(0, 0, 0, 0.25);
  }
  .cfd-item-token-redeemed { border-left: 5px solid #22c55e !important; background: rgba(20, 83, 45, 0.35) !important; }
  .cfd-item-token-purchase { border-left: 5px solid #f59e0b !important; background: rgba(120, 53, 15, 0.25) !important; }
  .cfd-item-info { display: flex; align-items: center; gap: 12px; flex: 1; min-width: 0; }
  .cfd-qty-badge {
    font-size: 1.25rem; font-weight: 900; color: #38bdf8;
    background: rgba(56, 189, 248, 0.15); border: 1.5px solid rgba(56, 189, 248, 0.35);
    padding: 4px 12px; border-radius: 10px; min-width: 44px; text-align: center;
  }
  .cfd-qty-token-redeemed { background: #16a34a !important; color: #ffffff !important; border-color: #22c55e !important; }
  .cfd-qty-token-purchase { background: #d97706 !important; color: #ffffff !important; border-color: #f59e0b !important; }
  .cfd-item-name { font-size: 1.25rem; font-weight: 800; color: #ffffff; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
  .cfd-badge-token-redeemed {
    display: inline-block; background: #166534; color: #86efac; font-size: 0.72rem;
    font-weight: 900; padding: 2px 7px; border-radius: 6px; margin-left: 8px; vertical-align: middle;
    border: 1px solid rgba(134, 239, 172, 0.4);
  }
  .cfd-badge-token-purchase {
    display: inline-block; background: #78350f; color: #fde68a; font-size: 0.72rem;
    font-weight: 900; padding: 2px 7px; border-radius: 6px; margin-left: 8px; vertical-align: middle;
    border: 1px solid rgba(253, 230, 138, 0.4);
  }
  .cfd-item-price { font-size: 1.35rem; font-weight: 900; color: #ffffff; margin-left: 14px; min-width: 80px; text-align: right; }
  .cfd-price-free { color: #4ade80 !important; font-weight: 900; }

  .cfd-right { width: 40%; background: #0b1329; display: flex; flex-direction: column; padding: 16px 20px; }
  .cfd-summary-panel { flex: 1; display: flex; flex-direction: column; justify-content: space-between; }
  .cfd-summary-line { display: flex; justify-content: space-between; align-items: center; font-size: 1.15rem; color: #94a3b8; padding: 4px 2px; }
  .cfd-summary-val { font-weight: 800; color: #e2e8f0; }

  .cfd-total-banner {
    background: linear-gradient(135deg, #1e3a8a 0%, #1d4ed8 100%);
    border: 2px solid #3b82f6; border-radius: 16px; padding: 12px 18px;
    text-align: center; margin-top: 8px; box-shadow: 0 10px 30px -5px rgba(29, 78, 216, 0.45);
  }
  .cfd-total-label { font-size: 0.85rem; font-weight: 900; letter-spacing: 2.5px; color: #93c5fd; text-transform: uppercase; margin-bottom: 2px; }
  .cfd-total-amount { font-size: 2.8rem; font-weight: 900; color: #ffffff; letter-spacing: 1px; line-height: 1.1; }

  .cfd-tender-box {
    background: rgba(30, 41, 59, 0.85); border: 2px solid #334155;
    border-radius: 14px; padding: 12px 16px; box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3); margin-top: auto;
  }
  .cfd-tender-row { display: flex; justify-content: space-between; align-items: center; font-size: 1.1rem; color: #94a3b8; padding: 3px 0; }
  .cfd-tender-change { border-top: 1px solid #334155; margin-top: 6px; padding-top: 6px; color: #4ade80 !important; }
  .cfd-completed-box {
    background: radial-gradient(circle at center, #166534 0%, #14532d 100%);
    border: 2px solid #22c55e; border-radius: 14px; padding: 12px 16px; text-align: center;
    box-shadow: 0 10px 25px -5px rgba(34, 197, 94, 0.4); margin-top: auto;
  }
  .cfd-completed-header { font-size: 0.95rem; font-weight: 900; letter-spacing: 2px; color: #86efac; display: flex; align-items: center; justify-content: center; gap: 6px; }
  .cfd-completed-header svg { width: 22px; height: 22px; stroke: #4ade80; stroke-width: 3; fill: none; }
  .cfd-completed-sub { font-size: 1.7rem; font-weight: 900; letter-spacing: 2px; color: #ffffff; margin: 3px 0 4px 0; }
  .cfd-completed-note { font-size: 1.1rem; font-weight: 900; letter-spacing: 1.5px; color: #93c5fd; text-transform: uppercase; margin: 3px 0 4px 0; }
  .cfd-completed-divider { height: 1px; background: rgba(134, 239, 172, 0.3); margin: 6px 0 6px 0; }
  .cfd-completed-row { display: flex; justify-content: space-between; align-items: center; font-size: 1.05rem; color: #dcfce7; padding: 3px 0; }
  .cfd-change-highlight { border-top: 1px solid rgba(134, 239, 172, 0.4); margin-top: 4px; padding-top: 4px; }
  .cfd-change-amount { font-size: 1.5rem; font-weight: 900; color: #ffffff; }
</style>
</head>
<body>
  <div id=""viewWelcome"" class=""cfd-welcome"">
    <div class=""cfd-welcome-card"">
      <div class=""cfd-welcome-icon"">
        <svg viewBox=""0 0 24 24"">
          <path d=""M6 2L3 6v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V6l-3-4z"" stroke-linecap=""round"" stroke-linejoin=""round""/>
          <line x1=""3"" y1=""6"" x2=""21"" y2=""6"" stroke-linecap=""round"" stroke-linejoin=""round""/>
          <path d=""M16 10a4 4 0 0 1-8 0"" stroke-linecap=""round"" stroke-linejoin=""round""/>
        </svg>
      </div>
      <h1 id=""cfdWelcomeTitle"" class=""cfd-welcome-title"">GFC CLUB</h1>
      <p id=""cfdSubMsg"" class=""cfd-welcome-subtitle"">Welcome!</p>
      <div id=""cfdClock"" class=""cfd-welcome-time""></div>
    </div>
  </div>

  <div id=""viewOrder"" class=""cfd-split"" style=""display:none;"">
    <div class=""cfd-left"">
      <div class=""cfd-left-header"">
        <span>CURRENT ORDER</span>
        <span id=""cfdItemCount"" class=""cfd-header-badge"">0 items</span>
      </div>
      <div id=""cfdItemList"" class=""cfd-item-list""></div>
    </div>
    <div class=""cfd-right"">
      <div class=""cfd-summary-panel"">
        <div class=""cfd-fixed-totals"">
          <div class=""cfd-summary-line"">
            <span>Subtotal</span>
            <span id=""cfdSubtotal"" class=""cfd-summary-val"">$0.00</span>
          </div>
          <div id=""cfdTaxRow"" class=""cfd-summary-line"" style=""display:none;"">
            <span>Sales Tax</span>
            <span id=""cfdTax"" class=""cfd-summary-val"">$0.00</span>
          </div>
          <div class=""cfd-total-banner"">
            <div id=""cfdTotalLabel"" class=""cfd-total-label"">TOTAL DUE</div>
            <div id=""cfdTotalDue"" class=""cfd-total-amount"">$0.00</div>
          </div>
        </div>
        <div id=""cfdDynamicBottom""></div>
      </div>
    </div>
  </div>

<script>
  function updateClock() {
    const el = document.getElementById('cfdClock');
    if (el) {
      const now = new Date();
      const options = { weekday: 'long', month: 'long', day: 'numeric', hour: 'numeric', minute: '2-digit' };
      el.textContent = now.toLocaleDateString('en-US', options).replace(',', ' •');
    }
  }
  setInterval(updateClock, 1000);
  updateClock();

  function formatMoney(num) {
    return '$' + (Number(num) || 0).toFixed(2);
  }

  window.updateCfdState = function(jsonStr) {
    try {
      const snap = JSON.parse(jsonStr);
      const isOrder = snap.Mode === 1 || snap.Mode === 2; // Cart or Completed
      
      document.getElementById('viewWelcome').style.display = isOrder ? 'none' : 'flex';
      document.getElementById('viewOrder').style.display = isOrder ? 'flex' : 'none';

      if (!isOrder) {
        document.getElementById('cfdWelcomeTitle').textContent = snap.StoreName || 'GFC CLUB';
        document.getElementById('cfdSubMsg').textContent = snap.HeaderMessage || 'Welcome!';
        return;
      }

      // Populate Items
      const listEl = document.getElementById('cfdItemList');
      listEl.innerHTML = '';
      const items = snap.Items || [];
      const totalQty = items.reduce((acc, it) => acc + (it.Quantity || 1), 0);
      document.getElementById('cfdItemCount').textContent = totalQty + (totalQty === 1 ? ' item' : ' items');

      items.forEach(it => {
        let isRedeemed = (it.Name && it.Name.indexOf('(TOKEN REDEEMED)') !== -1) || it.IsTokenRedemption;
        let isPurchase = (it.Name && it.Name.indexOf('(TOKEN SALE)') !== -1) || it.IsTokenPurchase;
        let displayName = it.Name || '';
        if (isRedeemed) displayName = displayName.replace('(TOKEN REDEEMED)', '').trim();
        if (isPurchase) {
          displayName = displayName.replace('(TOKEN SALE)', '').trim();
          if (displayName.indexOf('(') !== -1 && displayName.endsWith(')')) {
            const openIdx = displayName.indexOf('(');
            displayName = displayName.substring(openIdx + 1, displayName.length - 1).trim();
          }
        }

        let rowClass = 'cfd-item-row';
        let qtyClass = 'cfd-qty-badge';
        let badgeHtml = '';
        if (isRedeemed) {
          rowClass += ' cfd-item-token-redeemed';
          qtyClass += ' cfd-qty-token-redeemed';
          badgeHtml = '<span class=""cfd-badge-token-redeemed"">TOKEN REDEEMED</span>';
        } else if (isPurchase) {
          rowClass += ' cfd-item-token-purchase';
          qtyClass += ' cfd-qty-token-purchase';
          badgeHtml = '<span class=""cfd-badge-token-purchase"">TOKEN</span>';
        }

        const price = (it.Price !== undefined ? it.Price * (it.Quantity || 1) : it.TotalPrice) || 0;
        const priceHtml = isRedeemed 
          ? '<span class=""cfd-item-price cfd-price-free"">$0.00</span>' 
          : '<span class=""cfd-item-price"">' + formatMoney(price) + '</span>';

        const row = document.createElement('div');
        row.className = rowClass;
        row.innerHTML = `
          <div class=""cfd-item-info"">
            <span class=""${qtyClass}"">x${it.Quantity || 1}</span>
            <div style=""min-width:0;"">
              <span class=""cfd-item-name"">${displayName}</span>
              ${badgeHtml}
            </div>
          </div>
          ${priceHtml}
        `;
        listEl.appendChild(row);
      });

      // Totals
      document.getElementById('cfdSubtotal').textContent = formatMoney(snap.Subtotal);
      const taxVal = Number(snap.Tax) || 0;
      const taxRow = document.getElementById('cfdTaxRow');
      if (taxRow) {
        taxRow.style.display = taxVal > 0 ? 'flex' : 'none';
        document.getElementById('cfdTax').textContent = formatMoney(taxVal);
      }
      document.getElementById('cfdTotalDue').textContent = formatMoney(snap.TotalDue);

      const bottomEl = document.getElementById('cfdDynamicBottom');
      bottomEl.innerHTML = '';

      if (snap.Mode === 2) { // Completed
        document.getElementById('cfdTotalLabel').textContent = 'TOTAL PAID';
        let noteHtml = snap.PaymentNote ? `<div class=""cfd-completed-note"">${snap.PaymentNote}</div>` : '';
        let tenderDetails = '';
        const paidVal = snap.AmountPaid ?? snap.CashTendered;
        if (paidVal !== null && paidVal !== undefined && paidVal > 0) {
          tenderDetails = `
            <div class=""cfd-completed-divider""></div>
            <div class=""cfd-completed-row"">
              <span>Cash Given:</span>
              <span style=""font-weight:800;"">${formatMoney(paidVal)}</span>
            </div>
          `;
        }
        if (snap.ChangeDue !== null && snap.ChangeDue !== undefined && snap.ChangeDue > 0) {
          tenderDetails += `
            <div class=""cfd-completed-row cfd-change-highlight"">
              <span style=""font-weight:900;"">Change Due:</span>
              <span class=""cfd-change-amount"">${formatMoney(snap.ChangeDue)}</span>
            </div>
          `;
        }

        bottomEl.innerHTML = `
          <div class=""cfd-completed-box"">
            <div class=""cfd-completed-header"">
              <svg viewBox=""0 0 24 24""><polyline points=""20 6 9 17 4 12""></polyline></svg>
              TRANSACTION COMPLETE
            </div>
            <div class=""cfd-completed-sub"">THANK YOU!</div>
            ${noteHtml}
            ${tenderDetails}
          </div>
        `;
      } else { // Active Cart
        document.getElementById('cfdTotalLabel').textContent = 'TOTAL DUE';
        const paidVal = snap.AmountPaid ?? snap.CashTendered;
        if (paidVal !== null && paidVal !== undefined && paidVal > 0) {
          const change = snap.ChangeDue || 0;
          const isChange = change >= 0;
          bottomEl.innerHTML = `
            <div class=""cfd-tender-box"">
              <div class=""cfd-tender-row"">
                <span>Cash Given:</span>
                <span style=""font-weight:800;color:#ffffff;"">${formatMoney(paidVal)}</span>
              </div>
              <div class=""cfd-tender-row ${isChange ? 'cfd-tender-change' : 'cfd-tender-owed'}"">
                <span style=""font-weight:900;"">${isChange ? 'Change Due:' : 'Still Owed:'}</span>
                <span style=""font-size:1.45rem;font-weight:900;"">${formatMoney(Math.abs(change))}</span>
              </div>
            </div>
          `;
        }
      }
    } catch(e) {
      console.error('[CFD] Update error:', e);
    }
  };
</script>
</body>
</html>";
    }
}

public class CustomerDisplayManager : Java.Lang.Object, DisplayManager.IDisplayListener
{
    private readonly Context _context;
    private readonly DisplayManager _displayManager;
    private readonly ICustomerDisplayStateService? _stateService;
    private CustomerDisplayPresentation? _currentPresentation;
    private const string CfdKey = "GfcPos_CustomerDisplayEnabled";

    public CustomerDisplayManager(Context context, ICustomerDisplayStateService? stateService = null)
    {
        _context = context;
        _displayManager = (DisplayManager)context.GetSystemService(Context.DisplayService)!;
        _stateService = stateService;
        _displayManager.RegisterDisplayListener(this, new Handler(Looper.MainLooper));
    }

    public void CheckAndShow()
    {
        var isEnabled = Preferences.Default.Get(CfdKey, false);
        if (!isEnabled)
        {
            Dismiss();
            return;
        }

        var displays = _displayManager.GetDisplays(DisplayManager.DisplayCategoryPresentation);
        var secondaryDisplay = displays.FirstOrDefault(d => d.DisplayId != Display.DefaultDisplay);

        if (secondaryDisplay != null)
        {
            if (_currentPresentation != null && _currentPresentation.Display.DisplayId == secondaryDisplay.DisplayId)
            {
                return; // Already active on this display
            }

            Dismiss();
            _currentPresentation = new CustomerDisplayPresentation(_context, secondaryDisplay, _stateService);
            try
            {
                _currentPresentation.Show();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CFD] Failed to show presentation: {ex.Message}");
            }
        }
        else
        {
            Dismiss();
        }
    }

    public void Dismiss()
    {
        if (_currentPresentation != null)
        {
            try
            {
                _currentPresentation.Dismiss();
            }
            catch { }
            _currentPresentation = null;
        }
    }

    public void OnDisplayAdded(int displayId) => CheckAndShow();
    public void OnDisplayChanged(int displayId) => CheckAndShow();
    public void OnDisplayRemoved(int displayId)
    {
        if (_currentPresentation?.Display.DisplayId == displayId)
        {
            Dismiss();
        }
    }
}
#endif
