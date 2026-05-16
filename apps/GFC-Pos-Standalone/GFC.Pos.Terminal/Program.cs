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
var host = builder.HostEnvironment.BaseAddress.ToLower();
string apiBaseUrl;

if (host.Contains("localhost"))
{
    // Scenario 1: Local Development on Laptop
    apiBaseUrl = "https://localhost:7073/"; 
}
else
{
    // Scenario 2: Any Production Deployment (Panel or Server)
    apiBaseUrl = "https://gfc.lovanow.com/";
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
