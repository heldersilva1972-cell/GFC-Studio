import sys

path = r'c:\Users\hnsil\Documents\GFC\cursor files\GFC-System\GFC-Studio V2\apps\webapp\GFC.BlazorServer\Components\Pages\Liquor\LiquorHub.razor'
with open(path, 'r', encoding='utf-8') as f:
    content = f.read()

target = """                            }
                        }
                            </div>
                            
                            <div class="col-lg-4">
                                <div class="cart-sidebar sticky-top" style="top: 0px; z-index: 10;">
                                    <div class="card border-0 shadow-sm rounded-4 overflow-hidden">
                                        <div class="card-header bg-white border-bottom p-3">
                                            <h5 class="mb-0 fw-black d-flex align-items-center text-primary">
                                                <i class="bi bi-cart4 me-2"></i> Current Order
                                            </h5>
                                        </div>
                                        <div class="card-body p-0">
                                            @if (!_cart.Any(v => v.Value > 0))
                                            {
                                                <div class="text-center py-5 text-muted">
                                                    <i class="bi bi-cart-x display-6 mb-2 opacity-50"></i>
                                                    <p class="small mb-0">Your order is currently empty.</p>
                                                </div>
                                            }
                                            else
                                            {
                                                var cartItems = _cart.Where(e => e.Value > 0)
                                                    .Select(e => new { Item = _items.FirstOrDefault(i => i.Id == e.Key), Qty = e.Value })
                                                    .Where(x => x.Item != null)
                                                    .OrderBy(x => x.Item!.Vendor?.Name)
                                                    .ToList();

                                                <div class="cart-scroll-area p-4" style="max-height: 60vh; overflow-y: auto;">
                                                    @foreach (var vGroup in cartItems.GroupBy(x => x.Item!.Vendor))
                                                    {
                                                        <div class="vendor-cart-group mb-4 pb-3 border-bottom border-light">
                                                            <div class="d-flex justify-content-between align-items-center mb-2">
                                                                <span class="extra-small fw-black text-uppercase text-primary tracking-wider">@vGroup.Key?.Name (@vGroup.Sum(x => x.Qty * x.Item!.CurrentPrice).ToString("C"))</span>
                                                                @if (vGroup.Sum(x => x.Qty * x.Item!.CurrentPrice) < GetVendorMin(vGroup.Key))
                                                                {
                                                                    <span class="badge bg-danger pulse-animation" style="font-size: 0.6rem;">
                                                                        WAIT! NEED @((GetVendorMin(vGroup.Key) - vGroup.Sum(x => x.Qty * x.Item!.CurrentPrice)).ToString("C")) MORE
                                                                    </span>
                                                                }
                                                                else
                                                                {
                                                                    <span class="badge bg-success" style="font-size: 0.6rem;">MIN MET</span>
                                                                }
                                                            </div>
                                                            @foreach (var x in vGroup)
                                                            {
                                                                <div class="d-flex justify-content-between align-items-center mb-1">
                                                                    <div class="small text-muted flex-grow-1">
                                                                        <span class="fw-bold text-dark">@x.Qty ×</span> @x.Item!.Name
                                                                    </div>
                                                                    <div class="small fw-bold">@((x.Item.CurrentPrice * x.Qty).ToString("C"))</div>
                                                                </div>
                                                            }
                                                            <div class="d-flex justify-content-between mt-2 pt-2 border-top border-light opacity-75">
                                                                <span class="extra-small fw-bold">@vGroup.Key?.Name Total</span><span class="small fw-black ms-2">@vGroup.Sum(x => x.Qty * x.Item!.CurrentPrice).ToString("C")</span>
                                                                 <button class="btn btn-xs btn-outline-primary rounded-pill fw-black px-2 ms-auto" @onclick="() => ShowOrderConfirmation(vGroup.Key?.Id)" disabled="@(vGroup.Sum(x => x.Qty * x.Item!.CurrentPrice) < GetVendorMin(vGroup.Key))">Order</button>

                                                            </div>
                                                        </div>
                                                    }
                                                </div>
                                                <div class="cart-summary bg-light p-4">
                                                    <div class="d-flex justify-content-between mb-3">
                                                        <span class="text-muted fw-bold">Grand Total</span>
                                                        <span class="h4 fw-black text-primary mb-0">@GetCartTotal().ToString("C")</span>
                                                    </div>
                                                    @{ var canOrderAll = cartItems.GroupBy(x => x.Item!.Vendor).All(g => g.Sum(x => x.Qty * x.Item!.CurrentPrice) >= GetVendorMin(g.Key)); }
                                                    <button class="btn btn-primary w-100 py-3 rounded-pill fw-black shadow-lg" 
                                                            @onclick="SubmitOrderAsync" 
                                                            disabled="@(!canOrderAll)">
                                                        PLACE ALL ORDERS
                                                    </button>
                                                    @if (!canOrderAll)
                                                    {
                                                        <div class="alert alert-warning mt-3 border-0 rounded-4 p-2 extra-small text-center fw-bold">
                                                            <i class="bi bi-exclamation-circle me-1"></i> Some vendor minimums have not been met.
                                                        </div>
                                                    }
                                                </div>
                                            }
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>"""

