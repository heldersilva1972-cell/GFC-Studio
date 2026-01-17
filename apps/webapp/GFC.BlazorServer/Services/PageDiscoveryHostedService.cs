using GFC.BlazorServer.Services;

namespace GFC.BlazorServer;

/// <summary>
/// Hosted service that runs page discovery on application startup.
/// This ensures the AppPages table is always synchronized with the actual pages in the application.
/// </summary>
public class PageDiscoveryHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PageDiscoveryHostedService> _logger;

    public PageDiscoveryHostedService(
        IServiceProvider serviceProvider,
        ILogger<PageDiscoveryHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Running page discovery on application startup...");

        using var scope = _serviceProvider.CreateScope();
        var pageDiscovery = scope.ServiceProvider.GetRequiredService<PageDiscoveryService>();
        
        await pageDiscovery.DiscoverAndSyncPagesAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
