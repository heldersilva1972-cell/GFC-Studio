using GFC.BlazorServer.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services.Controllers;

/// <summary>
/// Background service that initializes and manages the controller status monitor
/// </summary>
public class ControllerStatusMonitorService : IHostedService
{
    private readonly ControllerStatusMonitor _statusMonitor;
    private readonly ControllerRegistryService _registryService;
    private readonly ILogger<ControllerStatusMonitorService> _logger;

    public ControllerStatusMonitorService(
        ControllerStatusMonitor statusMonitor,
        ControllerRegistryService registryService,
        ILogger<ControllerStatusMonitorService> logger)
    {
        _statusMonitor = statusMonitor;
        _registryService = registryService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Controller Status Monitor Service");

        try
        {
            // Get all controllers from the registry
            var controllers = await _registryService.GetControllersAsync(cancellationToken: cancellationToken);
            
            if (controllers.Any())
            {
                // Start monitoring with 60-second intervals to reduce traffic (Heartbeat only)
                // "Run only when something happens" -> We can't be purely event-driven with UDP, 
                // but 60s is much less aggressive than 3s.
                _statusMonitor.StartMonitoring(controllers, TimeSpan.FromSeconds(60));
                _logger.LogInformation("Controller Status Monitor started for {Count} controllers (Interval: 60s)", controllers.Count());
            }
            else
            {
                _logger.LogWarning("No controllers found in registry. Status monitoring not started.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start Controller Status Monitor");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Controller Status Monitor Service");
        _statusMonitor.Dispose();
        return Task.CompletedTask;
    }
}