replacement = """                                </div>
                            </div>
                        </div>
                        
                        <!-- RIGHT SIDE: CART (SCROLLABLE SIDEBAR) -->
                        <div class="col-lg-4 h-100 d-flex flex-column bg-light bg-opacity-50">
                            <div class="card border-0 shadow-none bg-transparent d-flex flex-column h-100">
                                <div class="card-header bg-white border-bottom p-4">
                                    <h5 class="mb-0 fw-black d-flex align-items-center text-primary">
                                        <i class="bi bi-cart4 me-2"></i> Current Order
                                    </h5>
                                </div>
                                <div class="card-body p-0 d-flex flex-column flex-grow-1 overflow-hidden">
                                    @if (!_cart.Any(v => v.Value > 0))
                                    {
                                        <div class="text-center py-5 px-4 animate-fade-in flex-grow-1 d-flex flex-column justify-content-center">
                                            <i class="bi bi-cart-x display-6 mb-2 opacity-50"></i>
                                            <p class="small mb-0">Your order is currently empty.</p>
                                        </div>
                                    }
                                    else
                                    {
                                        var cartItems = _cart.Where(e => e.Value > 0)
                                            .Select(e => new { Item = _items.FirstOrDefault(i => i.Id == e.Key), Qty = e.Value })
                                            .Where(x => x.Item != null)
                                            .OrderBy(x => x.Item!.Vendor?.Name)
                                            .ToList();

                                        <div class="flex-grow-1 overflow-y-auto p-4">
                                            @foreach (var vGroup in cartItems.GroupBy(x => x.Item!.Vendor))
                                            {
                                                <div class="vendor-cart-group mb-4 pb-3 border-bottom border-light">
                                                    <div class="d-flex justify-content-between align-items-center mb-2">
                                                        <span class="extra-small fw-black text-uppercase text-primary tracking-wider">@vGroup.Key?.Name (@vGroup.Sum(x => x.Qty * x.Item!.CurrentPrice).ToString("C"))</span>
                                                        @if (vGroup.Sum(x => x.Qty * x.Item!.CurrentPrice) < GetVendorMin(vGroup.Key))
                                                        {
                                                            <span class="badge bg-danger pulse-animation" style="font-size: 0.6rem;">
                                                                WAIT! NEED @((GetVendorMin(vGroup.Key) - vGroup.Sum(x => x.Qty * x.Item!.CurrentPrice)).ToString("C")) MORE
                                                                </span>
                                                        }
                                                        else
                                                        {
                                                            <span class="badge bg-success" style="font-size: 0.6rem;">MIN MET</span>
                                                        }
                                                    </div>
                                                    @foreach (var x in vGroup)
                                                    {
                                                        <div class="d-flex justify-content-between align-items-center mb-1">
                                                            <div class="small text-muted flex-grow-1">
                                                                <span class="fw-bold text-dark">@x.Qty ×</span> @x.Item!.Name
                                                            </div>
                                                            <div class="small fw-bold">@((x.Item.CurrentPrice * x.Qty).ToString("C"))</div>
                                                        </div>
                                                    }
                                                    <div class="d-flex justify-content-between mt-2 pt-2 border-top border-light opacity-75">
                                                        <span class="extra-small fw-bold">@vGroup.Key?.Name Total</span><span class="small fw-black ms-2">@vGroup.Sum(x => x.Qty * x.Item!.CurrentPrice).ToString("C")</span>
                                                         <button class="btn btn-xs btn-outline-primary rounded-pill fw-black px-2 ms-auto" @onclick="() => ShowOrderConfirmation(vGroup.Key?.Id)" disabled="@(vGroup.Sum(x => x.Qty * x.Item!.CurrentPrice) < GetVendorMin(vGroup.Key))">Order</button>
                                                    </div>
                                                </div>
                                            }
                                        </div>
                                        <div class="cart-summary bg-white border-top p-4">
                                            <div class="d-flex justify-content-between mb-3">
                                                <span class="text-muted fw-bold">Grand Total</span>
                                                <span class="h4 fw-black text-primary mb-0">@GetCartTotal().ToString("C")</span>
                                            </div>
                                            @{ var canOrderAll = cartItems.GroupBy(x => x.Item!.Vendor).All(g => g.Sum(x => x.Qty * x.Item!.CurrentPrice) >= GetVendorMin(g.Key)); }
                                            <button class="btn btn-primary w-100 py-3 rounded-pill fw-black shadow-lg" 
                                                    @onclick="SubmitOrderAsync" 
                                                    disabled="@(!canOrderAll)">
                                                PLACE ALL ORDERS
                                            </button>
                                            @if (!canOrderAll)
                                            {
                                                <div class="alert alert-warning mt-3 border-0 rounded-4 p-2 extra-small text-center fw-bold">
                                                    <i class="bi bi-exclamation-circle me-1"></i> Some vendor minimums have not been met.
                                                </div>
                                            }
                                        </div>
                                    }
                                </div>
                            </div>
                        </div>
                    </div>
                </div>"""

# Standardize line endings and whitespace for comparison
def normalize(s):
    return "\\n".join(line.rstrip() for line in s.splitlines())

if normalize(target) in normalize(content):
    new_content = content.replace(target, replacement)
    with open(path, 'w', encoding='utf-8') as f:
        f.write(new_content)
    print("Success")
else:
    # Try with exact match if normalized fails
    if target in content:
        new_content = content.replace(target, replacement)
        with open(path, 'w', encoding='utf-8') as f:
            f.write(new_content)
        print("Success (Exact)")
    else:
        print("Target not found")
        # Print a snippet of content around where we expect target to be
        idx = content.find('<div class="col-lg-4">')
        if idx != -1:
            print("Found partial match at index", idx)
            print("Context around found index:")
            print(repr(content[idx-100:idx+200]))
