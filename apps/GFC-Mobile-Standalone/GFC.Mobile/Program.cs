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

// Restore legacy HttpClient targeting
var apiBaseUrl = "https://localhost:7073/";
Console.WriteLine($"[GFC BOOT] API Target: {apiBaseUrl}");

// --- HTTP INTERCEPTORS ---
builder.Services.AddTransient<MobileAuthenticationHandler>();

// Authenticated HttpClient for local session
builder.Services.AddHttpClient("GFC_API", client => {
    client.BaseAddress = new Uri("https://localhost:7073/");
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
