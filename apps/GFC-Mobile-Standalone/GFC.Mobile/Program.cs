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

// Dynamic API Targeting: Use current origin root, fallback to production
var currentOrigin = builder.HostEnvironment.BaseAddress;
var apiBaseUrl = currentOrigin;

if (currentOrigin.Contains("localhost"))
{
    apiBaseUrl = "https://localhost:7073/";
}
else
{
    // [CRITICAL] The Mobile Hub is hosted as a standalone static site on mobile.lovanow.com
    // but the API resides on the main WebApp at gfc.lovanow.com.
    apiBaseUrl = "https://gfc.lovanow.com/";
}

Console.WriteLine($"[GFC BOOT] Origin: {currentOrigin}");
Console.WriteLine($"[GFC BOOT] API Target: {apiBaseUrl}");

// --- HTTP INTERCEPTORS ---
builder.Services.AddTransient<MobileAuthenticationHandler>();

// Authenticated HttpClient for local session
builder.Services.AddHttpClient("GFC_API", client => {
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<MobileAuthenticationHandler>();

// Provide the authenticated HttpClient as the default
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
