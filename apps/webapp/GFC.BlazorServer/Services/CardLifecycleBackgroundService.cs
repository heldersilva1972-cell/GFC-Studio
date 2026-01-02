using GFC.Core.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace GFC.BlazorServer.Services
{
    public class CardLifecycleBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CardLifecycleBackgroundService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24);

        public CardLifecycleBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<CardLifecycleBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Card Lifecycle Background Service starting...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessCardLifecycleAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while executing card lifecycle check.");
                }

                // Wait for 24 hours before next check
                // In production, we might want to schedule this at specific time (e.g. 2 AM)
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task ProcessCardLifecycleAsync(CancellationToken ct)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var lifecycleService = scope.ServiceProvider.GetRequiredService<KeyCardLifecycleService>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<CardLifecycleBackgroundService>>();

                logger.LogInformation("Running Daily Card Lifecycle Check...");
                
                var year = DateTime.Today.Year;
                // Run full evaluation and deactivation
                await lifecycleService.ProcessAllMembersAsync(year, ct);
            }
        }
    }
}
