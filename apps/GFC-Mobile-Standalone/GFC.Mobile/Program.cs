using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GFC.Mobile;
using GFC.Mobile.Services;
using GFC.Mobile.Auth;
using GFC.Core.Interfaces;
using Blazored.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Read API URL from appsettings.json or fallback to the main GFC server port
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7073/"; 
if (builder.HostEnvironment.IsDevelopment() && string.IsNullOrEmpty(builder.Configuration["ApiBaseUrl"]))
{
    // Ensure we hit the BlazorServer API, not the WASM host
    apiBaseUrl = "https://localhost:7073/"; 
}

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(apiBaseUrl),
    Timeout = TimeSpan.FromSeconds(2) // Prevent hanging if server is down
});

// --- GFC Mobile Bridge Services ---
builder.Services.AddSingleton<ConnectivityService>();          // Singleton: shared online/offline state
builder.Services.AddScoped<MobileReportingService>();          // Scoped (= singleton in WASM): shared outbox + sync events
builder.Services.AddScoped<IMobileReportingService>(sp => sp.GetRequiredService<MobileReportingService>());
builder.Services.AddScoped<IVersionService, VersionService>();
builder.Services.AddScoped<IUserManagementService, MobileUserManagementService>();
builder.Services.AddScoped<IShiftComplianceService, MobileShiftComplianceService>();
builder.Services.AddScoped<IUserUsageService, MobileUserUsageService>();
builder.Services.AddScoped<IDeviceTrustService, MobileDeviceTrustService>();

// Auth Setup
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<ICustomAuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider>(sp => 
    sp.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();
