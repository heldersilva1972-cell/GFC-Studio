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

// Read API URL from appsettings.json or fallback to current origin
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? builder.HostEnvironment.BaseAddress;

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(apiBaseUrl) 
});

// --- GFC Mobile Bridge Services (No Stubs) ---
builder.Services.AddScoped<IVersionService, VersionService>();
builder.Services.AddScoped<IMobileReportingService, MobileReportingService>();
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
