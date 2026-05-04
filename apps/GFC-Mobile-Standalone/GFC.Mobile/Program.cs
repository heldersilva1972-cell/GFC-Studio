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

// Smart API Resolver: Prioritize config, then auto-detect if on a remote server, then fallback to localhost dev port
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
if (string.IsNullOrEmpty(apiBaseUrl))
{
    var currentUri = new Uri(builder.HostEnvironment.BaseAddress);
    if (!currentUri.Host.Contains("localhost") && !currentUri.Host.Contains("127.0.0.1"))
    {
        // We are on a remote server (e.g. your-gfc-site.com). 
        // Point the API to the root of the current host.
        apiBaseUrl = $"{currentUri.Scheme}://{currentUri.Host}";
        if (!currentUri.IsDefaultPort) apiBaseUrl += $":{currentUri.Port}";
        apiBaseUrl += "/";
    }
    else
    {
        // We are developing locally.
        apiBaseUrl = "https://localhost:7073/"; 
    }
}
Console.WriteLine($"[GFC BOOT] API Target: {apiBaseUrl}");

// --- HTTP INTERCEPTORS ---
builder.Services.AddTransient<MobileAuthenticationHandler>();

builder.Services.AddHttpClient("GFC_API", (sp, client) => {
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
}).AddHttpMessageHandler<MobileAuthenticationHandler>();

// Provide the default HttpClient from the factory
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GFC_API"));

// --- GFC Mobile Bridge Services ---
builder.Services.AddScoped<IConnectivityService, MobileConnectivityService>();
builder.Services.AddScoped<MobileReportingService>();          // Scoped (= singleton in WASM): shared outbox + sync events
builder.Services.AddScoped<IMobileReportingService>(sp => sp.GetRequiredService<MobileReportingService>());
builder.Services.AddScoped<IVersionService, VersionService>();
builder.Services.AddScoped<IUserManagementService, MobileUserManagementService>();
builder.Services.AddScoped<IShiftComplianceService, MobileShiftComplianceService>();
builder.Services.AddScoped<IUserUsageService, MobileUserUsageService>();
builder.Services.AddScoped<IDeviceTrustService, MobileDeviceTrustService>();

// [NEW] STANDALONE HUB SERVICES
builder.Services.AddScoped<MobileAnalyticsService>();
builder.Services.AddScoped<MobileKeyCardService>();
builder.Services.AddScoped<MobileDuesService>();
builder.Services.AddScoped<MobileDiagnosticsService>();

// Auth Setup
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<ICustomAuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());
builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider>(sp => 
    sp.GetRequiredService<CustomAuthenticationStateProvider>());

builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();
