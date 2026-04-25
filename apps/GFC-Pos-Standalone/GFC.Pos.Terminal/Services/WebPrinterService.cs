using GFC.Pos.UI.Services;
using Microsoft.JSInterop;
using System.Text.Json;

namespace GFC.Pos.Terminal.Services;

public class WebPrinterService : IPrinterService
{
    private readonly IJSRuntime _js;

    public WebPrinterService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task<bool> PrintReceiptAsync(string content)
    {
        // For Web/PWA, we create a temporary hidden iframe with the content and print it
        await _js.InvokeVoidAsync("eval", $@"
            (function(content) {{
                const iframe = document.createElement('iframe');
                iframe.style.position = 'fixed';
                iframe.style.right = '0';
                iframe.style.bottom = '0';
                iframe.style.width = '0';
                iframe.style.height = '0';
                iframe.style.border = '0';
                document.body.appendChild(iframe);
                
                const doc = iframe.contentWindow.document;
                doc.open();
                doc.write(content);
                doc.close();
                
                iframe.contentWindow.focus();
                iframe.contentWindow.print();
                
                // Remove iframe after print dialog is handled
                setTimeout(() => document.body.removeChild(iframe), 1000);
            }})({JsonSerializer.Serialize(content)})");

        return true;
    }

    public Task<bool> KickDrawerAsync()
    {
        // Browser cannot kick drawer directly
        return Task.FromResult(false);
    }
}
