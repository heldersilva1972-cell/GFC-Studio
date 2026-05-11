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

// Configure HttpClient to point to the GFC Blazor Server API
// [SMART REDIRECT] Detect local development vs production deployment
var apiBaseUrl = builder.HostEnvironment.BaseAddress.Contains("localhost") 
    ? "https://localhost:7073/"  // Point to local webapp during development
    : "https://gfc.lovanow.com/"; // Point to production API for deployed PWA

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
