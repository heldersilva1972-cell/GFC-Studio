using GFC.BlazorServer.Services.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GFC.BlazorServer.Services;

/// <summary>
/// Monitors the health and connectivity of the physical access controller
/// </summary>
public class ControllerHealthService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ControllerHealthService> _logger;
    private bool _isOnline = false;
    private DateTime? _lastSuccessfulPing;
    private DateTime? _lastFailedPing;
    private int _consecutiveFailures = 0;

    public ControllerHealthService(
        IServiceScopeFactory scopeFactory,
        ILogger<ControllerHealthService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    /// <summary>
    /// Check if controller is online
    /// </summary>
    public bool IsOnline => _isOnline;

    /// <summary>
    /// Last successful ping time
    /// </summary>
    public DateTime? LastSuccessfulPing => _lastSuccessfulPing;

    /// <summary>
    /// Last failed ping time
    /// </summary>
    public DateTime? LastFailedPing => _lastFailedPing;

    /// <summary>
    /// Number of consecutive ping failures
    /// </summary>
    public int ConsecutiveFailures => _consecutiveFailures;

    /// <summary>
    /// Ping the controller to check connectivity
    /// </summary>
    public async Task<bool> PingControllerAsync(CancellationToken ct = default)
    {
        bool wasOnline = _isOnline;
        bool currentlyOnline;

        using var scope = _scopeFactory.CreateScope();
        var controllerClient = scope.ServiceProvider.GetRequiredService<IControllerClient>();
        var notifService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        try
        {
            // Try to ping controller - this is a lightweight operation
            var success = await controllerClient.PingAsync(ct);
            
            if (success)
            {
                currentlyOnline = true;
                _lastSuccessfulPing = DateTime.UtcNow;
                _logger.LogDebug("Controller ping successful");
            }
            else
            {
                throw new Exception("Ping returned false");
            }
        }
        catch (Exception ex)
        {
            currentlyOnline = false;
            _lastFailedPing = DateTime.UtcNow;
            
            if (_consecutiveFailures == 0)
            {
                _logger.LogWarning(ex, "Controller ping failed - controller may be offline");
            }
            else if (_consecutiveFailures % 10 == 0)
            {
                _logger.LogError(ex, "Controller ping failed {Count} consecutive times", _consecutiveFailures);
            }
        }

        // State Change Logic
        if (currentlyOnline)
        {
            if (!wasOnline && _consecutiveFailures >= 3)
            {
                // We were confirmed offline, now we are back
                _ = notifService.NotifySystemAlertAsync(
                    "✅ Primary Controller RESTORED",
                    "Communication with the GFC Access Controller has been re-established.",
                    "/controllers");
            }
            _consecutiveFailures = 0;
            _isOnline = true;
        }
        else
        {
            _consecutiveFailures++;
            if (wasOnline && _consecutiveFailures == 3)
            {
                // We've failed 3 times, officially mark as offline and notify
                _isOnline = false;
                _ = notifService.NotifySystemAlertAsync(
                    "🚨 Primary Controller OFFLINE",
                    "The GFC Access Controller is not responding to heartbeats. Physical door tracking may be interrupted.",
                    "/controllers/maintenance");
            }
            else if (_consecutiveFailures < 3)
            {
                // Don't change status yet, wait for 3 failures to avoid noise
                // If it was online, stay online. If it was offline, stay offline.
                _isOnline = wasOnline; 
            }
            else
            {
                _isOnline = false;
            }
        }

        return currentlyOnline;
    }

    /// <summary>
    /// Get controller health status
    /// </summary>
    public ControllerHealthStatus GetHealthStatus()
    {
        return new ControllerHealthStatus
        {
            IsOnline = _isOnline,
            LastSuccessfulPing = _lastSuccessfulPing,
            LastFailedPing = _lastFailedPing,
            ConsecutiveFailures = _consecutiveFailures,
            Status = _isOnline ? "Online" : "Offline",
            StatusClass = _isOnline ? "success" : "danger"
        };
    }
}

/// <summary>
/// Controller health status information
/// </summary>
public class ControllerHealthStatus
{
    public bool IsOnline { get; set; }
    public DateTime? LastSuccessfulPing { get; set; }
    public DateTime? LastFailedPing { get; set; }
    public int ConsecutiveFailures { get; set; }
    public string Status { get; set; } = "Unknown";
    public string StatusClass { get; set; } = "secondary";
}
