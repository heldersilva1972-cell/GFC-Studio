using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GFC.Pos.Terminal;
using GFC.Pos.Terminal.Services;
using Blazored.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient to point to the GFC Blazor Server API
// Assuming the server is running on https://localhost:7073
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("https://localhost:7073/") 
});

builder.Services.AddBlazoredToast();
builder.Services.AddSingleton<ConnectivityService>();           // Singleton: shared online/offline state
builder.Services.AddScoped<PosTerminalService>();               // Scoped (= singleton in WASM): shared outbox + sync events
builder.Services.AddScoped<IPosTerminalService>(sp => sp.GetRequiredService<PosTerminalService>());

await builder.Build().RunAsync();
