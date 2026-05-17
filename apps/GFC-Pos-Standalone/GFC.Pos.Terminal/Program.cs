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

// [PRODUCTION-HYBRID] If hosted on pos.lovanow.com, we must point to gfc.lovanow.com for the API
// as the pos. subdomain is often used for static file hosting only.
if (apiBaseUrl.Contains("pos.lovanow.com", StringComparison.OrdinalIgnoreCase))
{
    apiBaseUrl = "https://gfc.lovanow.com/";
    Console.WriteLine($"[POS] PRODUCTION ROUTING ACTIVE: API -> {apiBaseUrl}");
}
// If we're running as a sub-app (e.g. /pos/), we point to the root for the API
else if (apiBaseUrl.Contains("/pos", StringComparison.OrdinalIgnoreCase))
{
    var uri = new Uri(apiBaseUrl);
    apiBaseUrl = $"{uri.Scheme}://{uri.Authority}/";
}
// If we're running locally on the standalone dev port, route to the BlazorServer backend matching the protocol
else if (apiBaseUrl.Contains("localhost:7157", StringComparison.OrdinalIgnoreCase) || 
         apiBaseUrl.Contains("localhost:5091", StringComparison.OrdinalIgnoreCase) ||
         apiBaseUrl.Contains("localhost:5100", StringComparison.OrdinalIgnoreCase))
{
    apiBaseUrl = apiBaseUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase) ? "https://localhost:7073/" : "http://localhost:5207/";
    Console.WriteLine($"[POS] LOCAL DEV ROUTING ACTIVE: API -> {apiBaseUrl}");
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
