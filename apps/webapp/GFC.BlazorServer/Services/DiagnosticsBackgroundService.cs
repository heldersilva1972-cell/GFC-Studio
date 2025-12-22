// [NEW]
using GFC.BlazorServer.Services.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class DiagnosticsBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<DiagnosticsBackgroundService> _logger;

        public DiagnosticsBackgroundService(IServiceProvider services, ILogger<DiagnosticsBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Diagnostics Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var systemPerformanceService = scope.ServiceProvider.GetRequiredService<ISystemPerformanceService>();
                        var performanceHistoryService = scope.ServiceProvider.GetRequiredService<IPerformanceHistoryService>();
                        var alertManagementService = scope.ServiceProvider.GetRequiredService<IAlertManagementService>();

                        // 1. Get current performance metrics
                        var metrics = await systemPerformanceService.GetPerformanceMetricsAsync(stoppingToken);

                        // 2. Store a snapshot
                        await performanceHistoryService.AddPerformanceSnapshotAsync(metrics, stoppingToken);

                        // 3. Check for alerts
                        await alertManagementService.CheckAlertsAsync(metrics, stoppingToken);

                        // 4. Purge old snapshots
                        await performanceHistoryService.PurgeOldSnapshotsAsync(7, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while collecting diagnostics.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            _logger.LogInformation("Diagnostics Background Service is stopping.");
        }
    }
}
