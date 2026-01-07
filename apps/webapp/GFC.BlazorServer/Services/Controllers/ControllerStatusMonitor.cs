using GFC.BlazorServer.Data.Entities;
using GFC.BlazorServer.Services.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Centralized service that monitors controller connection status.
/// All components should subscribe to this service instead of polling individually.
/// </summary>
public class ControllerStatusMonitor : IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ControllerStatusMonitor> _logger;
    private readonly Dictionary<int, bool> _controllerStatuses = new();
    private readonly Dictionary<int, DateTime> _lastChecked = new();
    private PeriodicTimer? _timer;
    private CancellationTokenSource? _cts;
    private readonly SemaphoreSlim _lock = new(1, 1);
    
    /// <summary>
    /// Event raised when a controller's status changes
    /// </summary>
    public event Action<int, bool>? StatusChanged;
    
    /// <summary>
    /// Event raised when all statuses have been refreshed
    /// </summary>
    public event Action? StatusesRefreshed;

    public ControllerStatusMonitor(
        IServiceProvider serviceProvider,
        ILogger<ControllerStatusMonitor> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Start monitoring the specified controllers
    /// </summary>
    public void StartMonitoring(IEnumerable<ControllerDevice> controllers, TimeSpan checkInterval)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _timer?.Dispose();

        _cts = new CancellationTokenSource();
        _timer = new PeriodicTimer(checkInterval);

        // Initialize status dictionary
        foreach (var controller in controllers)
        {
            if (!_controllerStatuses.ContainsKey(controller.Id))
            {
                _controllerStatuses[controller.Id] = false;
            }
        }

        _ = Task.Run(async () =>
        {
            // Initial check
            await CheckAllControllersAsync(controllers);

            // Periodic checks
            try
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    await CheckAllControllersAsync(controllers);
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when stopping
            }
        }, _cts.Token);
    }

    /// <summary>
    /// Get the current status of a controller
    /// </summary>
    public bool GetStatus(int controllerId)
    {
        return _controllerStatuses.TryGetValue(controllerId, out var status) && status;
    }

    /// <summary>
    /// Get the last time a controller was checked
    /// </summary>
    public DateTime? GetLastChecked(int controllerId)
    {
        return _lastChecked.TryGetValue(controllerId, out var time) ? time : null;
    }

    /// <summary>
    /// Force an immediate check of a specific controller
    /// </summary>
    public async Task<bool> CheckControllerAsync(ControllerDevice controller)
    {
        await _lock.WaitAsync();
        try
        {
            var wasOnline = _controllerStatuses.TryGetValue(controller.Id, out var previousStatus) && previousStatus;
            
            try
            {
                // Create a scope to get the scoped IControllerClient
                using var scope = _serviceProvider.CreateScope();
                var controllerClient = scope.ServiceProvider.GetRequiredService<IControllerClient>();
                
                var status = await controllerClient.GetRunStatusAsync(controller.Id, CancellationToken.None);
                var isOnline = status != null && status.IsOnline;
                
                _controllerStatuses[controller.Id] = isOnline;
                _lastChecked[controller.Id] = DateTime.UtcNow;

                // Raise event if status changed
                if (wasOnline != isOnline)
                {
                    _logger.LogInformation(
                        "Controller {ControllerId} ({Name}) status changed: {OldStatus} -> {NewStatus}",
                        controller.Id, controller.Name, wasOnline ? "Online" : "Offline", isOnline ? "Online" : "Offline");
                    
                    StatusChanged?.Invoke(controller.Id, isOnline);
                }

                return isOnline;
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Failed to check controller {ControllerId} status", controller.Id);
                
                var isOnline = false;
                _controllerStatuses[controller.Id] = isOnline;
                _lastChecked[controller.Id] = DateTime.UtcNow;

                if (wasOnline != isOnline)
                {
                    StatusChanged?.Invoke(controller.Id, isOnline);
                }

                return false;
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task CheckAllControllersAsync(IEnumerable<ControllerDevice> controllers)
    {
        var tasks = controllers.Select(c => CheckControllerAsync(c));
        await Task.WhenAll(tasks);
        
        StatusesRefreshed?.Invoke();
    }

    public void Dispose()
    {
        try
        {
            _cts?.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // Already disposed, ignore
        }
        
        _cts?.Dispose();
        _timer?.Dispose();
        _lock?.Dispose();
    }
}
