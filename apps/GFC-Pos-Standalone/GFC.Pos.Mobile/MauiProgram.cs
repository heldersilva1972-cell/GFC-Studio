using Microsoft.Extensions.Logging;
using GFC.Pos.UI.Services;
using GFC.Core.Interfaces;
using Blazored.Toast;
using GFC.Pos.Mobile.Services;

namespace GFC.Pos.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
        builder.Services.AddBlazoredToast();
        builder.Services.AddTransient<GfcApiAuthenticationHandler>();
        builder.Services.AddHttpClient("GfcApi", client => client.BaseAddress = new Uri("https://gfc.lovanow.com/"))
            .AddHttpMessageHandler<GfcApiAuthenticationHandler>();
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GfcApi"));

        builder.Services.AddScoped<ConnectivityService>();
        builder.Services.AddScoped<PosTerminalService>();
        builder.Services.AddScoped<IPosTerminalService>(sp => sp.GetRequiredService<PosTerminalService>());
        builder.Services.AddSingleton<IVersionService, PosVersionService>();
        builder.Services.AddSingleton<IStationSettingsService, MauiStationSettingsService>();
        builder.Services.AddSingleton<IPrinterConfigService, PrinterConfigService>();
        builder.Services.AddSingleton<IPrinterService, MauiPrinterService>();
        builder.Services.AddScoped<DeviceActivationService>();

#if ANDROID
        builder.Services.AddSingleton<IUpdateService, AndroidUpdateService>();
#else
        builder.Services.AddSingleton<IUpdateService, FallbackUpdateService>();
#endif

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
