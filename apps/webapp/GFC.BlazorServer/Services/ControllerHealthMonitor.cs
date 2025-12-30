using GFC.Core.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Background service that periodically pings the controller to monitor health
/// </summary>
public class ControllerHealthMonitor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ControllerHealthMonitor> _logger;

    public ControllerHealthMonitor(
        IServiceProvider serviceProvider,
        ILogger<ControllerHealthMonitor> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Controller Health Monitor started");

        // Wait 10 seconds before starting to allow app to fully initialize
        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var healthService = scope.ServiceProvider.GetRequiredService<ControllerHealthService>();

                // Ping the controller
                await healthService.PingControllerAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in controller health monitor");
            }

            // Ping every 30 seconds
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }

        _logger.LogInformation("Controller Health Monitor stopped");
    }
}
