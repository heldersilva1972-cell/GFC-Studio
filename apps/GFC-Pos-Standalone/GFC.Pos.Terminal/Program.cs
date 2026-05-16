using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GFC.Pos.Terminal;
using GFC.Pos.Terminal.Services;
using GFC.Core.Interfaces;
using GFC.Pos.UI.Services;
using Blazored.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// [SMART-DISCOVERY] Automatically resolve the correct API endpoint
// We use the HostEnvironment.BaseAddress to ensure we talk back to the same server 
// that served the PWA, supporting both local LAN IP access and public domain access.
var apiBaseUrl = builder.HostEnvironment.BaseAddress;

// If we're running as a sub-app (e.g. /pos/), we point to the root for the API
if (apiBaseUrl.Contains("/pos", StringComparison.OrdinalIgnoreCase))
{
    var uri = new Uri(apiBaseUrl);
    apiBaseUrl = $"{uri.Scheme}://{uri.Authority}/";
}

builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(apiBaseUrl) 
});

builder.Services.AddBlazoredToast();
builder.Services.AddScoped<ConnectivityService>();              // Scoped (= singleton in WASM): shared state
builder.Services.AddScoped<PosTerminalService>();               // Scoped (= singleton in WASM): shared outbox + sync events
builder.Services.AddScoped<IPosTerminalService>(sp => sp.GetRequiredService<PosTerminalService>());
builder.Services.AddSingleton<IVersionService, PosVersionService>();
builder.Services.AddScoped<IStationSettingsService, WebStationSettingsService>();
builder.Services.AddScoped<IPrinterConfigService, WebPrinterConfigService>();
builder.Services.AddScoped<IPrinterService, WebPrinterService>();

await builder.Build().RunAsync();
