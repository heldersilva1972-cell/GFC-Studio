using GFC.Core.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class TemporaryCardLifecycleBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TemporaryCardLifecycleBackgroundService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

        public TemporaryCardLifecycleBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<TemporaryCardLifecycleBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Temporary Card Lifecycle Background Service starting...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessExpirationsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing temporary card expiration check.");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task ProcessExpirationsAsync(CancellationToken ct)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var lifecycleService = scope.ServiceProvider.GetRequiredService<TemporaryCardLifecycleService>();
                await lifecycleService.CheckExpirationsAsync(ct);
            }
        }
    }
}
