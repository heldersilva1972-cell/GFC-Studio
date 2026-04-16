using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using GFC.Core.Interfaces;
using GFC.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Authorization — required for @attribute [Authorize] and AuthenticationStateProvider injection
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

// Mobile Services
builder.Services.AddScoped<IMobileReportingService, MobileReportingService>();
builder.Services.AddScoped<IVersionService, VersionService>();

await builder.Build().RunAsync();
